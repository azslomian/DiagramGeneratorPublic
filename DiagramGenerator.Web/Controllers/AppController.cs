using DiagramGenerator.DataAccess.Model;
using DiagramGenerator.Domain.Repositories.Interfaces;
using DiagramGenerator.Web.Services;
using DiagramGenerator.Web.ViewModels;
using DiagramGenerator.Web.ViewModels.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DiagramGenerator.Web.Controllers
{
    public class AppController : Controller
    {
        private readonly IMailService mailService;
        private readonly IDiagramRepository diagramRepository;

        public AppController(IMailService mailService, IDiagramRepository diagramRepository)
        {
            this.mailService = mailService;
            this.diagramRepository = diagramRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("contact")]
        public IActionResult Contact()
        {
            ViewBag.PageTitle = "Contact Us";
            return View();
        }

        //[HttpGet("inputs")]
        //public IActionResult Inputs()
        //{
        //    ViewBag.PageTitle = "Inputs";
        //    return View();
        //}


        //[HttpGet("outputs")]
        //public IActionResult Outputs()
        //{
        //    ViewBag.PageTitle = "Outputs";
        //    return View();
        //}

        [HttpPost("contact")]
        public IActionResult Contact(ContactViewModel model)
        {
            if (ModelState.IsValid)
            {
                mailService.SendMessage("azslomian@gmail.com", model.Name);
                ViewBag.UserMessage = "Mail Sent";
                ModelState.Clear();
            }

            return View();
        }

        public IActionResult About()
        {
            ViewBag.PageTitle = "About";
            return View();
        }

        [Authorize]
        [HttpGet("shop")]
        public IActionResult Shop()
        {
            ViewBag.Title = "Shop";
            //var results = diagramRepository.GetAll();

            var result = new List<Operation>();
            for (int i = 0; i<10; i++)
            {
                result.Add(new Operation()
                {
                    Name = "Roman",
                    Description = "New Perfect Man"
                });
                result.Add(new Operation()
                {
                    Name = "Karta",
                    Description = "New Perfect Card"
                });
                result.Add(new Operation()
                {
                    Name = "Tomek",
                    Description = "Some Say Man"
                });
            }
            
            return View(result);
        }
    }
}
