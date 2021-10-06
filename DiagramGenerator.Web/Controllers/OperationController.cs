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
    public class OperationController : Controller
    {
        private readonly ILogger<OperationController> logger;
        private readonly IDiagramManager diagramManager;
        private readonly IProcessManager processManager;
        private readonly IOperationManager operationManager;
        private readonly IMapper mapper;


        public OperationController(ILogger<OperationController> logger,
             IDiagramManager diagramManager,
             IOperationManager operationManager,
             IProcessManager processManager,
             IMapper mapper)
        {
            this.logger = logger;
            this.diagramManager = diagramManager;
            this.operationManager = operationManager;
            this.processManager = processManager;
            this.mapper = mapper;
        }

        //[HttpGet]
        //public ActionResult<IList<Operation>> Get(bool includeItems = true)
        //{
        //    return Ok(mapper.Map<IList<Operation>, IList<OperationViewModel>>(operationManager.GetAll()));
        //}

        //[HttpGet("{id}")]
        //public ActionResult<Input> Get(Guid id)
        //{
        //    return Ok(mapper.Map<Operation, OperationViewModel>(operationManager.GetById(id)));
        //}

        [HttpPost("add-operation")]
        public IActionResult AddOperation(OperationViewModel model)
        {
            if (ModelState.IsValid)
            {
                ViewBag.UserMessage = "New Operation created successfully";
                ModelState.Clear();
            }
            else
            {
                return BadRequest("Invalid Model");
            }

            processManager.AddOperation(mapper.Map<OperationViewModel, Operation>(model));
            return View(new OperationViewModel() { ProcessId = model.ProcessId, DiagramId = model.DiagramId });
        }

        [HttpGet("add-operation")]
        public IActionResult AddOperation(Guid processId)
        {
            var diagramId = processManager.GetById(processId).DiagramId;
            return View(new OperationViewModel() { ProcessId = processId, DiagramId = diagramId });
        }

        [HttpPost("edit-operation")]
        public IActionResult EditOperation(OperationViewModel model)
        {
            if (ModelState.IsValid)
            {
                ViewBag.UserMessage = "Operation edited successfully";
            }
            else
            {
                return BadRequest("Invalid Model");
            }

            operationManager.Update(model.Id, mapper.Map<OperationViewModel, Operation>(model));
            return View(model);
        }

        [HttpGet("edit-operation")]
        public IActionResult EditOperation(Guid id)
        {
            var operation = mapper.Map<Operation, OperationViewModel>(operationManager.GetByIdEager(id));
            return View(operation);
        }

        [HttpGet("remove-operation")]
        public IActionResult RemoveOperation(Guid id)
        {
            var operation = operationManager.GetById(id);
            processManager.RemoveOperation(operation);
            ViewBag.PageTitle = "Operation";
            var process = mapper.Map<Process, ProcessViewModel>(processManager.GetById(operation.ProcessId));

            var model = new ProcessPageViewModel()
            {
                Process = process,
                DiagramId = process.DiagramId
            };

            return RedirectToAction("Index", "Process", model);
        }
    }
}
