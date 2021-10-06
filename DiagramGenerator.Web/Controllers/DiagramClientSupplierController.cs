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
    public class DiagramClientSupplierController : Controller
    {
        private readonly ILogger<DiagramClientSupplierController> logger;
        private readonly IDiagramManager diagramManager;
        private readonly IOutputManager outputManager;
        private readonly ICriterionManager criterionManager;
        private readonly IProcessManager processManager;
        private readonly IClientManager clientManager;
        private readonly ISupplierManager supplierManager;
        private readonly IMapper mapper;


        public DiagramClientSupplierController(ILogger<DiagramClientSupplierController> logger,
             IDiagramManager diagramManager,
             IOutputManager outputManager,
             IProcessManager processManager,
             ICriterionManager criterionManager,
             IClientManager clientManager,
             ISupplierManager supplierManager,
             IMapper mapper)
        {
            this.logger = logger;
            this.diagramManager = diagramManager;
            this.outputManager = outputManager;
            this.processManager = processManager;
            this.criterionManager = criterionManager;
            this.clientManager = clientManager;
            this.supplierManager = supplierManager;
            this.mapper = mapper;
        }

        [HttpGet]
        public IActionResult Index(Guid diagramId)
        {
            string userMail = User.FindFirst(ClaimTypes.Name)?.Value;
            var diagram = diagramManager.GetByIdEager(diagramId);

            var supplierIds = diagram.Suppliers.Select(i => new { i.SupplierId }).ToList();
            var allSuppliers = mapper.Map<IList<Supplier>, IList<DiagramSupplierViewModel>>(supplierManager.GetAll(userMail)).ToList();

            foreach (var supplier in allSuppliers)
            {
                var existingSupplier = supplierIds.FirstOrDefault(x => x.SupplierId == supplier.Id);
                if (existingSupplier != null)
                {
                    supplier.IsSelected = true;
                }
            }

            var pageViewModel = new DiagramSuppliersPageViewModel()
            {
                DiagramId = diagramId,
                Suppliers = allSuppliers,
            };

            return View(pageViewModel);
        }

        [HttpGet]
        public IActionResult Clients(Guid diagramId)
        {
            string userMail = User.FindFirst(ClaimTypes.Name)?.Value;
            var diagram = diagramManager.GetByIdEager(diagramId);

            var clientIds = diagram.Clients.Select(i => new { i.ClientId }).ToList();
            var allClients = mapper.Map<IList<Client>, IList<DiagramClientViewModel>>(clientManager.GetAll(userMail)).ToList();

            foreach (var client in allClients)
            {
                var existingClient = clientIds.FirstOrDefault(x => x.ClientId == client.Id);
                if (existingClient != null)
                {
                    client.IsSelected = true;
                }
            }

            var pageViewModel = new DiagramClientsPageViewModel()
            {
                DiagramId = diagramId,
                Clients = allClients
            };

            return View(pageViewModel);
        }

        [HttpPost("save-suppliers")]
        public IActionResult SaveSuppliers(DiagramSuppliersPageViewModel model)
        {
            if (ModelState.IsValid)
            {
                ViewBag.UserMessage = "Suppliers edited successfully";
            }
            else
            {
                return BadRequest("Invalid Model");
            }

            var selectedSuppliers = model.Suppliers.Where(x => x.IsSelected == true).ToList();
            selectedSuppliers.ForEach(y => y.DiagramId = model.DiagramId);
            diagramManager.UpdateSuppliers(model.DiagramId, mapper.Map<List<DiagramSupplierViewModel>, List<DiagramSupplier>>(selectedSuppliers));
            return View("Index", model);
        }

        [HttpPost("save-clients")]
        public IActionResult SaveClients(DiagramClientsPageViewModel model)
        {
            if (ModelState.IsValid)
            {
                ViewBag.UserMessage = "Clients edited successfully";
            }
            else
            {
                return BadRequest("Invalid Model");
            }

            var selectedClients = model.Clients.Where(x => x.IsSelected == true).ToList();
            selectedClients.ForEach(y => y.DiagramId = model.DiagramId);
            diagramManager.UpdateClients(model.DiagramId, mapper.Map<List<DiagramClientViewModel>, List<DiagramClient>>(selectedClients));
            return View("Clients", model);
        }
    } 
}
