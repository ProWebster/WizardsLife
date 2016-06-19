using Lib.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WizardsLife.Models.Modals.Owls;

namespace WizardsLife.Controllers
{
    public class ModalController : Controller
    {
        public ActionResult SignUpOrLogIn()
        {
            return PartialView("_SignUpOrLogIn");
        }


        #region Owls

        public ActionResult Owls(Owls_VM viewModel = null)
        {
            if (viewModel == null)
                viewModel = new Owls_VM();
            viewModel.Conversations = Lib.DatabaseManager.OwlManager.Current.GetConversations(int.Parse(User.Identity.Name));

            return PartialView("_Owls", viewModel);
        }
        
        public ActionResult OpenCreateConversation()
        {
            List<int> friendIds = Lib.DatabaseManager.FriendsManager.Current.GetFromUser(int.Parse(User.Identity.Name));
            List<User> friends = Lib.DatabaseManager.UserManager.Current.GetFromIds(friendIds);

            CreateConversation_VM viewModel = new CreateConversation_VM();
            viewModel.FriendNames = friends.Select(x => x.CharName + " " + x.CharFamilyName).ToList();


            return PartialView("_OwlsCreateConversation", viewModel);
        }

        public ActionResult CreateConversation(Models.Modals.Owls.CreateConversation_VM viewModel)
        {
            int userId = int.Parse(User.Identity.Name);

            viewModel.Error = "";

            // Check if all required fields has a value
            if (string.IsNullOrWhiteSpace(viewModel.Recipient))
                viewModel.Error = "You must enter a recipient.";
            else if (string.IsNullOrWhiteSpace(viewModel.Content))
                viewModel.Error = "You must enter a message.";

            // Check if all recipients was found
            List<User> users = new List<User>();
            if (!string.IsNullOrWhiteSpace(viewModel.Recipient))
            {
                string[] recipients = viewModel.Recipient.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string recipient in recipients)
                {
                    if (!string.IsNullOrWhiteSpace(recipient.Trim()))
                    {
                        User u = Lib.DatabaseManager.UserManager.Current.GetFromCharName(recipient.Trim());
                        if (u == null)
                        {
                            // Error!
                            viewModel.Error = recipient + " does not exist as a user!";
                            break;
                        }
                        else
                            users.Add(u);
                    }
                }
            }

            if (users.Count == 0)
                viewModel.Error = "You must enter a valid recipient.";

            users.Add(new Lib.Entity.User { Id = userId });

            // If there are at least one error - return it to the view
            if (!string.IsNullOrWhiteSpace(viewModel.Error))
                return PartialView("_OwlsCreateConversation", viewModel);

            
            // All data is given - create the conversation!
            if (string.IsNullOrWhiteSpace(viewModel.Subject))
                viewModel.Subject = "No subject";


            OwlConversation conversation = new OwlConversation();
            conversation.Subject = viewModel.Subject;
            conversation.UserIds = users.Select(x => x.Id).ToList();

            conversation.Id = Lib.DatabaseManager.OwlManager.Current.CreateConversation(conversation, userId);

            if (conversation.Id <= 0)
            {
                viewModel.Error = "Unknown error occurred!";
                return PartialView("_OwlsCreateConversation", viewModel);
            }


            Owl owl = new Owl();
            owl.OwlConversationId = conversation.Id;
            owl.UserId = userId;
            owl.Content = viewModel.Content.Replace(System.Environment.NewLine, "<br>");
            owl.Id = Lib.DatabaseManager.OwlManager.Current.CreateOwl(owl, conversation);


            return Owls();
        }

        public ActionResult OpenConversation(int conversationId)
        {
            int userId = int.Parse(User.Identity.Name);

            OwlConversation conversation = Lib.DatabaseManager.OwlManager.Current.GetConversation(conversationId);

            Conversation_VM viewModel = new Conversation_VM();
            viewModel.Conversation = conversation;

            // Mark conversation as read!
            Lib.DatabaseManager.OwlManager.Current.MarkAsRead(conversationId, userId);

            ViewData["OwlsCount"] = Lib.DatabaseManager.OwlManager.Current.MustReadCount(userId);

            return PartialView("_OwlConversation", viewModel);
        }

        public ActionResult OwlReply(Conversation_VM viewModel)
        {
            viewModel.Conversation = Lib.DatabaseManager.OwlManager.Current.GetConversation(viewModel.Conversation.Id);

            if (!string.IsNullOrWhiteSpace(viewModel.ReplyContent))
            {
                Owl owl = new Owl();
                owl.OwlConversationId = viewModel.Conversation.Id;
                owl.UserId = int.Parse(User.Identity.Name);
                owl.Content = viewModel.ReplyContent.Replace(System.Environment.NewLine, "<br>");
                Lib.DatabaseManager.OwlManager.Current.CreateOwl(owl, viewModel.Conversation);
                viewModel.ReplyContent = "";
            }

            return PartialView("_OwlConversation", viewModel);
        }

        #endregion


        #region Friends

        public ActionResult Friends()
        {
            return PartialView("_Friends");
        }

        #endregion
    }
}