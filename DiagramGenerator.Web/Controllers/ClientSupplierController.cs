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
    public class ClientSupplierController : Controller
    {
        private readonly IDiagramManager diagramManager;
        private readonly IClientManager clientManager;
        private readonly ISupplierManager supplierManager;
        private readonly IMapper mapper;


        public ClientSupplierController(
             IDiagramManager diagramManager,
             IClientManager clientManager,
             ISupplierManager supplierManager,
             IMapper mapper)
        {
            this.diagramManager = diagramManager;
            this.clientManager = clientManager;
            this.supplierManager = supplierManager;
            this.mapper = mapper;
        }

        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.PageTitle = "Clients and Suppliers";
            string userMail = User.FindFirst(ClaimTypes.Name)?.Value;

            var model = new ClientSupplierPageViewModel()
            {
                Clients = mapper.Map<IList<Client>, IList<ClientViewModel>>(clientManager.GetAll(userMail)),
                Suppliers = mapper.Map<IList<Supplier>, IList<SupplierViewModel>>(supplierManager.GetAll(userMail)),
            };

            return View(model);
        }

        //[HttpGet("clients/{id}")]
        //public ActionResult<Input> Get(Guid id)
        //{
        //    return Ok(mapper.Map<Client, ClientViewModel>(clientManager.GetById(id)));
        //}

        [HttpPost("add-client")]
        public IActionResult AddClient(ClientViewModel model)
        {
            string userMail = User.FindFirst(ClaimTypes.Name)?.Value;
            model.UserEmail = userMail;
            if (ModelState.IsValid)
            {
                ViewBag.UserMessage = "New Client created successfully";
                ModelState.Clear();
            }
            else
            {
                return BadRequest("Invalid Model");
            }

            clientManager.Create(mapper.Map<ClientViewModel, Client>(model));
            return View(model);
        }

        [HttpGet("add-client")]
        public IActionResult AddClient()
        {
            return View(new ClientViewModel());
        }

        [HttpPost("edit-client")]
        public IActionResult EditClient(ClientViewModel model)
        {
            string userMail = User.FindFirst(ClaimTypes.Name)?.Value;
            model.UserEmail = userMail;
            if (ModelState.IsValid)
            {
                ViewBag.UserMessage = "Client edited successfully";
            }
            else
            {
                return BadRequest("Invalid Model");
            }

            clientManager.Update(model.Id, mapper.Map<ClientViewModel, Client>(model));
            return View(model);
        }

        [HttpGet("edit-client")]
        public IActionResult EditClient(Guid id)
        {
            var client = mapper.Map<Client, ClientViewModel>(clientManager.GetById(id));
            return View(client);
        }

        [HttpGet("remove-client")]
        public IActionResult RemoveClient(Guid id)
        {
            clientManager.Delete(id);
            ViewBag.PageTitle = "Clients";
            string userMail = User.FindFirst(ClaimTypes.Name)?.Value;

            var model = new ClientSupplierPageViewModel()
            {
                Clients = mapper.Map<IList<Client>, IList<ClientViewModel>>(clientManager.GetAll(userMail)),
                Suppliers = mapper.Map<IList<Supplier>, IList<SupplierViewModel>>(supplierManager.GetAll(userMail)),
            };

            return View("Index", model);
        }

        [HttpPost("add-supplier")]
        public IActionResult AddSupplier(SupplierViewModel model)
        {
            string userMail = User.FindFirst(ClaimTypes.Name)?.Value;
            model.UserEmail = userMail;
            if (ModelState.IsValid)
            {
                ViewBag.UserMessage = "New Supplier created successfully";
                ModelState.Clear();
            }
            else
            {
                return BadRequest("Invalid Model");
            }

            supplierManager.Create(mapper.Map<SupplierViewModel, Supplier>(model));
            return View(model);
        }

        [HttpGet("add-supplier")]
        public IActionResult AddSupplier()
        {
            return View();
        }

        [HttpPost("edit-supplier")]
        public IActionResult EditSupplier(SupplierViewModel model)
        {
            string userMail = User.FindFirst(ClaimTypes.Name)?.Value;
            model.UserEmail = userMail;
            if (ModelState.IsValid)
            {
                ViewBag.UserMessage = "Supplier edited successfully";
            }
            else
            {
                return BadRequest("Invalid Model");
            }

            supplierManager.Update(model.Id, mapper.Map<SupplierViewModel, Supplier>(model));
            return View(model);
        }

        [HttpGet("edit-supplier")]
        public IActionResult EditSupplier(Guid id)
        {
            var supplier = mapper.Map<Supplier, SupplierViewModel>(supplierManager.GetById(id));
            return View(supplier);
        }

        [HttpGet("remove-supplier")]
        public IActionResult RemoveSupplier(Guid id)
        {
            supplierManager.Delete(id);
            string userMail = User.FindFirst(ClaimTypes.Name)?.Value;

            var model = new ClientSupplierPageViewModel()
            {
                Clients = mapper.Map<IList<Client>, IList<ClientViewModel>>(clientManager.GetAll(userMail)),
                Suppliers = mapper.Map<IList<Supplier>, IList<SupplierViewModel>>(supplierManager.GetAll(userMail)),
            };

            return View("Index", model);
        }
    } 
}
