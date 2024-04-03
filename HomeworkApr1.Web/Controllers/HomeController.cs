using HomeworkApr1.Data;
using HomeworkApr1.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace HomeworkApr1.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly string _connectionString;
        public HomeController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConStr");
        }
        public IActionResult Index()
        {
            var repo = new QARepository(_connectionString);
            return View(repo.GetAllQuestions());
        }
        public IActionResult ViewQuestion(int id)
        {
            var repo = new QARepository(_connectionString);
            return View(repo.GetQuestionById(id));
        }
        [Authorize]
        public IActionResult AskQuestion()
        {
            return View();
        }
        [HttpPost]
        [Authorize]
        public IActionResult AskQuestion(Question q, List<string> tags)
        {
            if (q == null)
            {
                return RedirectToAction("Index");
            }
            var repo = new QARepository(_connectionString);
            var userId = GetCurrentUserId();
            if(userId == null)
            {
                return RedirectToAction("Index");
            }
            q.UserId = userId.Value;
            q.DatePosted = DateTime.Now;
            repo.AskQuestion(q, tags);
            return Redirect($"ViewQuestion?id={q.Id}");
        }
        private int? GetCurrentUserId()
        {
            var repo = new UserRepository(_connectionString);
            if (!User.Identity.IsAuthenticated)
            {
                return null;
            }
            var user = repo.GetByEmail(User.Identity.Name);
            if (user == null)
            {
                return null;
            }

            return user.Id;
        }
        [HttpPost]
        [Authorize]
        public IActionResult AddAnswer(Answer a)
        {
            var userId = GetCurrentUserId();
            if (userId == null)
            {
                return RedirectToAction("Index");
            }
            a.UserId = userId.Value;
            a.DateAnswered = DateTime.Now;
            var repo = new QARepository(_connectionString);
            repo.AddAnswer(a);
            return Redirect($"ViewQuestion?id={a.QuestionId}");
        }
    }
}