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
    public class InputController : Controller
    {
        private readonly ILogger<DiagramController> logger;
        private readonly IDiagramManager diagramManager;
        private readonly IInputManager inputManager;
        private readonly IRequirementManager requirementManager;
        private readonly IMethodManager methodManager;
        private readonly IMapper mapper;


        public InputController(ILogger<DiagramController> logger,
             IDiagramManager diagramManager,
             IInputManager inputManager,
             IRequirementManager requirementManager,
             IMethodManager methodManager,
             IMapper mapper)
        {
            this.logger = logger;
            this.diagramManager = diagramManager;
            this.inputManager = inputManager;
            this.requirementManager = requirementManager;
            this.methodManager = methodManager;
            this.mapper = mapper;
        }

        [HttpGet]
        public IActionResult Index()
        {   
            ViewBag.PageTitle = "Inputs";
            string userMail = User.FindFirst(ClaimTypes.Name)?.Value;

            var model = new InputPageViewModel()
            {
                Inputs = mapper.Map<IList<Input>, IList<InputViewModel>>(inputManager.GetAll(userMail)),
                Requirements = mapper.Map<IList<Requirement>, IList<RequirementViewModel>>(requirementManager.GetAll(userMail)),
                Methods = mapper.Map<IList<Method>, IList<MethodViewModel>>(methodManager.GetAll(userMail)),
            };

            return View(model);
        }

        //[HttpGet]
        //public ActionResult<IList<Input>> Get(bool includeItems = true)
        //{
        //    string userMail = User.FindFirst(ClaimTypes.Name)?.Value;
        //    return Ok(mapper.Map<IList<Input>, IList<InputViewModel>>(inputManager.GetAll(userMail)));
        //}

        //[HttpGet("{id}")]
        //public ActionResult<Input> Get(Guid id)
        //{
        //    return Ok(mapper.Map<Input, InputViewModel>(inputManager.GetById(id)));
        //}

        [HttpPost("add-input")]
        public IActionResult AddInput(InputViewModel model)
        {
            string userMail = User.FindFirst(ClaimTypes.Name)?.Value;
            model.UserEmail = userMail;
            if (ModelState.IsValid)
            {
                ViewBag.UserMessage = "New Input created successfully";
                ModelState.Clear();
            }
            else
            {
                return BadRequest("Invalid Model");
            }

            inputManager.Create(mapper.Map<InputViewModel, Input>(model));
            return View(model);
        }

        [HttpGet("add-input")]
        public IActionResult AddInput(InputType type)
        {
            return View(new InputViewModel { Type = type });
        }

        [HttpPost("edit-input")]
        public IActionResult EditInput(InputViewModel model)
        {
            string userMail = User.FindFirst(ClaimTypes.Name)?.Value;
            model.UserEmail = userMail;
            if (ModelState.IsValid)
            {
                ViewBag.UserMessage = "Input edited successfully";
            }
            else
            {
                return BadRequest("Invalid Model");
            }

            inputManager.Update(model.Id, mapper.Map<InputViewModel, Input>(model));
            return View(model);
        }

        [HttpGet("edit-input")]
        public IActionResult EditInput(Guid id)
        {
            var input = mapper.Map<Input, InputViewModel>(inputManager.GetById(id));
            return View(input);
        }

        [HttpGet("remove-input")]
        public IActionResult RemoveInput(Guid id)
        {
            inputManager.Delete(id);
            ViewBag.PageTitle = "Inputs";
            string userMail = User.FindFirst(ClaimTypes.Name)?.Value;

            var model = new InputPageViewModel()
            {
                Inputs = mapper.Map<IList<Input>, IList<InputViewModel>>(inputManager.GetAll(userMail)),
                Requirements = mapper.Map<IList<Requirement>, IList<RequirementViewModel>>(requirementManager.GetAll(userMail)),
                Methods = mapper.Map<IList<Method>, IList<MethodViewModel>>(methodManager.GetAll(userMail)),
            };

            return View("Index", model);
        }

        [HttpPost("add-requirement")]
        public IActionResult AddRequirement(RequirementViewModel model)
        {
            string userMail = User.FindFirst(ClaimTypes.Name)?.Value;
            model.UserEmail = userMail;
            if (ModelState.IsValid)
            {
                ViewBag.UserMessage = "New Requirement created successfully";
                ModelState.Clear();
            }
            else
            {
                return BadRequest("Invalid Model");
            }

            requirementManager.Create(mapper.Map<RequirementViewModel, Requirement>(model));
            return View(model);
        }

        [HttpGet("add-requirement")]
        public IActionResult AddRequirement()
        {
            return View();
        }

        [HttpPost("edit-requirement")]
        public IActionResult EditRequirement(RequirementViewModel model)
        {
            string userMail = User.FindFirst(ClaimTypes.Name)?.Value;
            model.UserEmail = userMail;
            if (ModelState.IsValid)
            {
                ViewBag.UserMessage = "Requirement edited successfully";
            }
            else
            {
                return BadRequest("Invalid Model");
            }

            requirementManager.Update(model.Id, mapper.Map<RequirementViewModel, Requirement>(model));
            return View(model);
        }

        [HttpGet("edit-requirement")]
        public IActionResult EditRequirement(Guid id)
        {
            var requirement = mapper.Map<Requirement, RequirementViewModel>(requirementManager.GetById(id));
            return View(requirement);
        }

        [HttpGet("remove-requirement")]
        public IActionResult RemoveRequirement(Guid id)
        {
            requirementManager.Delete(id);
            string userMail = User.FindFirst(ClaimTypes.Name)?.Value;

            var model = new InputPageViewModel()
            {
                Inputs = mapper.Map<IList<Input>, IList<InputViewModel>>(inputManager.GetAll(userMail)),
                Requirements = mapper.Map<IList<Requirement>, IList<RequirementViewModel>>(requirementManager.GetAll(userMail)),
                Methods = mapper.Map<IList<Method>, IList<MethodViewModel>>(methodManager.GetAll(userMail)),
            };

            return View("Index", model);
        }

        [HttpPost("add-method")]
        public IActionResult AddMethod(MethodViewModel model)
        {
            string userMail = User.FindFirst(ClaimTypes.Name)?.Value;
            model.UserEmail = userMail;
            if (ModelState.IsValid)
            {
                ViewBag.UserMessage = "New Method created successfully";
                ModelState.Clear();
            }
            else
            {
                return BadRequest("Invalid Model");
            }

            methodManager.Create(mapper.Map<MethodViewModel, Method>(model));
            return View(model);
        }

        [HttpGet("add-method")]
        public IActionResult AddMethod()
        {
            return View();
        }

        [HttpPost("edit-method")]
        public IActionResult EditMethod(MethodViewModel model)
        {
            string userMail = User.FindFirst(ClaimTypes.Name)?.Value;
            model.UserEmail = userMail;
            if (ModelState.IsValid)
            {
                ViewBag.UserMessage = "Requirement edited successfully";
            }
            else
            {
                return BadRequest("Invalid Model");
            }

            methodManager.Update(model.Id, mapper.Map<MethodViewModel, Method>(model));
            return View(model);
        }

        [HttpGet("edit-method")]
        public IActionResult EditMethod(Guid id)
        {
            var method = mapper.Map<Method, MethodViewModel>(methodManager.GetById(id));
            return View(method);
        }

        [HttpGet("remove-method")]
        public IActionResult RemoveMethod(Guid id)
        {
            methodManager.Delete(id);
            string userMail = User.FindFirst(ClaimTypes.Name)?.Value;

            var model = new InputPageViewModel()
            {
                Inputs = mapper.Map<IList<Input>, IList<InputViewModel>>(inputManager.GetAll(userMail)),
                Requirements = mapper.Map<IList<Requirement>, IList<RequirementViewModel>>(requirementManager.GetAll(userMail)),
                Methods = mapper.Map<IList<Method>, IList<MethodViewModel>>(methodManager.GetAll(userMail)),
            };

            return View("Index", model);
        }
    } 
}
