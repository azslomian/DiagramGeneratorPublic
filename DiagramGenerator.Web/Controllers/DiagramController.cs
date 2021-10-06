using AutoMapper;
using DiagramGenerator.DataAccess.Model;
using DiagramGenerator.Domain.Services.Interfaces;
using DiagramGenerator.Web.ViewModels;
using DiagramGenerator.Web.ViewModels.Diagram;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DiagramGenerator.Web.Controllers
{
    [Authorize]
    public class DiagramController : Controller
    {
        private readonly ILogger<DiagramController> logger;
        private readonly IDiagramManager diagramManager;
        private readonly IMapper mapper;


        public DiagramController(ILogger<DiagramController> logger,
             IDiagramManager diagramManager,
             IMapper mapper)
        {
            this.logger = logger;
            this.diagramManager = diagramManager;
            this.mapper = mapper;
        }

        [HttpGet]
        public IActionResult Index()
        {
            string userMail = User.FindFirst(ClaimTypes.Name)?.Value;
            ViewBag.PageTitle = "Diagrams";
            var diagram = mapper.Map<IList<Diagram>, IList<DiagramViewModel>>(diagramManager.GetAll(userMail));
            return View(diagram);
        }

        //[HttpGet("get-all")]
        //public ActionResult<IList<Diagram>> Get(bool includeItems = true)
        //{
        //    string userMail = User.FindFirst(ClaimTypes.Name)?.Value;
        //    return Ok(mapper.Map<IList<Diagram>, IList<DiagramViewModel>>(diagramManager.GetAll(userMail)));
        //}

        //[HttpGet("{id}")]
        //public ActionResult<Diagram> Get(Guid id)
        //{
        //    return Ok(mapper.Map<Diagram, DiagramViewModel>(diagramManager.GetById(id)));
        //}

        [HttpPost("add-diagram")]
        public IActionResult AddDiagram(DiagramViewModel model)
        {
            string userMail = User.FindFirst(ClaimTypes.Name)?.Value;
            model.UserEmail = userMail;
            if (ModelState.IsValid)
            {
                ViewBag.UserMessage = "New Diagram created successfully";
                ModelState.Clear();
            }
            else
            {
                return BadRequest("Invalid Model");
            }

            diagramManager.Create(mapper.Map<DiagramViewModel, Diagram>(model));
            return View();
        }

        [HttpGet("add-diagram")]
        public IActionResult AddDiagram()
        {
            return View();
        }

        [HttpPost("edit-diagram")]
        public IActionResult EditDiagram(DiagramViewModel model)
        {
            string userMail = User.FindFirst(ClaimTypes.Name)?.Value;
            model.UserEmail = userMail;
            if (ModelState.IsValid)
            {
                ViewBag.UserMessage = "Diagram edited successfully";
            }
            else
            {
                return BadRequest("Invalid Model");
            }

            diagramManager.Update(model.Id, mapper.Map<DiagramViewModel, Diagram>(model));
            return View(model);
        }

        [HttpGet("edit-diagram")]
        public IActionResult EditDiagram(Guid id)
        {
            var diagram = mapper.Map<Diagram, DiagramViewModel>(diagramManager.GetById(id));
            return View(diagram);
        }

        [HttpGet("remove-diagram")]
        public IActionResult RemoveDiagram(Guid id)
        {
            string userMail = User.FindFirst(ClaimTypes.Name)?.Value;
            diagramManager.Delete(id);
            ViewBag.PageTitle = "Diagram";
            var Diagram = mapper.Map<IList<Diagram>, IList<DiagramViewModel>>(diagramManager.GetAll(userMail));
            return View("Index", Diagram);
        }

    }
}
