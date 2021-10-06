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
    public class DiagramOutputController : Controller
    {
        private readonly ILogger<DiagramOutputController> logger;
        private readonly IDiagramManager diagramManager;
        private readonly IOutputManager outputManager;
        private readonly ICriterionManager criterionManager;
        private readonly IProcessManager processManager;
        private readonly IMapper mapper;


        public DiagramOutputController(ILogger<DiagramOutputController> logger,
             IDiagramManager diagramManager,
             IOutputManager outputManager,
             IProcessManager processManager,
             ICriterionManager criterionManager,
             IMapper mapper)
        {
            this.logger = logger;
            this.diagramManager = diagramManager;
            this.outputManager = outputManager;
            this.processManager = processManager;
            this.criterionManager = criterionManager;
            this.mapper = mapper;
        }

        [HttpGet]
        public IActionResult Index(Guid diagramId)
        {
            string userMail = User.FindFirst(ClaimTypes.Name)?.Value;
            var diagram = diagramManager.GetByIdEager(diagramId);

            var outputIds = diagram.Outputs.Select(i => new { i.OutputId, i.Quantity } ).ToList();
            var allOutputs = mapper.Map<IList<Output>, IList<DiagramOutputViewModel>>(outputManager.GetAll(userMail)).ToList();
           
            foreach (var output in allOutputs)
            {
                var existingOutput = outputIds.FirstOrDefault(x => x.OutputId == output.Id);
                if (existingOutput != null)
                {
                    output.IsSelected = true;
                    output.Quantity = existingOutput.Quantity;
                }
            }

            var pageViewModel = new DiagramOutputPageViewModel()
            {
                DiagramId = diagramId,
                Outputs = allOutputs
            };

            return View(pageViewModel);
        }

        [HttpGet]
        public IActionResult Criteria(Guid diagramId)
        {
            string userMail = User.FindFirst(ClaimTypes.Name)?.Value;
            var diagram = diagramManager.GetByIdEager(diagramId);

            var criterionIds = diagram.Criteria.Select(i => new { i.CriterionId }).ToList();
            var allCriteria = mapper.Map<IList<Criterion>, IList<DiagramCriterionViewModel>>(criterionManager.GetAll(userMail)).ToList();

            foreach (var criterion in allCriteria)
            {
                var existingCriterion = criterionIds.FirstOrDefault(x => x.CriterionId == criterion.Id);
                if (existingCriterion != null)
                {
                    criterion.IsSelected = true;
                }
            }

            var pageViewModel = new DiagramCriteriumPageViewModel()
            {
                DiagramId = diagramId,
                Criteria = allCriteria
            };

            return View(pageViewModel);
        }

        [HttpPost("save-outputs")]
        public IActionResult SaveOutputs(DiagramOutputPageViewModel model)
        {
            if (ModelState.IsValid)
            {
                ViewBag.UserMessage = "Output edited successfully";
            }
            else
            {
                return BadRequest("Invalid Model");
            }

            model.Outputs = model.Outputs == null ? new List<DiagramOutputViewModel>() : model.Outputs;

            var selectedOutputs = model.Outputs.Where(x => x.IsSelected == true).ToList();
            selectedOutputs.ForEach(y => y.DiagramId = model.DiagramId);
            diagramManager.UpdateOutputs(model.DiagramId, mapper.Map<List<DiagramOutputViewModel>, List<DiagramOutput>>(selectedOutputs));
            return View("Index", model);
        }

        [HttpPost("save-criteria")]
        public IActionResult SaveCriteria(DiagramCriteriumPageViewModel model)
        {
            if (ModelState.IsValid)
            {
                ViewBag.UserMessage = "Criteria edited successfully";
            }
            else
            {
                return BadRequest("Invalid Model");
            }

            model.Criteria = model.Criteria == null ? new List<DiagramCriterionViewModel>() : model.Criteria;

            var selectedCriteria = model.Criteria.Where(x => x.IsSelected == true).ToList();
            selectedCriteria.ForEach(y => y.DiagramId = model.DiagramId);
            diagramManager.UpdateCriteria(model.DiagramId, mapper.Map<List<DiagramCriterionViewModel>, List<DiagramCriterion>>(selectedCriteria));
            return View("Criteria", model);
        }
    } 
}
