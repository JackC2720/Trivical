using Microsoft.AspNetCore.Mvc;
using Trivical.Core.Models;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using J2N.Collections.Generic.Extensions;

namespace Trivical.Core.ViewComponents
{
    public class QuizViewComponent : ViewComponent
    {

        private readonly HttpClient _httpClient;
        private readonly IMemoryCache _memoryCache;

        public QuizViewComponent(HttpClient httpClient, IMemoryCache memoryCache)
        {
            _httpClient = httpClient;
            _memoryCache = memoryCache;
        }
        //Invoke calls the default partial view
        public IViewComponentResult Invoke()
        {
            List<Question> questions = GetQuizQuestionsAsync().Result;
            foreach (Question question in questions)
            {
                question.incorrectAnswers.Add(question.correctAnswer);
                question.incorrectAnswers = question.incorrectAnswers.OrderBy(a => Guid.NewGuid()).ToList();
            }
            return View(questions);
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
