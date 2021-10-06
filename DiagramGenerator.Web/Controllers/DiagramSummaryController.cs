using AutoMapper;
using DiagramGenerator.DataAccess.Model;
using DiagramGenerator.DataAccess.Model.ManyToMany;
using DiagramGenerator.Domain.Services.Interfaces;
using DiagramGenerator.Web.ViewModels;
using DiagramGenerator.Web.ViewModels.Helpers;
using DiagramGenerator.Web.ViewModels.Pages;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DiagramGenerator.Web.Controllers
{
    [Authorize]
    public class DiagramSummaryController : Controller
    {
        private readonly IDiagramManager diagramManager;
        private readonly IWordExportManager wordExportManager;
        private readonly IMapper mapper;
        private readonly IPdfExportManager pdfExportManager;

        public DiagramSummaryController(
             IDiagramManager diagramManager,
             IWordExportManager wordExportManager,
             IPdfExportManager pdfExportManager,
             IMapper mapper)
        {
            this.diagramManager = diagramManager;
            this.wordExportManager = wordExportManager;
            this.pdfExportManager = pdfExportManager;
            this.mapper = mapper;
        }

        [HttpGet]
        public IActionResult Index(Guid diagramId)
        {
            var diagram = mapper.Map<Diagram, DiagramSummaryPageViewModel>(diagramManager.GetDiagramIncludeAll(diagramId));
            diagram.Validation = Validation(diagram);
            return View(diagram);
        }

        [HttpPost("generate-diagram")]
        public IActionResult GenerateDiagram(Guid diagramId)
        {
            var diagram = mapper.Map<Diagram, DiagramSummaryPageViewModel>(diagramManager.GetDiagramIncludeAll(diagramId));
            return View("Index", diagramId);
        }

        [HttpGet("generate-word")]
        public IActionResult GenerateWord(Guid diagramId)
        {
            var diagram = diagramManager.GetDiagramIncludeAll(diagramId);

            using (MemoryStream mem = new MemoryStream())
            {
                using (WordprocessingDocument wordDoc = WordprocessingDocument.Create(mem, WordprocessingDocumentType.Document, true))
                {
                    wordExportManager.Export(diagram, wordDoc);
                }

                var fileName = $"{diagram.Name}.docx";

                Response.Headers.Add("Access-Control-Expose-Headers", "Content-Disposition");

                return File(mem.ToArray(), "application/vnd.openxmlformats-officedocument.wordprocessingml.document", fileName);
            }
        }

        [HttpGet("generate-pdf")]
        public IActionResult GeneratePdf(Guid diagramId)
        {
            var diagram = diagramManager.GetDiagramIncludeAll(diagramId);
            var document = pdfExportManager.Export(diagram);
            //Saving the PDF to the MemoryStream
            MemoryStream stream = new MemoryStream();
            document.Save(stream);

            //Set the position as '0'
            stream.Position = 0;

            //Download the PDF document in the browser
            FileStreamResult fileStreamResult = new FileStreamResult(stream, "application/pdf");
            fileStreamResult.FileDownloadName = $"{diagram.Name}.pdf";
            return fileStreamResult;
        }

        private List<ValidationViewModel> Validation(DiagramSummaryPageViewModel model)
        {
            var validation = new List<ValidationViewModel>();

            if (model.Process != null)
            {
                var sum = model.Process.Operations.Sum(x => x.TimeInMinutes);
                var storageOperations = model.Process.Operations.Where(x => x.Type == DataAccess.Model.Enum.OperationType.Storage).Sum(x => x.TimeInMinutes);
                var manufacturingOperations = model.Process.Operations.Where(x => x.Type == DataAccess.Model.Enum.OperationType.Manufacturing).Sum(x => x.TimeInMinutes);
                var controlOperations = model.Process.Operations.Where(x => x.Type == DataAccess.Model.Enum.OperationType.Control).Sum(x => x.TimeInMinutes);
                var transportOperations = model.Process.Operations.Where(x => x.Type == DataAccess.Model.Enum.OperationType.Transport).Sum(x => x.TimeInMinutes);

                validation.Add(new ValidationViewModel() { Text = "Process duration: " + sum });
                validation.Add(new ValidationViewModel() { Text = "Time need for Storage: " + storageOperations });
                validation.Add(new ValidationViewModel() { Text = "Time need for Manufacturing: " + manufacturingOperations });
                validation.Add(new ValidationViewModel() { Text = "Time need for Control: " + controlOperations });
                validation.Add(new ValidationViewModel() { Text = "Time need for Transport: " + transportOperations });
            }
            
            //var humanResources = model.Inputs.Where(y => y.Type == DataAccess.Model.Enum.InputType.HumanResource);
            //var employees = humanResources.Sum(x => x.Quantity);
            //var operationEmployees = model.Process.Operations.Sum(x => x.Employees);
            //if (operationEmployees > employees)
            //{
            //    validation.Add(new ValidationViewModel() { Text = "You have not enough employees: " });
            //    validation.Add(new ValidationViewModel() { Text = "Human Resources: " + employees + " but for operations are needed: " + operationEmployees });
            //} 
            //else if (operationEmployees < employees)
            //{
            //    validation.Add(new ValidationViewModel() { Text = "You have too much employees: " });
            //    validation.Add(new ValidationViewModel() { Text = "Human Resources: " + employees + " but for operations are needed: " + operationEmployees });
            //}

            return validation;
        }
    } 
}
