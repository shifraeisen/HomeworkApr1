using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeworkApr1.Data
{
    public class Answer
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int QuestionId { get; set; }
        public string Content { get; set; }
        public DateTime DateAnswered { get; set; }

        public Question Question { get; set; }
        public User User { get; set; }
    }
}
