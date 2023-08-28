using Microsoft.AspNetCore.Mvc;
using Trivical.Core.Models;
using Trivical.Core.Services;
using Umbraco.Cms.Core.Cache;
using Umbraco.Cms.Core.Logging;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Infrastructure.Persistence;
using Umbraco.Cms.Web.Website.Controllers;

namespace Trivical.Core.Controllers.Render
{
	public class QuizController : SurfaceController
    {
        private readonly QuizApiService _quizApiService;

        public QuizController(IUmbracoContextAccessor umbracoContextAccessor, IUmbracoDatabaseFactory databaseFactory, ServiceContext services, AppCaches appCaches, IProfilingLogger profilingLogger, IPublishedUrlProvider publishedUrlProvider) : base(umbracoContextAccessor, databaseFactory, services, appCaches, profilingLogger, publishedUrlProvider)
		{
		}

		public ActionResult Index()
        {
            List<Question> questions = _quizApiService.GetQuestionsAsync().Result;
            return View("Quiz", questions);
        }

        [HttpPost]
        public ActionResult SubmitAnswers(List<UserAnswer> userAnswers)
        {
            List<Question> questions = _quizApiService.GetQuestionsAsync().Result;
            int score = CalculateScore(questions, userAnswers);
            // Store the score and userAnswers in a session or database
            return View("Results", score);
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
