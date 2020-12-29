using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Support_Your_Locals.Infrastructure;
using Support_Your_Locals.Models;
using Support_Your_Locals.Models.Repositories;
using Support_Your_Locals.Models.ViewModels;
using System.Collections.Generic;
using System.Linq;



namespace Support_Your_Locals.Controllers
{
    public class AdminController : Controller
    {

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
            var users = repository.GetUsers()
                .Where(notAdmin)
                .GroupJoin(businesses, user => user.UserID, business => business.UserID, (user, businesses) =>
                {
                    user.Businesses = businesses.ToList();
                    return user;
                })
                .OrderBy(user => user.Businesses.Count);

            var userWithMostBusinesess = users.Aggregate<User, (int, User), User>((0, null), 
                (biggestUser, next) => biggestUser.Item1 < next.Businesses.Count ? (next.Businesses.Count, next) : biggestUser, 
                biggestUser => biggestUser.Item2);

            var questions = repository.GetQuestions()
                .OrderBy(question => question.IsAnswered)
                .GroupBy(question => question.Email);

            return View(new AdminViewModel()
            {
                Businesses = businesses,
                Users = users,
                TotalBusiness = businesses.Count(),
                TotalProducts = repository.GetProducts().Count(),
                TotalUsers = users.Count(),
                Questions = questions,
                UserWithMostBusinesess = userWithMostBusinesess,
            }); ;
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
            new Mailer(configuration).AnswerQuestion(model);
            repository.AnswerQuestion(model.QuestionId, model.AnwserText);
            return RedirectToAction("Index");
        }

        private bool notAdmin(User user)
        {
            return !configuration.GetSection("Admin:AdminEmails").Get<List<string>>().Contains(user.Email);
        } 
    }
}
