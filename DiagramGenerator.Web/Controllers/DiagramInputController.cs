using AutoMapper;
using DiagramGenerator.DataAccess.Model;
using DiagramGenerator.DataAccess.Model.ManyToMany;
using DiagramGenerator.Domain.Services.Interfaces;
using DiagramGenerator.Web.ViewModels;
using DiagramGenerator.Web.ViewModels.Diagram;
using DiagramGenerator.Web.ViewModels.Pages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace DiagramGenerator.Web.Controllers
{
    [Authorize]
    public class DiagramInputController : Controller
    {
        private readonly ILogger<DiagramInputController> logger;
        private readonly IDiagramManager diagramManager;
        private readonly IInputManager inputManager;
        private readonly IProcessManager processManager;
        private readonly IMapper mapper;
        private readonly IMethodManager methodManager;
        private readonly IRequirementManager requirementManager;

        public DiagramInputController(ILogger<DiagramInputController> logger,
             IDiagramManager diagramManager,
             IInputManager inputManager,
             IProcessManager processManager,
             IMethodManager methodManager,
             IRequirementManager requirementManager,
             IMapper mapper)
        {
            this.logger = logger;
            this.diagramManager = diagramManager;
            this.inputManager = inputManager;
            this.processManager = processManager;
            this.methodManager = methodManager;
            this.requirementManager = requirementManager;
            this.mapper = mapper;
        }

        [HttpGet]
        public IActionResult Index(Guid diagramId)
        {
            string userMail = User.FindFirst(ClaimTypes.Name)?.Value;
            var diagram = diagramManager.GetByIdEager(diagramId);
            var inputIds = diagram.Inputs.Select(i => new { i.InputId, i.Quantity } ).ToList();
            var allInputs = mapper.Map<IList<Input>, IList<DiagramInputViewModel>>(inputManager.GetAll(userMail)).ToList();

            foreach (var input in allInputs)
            {
                var existingInput = inputIds.FirstOrDefault(x => x.InputId == input.Id);
                if (existingInput != null)
                {
                    input.IsSelected = true;
                    input.Quantity = existingInput.Quantity;
                }

                input.DiagramId = diagramId;
            }

            var pageViewModel = new DiagramInputPageViewModel()
            {
                DiagramId = diagramId,
                Inputs = allInputs,
            };

            return View(pageViewModel);
        }

        [HttpGet]
        public IActionResult Methods(Guid diagramId)
        {
            string userMail = User.FindFirst(ClaimTypes.Name)?.Value;
            var diagram = diagramManager.GetByIdEager(diagramId);

            var methodIds = diagram.Methods.Select(i => new { i.MethodId }).ToList();
            var allMethods = mapper.Map<IList<Method>, IList<DiagramMethodViewModel>>(methodManager.GetAll(userMail)).ToList();

            foreach (var method in allMethods)
            {
                var existingMethod = methodIds.FirstOrDefault(x => x.MethodId == method.Id);
                if (existingMethod != null)
                {
                    method.IsSelected = true;
                }

                method.DiagramId = diagramId;
            }

            var pageViewModel = new DiagramMethodPageViewModel()
            {
                DiagramId = diagramId,
                Methods = allMethods,
            };

            return View(pageViewModel);
        }

        [HttpGet]
        public IActionResult Requirements(Guid diagramId)
        {
            string userMail = User.FindFirst(ClaimTypes.Name)?.Value;
            var diagram = diagramManager.GetByIdEager(diagramId);

            var requirementIds = diagram.Requirements.Select(i => new { i.RequirementId }).ToList();
            var allRequirements = mapper.Map<IList<Requirement>, IList<DiagramRequirementViewModel>>(requirementManager.GetAll(userMail)).ToList();

            foreach (var requirement in allRequirements)
            {
                var existingRequirement = requirementIds.FirstOrDefault(x => x.RequirementId == requirement.Id);
                if (existingRequirement != null)
                {
                    requirement.IsSelected = true;
                }

                requirement.DiagramId = diagramId;
            }

            var pageViewModel = new DiagramRequirementPageViewModel()
            {
                DiagramId = diagramId,
                Requirements = allRequirements
            };

            return View(pageViewModel);
        }

        [HttpPost("save-inputs")]
        public IActionResult SaveInputs(DiagramInputPageViewModel model)
        {
            if (ModelState.IsValid)
            {
                ViewBag.UserMessage = "Inputs edited successfully";
            }
            else
            {
                return BadRequest("Invalid Model");
            }

            model.Inputs = model.Inputs == null ? new List<DiagramInputViewModel>() : model.Inputs;
            var selectedInputs = model.Inputs.Where(x => x.IsSelected == true).ToList();
            //selectedInputs.ForEach(y => y.DiagramId = model.DiagramId);
            diagramManager.UpdateInputs(model.DiagramId, mapper.Map<List<DiagramInputViewModel>, List<DiagramInput>>(selectedInputs));
            return View("Index", model);
        }

        [HttpPost("save-methods")]
        public IActionResult SaveMethods(DiagramMethodPageViewModel model)
        {
            if (ModelState.IsValid)
            {
                ViewBag.UserMessage = "Methods edited successfully";
            }
            else
            {
                return BadRequest("Invalid Model");
            }

            model.Methods = model.Methods == null ? new List<DiagramMethodViewModel>() : model.Methods;

            var selectedMethods = model.Methods.Where(x => x.IsSelected == true).ToList();
            //selectedInputs.ForEach(y => y.DiagramId = model.DiagramId);
            diagramManager.UpdateMethods(model.DiagramId, mapper.Map<List<DiagramMethodViewModel>, List<DiagramMethod>>(selectedMethods));
            return View("Methods", model);
        }

        [HttpPost("save-requirements")]
        public IActionResult SaveRequirements(DiagramRequirementPageViewModel model)
        {
            if (ModelState.IsValid)
            {
                ViewBag.UserMessage = "Requirements edited successfully";
            }
            else
            {
                return BadRequest("Invalid Model");
            }

            model.Requirements = model.Requirements == null ? new List<DiagramRequirementViewModel>() : model.Requirements;

            var selectedRequirements = model.Requirements.Where(x => x.IsSelected == true).ToList();
            //selectedInputs.ForEach(y => y.DiagramId = model.DiagramId);
            diagramManager.UpdateRequirements(model.DiagramId, mapper.Map<List<DiagramRequirementViewModel>, List<DiagramRequirement>>(selectedRequirements));
            return View("Requirements", model);
        }


    } 
}
