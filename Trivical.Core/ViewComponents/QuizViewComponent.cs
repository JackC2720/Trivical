using Microsoft.AspNetCore.Mvc;
using Trivical.Core.Models;
using Trivical.Core.Services;

namespace Trivical.Core.ViewComponents
{
    public class QuizViewComponent : ViewComponent
    {
        private readonly QuizApiService _quizApiService;
        public IViewComponentResult Invoke()
        {
            List<Question> questions = _quizApiService.GetQuestionsAsync().Result;
            
            return View(questions);
        }

        [HttpPost]
        public IViewComponentResult SubmitAnswers(List<UserAnswer> userAnswers)
        {
            List<Question> questions = _quizApiService.GetQuestionsAsync().Result;
            int score = CalculateScore(questions, userAnswers);
            // Store the score and userAnswers in a session or database
            return View("_Results", score);
        }

        private int CalculateScore(List<Question> questions, List<UserAnswer> userAnswers)
        {
            int score = 0;
            for (int i = 0; i < questions.Count; i++)
            {
                if (userAnswers[i].SelectedAnswer == questions[i].correctAnswer)
                {
                    score++;
                }
            }
            return score;
        }
    }
}
