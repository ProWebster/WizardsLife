using Lib.Entity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WizardsLife.Models.Sorting;

namespace WizardsLife.Controllers.Game
{
    [Authorize]
    public class SortingController : Controller
    {
        public ActionResult Index()
        {
            int userId = Int32.Parse(User.Identity.Name);

            User u = Lib.DatabaseManager.UserManager.Current.Get(userId);
            if (u.Status == Lib.Entity.User.UserStatus.Ready)
                return RedirectToAction("Index", "News");

            List<SortingQuizValue> answeredQuestions = Lib.DatabaseManager.SortingQuizValueManager.Current.GetFromUser(userId);

            SortingQuizValue latestAnsweredQuestion = answeredQuestions.OrderByDescending(x => x.QuestionNo).FirstOrDefault();

            int questionNo = 1;
            if (latestAnsweredQuestion != null)
                questionNo = latestAnsweredQuestion.QuestionNo + 1;

            SortingQuestion_VM sq = new SortingQuestion_VM();
            sq.Question = Lib.Entity.SortingQuizQuestion.QuizQuestions.FirstOrDefault(x => x.QuestionNo == questionNo);

            if (sq.Question == null)
                return RedirectToAction("SortedResult");

            return View(sq);
        }

        public ActionResult AnswerQuestion(SortingQuestion_VM viewModel)
        {
            if (!viewModel.Value.HasValue) // If question has not been answered!
            {
                viewModel.Question = Lib.Entity.SortingQuizQuestion.QuizQuestions.FirstOrDefault(x => x.QuestionNo == viewModel.Question.QuestionNo);
                return PartialView("_SortingQuestion", viewModel);
            }

            int userId = Int32.Parse(User.Identity.Name);

            // Save response
            Lib.DatabaseManager.SortingQuizValueManager.Current.Create(new SortingQuizValue { UserId = userId, QuestionNo = viewModel.Question.QuestionNo, AnswerValue = viewModel.Value.Value });

            // Get next question
            SortingQuestion_VM sq = new SortingQuestion_VM();
            sq.Question = Lib.Entity.SortingQuizQuestion.QuizQuestions.FirstOrDefault(x => x.QuestionNo == viewModel.Question.QuestionNo + 1);

            // If there is another question, show it!
            if (sq.Question != null)
                return PartialView("_SortingQuestion", sq);
            else
            {
                // Otherwise - get sorting result!
                return Content("redirect:" + Url.Action("SortedResult"));// RedirectToActionPermanent("SortedResult");
            }
        }

        public ActionResult SortedResult()
        {
            int userId = Int32.Parse(User.Identity.Name);

            User u = Lib.DatabaseManager.UserManager.Current.Get(userId);
            if (u.Status == Lib.Entity.User.UserStatus.Ready)
                return RedirectToAction("Index", "News");

            List<SortingQuizValue> values = Lib.DatabaseManager.SortingQuizValueManager.Current.GetFromUser(userId);

            int valueWithMostOccurrences = values.Select(x=>x.AnswerValue).GroupBy(i => i).OrderByDescending(grp => grp.Count()).Select(grp => grp.Key).First();

            switch (valueWithMostOccurrences)
            {
                case 1:
                    // Slytherin
                    u.House = Lib.Entity.User.Houses.Slytherin;
                    break;
                case 2:
                    // Ravenclaw
                    u.House = Lib.Entity.User.Houses.Ravenclaw;
                    break;
                case 3:
                    // Gryffindor
                    u.House = Lib.Entity.User.Houses.Gryffindor;
                    break;
                case 4:
                    // Hufflepuff
                    u.House = Lib.Entity.User.Houses.Hufflepuff;
                    break;
            }

            u.Status = Lib.Entity.User.UserStatus.Ready;
            Lib.DatabaseManager.UserManager.Current.Update(u);
            
            return View(new SortedResult_VM { House = u.House, CharName = u.CharName });
        }
    }
}