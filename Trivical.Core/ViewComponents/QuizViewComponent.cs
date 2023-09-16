using Microsoft.AspNetCore.Mvc;
using Trivical.Core.Models;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;

namespace Trivical.Core.ViewComponents
{
    public class QuizViewComponent : ViewComponent
    {

        private readonly HttpClient _httpClient;
        private readonly IMemoryCache _memoryCache;

        public IViewComponentResult Invoke()
        {
            List<Question> questions = GetQuizQuestionsAsync().Result;

            return View(questions);
        }

        [HttpPost]
        public IViewComponentResult SubmitAnswers(List<UserAnswer> userAnswers)
        {
            List<Question> questions = GetQuizQuestionsAsync().Result;
            int score = 0;
            // Store the score and userAnswers in a session or database
            return View("_Results", score);
        }


        public async Task<List<Question>> GetQuizQuestionsAsync()
        {
            DateTime currentDate = DateTime.Now.Date;
            string cacheKey = "QuizResponse";

            //Trys to get cached questions
            if (_memoryCache.TryGetValue(cacheKey, out List<Question> cachedQuestions))
            {
                return cachedQuestions;
            }

            HttpResponseMessage response = await _httpClient.GetAsync("https://the-trivia-api.com/v2/questions?region=GB&types=text_choice&limit=2");
            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                List<Question> questions = JsonConvert.DeserializeObject<List<Question>>(json);

                // Calculate the time remaining until midnight
                DateTime midnight = currentDate.AddDays(1);
                TimeSpan timeUntilMidnight = midnight - DateTime.Now;

                // Cache the questions with an expiration time set to midnight
                _memoryCache.Set(cacheKey, questions, timeUntilMidnight);

                return questions;
            }

            return null;
        }

    }
}
