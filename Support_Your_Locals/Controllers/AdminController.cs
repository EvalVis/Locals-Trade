using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Support_Your_Locals.Infrastructure;
using Support_Your_Locals.Models;
using Support_Your_Locals.Models.Repositories;
using Support_Your_Locals.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;



namespace Support_Your_Locals.Controllers
{
    public class AdminController : Controller
    {

        public static event EventHandler<ResponseEventArgs> ResponseEvent;

        private IConfiguration configuration;
        private ILegacyServiceRepository repository;

        public AdminController(ILegacyServiceRepository repository, IConfiguration configuration)
        {
            this.configuration = configuration;
            this.repository = repository;
        }
        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public ActionResult Index()
        {
            var businesses = repository.GetBusinesses();
            var usersBusinesses = businesses.GroupBy(business => business.User);
            var questions = repository.GetQuestions().OrderBy(question => question.IsAnswered);
            var users = repository.GetUsers().Where(notAdmin);
            return View(new AdminViewModel()
            {
                Businesses = businesses,
                UsersBusinesses = usersBusinesses,
                Users = repository.GetUsers().Where(notAdmin),
                TotalBusiness = businesses.Count(),
                TotalProducts = repository.GetProducts().Count(),
                TotalUsers = users.Count(),
                Questions = questions,
            });
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult DeleteBusiness(long id)
        {
            repository.DeleteBusiness(id);
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult DeleteUser(long id)
        {
            repository.DeleteUser(id);
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Administrator")]
        [HttpGet]
        public ActionResult AnswerQuestion(long id, string email)
        {
            return View(new AnswerQuestionViewModel()
            {
                QuestionId = id,
                Email = email,
            });
        }


        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public ActionResult AnswerQuestion(AnswerQuestionViewModel model)
        {
            ResponseEvent(this, new ResponseEventArgs(model.Email, model.AnwserText));
            repository.AnswerQuestion(model.QuestionId, model.AnwserText);
            return RedirectToAction("Index");
        }

        private bool notAdmin(User user)
        {
            return !configuration.GetSection("Admin:AdminEmails").Get<List<string>>().Contains(user.Email);
        } 
    }
}
