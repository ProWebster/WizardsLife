using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Entity
{
    public class SortingQuizQuestion
    {
        public int QuestionNo { get; set; }
        public string Question { get; set; }
        public List<KeyValuePair<int, string>> Options { get; set; }



        public static List<SortingQuizQuestion> QuizQuestions =
            new List<SortingQuizQuestion>
            {
                new SortingQuizQuestion
                {
                    QuestionNo = 1,
                    Question = "Which class would you like most?",
                    Options = new List<KeyValuePair<int, string>>
                    {
                        new KeyValuePair<int, string>(1, "Potions"),
                        new KeyValuePair<int, string>(2, "Charms"),
                        new KeyValuePair<int, string>(3, "Defense Against the Dark Arts"),
                        new KeyValuePair<int, string>(4, "History of Magic")
                    }
                },
                new SortingQuizQuestion
                {
                    QuestionNo = 2,
                    Question = "Which spell would you cast on yourself?",
                    Options = new List<KeyValuePair<int, string>>
                    {
                        new KeyValuePair<int, string>(1, "Mind Reader"),
                        new KeyValuePair<int, string>(2, "Fly"),
                        new KeyValuePair<int, string>(3, "Invisible"),
                        new KeyValuePair<int, string>(4, "Strength")
                    }
                },
                new SortingQuizQuestion
                {
                    QuestionNo = 3,
                    Question = "Which animal would you choose for your Hogwarts Pet?",
                    Options = new List<KeyValuePair<int, string>>
                    {
                        new KeyValuePair<int, string>(1, "Rat"),
                        new KeyValuePair<int, string>(2, "Toad"),
                        new KeyValuePair<int, string>(3, "Owl"),
                        new KeyValuePair<int, string>(4, "Cat")
                    }
                },
                new SortingQuizQuestion
                {
                    QuestionNo = 4,
                    Question = "Which flavor of Bertie Botts Beans would you prefer?",
                    Options = new List<KeyValuePair<int, string>>
                    {
                        new KeyValuePair<int, string>(1, "Chocolate"),
                        new KeyValuePair<int, string>(2, "Licorice"),
                        new KeyValuePair<int, string>(3, "Strawberry"),
                        new KeyValuePair<int, string>(4, "Peanut Butter")
                    }
                },
                new SortingQuizQuestion
                {
                    QuestionNo = 5,
                    Question = "Which position in Quidditch would you most like to play?",
                    Options = new List<KeyValuePair<int, string>>
                    {
                        new KeyValuePair<int, string>(1, "Bludger"),
                        new KeyValuePair<int, string>(2, "Keeper"),
                        new KeyValuePair<int, string>(3, "Seeker"),
                        new KeyValuePair<int, string>(4, "Chaser")
                    }
                },
                new SortingQuizQuestion
                {
                    QuestionNo = 6,
                    Question = "What would you have done with Hagrid's Dragon?",
                    Options = new List<KeyValuePair<int, string>>
                    {
                        new KeyValuePair<int, string>(1, "Told Dumbledore"),
                        new KeyValuePair<int, string>(2, "Raised it Yourself"),
                        new KeyValuePair<int, string>(3, "Let it go"),
                        new KeyValuePair<int, string>(4, "Given it to the appropriate department at the Ministry og magic")
                    }
                },
                new SortingQuizQuestion
                {
                    QuestionNo = 7,
                    Question = "If you had a portrait that guarded your room, what would it be?",
                    Options = new List<KeyValuePair<int, string>>
                    {
                        new KeyValuePair<int, string>(1, "Snake"),
                        new KeyValuePair<int, string>(2, "Knight"),
                        new KeyValuePair<int, string>(3, "Fat Lady"),
                        new KeyValuePair<int, string>(4, "Unicorn")
                    }
                },
                new SortingQuizQuestion
                {
                    QuestionNo = 8,
                    Question = "Who would you most like to turn into a toad?",
                    Options = new List<KeyValuePair<int, string>>
                    {
                        new KeyValuePair<int, string>(1, "Hagrid"),
                        new KeyValuePair<int, string>(2, "Professor Snape"),
                        new KeyValuePair<int, string>(3, "Draco Malfoy"),
                        new KeyValuePair<int, string>(4, "You-Know-Who")
                    }
                },
                new SortingQuizQuestion
                {
                    QuestionNo = 9,
                    Question = "What are your best qualities?",
                    Options = new List<KeyValuePair<int, string>>
                    {
                        new KeyValuePair<int, string>(1, "Smart"),
                        new KeyValuePair<int, string>(2, "Loyal"),
                        new KeyValuePair<int, string>(3, "Brave"),
                        new KeyValuePair<int, string>(4, "Kind")
                    }
                },
                new SortingQuizQuestion
                {
                    QuestionNo = 10,
                    Question = "What do you want to be when you grow up?",
                    Options = new List<KeyValuePair<int, string>>
                    {
                        new KeyValuePair<int, string>(1, "An official for the Quidditch World Cup"),
                        new KeyValuePair<int, string>(2, "A Professor at Hogwarts"),
                        new KeyValuePair<int, string>(3, "An Auror"),
                        new KeyValuePair<int, string>(4, "A worker in the Ministry of Magic")
                    }
                }
            };
    }
}
