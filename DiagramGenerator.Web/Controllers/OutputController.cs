using AutoMapper;
using DiagramGenerator.DataAccess.Model;
using DiagramGenerator.DataAccess.Model.Enum;
using DiagramGenerator.Domain.Services.Interfaces;
using DiagramGenerator.Web.ViewModels;
using DiagramGenerator.Web.ViewModels.Pages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace DiagramGenerator.Web.Controllers
{
    [Authorize]
    public class OutputController : Controller
    {
        private readonly ILogger<DiagramController> logger;
        private readonly IDiagramManager diagramManager;
        private readonly IOutputManager outputManager;
        private readonly ICriterionManager criterionManager;
        private readonly IMapper mapper;


        public OutputController(ILogger<DiagramController> logger,
             IDiagramManager diagramManager,
             IOutputManager outputManager,
             ICriterionManager criterionManager,
             IMapper mapper)
        {
            this.logger = logger;
            this.diagramManager = diagramManager;
            this.outputManager = outputManager;
            this.criterionManager = criterionManager;
            this.mapper = mapper;
        }

        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.PageTitle = "Outputs";
            string userMail = User.FindFirst(ClaimTypes.Name)?.Value;

            var model = new OutputPageViewModel()
            {
                Outputs = mapper.Map<IList<Output>, IList<OutputViewModel>>(outputManager.GetAll(userMail)),
                Criteria = mapper.Map<IList<Criterion>, IList<CriterionViewModel>>(criterionManager.GetAll(userMail)),
            };

            return View(model);
        }

        //[HttpGet("get-all")]
        //public ActionResult<IList<Output>> Get(bool includeItems = true)
        //{
        //    string userMail = User.FindFirst(ClaimTypes.Name)?.Value;
        //    return Ok(mapper.Map<IList<Output>, IList<OutputViewModel>>(outputManager.GetAll(userMail)));
        //}

        //[HttpGet("{id}")]
        //public ActionResult<Output> Get(Guid id)
        //{
        //    return Ok(mapper.Map<Output, OutputViewModel>(outputManager.GetById(id)));
        //}

        [HttpPost("add-output")]
        public IActionResult AddOutput(OutputViewModel model)
        {
            string userMail = User.FindFirst(ClaimTypes.Name)?.Value;
            model.UserEmail = userMail;
            if (ModelState.IsValid)
            {
                ViewBag.UserMessage = "New Output created successfully";
                ModelState.Clear();
            }
            else
            {
                return BadRequest("Invalid Model");
            }

            outputManager.Create(mapper.Map<OutputViewModel, Output>(model));
            return View(model);
        }

        [HttpGet("add-output")]
        public IActionResult AddOutput(OutputType type)
        {
            return View(new OutputViewModel { Type = type });
        }

        [HttpPost("edit-output")]
        public IActionResult EditOutput(OutputViewModel model)
        {
            string userMail = User.FindFirst(ClaimTypes.Name)?.Value;
            model.UserEmail = userMail;
            if (ModelState.IsValid)
            {
                ViewBag.UserMessage = "Output edited successfully";
            }
            else
            {
                return BadRequest("Invalid Model");
            }

            outputManager.Update(model.Id, mapper.Map<OutputViewModel, Output>(model));
            return View(model);
        }

        [HttpGet("edit-output")]
        public IActionResult EditOutput(Guid id)
        {
            var output = mapper.Map<Output, OutputViewModel>(outputManager.GetById(id));
            return View(output);
        }

        [HttpGet("remove-output")]
        public IActionResult RemoveOutput(Guid id)
        {
            outputManager.Delete(id);
            ViewBag.PageTitle = "Outputs";
            string userMail = User.FindFirst(ClaimTypes.Name)?.Value;

            var model = new OutputPageViewModel()
            {
                Outputs = mapper.Map<IList<Output>, IList<OutputViewModel>>(outputManager.GetAll(userMail)),
                Criteria = mapper.Map<IList<Criterion>, IList<CriterionViewModel>>(criterionManager.GetAll(userMail)),
            };

            return View("Index", model);
        }

        [HttpPost("add-criterion")]
        public IActionResult AddCriterion(CriterionViewModel model)
        {
            string userMail = User.FindFirst(ClaimTypes.Name)?.Value;
            model.UserEmail = userMail;
            if (ModelState.IsValid)
            {
                ViewBag.UserMessage = "New Criterion created successfully";
                ModelState.Clear();
            }
            else
            {
                return BadRequest("Invalid Model");
            }

            criterionManager.Create(mapper.Map<CriterionViewModel, Criterion>(model));
            return View(model);
        }

        [HttpGet("add-criterion")]
        public IActionResult AddCriterion()
        {
            return View();
        }

        [HttpPost("edit-criterion")]
        public IActionResult EditCriterion(CriterionViewModel model)
        {
            string userMail = User.FindFirst(ClaimTypes.Name)?.Value;
            model.UserEmail = userMail;
            if (ModelState.IsValid)
            {
                ViewBag.UserMessage = "Criterion edited successfully";
            }
            else
            {
                return BadRequest("Invalid Model");
            }

            criterionManager.Update(model.Id, mapper.Map<CriterionViewModel, Criterion>(model));
            return View(model);
        }

        [HttpGet("edit-criterion")]
        public IActionResult EditCriterion(Guid id)
        {
            var criterion = mapper.Map<Criterion, CriterionViewModel>(criterionManager.GetById(id));
            return View(criterion);
        }

        [HttpGet("remove-criterion")]
        public IActionResult RemoveCriterion(Guid id)
        {
            criterionManager.Delete(id);
            ViewBag.PageTitle = "Outputs";
            string userMail = User.FindFirst(ClaimTypes.Name)?.Value;

            var model = new OutputPageViewModel()
            {
                Outputs = mapper.Map<IList<Output>, IList<OutputViewModel>>(outputManager.GetAll(userMail)),
                Criteria = mapper.Map<IList<Criterion>, IList<CriterionViewModel>>(criterionManager.GetAll(userMail)),
            };

            return View("Index", model);
        }
    } 
}
