using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using Trivical.Core.Models;

namespace Trivical.Core.Services
{

    public class QuizApiService
    {
        private readonly HttpClient _httpClient;
        private readonly IMemoryCache _memoryCache;

        public QuizApiService(IMemoryCache memoryCache)
        {
            _httpClient = new HttpClient();
            _memoryCache = memoryCache;
        }

        public async Task<List<Question>> GetQuestionsAsync()
        {
            DateTime currentDate = DateTime.Now.Date;

            if (_memoryCache.TryGetValue(currentDate, out List<Question> cachedQuestions))
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
                _memoryCache.Set(currentDate, questions, timeUntilMidnight);

                return questions;
            }

            return null;
        }
    }
}
