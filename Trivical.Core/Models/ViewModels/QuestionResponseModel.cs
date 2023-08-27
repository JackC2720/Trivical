namespace Trivical.Core.Models.ViewModels
{
	public class Result
	{
		public string category { get; set; }
		public string type { get; set; }
		public string difficulty { get; set; }
		public string question { get; set; }
		public string correct_answer { get; set; }
		public List<string> incorrect_answers { get; set; }
	}

	public class Root
	{
		public int response_code { get; set; }
		public List<Result> results { get; set; }
	}
    public class ViewReturn
    {
        public List<Question> questions { get; set; }
    }
    public class Question
	{
		public string question { get; set; }
		public List<Answers> answers { get; set; }
	}
    public class Answers
    {
		public string answer { get; set; }
		public bool correct { get; set; }
    }
}
