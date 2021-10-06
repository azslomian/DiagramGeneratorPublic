using AutoMapper;
using DiagramGenerator.DataAccess.Model;
using DiagramGenerator.Domain.Services.Interfaces;
using DiagramGenerator.Web.ViewModels;
using DiagramGenerator.Web.ViewModels.Pages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace DiagramGenerator.Web.Controllers
{
    [Authorize]
    public class ProcessController : Controller
    {
        private readonly ILogger<ProcessController> logger;
        private readonly IDiagramManager diagramManager;
        private readonly IProcessManager processManager;
        private readonly IMapper mapper;


        public ProcessController(ILogger<ProcessController> logger,
             IDiagramManager diagramManager,
             IProcessManager processManager,
             IMapper mapper)
        {
            this.logger = logger;
            this.diagramManager = diagramManager;
            this.processManager = processManager;
            this.mapper = mapper;
        }

        [HttpGet]
        public IActionResult Index(Guid diagramId)
        {
            var model = PrepareProcessPageView(diagramId);
            return View(model);
        }

        [HttpGet("get-all")]
        public ActionResult<IList<Process>> Get(bool includeItems = true)
        {
            return Ok(mapper.Map<IList<Process>, IList<ProcessViewModel>>(processManager.GetAll()));
        }

        [HttpGet("{id}")]
        public ActionResult<Input> Get(Guid id)
        {
            return Ok(mapper.Map<Process, ProcessViewModel>(processManager.GetById(id)));
        }

        [HttpPost("add-process")]
        public IActionResult AddProcess(ProcessViewModel model)
        {
            if (ModelState.IsValid)
            {
                ViewBag.UserMessage = "New Process created successfully";
                ModelState.Clear();
            }
            else
            {
                return BadRequest("Invalid Model");
            }

            diagramManager.AddProcess(mapper.Map<ProcessViewModel, Process>(model));
            var processPageModel = PrepareProcessPageView(model.DiagramId);
            return View("Index", processPageModel);
        }

        [HttpGet("add-process")]
        public IActionResult AddProcess(Guid diagramId)
        {
            return View(new ProcessViewModel() { DiagramId = diagramId });
        }

        [HttpPost("edit-process")]
        public IActionResult EditProcess(ProcessViewModel model)
        {
            if (ModelState.IsValid)
            {
                ViewBag.UserMessage = "Process edited successfully";
            }
            else
            {
                return BadRequest("Invalid Model");
            }

            processManager.Update(model.Id, mapper.Map<ProcessViewModel, Process>(model));
            return View(model);
        }

        [HttpGet("edit-process")]
        public IActionResult EditProcess(Guid id)
        {
            var process = mapper.Map<Process, ProcessViewModel>(processManager.GetById(id));
            return View(process);
        }

        [HttpGet("remove-process")]
        public IActionResult RemoveProcess(Guid id, Guid diagramId)
        {
            processManager.Delete(id);
            var model = PrepareProcessPageView(diagramId);
            return View("Index", model);
        }

        private ProcessPageViewModel PrepareProcessPageView(Guid diagramId)
        {
            ViewBag.PageTitle = "Process";
            var process = mapper.Map<Process, ProcessViewModel>(processManager.GetProcessByDiagramId(diagramId));
            var processDiagramId = diagramId != Guid.Empty ? diagramId : process.DiagramId;

            return new ProcessPageViewModel()
            {
                Process = process,
                DiagramId = processDiagramId
            };
        }
    } 
}
