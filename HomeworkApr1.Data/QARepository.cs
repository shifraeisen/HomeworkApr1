using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeworkApr1.Data
{
    public class QARepository
    {
        private readonly string _connectionString;
        public QARepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        public List<Question> GetAllQuestions()
        {
            using var ctx = new QADataContext(_connectionString);
            return ctx.Questions.Include(q => q.Answers).Include(q => q.QuestionsTags).ThenInclude(qt => qt.Tag).ToList();
        }
        public Question GetQuestionById(int id)
        {
            using var ctx = new QADataContext(_connectionString);
            return ctx.Questions.Include(q => q.Answers).ThenInclude(a => a.User).Include(q => q.QuestionsTags).ThenInclude(qt => qt.Tag).Include(q => q.User).FirstOrDefault(q => q.Id == id);
        }
        public void AskQuestion(Question question, List<string> tags)
        {
            using var ctx = new QADataContext(_connectionString);
            ctx.Questions.Add(question);
            ctx.SaveChanges();
            foreach(var t in tags)
            {
                var tag = ctx.Tags.FirstOrDefault(x => x.Name == t);
                int tagId;
                if(tag == null)
                {
                    var tg = new Tag { Name = t };
                    ctx.Tags.Add(tg);
                    ctx.SaveChanges();
                    tagId = tg.Id;
                }
                else
                {
                    tagId = tag.Id;
                }
                ctx.QuestionsTags.Add(new QuestionsTags
                {
                    QuestionId = question.Id,
                    TagId = tagId
                });
            }
            ctx.SaveChanges();
        }
        public void AddAnswer(Answer a)
        {
            using var ctx = new QADataContext(_connectionString);
            ctx.Answers.Add(a);
            ctx.SaveChanges();
        }
    }
}
