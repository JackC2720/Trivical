using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Trivical.Core.Models.ViewModels;


namespace Trivical.Core.Components
{
	public class QuestionsViewComponent : ViewComponent
	{
		private readonly HttpClient _httpClient;

		public QuestionsViewComponent(HttpClient httpClient)
		{
			_httpClient = httpClient;
		}

		public async Task<IViewComponentResult> InvokeAsync()
		{
			// Use HttpClient to fetch data from the API
			HttpResponseMessage response = await _httpClient.GetAsync("https://opentdb.com/api.php?amount=10&type=multiple");

			if (response.IsSuccessStatusCode)
			{
				var content = await response.Content.ReadAsStringAsync();
                // Deserialize JSON response
                var triviaQuestions = JsonConvert.DeserializeObject<Root>(content);

                ViewReturn viewReturn = new ViewReturn();
                viewReturn.questions = new List<Question>();

                foreach (var questionResult in triviaQuestions.results)
                {
                    Question question = new Question
                    {
                        question = questionResult.question,
                        answers = new List<Answers>()
                    };

                    // Add correct answer
                    question.answers.Add(new Answers
                    {
                        answer = questionResult.correct_answer,
                        correct = true
                    });

                    // Add incorrect answers
                    foreach (var incorrectAnswer in questionResult.incorrect_answers)
                    {
                        question.answers.Add(new Answers
                        {
                            answer = incorrectAnswer,
                            correct = false
                        });
                    }

                    viewReturn.questions.Add(question);
                }

                return View(viewReturn);
			}
			else
			{
				// Handle error case
				return View(new Root());
			}
		}
	}
}
