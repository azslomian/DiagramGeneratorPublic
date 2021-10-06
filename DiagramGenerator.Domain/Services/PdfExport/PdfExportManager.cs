using DiagramGenerator.DataAccess.Model.Enum;
using DiagramGenerator.Domain.Services.Interfaces;
using Syncfusion.Drawing;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace DiagramGenerator.Domain.Services
{
    public class PdfExportManager : IPdfExportManager
    {

        public PdfExportManager()
        {
        }

        public PdfDocument Export(DataAccess.Model.Diagram diagram)
        {
            PdfDocument document = new PdfDocument();
            document.PageSettings.Orientation = PdfPageOrientation.Landscape;

            //Add a new page to the document
            PdfPage page = document.Pages.Add();
            page.Rotation = PdfPageRotateAngle.RotateAngle90;


            //Initialize graphics for the page
            PdfGraphics graphics = page.Graphics;
            var moveDown = -20;
            var moveRight = 20;
            var processShapeCenter = 310 + moveRight + 120 / 2;
            var path = Environment.CurrentDirectory;
            Stream fontStream = File.Exists("Font/arial.ttf") ? File.OpenRead("Font/arial.ttf") : null;
            //Assembly assembly = typeof(PdfExportManager).GetTypeInfo().Assembly;
            //Stream fontStream = assembly.GetManifestResourceStream("PdfUnicodeFontSample.Assets.arial.ttf");
            var defaultTrueFont = new PdfTrueTypeFont(fontStream, 9);
            var defaultFont = new PdfStandardFont(PdfFontFamily.Helvetica, 9);
            var defaultTextColor = PdfBrushes.DarkBlue;
            var defaultTitleColor = PdfBrushes.DarkBlue;
            var defaultShapeColor = PdfPens.DarkBlue;
            var defaultArrowColor = PdfPens.DarkOliveGreen;

            graphics.DrawString(diagram.Name, new PdfStandardFont(PdfFontFamily.TimesRoman, 16), PdfBrushes.DarkBlue, new PointF(processShapeCenter, 30 + moveDown), new PdfStringFormat() { Alignment = PdfTextAlignment.Center, WordWrap = PdfWordWrapType.Word });
            graphics.DrawString(diagram.Description, new PdfStandardFont(PdfFontFamily.TimesRoman, 9), PdfBrushes.DarkBlue, new PointF(processShapeCenter, 46 + moveDown), new PdfStringFormat() { Alignment = PdfTextAlignment.Center, WordWrap = PdfWordWrapType.Word });


            var suppliersData = diagram.Suppliers.OrderBy(z => z.Supplier.Lp).Select(x => x.Supplier.Lp + ". "  + x.Supplier.Name).ToList();
            var humanResourcesData = diagram.Inputs.OrderBy(z => z.Input.Lp).Where(x => x.Input.Type == InputType.HumanResource).Select(x => x.Input.Lp + ". " + x.Input.Name + ": " + x.Quantity).ToList();
            var materialsData = diagram.Inputs.OrderBy(z => z.Input.Lp).Where(x => x.Input.Type == InputType.Material).Select(x => x.Input.Lp + ". " + x.Input.Name + ": " + x.Quantity).ToList();
            var equipmentData = diagram.Inputs.OrderBy(z => z.Input.Lp).Where(x => x.Input.Type == InputType.Equipment).Select(x => x.Input.Lp + ". " + x.Input.Name + ": " + x.Quantity).ToList();
            var inputDocumentsData = diagram.Inputs.OrderBy(z => z.Input.Lp).Where(x => x.Input.Type == InputType.Document).Select(x => x.Input.Lp + ". " + x.Input.Name + ": " + x.Quantity).ToList();
            var otherInputsData = diagram.Inputs.OrderBy(z => z.Input.Lp).Where(x => x.Input.Type == InputType.Other).Select(x => x.Input.Lp + ". " + x.Input.Name + ": " + x.Quantity).ToList();
            var methodsData = diagram.Methods.OrderBy(z => z.Method.Lp).Select(x => x.Method.Lp + ". " + x.Method.Name).ToList();
            var requirementsData = diagram.Requirements.OrderBy(z => z.Requirement.Lp).Select(x => x.Requirement.Lp + ". " + x.Requirement.Name).ToList();
            var operationsData = diagram.Process.Operations.OrderBy(z => z.Lp).Select(x => $"{x.Lp}. {x.Name} ({x.Type}): Employees: {x.Employees} Duration: {x.TimeInMinutes}").ToList();
            var outputsData = diagram.Outputs.OrderBy(z => z.Output.Lp).Where(x => x.Output.Type == OutputType.Other).Select(x => x.Output.Lp + ". " + x.Output.Name + ": " + x.Quantity).ToList();
            var outputDocumentsData = diagram.Outputs.OrderBy(z => z.Output.Lp).Where(x => x.Output.Type == OutputType.Document).Select(x => x.Output.Lp + ". " + x.Output.Name + ": " + x.Quantity).ToList();
            var productsData = diagram.Outputs.OrderBy(z => z.Output.Lp).Where(x => x.Output.Type == OutputType.Product).Select(x => x.Output.Lp + ". " + x.Output.Name + ": " + x.Quantity).ToList();
            var criteriaData = diagram.Criteria.OrderBy(z => z.Criterion.Lp).Select(x => x.Criterion.Lp + ". " + x.Criterion.Name).ToList();
            var clientsData = diagram.Clients.OrderBy(z => z.Client.Lp).Select(x => x.Client.Lp + ". " + x.Client.Name).ToList();
            var processTitle = "Process" + Environment.NewLine + diagram.Process.Name;

            RectangleF suppliersRect = new RectangleF(10 + moveRight, 200 + moveDown, 120, 80);

            RectangleF humanResourcesRect = new RectangleF(160 + moveRight, 40 + moveDown, 120, 80);
            RectangleF equipmentRect = new RectangleF(160 + moveRight, 140 + moveDown, 120, 80);
            RectangleF materialsRect = new RectangleF(160 + moveRight, 240 + moveDown, 120, 80);
            RectangleF methodsRect = new RectangleF(160 + moveRight, 340 + moveDown, 120, 80);
            RectangleF requirementsRect = new RectangleF(160 + moveRight, 440 + moveDown, 120, 80);

            RectangleF processRect = new RectangleF(310 + moveRight, 120 + moveDown, 120, 300);

            RectangleF criteriaRect = new RectangleF(460 + moveRight, 40 + moveDown, 120, 80);
            RectangleF productsRect = new RectangleF(460 + moveRight, 240 + moveDown, 120, 80);
            RectangleF outputsRect = new RectangleF(460 + moveRight, 440 + moveDown, 120, 80);

            RectangleF clientsRect = new RectangleF(610 + moveRight, 240 + moveDown, 120, 80);


            var suppliers = AddRectangle(graphics, suppliersRect, defaultTitleColor, "Suppliers", defaultTrueFont, defaultTextColor, suppliersData);
            var humanResources = AddRectangle(graphics, humanResourcesRect, defaultTitleColor, "Human Resources", defaultTrueFont, defaultTextColor, humanResourcesData);
            var equipment = AddRectangle(graphics, equipmentRect, defaultTitleColor, "Equipment", defaultTrueFont, defaultTextColor, equipmentData);
            var inputs = AddRectangle(graphics, materialsRect, defaultTitleColor, "Materials", defaultTrueFont, defaultTextColor, materialsData);
            var methods = AddRectangle(graphics, methodsRect, defaultTitleColor, "Methods", defaultTrueFont, defaultTextColor, methodsData);
            var requirements = AddRectangle(graphics, requirementsRect, defaultTitleColor, "Requirements", defaultTrueFont, defaultTextColor, requirementsData);
            var process = AddRectangle(graphics, processRect, defaultTitleColor, processTitle, defaultTrueFont, defaultTextColor, operationsData);
            var criteria = AddRectangle(graphics, criteriaRect, defaultTitleColor, "Criteria", defaultTrueFont, defaultTextColor, criteriaData);
            var products = AddRectangle(graphics, productsRect, defaultTitleColor, "Products", defaultTrueFont, defaultTextColor, productsData);
            var outputs = AddRectangle(graphics, outputsRect, defaultTitleColor, "Outputs", defaultTrueFont, defaultTextColor, outputsData);
            var clients = AddRectangle(graphics, clientsRect, defaultTitleColor, "Clients", defaultTrueFont, defaultTextColor, clientsData);

            //var arrow = AddArrow(graphics, new PointF(160, 70), new PointF(440, 70));
            var suppliersHumanResourcesArrow = AddArrowBetween(graphics, suppliersRect, humanResourcesRect);
            var suppliersEquipmentArrow = AddArrowBetween(graphics, suppliersRect, equipmentRect);
            var suppliersInputsArrow = AddArrowBetween(graphics, suppliersRect, materialsRect);
            var suppliersMethodsArrow = AddArrowBetween(graphics, suppliersRect, methodsRect);
            var suppliersRequirementsArrow = AddArrowBetween(graphics, suppliersRect, requirementsRect);
            var HRsProcessArrow = AddArrowBetween(graphics, humanResourcesRect, processRect);
            var equipmentProcessArrow = AddArrowBetween(graphics, equipmentRect, processRect);
            var inputsProcessArrow = AddArrowBetween(graphics, materialsRect, processRect);
            var methodsProcessArrow = AddArrowBetween(graphics, methodsRect, processRect);
            var requirementsProcessArrow = AddArrowBetween(graphics, requirementsRect, processRect);
            var processCriteriaArrow = AddArrowBetween(graphics, processRect, criteriaRect);
            var processOutputsArrow = AddArrowBetween(graphics, processRect, outputsRect);
            var processProductsArrow = AddArrowBetween(graphics, processRect, productsRect);
            var productsClientsArrow = AddArrowBetween(graphics, productsRect, clientsRect);

            //Draw the PDF path
            graphics.DrawPath(defaultShapeColor, suppliers);
            graphics.DrawPath(defaultShapeColor, outputs);
            graphics.DrawPath(defaultShapeColor, humanResources);
            graphics.DrawPath(defaultShapeColor, equipment);
            graphics.DrawPath(defaultShapeColor, inputs);
            graphics.DrawPath(defaultShapeColor, methods);
            graphics.DrawPath(defaultShapeColor, requirements);
            graphics.DrawPath(defaultShapeColor, process);
            graphics.DrawPath(defaultShapeColor, criteria);
            graphics.DrawPath(defaultShapeColor, products);
            graphics.DrawPath(defaultShapeColor, outputs);
            graphics.DrawPath(defaultShapeColor, clients);

            graphics.DrawPath(defaultArrowColor, suppliersHumanResourcesArrow);
            graphics.DrawPath(defaultArrowColor, suppliersEquipmentArrow);
            graphics.DrawPath(defaultArrowColor, suppliersInputsArrow);
            graphics.DrawPath(defaultArrowColor, suppliersMethodsArrow);
            graphics.DrawPath(defaultArrowColor, suppliersRequirementsArrow);
            graphics.DrawPath(defaultArrowColor, HRsProcessArrow);
            graphics.DrawPath(defaultArrowColor, equipmentProcessArrow);
            graphics.DrawPath(defaultArrowColor, inputsProcessArrow);
            graphics.DrawPath(defaultArrowColor, methodsProcessArrow);
            graphics.DrawPath(defaultArrowColor, requirementsProcessArrow);
            graphics.DrawPath(defaultArrowColor, processCriteriaArrow);
            graphics.DrawPath(defaultArrowColor, processOutputsArrow);
            graphics.DrawPath(defaultArrowColor, processProductsArrow);
            graphics.DrawPath(defaultArrowColor, productsClientsArrow);

            return document;
        }

        private PdfPath AddRectangle(PdfGraphics graphics, RectangleF rect, PdfBrush titleColor, string title, PdfTrueTypeFont font, PdfBrush textColor, List<string> data)
        {
            int dimension = 10;

            //Create the PdfPath
            PdfPath pdfPath = new PdfPath();

            //Draw the rounded rectangle as separate 4 arcs with same dimensions of width and height
            pdfPath.AddArc(rect.X, rect.Y, dimension, dimension, 180, 90);
            pdfPath.AddArc(rect.X + rect.Width - dimension, rect.Y, dimension, dimension, 270, 90);
            pdfPath.AddArc(rect.X + rect.Width - dimension, rect.Y + rect.Height - dimension, dimension, dimension, 0, 90);
            pdfPath.AddArc(rect.X, rect.Y + rect.Height - dimension, dimension, dimension, 90, 90);

            StringBuilder dataText = new StringBuilder();
            data.ForEach(x => dataText.AppendLine("  " + x));

            graphics.DrawString(title, font, titleColor, rect, new PdfStringFormat() { Alignment = PdfTextAlignment.Center, LineAlignment = PdfVerticalAlignment.Top, WordWrap = PdfWordWrapType.Word });
            graphics.DrawString(dataText.ToString(), font, textColor, rect, new PdfStringFormat() { Alignment = PdfTextAlignment.Left, LineAlignment = PdfVerticalAlignment.Middle, WordWrap = PdfWordWrapType.Word });
            //graphics.DrawString("Welcome to Syncfusion", font, textColor, new PointF(0, 0));

            //Close the path
            pdfPath.CloseFigure();
            return pdfPath;
        }

        private PdfPath AddArrow(PdfGraphics graphics, PointF arrowStart, PointF arrowEnd)
        {
            const int arrowHeadSize = 10;

            //// dx,dy = arrow line vector
            var dx = arrowEnd.X - arrowStart.X;
            var dy = arrowEnd.Y - arrowStart.Y;

            //// normalize
            var length = Math.Sqrt(dx * dx + dy * dy);
            var unitDx = (dx / length) * 0.4;
            var unitDy = (dy / length) * 0.4;

            var arrowRightPoint = new Point(
                Convert.ToInt32(arrowEnd.X - unitDx * arrowHeadSize - unitDy * arrowHeadSize),
                Convert.ToInt32(arrowEnd.Y - unitDy * arrowHeadSize + unitDx * arrowHeadSize));
            var arrowLeftPoint = new Point(
                Convert.ToInt32(arrowEnd.X - unitDx * arrowHeadSize + unitDy * arrowHeadSize),
                Convert.ToInt32(arrowEnd.Y - unitDy * arrowHeadSize - unitDx * arrowHeadSize));

            //Create the PdfPath
            PdfPath pdfPath = new PdfPath();

            //Draw the rounded rectangle as separate 4 arcs with same dimensions of width and height
            pdfPath.AddLine(arrowStart, arrowEnd);
            pdfPath.AddLine(arrowEnd, arrowRightPoint);
            pdfPath.AddLine(arrowEnd, arrowLeftPoint);
            pdfPath.AddLine(arrowLeftPoint, arrowEnd);

            //Close the path
            pdfPath.CloseFigure();
            return pdfPath;
        }

        private PdfPath AddArrowBetween(PdfGraphics graphics, RectangleF rectangleStart, RectangleF rectangleEnd)
        {
            var rect1ConnectionPoint = new PointF(rectangleStart.Right, (rectangleStart.Top + rectangleStart.Bottom) / 2);
            var rect2ConnectionPoint = new PointF(rectangleEnd.Left, (rectangleEnd.Top + rectangleEnd.Bottom) / 2);

            return AddArrow(graphics, rect1ConnectionPoint, rect2ConnectionPoint);
        }
    }
}
