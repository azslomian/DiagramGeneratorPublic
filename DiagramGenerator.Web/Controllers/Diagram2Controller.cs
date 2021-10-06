using DiagramGenerator.DataAccess.Model;
using DiagramGenerator.Domain.Repositories.Interfaces;
using DiagramGenerator.Domain.Services.Interfaces;
using DiagramGenerator.Web.ViewModels;
using DiagramGenerator.Web.ViewModels.Diagram;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using Microsoft.AspNetCore.Identity;
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
    public class Diagram2Controller : Controller
    {
        private readonly ILogger<Diagram2Controller> logger;
        private readonly IDiagramRepository diagramRepository;
        private readonly IOperationRepository operationRepository;
        private readonly UserManager<User> userManager;
        private readonly IWordExportManager wordExportManager;


        public Diagram2Controller(ILogger<Diagram2Controller> logger, 
            IDiagramRepository diagramRepository,
            IOperationRepository operationRepository,
            UserManager<User> userManager,
            IWordExportManager wordExportManager)
        {
            this.logger = logger;
            this.diagramRepository = diagramRepository;
            this.operationRepository = operationRepository;
            this.userManager = userManager;
            this.wordExportManager = wordExportManager;
        }

        [HttpGet]
        public ActionResult<List<Operation>> Get()
        {
            return Ok(operationRepository.GetAll());
        }

        [HttpGet("get-Diagrams-by-username")]
        public ActionResult<List<Diagram>> GetDiagramsByUsername()
        {
            return Ok(diagramRepository.GetDiagramsByUser(User.Identity.Name));
        }

        [HttpPost]
        public async Task<ActionResult<List<Diagram>>> GenerateDiagram(DiagramViewModel model)
        {
            var currentUser = await userManager.FindByNameAsync(User.Identity.Name);
            return Ok(diagramRepository.GetDiagramsByUser(User.Identity.Name));
        }

        [HttpGet("export-diagram")]
        public async Task<IActionResult> GenerateWord()
        {
            //var currentUser = await userManager.FindByNameAsync(User.Identity.Name);
            string userMail = User.FindFirst(ClaimTypes.Name)?.Value;
            var id = diagramRepository.GetAll("adelo@vp.pl").FirstOrDefault().Id;
            
            var diagram = diagramRepository.GetDiagramIncludeAll(id);

            using (MemoryStream mem = new MemoryStream())
            {
                using (WordprocessingDocument wordDoc = WordprocessingDocument.Create(mem, WordprocessingDocumentType.Document, true))
                {
                    wordExportManager.Export(diagram, wordDoc);
                }

                var fileName = $"{"Roman"}.docx";

                Response.Headers.Add("Access-Control-Expose-Headers", "Content-Disposition");

                return File(mem.ToArray(), "application/vnd.openxmlformats-officedocument.wordprocessingml.document", fileName);
            }
        }
    } 
}
