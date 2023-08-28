using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trivical.Core.Models
{
    public class Question
    {
        public QuestionText question { get; set; }
        public List<string> incorrectAnswers { get; set; }
        public string correctAnswer { get; set; }
    }
    public class QuestionText
    {
        public string text { get; set; }
    }
}
