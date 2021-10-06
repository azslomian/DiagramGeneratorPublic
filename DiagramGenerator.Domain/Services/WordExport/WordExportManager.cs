using System.Collections.Generic;
using DiagramGenerator.Domain.Services.Interfaces;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

using Wp = DocumentFormat.OpenXml.Drawing.Wordprocessing;
using A = DocumentFormat.OpenXml.Drawing;
using Wps = DocumentFormat.OpenXml.Office2010.Word.DrawingShape;
using V = DocumentFormat.OpenXml.Vml;
using Wvml = DocumentFormat.OpenXml.Vml.Wordprocessing;
using A14 = DocumentFormat.OpenXml.Office2010.Drawing;
using Wp14 = DocumentFormat.OpenXml.Office2010.Word.Drawing;
using Ovml = DocumentFormat.OpenXml.Vml.Office;
using System;
using System.Linq;
using DocumentFormat.OpenXml.Vml.Office;
using DocumentFormat.OpenXml.Office2010.Word.DrawingShape;
using DocumentFormat.OpenXml.Vml;
using DiagramGenerator.Domain.Dtos.WordExport;
using System.Text.RegularExpressions;

namespace DiagramGenerator.Domain.Services
{
    public class WordExportManager : IWordExportManager
    {
        private readonly Random random;
        private IEnumerable<WordprocessingShape> shapes2;

        public WordExportManager()
        {
            random = new Random();
        }

        public void Export(DataAccess.Model.Diagram diagram, WordprocessingDocument wordDoc)
        {
            //IEnumerable<WordprocessingShape> shapes2 = wordDoc.MainDocumentPart.Document.Body.Descendants<WordprocessingShape>();
            wordDoc.AddMainDocumentPart();

            Document doc = new Document();
            
            Body body = new Body();
            PageSize pageSize = new PageSize() { Orient = PageOrientationValues.Landscape };

            var suppliers = diagram.Suppliers.Select(x => new WordEntityDto() { Name = x.Supplier.Name, Description = x.Supplier.Description, Lp = x.Supplier.Lp }).OrderBy(z => z.Lp).ToList();
            var inputs = diagram.Inputs.Select(x => new WordEntityQuantityAndTypeDto() { Name = x.Input.Name, Description = x.Input.Description, Lp = x.Input.Lp, Quantity = x.Quantity, Type = EnumHelper<DiagramGenerator.DataAccess.Model.Enum.InputType>.GetDisplayValue(x.Input.Type)  }).OrderBy(z => z.Lp).ToList();
            var methods = diagram.Methods.Select(x => new WordEntityDto() { Name = x.Method.Name, Description = x.Method.Description, Lp = x.Method.Lp}).OrderBy(z => z.Lp).ToList();
            var requirements = diagram.Requirements.Select(x => new WordEntityDto() { Name = x.Requirement.Name, Description = x.Requirement.Description, Lp = x.Requirement.Lp }).OrderBy(z => z.Lp).ToList();
            var outputs = diagram.Outputs.Select(x => new WordEntityQuantityAndTypeDto() { Name = x.Output.Name, Description = x.Output.Description, Lp = x.Output.Lp, Quantity = x.Quantity, Type = EnumHelper<DiagramGenerator.DataAccess.Model.Enum.OutputType>.GetDisplayValue(x.Output.Type) }).OrderBy(z => z.Lp).ToList();
            var criteria = diagram.Criteria.Select(x => new WordEntityDto() { Name = x.Criterion.Name, Description = x.Criterion.Description, Lp = x.Criterion.Lp }).OrderBy(z => z.Lp).ToList();
            var clients = diagram.Clients.Select(x => new WordEntityDto() { Name = x.Client.Name, Description = x.Client.Description, Lp = x.Client.Lp }).OrderBy(z => z.Lp).ToList();
            var process = new WordEntityDto() { Name = diagram.Process.Name, Description = diagram.Process.Description, Lp = diagram.Process.Lp };

            var operations = diagram.Process.Operations.Select(x => new WordOperationDto() 
            { 
                Name = x.Name, 
                Description = x.Description, 
                Lp = x.Lp,
                Employees = x.Employees,
                TimeInMinutes = x.TimeInMinutes,
                Type = EnumHelper<DataAccess.Model.Enum.OperationType>.GetDisplayValue(x.Type)
            }).OrderBy(z => z.Lp).ToList();

            var processTitlePara = FeatureTitleParagraph("Process");
            var operationsTitlePara = FeatureTitleParagraph("Operations");
            var suppliersTitlePara = FeatureTitleParagraph("Suppliers");
            var inputsTitlePara = FeatureTitleParagraph("Inputs");
            var methodsTitlePara = FeatureTitleParagraph("Methods");
            var requirementsTitlePara = FeatureTitleParagraph("Requirements");
            var outputsTitlePara = FeatureTitleParagraph("Outputs");
            var criteriaTitlePara = FeatureTitleParagraph("Criteria");
            var clientsTitlePara = FeatureTitleParagraph("Clients");

            var processPara = FeatureParagraph(new List<WordEntityDto>() { process } );
            var operationPara = OperationFeatureParagraph(operations);
            var suppliersPara = FeatureParagraph(suppliers);
            var inputsPara = QuantityTypeFeatureParagraph(inputs);
            var methodsPara = FeatureParagraph(methods);
            var requirementsPara = FeatureParagraph(requirements);
            var outputsPara = QuantityTypeFeatureParagraph(outputs);
            var criteriaPara = FeatureParagraph(criteria);
            var clientsPara = FeatureParagraph(clients);

            var header = Header(diagram.Name, diagram.Description);
            doc.Append(header);

            var rectangle = AddRectangle();
            body.Append(rectangle);

            body.Append(processTitlePara);
            body.Append(processPara);
            body.Append(operationsTitlePara);
            body.Append(operationPara);
            body.Append(suppliersTitlePara);
            body.Append(suppliersPara);
            body.Append(inputsTitlePara);
            body.Append(inputsPara);
            body.Append(methodsTitlePara);
            body.Append(methodsPara);
            body.Append(requirementsTitlePara);
            body.Append(requirementsPara);
            body.Append(outputsTitlePara);
            body.Append(outputsPara);
            body.Append(criteriaTitlePara);
            body.Append(criteriaPara);
            body.Append(clientsTitlePara);
            body.Append(clientsPara);


            doc.Append(body);

            wordDoc.MainDocumentPart.Document = doc;

            wordDoc.Close();
        }

        private Paragraph Header(string diagramTitle, string diagramDescription)
        {
            Paragraph para = new Paragraph();

            var headerProperties = HeaderProperties();

            Run run = new Run();
            RunProperties titleProperties = new RunProperties();

            Text title = new Text() { Text = diagramTitle };
            Text description = new Text() { Text = diagramDescription };

            run.Append(titleProperties);
            run.Append(title);
            run.Append(new Break());
            run.Append(description);

            run.AppendChild(new Break());
            run.AppendChild(new Break());

            para.Append(headerProperties);
            para.Append(run);

            return para;
        }

        private Paragraph FeatureParagraph(List<WordEntityDto> entities)
        {
            Paragraph para = new Paragraph();

            var featureProperties = FeatureProperties();

            Run run = new Run();
            RunProperties captureTitleProperties = new RunProperties();

            foreach (var entity in entities)
            {
                run.AppendChild(new Text($"{entity.Lp}. {entity.Name} - Description: {entity.Description}"));
                run.AppendChild(new Break());
                run.AppendChild(new Break());
            }

            para.Append(featureProperties);
            para.Append(run);

            return para;
        }

        private Paragraph QuantityTypeFeatureParagraph(List<WordEntityQuantityAndTypeDto> entities)
        {
            Paragraph para = new Paragraph();

            var featureProperties = FeatureProperties();

            Run run = new Run();
            RunProperties captureTitleProperties = new RunProperties();

            foreach (var entity in entities)
            {
                run.AppendChild(new Text($"{entity.Lp}. {entity.Name} ({entity.Type}): Quantity: {entity.Quantity} Description: {entity.Description}"));
                run.AppendChild(new Break());
                run.AppendChild(new Break());
            }

            para.Append(featureProperties);
            para.Append(run);

            return para;
        }

        private Paragraph OperationFeatureParagraph(List<WordOperationDto> entities)
        {
            Paragraph para = new Paragraph();

            var featureProperties = FeatureProperties();

            Run run = new Run();
            RunProperties captureTitleProperties = new RunProperties();

            foreach (var entity in entities)
            {
                run.AppendChild(new Text($"{entity.Lp}. {entity.Name} ({entity.Type}): Time: {entity.TimeInMinutes}, Number of Employees: {entity.Employees} - Descritpion: {entity.Description}"));
                run.AppendChild(new Break());
                run.AppendChild(new Break());
            }

            para.Append(featureProperties);
            para.Append(run);

            return para;
        }

        private Paragraph FeatureTitleParagraph(string title)
        {
            Paragraph para = new Paragraph();

            var headerProperties = FeatureHeaderProperties();

            Run run = new Run();
            RunProperties titleProperties = new RunProperties();

            run.Append(titleProperties);
            run.Append(new Text() { Text = title });
            run.Append(new Break());

            run.AppendChild(new Break());

            para.Append(headerProperties);
            para.Append(run);

            return para;
        }

        private Paragraph AddRectangle()
        {
            Paragraph para = new Paragraph();

            var headerProperties = FeatureHeaderProperties();

            Run run1 = new Run();

            //NoProof noProof1 = new NoProof();

            //runProperties1.Append(noProof1);
            //Wps.ShapeProperties shapeProperties1 = new Wps.ShapeProperties() { BlackWhiteMode = A.BlackWhiteModeValues.Auto };

            Rectangle rectangle = new Rectangle();

            rectangle.Title = "Statek";
            rectangle.StrokeColor = "blue";
            rectangle.HorizontalAlignment = HorizontalRuleAlignmentValues.Center;
            rectangle.FillColor = "yellow";
            rectangle.Id = "new rect";

            run1.Append(rectangle);
            para.Append(run1);


            return para;
        }

        private ParagraphProperties HeaderProperties()
        {
            ParagraphProperties headerProperties = new ParagraphProperties();
            ParagraphStyleId paragraphStyleId = new ParagraphStyleId() { Val = "Normal" };
            Justification justificationCenter = new Justification() { Val = JustificationValues.Center };
            ParagraphMarkRunProperties paragraphMarkRunProperties1 = new ParagraphMarkRunProperties();

            headerProperties.Append(paragraphStyleId);
            headerProperties.Append(justificationCenter);
            headerProperties.Append(paragraphMarkRunProperties1);

            return headerProperties;
        }

        private ParagraphProperties FeatureHeaderProperties()
        {
            ParagraphProperties headerProperties = new ParagraphProperties();
            ParagraphStyleId paragraphStyleId = new ParagraphStyleId() { Val = "Normal" };
            Justification justificationCenter = new Justification() { Val = JustificationValues.Center };
            ParagraphMarkRunProperties paragraphMarkRunProperties1 = new ParagraphMarkRunProperties();

            headerProperties.Append(paragraphStyleId);
            headerProperties.Append(justificationCenter);
            headerProperties.Append(paragraphMarkRunProperties1);

            return headerProperties;
        }

        private ParagraphProperties FeatureProperties()
        {
            ParagraphProperties featureParagraphProperties = new ParagraphProperties();
            ParagraphStyleId paragraphStyleId = new ParagraphStyleId() { Val = "Normal" };
            Justification justificationStart = new Justification() { Val = JustificationValues.Start };
            ParagraphMarkRunProperties paragraphMarkRunProperties2 = new ParagraphMarkRunProperties();

            featureParagraphProperties.Append(paragraphStyleId);
            featureParagraphProperties.Append(justificationStart);
            featureParagraphProperties.Append(paragraphMarkRunProperties2);

            return featureParagraphProperties;
        }

       

        public Run AddShape()
        {
            Run run1 = new Run();

            RunProperties runProperties1 = new RunProperties();
            NoProof noProof1 = new NoProof();

            runProperties1.Append(noProof1);

            AlternateContent alternateContent1 = new AlternateContent();

            AlternateContentChoice alternateContentChoice1 = new AlternateContentChoice() { Requires = "wps" };

            Drawing drawing1 = new Drawing();

            Wp.Anchor anchor1 = new Wp.Anchor() { DistanceFromTop = (UInt32Value)0U, DistanceFromBottom = (UInt32Value)0U, DistanceFromLeft = (UInt32Value)0U, DistanceFromRight = (UInt32Value)0U, SimplePos = false, RelativeHeight = (UInt32Value)251658240U, BehindDoc = true, Locked = false, LayoutInCell = false, AllowOverlap = true };
            anchor1.AnchorId = GetRandomHexNumber(8);
            Wp.SimplePosition simplePosition1 = new Wp.SimplePosition() { X = 0L, Y = 0L };

            Wp.HorizontalPosition horizontalPosition1 = new Wp.HorizontalPosition() { RelativeFrom = Wp.HorizontalRelativePositionValues.Page };
            Wp.PositionOffset positionOffset1 = new Wp.PositionOffset();
            positionOffset1.Text = "0";

            horizontalPosition1.Append(positionOffset1);

            Wp.VerticalPosition verticalPosition1 = new Wp.VerticalPosition() { RelativeFrom = Wp.VerticalRelativePositionValues.Page };
            Wp.PositionOffset positionOffset2 = new Wp.PositionOffset();
            positionOffset2.Text = "Hi my friend";

            verticalPosition1.Append(positionOffset2);
            Wp.Extent extent1 = new Wp.Extent() { Cx = 6858000L, Cy = 76200L };
            Wp.EffectExtent effectExtent1 = new Wp.EffectExtent() { LeftEdge = 0L, TopEdge = 0L, RightEdge = 0L, BottomEdge = 0L };
            Wp.WrapNone wrapNone1 = new Wp.WrapNone();
            Wp.DocProperties docProperties1 = new Wp.DocProperties() { Id = 1U, Name = "Shape 1" }; // Id and name should be unique for every shape

            Wp.NonVisualGraphicFrameDrawingProperties nonVisualGraphicFrameDrawingProperties1 = new Wp.NonVisualGraphicFrameDrawingProperties();

            A.GraphicFrameLocks graphicFrameLocks1 = new A.GraphicFrameLocks();
            graphicFrameLocks1.AddNamespaceDeclaration("a", "http://schemas.openxmlformats.org/drawingml/2006/main");

            nonVisualGraphicFrameDrawingProperties1.Append(graphicFrameLocks1);

            A.Graphic graphic1 = new A.Graphic();
            graphic1.AddNamespaceDeclaration("a", "http://schemas.openxmlformats.org/drawingml/2006/main");

            A.GraphicData graphicData1 = new A.GraphicData() { Uri = "http://schemas.microsoft.com/office/word/2010/wordprocessingShape" };

            Wps.WordprocessingShape wordprocessingShape1 = new Wps.WordprocessingShape();

            Wps.NonVisualDrawingShapeProperties nonVisualDrawingShapeProperties1 = new Wps.NonVisualDrawingShapeProperties();
            A.ShapeLocks shapeLocks1 = new A.ShapeLocks();

            nonVisualDrawingShapeProperties1.Append(shapeLocks1);

            Wps.ShapeProperties shapeProperties1 = new Wps.ShapeProperties() { BlackWhiteMode = A.BlackWhiteModeValues.Auto };

            A.Transform2D transform2D1 = new A.Transform2D();
            A.Offset offset1 = new A.Offset() { X = 0L, Y = 0L };
            A.Extents extents1 = new A.Extents() { Cx = 6858000L, Cy = 76200L };

            transform2D1.Append(offset1);
            transform2D1.Append(extents1);

            A.CustomGeometry customGeometry1 = new A.CustomGeometry();
            A.AdjustValueList adjustValueList1 = new A.AdjustValueList();

            A.ShapeGuideList shapeGuideList1 = new A.ShapeGuideList();
            A.ShapeGuide shapeGuide1 = new A.ShapeGuide() { Name = "T0", Formula = "*/ 0 w 6858000" };
            A.ShapeGuide shapeGuide2 = new A.ShapeGuide() { Name = "T1", Formula = "*/ 0 h 76200" };
            A.ShapeGuide shapeGuide3 = new A.ShapeGuide() { Name = "T2", Formula = "*/ 0 w 6858000" };
            A.ShapeGuide shapeGuide4 = new A.ShapeGuide() { Name = "T3", Formula = "*/ 76200 h 76200" };
            A.ShapeGuide shapeGuide5 = new A.ShapeGuide() { Name = "T4", Formula = "*/ 6858000 w 6858000" };
            A.ShapeGuide shapeGuide6 = new A.ShapeGuide() { Name = "T5", Formula = "*/ 76200 h 76200" };
            A.ShapeGuide shapeGuide7 = new A.ShapeGuide() { Name = "T6", Formula = "*/ 6858000 w 6858000" };
            A.ShapeGuide shapeGuide8 = new A.ShapeGuide() { Name = "T7", Formula = "*/ 0 h 76200" };
            A.ShapeGuide shapeGuide9 = new A.ShapeGuide() { Name = "T8", Formula = "*/ 0 w 6858000" };
            A.ShapeGuide shapeGuide10 = new A.ShapeGuide() { Name = "T9", Formula = "*/ 0 h 76200" };
            A.ShapeGuide shapeGuide11 = new A.ShapeGuide() { Name = "T10", Formula = "*/ 0 60000 65536" };
            A.ShapeGuide shapeGuide12 = new A.ShapeGuide() { Name = "T11", Formula = "*/ 0 60000 65536" };
            A.ShapeGuide shapeGuide13 = new A.ShapeGuide() { Name = "T12", Formula = "*/ 0 60000 65536" };
            A.ShapeGuide shapeGuide14 = new A.ShapeGuide() { Name = "T13", Formula = "*/ 0 60000 65536" };
            A.ShapeGuide shapeGuide15 = new A.ShapeGuide() { Name = "T14", Formula = "*/ 0 60000 65536" };
            A.ShapeGuide shapeGuide16 = new A.ShapeGuide() { Name = "T15", Formula = "*/ 0 w 6858000" };
            A.ShapeGuide shapeGuide17 = new A.ShapeGuide() { Name = "T16", Formula = "*/ 0 h 76200" };
            A.ShapeGuide shapeGuide18 = new A.ShapeGuide() { Name = "T17", Formula = "*/ 6858000 w 6858000" };
            A.ShapeGuide shapeGuide19 = new A.ShapeGuide() { Name = "T18", Formula = "*/ 76200 h 76200" };

            shapeGuideList1.Append(shapeGuide1);
            shapeGuideList1.Append(shapeGuide2);
            shapeGuideList1.Append(shapeGuide3);
            shapeGuideList1.Append(shapeGuide4);
            shapeGuideList1.Append(shapeGuide5);
            shapeGuideList1.Append(shapeGuide6);
            shapeGuideList1.Append(shapeGuide7);
            shapeGuideList1.Append(shapeGuide8);
            shapeGuideList1.Append(shapeGuide9);
            shapeGuideList1.Append(shapeGuide10);
            shapeGuideList1.Append(shapeGuide11);
            shapeGuideList1.Append(shapeGuide12);
            shapeGuideList1.Append(shapeGuide13);
            shapeGuideList1.Append(shapeGuide14);
            shapeGuideList1.Append(shapeGuide15);
            shapeGuideList1.Append(shapeGuide16);
            shapeGuideList1.Append(shapeGuide17);
            shapeGuideList1.Append(shapeGuide18);
            shapeGuideList1.Append(shapeGuide19);

            A.AdjustHandleList adjustHandleList1 = new A.AdjustHandleList();

            A.ConnectionSiteList connectionSiteList1 = new A.ConnectionSiteList();

            A.ConnectionSite connectionSite1 = new A.ConnectionSite() { Angle = "T10" };
            A.Position position1 = new A.Position() { X = "T0", Y = "T1" };

            connectionSite1.Append(position1);

            A.ConnectionSite connectionSite2 = new A.ConnectionSite() { Angle = "T11" };
            A.Position position2 = new A.Position() { X = "T2", Y = "T3" };

            connectionSite2.Append(position2);

            A.ConnectionSite connectionSite3 = new A.ConnectionSite() { Angle = "T12" };
            A.Position position3 = new A.Position() { X = "T4", Y = "T5" };

            connectionSite3.Append(position3);

            A.ConnectionSite connectionSite4 = new A.ConnectionSite() { Angle = "T13" };
            A.Position position4 = new A.Position() { X = "T6", Y = "T7" };

            connectionSite4.Append(position4);

            A.ConnectionSite connectionSite5 = new A.ConnectionSite() { Angle = "T14" };
            A.Position position5 = new A.Position() { X = "T8", Y = "T9" };

            connectionSite5.Append(position5);

            connectionSiteList1.Append(connectionSite1);
            connectionSiteList1.Append(connectionSite2);
            connectionSiteList1.Append(connectionSite3);
            connectionSiteList1.Append(connectionSite4);
            connectionSiteList1.Append(connectionSite5);
            A.Rectangle rectangle1 = new A.Rectangle() { Left = "T15", Top = "T16", Right = "T17", Bottom = "T18" };

            A.PathList pathList1 = new A.PathList();

            A.Path path1 = new A.Path() { Width = 6858000L, Height = 76200L };

            A.MoveTo moveTo1 = new A.MoveTo();
            A.Point point1 = new A.Point() { X = "0", Y = "0" };

            moveTo1.Append(point1);

            A.LineTo lineTo1 = new A.LineTo();
            A.Point point2 = new A.Point() { X = "0", Y = "76200" };

            lineTo1.Append(point2);

            A.LineTo lineTo2 = new A.LineTo();
            A.Point point3 = new A.Point() { X = "6858000", Y = "76200" };

            lineTo2.Append(point3);

            A.LineTo lineTo3 = new A.LineTo();
            A.Point point4 = new A.Point() { X = "6858000", Y = "0" };

            lineTo3.Append(point4);

            A.LineTo lineTo4 = new A.LineTo();
            A.Point point5 = new A.Point() { X = "0", Y = "0" };

            lineTo4.Append(point5);

            path1.Append(moveTo1);
            path1.Append(lineTo1);
            path1.Append(lineTo2);
            path1.Append(lineTo3);
            path1.Append(lineTo4);

            pathList1.Append(path1);

            customGeometry1.Append(adjustValueList1);
            customGeometry1.Append(shapeGuideList1);
            customGeometry1.Append(adjustHandleList1);
            customGeometry1.Append(connectionSiteList1);
            customGeometry1.Append(rectangle1);
            customGeometry1.Append(pathList1);

            A.SolidFill solidFill1 = new A.SolidFill();
            A.RgbColorModelHex rgbColorModelHex1 = new A.RgbColorModelHex() { Val = "00AFEF" };

            solidFill1.Append(rgbColorModelHex1);

            A.Outline outline1 = new A.Outline();
            A.NoFill noFill1 = new A.NoFill();

            outline1.Append(noFill1);

            A.ShapePropertiesExtensionList shapePropertiesExtensionList1 = new A.ShapePropertiesExtensionList();

            A.ShapePropertiesExtension shapePropertiesExtension1 = new A.ShapePropertiesExtension() { Uri = "{91240B29-F687-4F45-9708-019B960494DF}" };

            A14.HiddenLineProperties hiddenLineProperties1 = new A14.HiddenLineProperties() { Width = 9525 };
            hiddenLineProperties1.AddNamespaceDeclaration("a14", "http://schemas.microsoft.com/office/drawing/2010/main");

            A.SolidFill solidFill2 = new A.SolidFill();
            A.RgbColorModelHex rgbColorModelHex2 = new A.RgbColorModelHex() { Val = "000000" };

            solidFill2.Append(rgbColorModelHex2);
            A.Round round1 = new A.Round();
            A.HeadEnd headEnd1 = new A.HeadEnd();
            A.TailEnd tailEnd1 = new A.TailEnd();

            hiddenLineProperties1.Append(solidFill2);
            hiddenLineProperties1.Append(round1);
            hiddenLineProperties1.Append(headEnd1);
            hiddenLineProperties1.Append(tailEnd1);

            shapePropertiesExtension1.Append(hiddenLineProperties1);

            shapePropertiesExtensionList1.Append(shapePropertiesExtension1);

            shapeProperties1.Append(transform2D1);
            shapeProperties1.Append(customGeometry1);
            shapeProperties1.Append(solidFill1);
            shapeProperties1.Append(outline1);
            shapeProperties1.Append(shapePropertiesExtensionList1);

            Wps.TextBodyProperties textBodyProperties1 = new Wps.TextBodyProperties() { Rotation = 0, Vertical = A.TextVerticalValues.Horizontal, Wrap = A.TextWrappingValues.Square, LeftInset = 91440, TopInset = 45720, RightInset = 91440, BottomInset = 45720, Anchor = A.TextAnchoringTypeValues.Top, AnchorCenter = false, UpRight = true };
            A.NoAutoFit noAutoFit1 = new A.NoAutoFit();

            textBodyProperties1.Append(noAutoFit1);

            wordprocessingShape1.Append(nonVisualDrawingShapeProperties1);
            wordprocessingShape1.Append(shapeProperties1);
            wordprocessingShape1.Append(textBodyProperties1);

            graphicData1.Append(wordprocessingShape1);

            graphic1.Append(graphicData1);

            Wp14.RelativeWidth relativeWidth1 = new Wp14.RelativeWidth() { ObjectId = Wp14.SizeRelativeHorizontallyValues.Page };
            Wp14.PercentageWidth percentageWidth1 = new Wp14.PercentageWidth();
            percentageWidth1.Text = "0";

            relativeWidth1.Append(percentageWidth1);

            Wp14.RelativeHeight relativeHeight1 = new Wp14.RelativeHeight() { RelativeFrom = Wp14.SizeRelativeVerticallyValues.Page };
            Wp14.PercentageHeight percentageHeight1 = new Wp14.PercentageHeight();
            percentageHeight1.Text = "0";

            relativeHeight1.Append(percentageHeight1);

            anchor1.Append(simplePosition1);
            anchor1.Append(horizontalPosition1);
            anchor1.Append(verticalPosition1);
            anchor1.Append(extent1);
            anchor1.Append(effectExtent1);
            anchor1.Append(wrapNone1);
            anchor1.Append(docProperties1);
            anchor1.Append(nonVisualGraphicFrameDrawingProperties1);
            anchor1.Append(graphic1);
            anchor1.Append(relativeWidth1);
            anchor1.Append(relativeHeight1);

            drawing1.Append(anchor1);

            alternateContentChoice1.Append(drawing1);

            AlternateContentFallback alternateContentFallback1 = new AlternateContentFallback();

            Picture picture1 = new Picture();
            //Use Id same as what you used in DocProperties 
            V.Shape shape1 = new V.Shape() { Id = "Shape 1", Style = "position:absolute;margin-left:0;margin-top:63pt;width:540pt;height:6pt;z-index:-251658240;visibility:visible;mso-wrap-style:square;mso-width-percent:0;mso-height-percent:0;mso-wrap-distance-left:0;mso-wrap-distance-top:0;mso-wrap-distance-right:0;mso-wrap-distance-bottom:0;mso-position-horizontal:absolute;mso-position-horizontal-relative:page;mso-position-vertical:absolute;mso-position-vertical-relative:page;mso-width-percent:0;mso-height-percent:0;mso-width-relative:page;mso-height-relative:page;v-text-anchor:top", CoordinateSize = "6858000,76200", OptionalString = "_x0000_s1026", AllowInCell = false, FillColor = "#00afef", Stroked = false, EdgePath = "m,l,76200r6858000,l6858000,,,e", EncodedPackage = "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\nAAQA8wAAAAgHAAAAAA==\n" };

            shape1.AddNamespaceDeclaration("w14", "http://schemas.microsoft.com/office/word/2010/wordml");
            shape1.AddNamespaceDeclaration("o", "urn:schemas-microsoft-com:office:office");
            shape1.AddNamespaceDeclaration("v", "urn:schemas-microsoft-com:vml");
            shape1.SetAttribute(new OpenXmlAttribute("w14", "anchorId", "http://schemas.microsoft.com/office/word/2010/wordml", anchor1.AnchorId));//Set anchorId attribute value same as anchorID
            V.Path path2 = new V.Path() { TextboxRectangle = "0,0,6858000,76200", ShowArrowhead = true, ConnectionPointType = Ovml.ConnectValues.Custom, ConnectionPoints = "0,0;0,76200;6858000,76200;6858000,0;0,0", ConnectAngles = "0,0,0,0,0" };
            Wvml.TextWrap textWrap1 = new Wvml.TextWrap() { AnchorX = Wvml.HorizontalAnchorValues.Page, AnchorY = Wvml.VerticalAnchorValues.Page };

            shape1.Append(path2);
            shape1.Append(textWrap1);

            picture1.Append(shape1);

            alternateContentFallback1.Append(picture1);

            alternateContent1.Append(alternateContentChoice1);
            alternateContent1.Append(alternateContentFallback1);

            run1.Append(runProperties1);
            run1.Append(alternateContent1);
            return run1;
        }

        public string GetRandomHexNumber(int digits)
        {
            byte[] buffer = new byte[digits / 2];
            random.NextBytes(buffer);
            string result = String.Concat(buffer.Select(x => x.ToString("X2")).ToArray());
            if (digits % 2 == 0)
                return result;
            return result + random.Next(16).ToString("X");
        }


        //public void Export2(DataAccess.Model.Diagram diagram, WordprocessingDocument wordDoc)
        //{
        //    Document document1 = new Document() { MCAttributes = new MarkupCompatibilityAttributes() { Ignorable = "w14 w15 wp14" } };
        //    wordDoc.AddMainDocumentPart();
        //    //document1.AddNamespaceDeclaration("wpc", "http://schemas.microsoft.com/office/word/2010/wordprocessingCanvas");
        //    //document1.AddNamespaceDeclaration("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
        //    //document1.AddNamespaceDeclaration("o", "urn:schemas-microsoft-com:office:office");
        //    //document1.AddNamespaceDeclaration("r", "http://schemas.openxmlformats.org/officeDocument/2006/relationships");
        //    //document1.AddNamespaceDeclaration("m", "http://schemas.openxmlformats.org/officeDocument/2006/math");
        //    //document1.AddNamespaceDeclaration("v", "urn:schemas-microsoft-com:vml");
        //    //document1.AddNamespaceDeclaration("wp14", "http://schemas.microsoft.com/office/word/2010/wordprocessingDrawing");
        //    //document1.AddNamespaceDeclaration("wp", "http://schemas.openxmlformats.org/drawingml/2006/wordprocessingDrawing");
        //    //document1.AddNamespaceDeclaration("w10", "urn:schemas-microsoft-com:office:word");
        //    //document1.AddNamespaceDeclaration("w", "http://schemas.openxmlformats.org/wordprocessingml/2006/main");
        //    //document1.AddNamespaceDeclaration("w14", "http://schemas.microsoft.com/office/word/2010/wordml");
        //    //document1.AddNamespaceDeclaration("w15", "http://schemas.microsoft.com/office/word/2012/wordml");
        //    //document1.AddNamespaceDeclaration("wpg", "http://schemas.microsoft.com/office/word/2010/wordprocessingGroup");
        //    //document1.AddNamespaceDeclaration("wpi", "http://schemas.microsoft.com/office/word/2010/wordprocessingInk");
        //    //document1.AddNamespaceDeclaration("wne", "http://schemas.microsoft.com/office/word/2006/wordml");
        //    //document1.AddNamespaceDeclaration("wps", "http://schemas.microsoft.com/office/word/2010/wordprocessingShape");

        //    var Schedule = new Dictionary<int, IDictionary<string, Dictionary<string, Tuple<string,string>>>>();
        //    var tempTuple = new Tuple<string, string>("2", "@");
        //    var tempDictinary = new Dictionary<string, Dictionary<string, Tuple<string, string>>>();
        //    var eventList = new Dictionary<string, Tuple<string, string>>();
        //    eventList.Add("TempTime", tempTuple);
        //    tempDictinary.Add("time", eventList);
        //    Schedule.Add(1, tempDictinary);
        //    Schedule.Add(2, tempDictinary);
        //    Schedule.Add(3, tempDictinary);

        //    Body body1 = new Body();

        //    Paragraph paragraph1 = new Paragraph() { RsidParagraphAddition = "00FA5335", RsidParagraphProperties = "00FA5335", RsidRunAdditionDefault = "00FA5335" };

        //    ParagraphProperties paragraphProperties1 = new ParagraphProperties();
        //    SpacingBetweenLines spacingBetweenLines1 = new SpacingBetweenLines() { After = "0", Line = "240", LineRule = LineSpacingRuleValues.Auto };
        //    Justification justification1 = new Justification() { Val = JustificationValues.Center };

        //    ParagraphMarkRunProperties paragraphMarkRunProperties1 = new ParagraphMarkRunProperties();
        //    FontSize fontSize1 = new FontSize() { Val = "20" };

        //    paragraphMarkRunProperties1.Append(fontSize1);

        //    paragraphProperties1.Append(spacingBetweenLines1);
        //    paragraphProperties1.Append(justification1);
        //    paragraphProperties1.Append(paragraphMarkRunProperties1);
        //    BookmarkStart bookmarkStart1 = new BookmarkStart() { Name = "_GoBack", Id = "0" };
        //    BookmarkEnd bookmarkEnd1 = new BookmarkEnd() { Id = "0" };

        //    Run run1 = new Run();

        //    RunProperties runProperties1 = new RunProperties();
        //    NoProof noProof1 = new NoProof();
        //    FontSize fontSize2 = new FontSize() { Val = "20" };
        //    Languages languages1 = new Languages() { EastAsia = "ru-RU" };

        //    runProperties1.Append(noProof1);
        //    runProperties1.Append(fontSize2);
        //    runProperties1.Append(languages1);

        //    AlternateContent alternateContent1 = new AlternateContent();

        //    AlternateContentChoice alternateContentChoice1 = new AlternateContentChoice() { Requires = "wps" };

        //    Drawing drawing1 = new Drawing();

        //    Wp.Anchor anchor1 = new Wp.Anchor() { DistanceFromTop = (UInt32Value)0U, DistanceFromBottom = (UInt32Value)0U, DistanceFromLeft = (UInt32Value)114300U, DistanceFromRight = (UInt32Value)114300U, SimplePos = false, RelativeHeight = (UInt32Value)251659264U, BehindDoc = false, Locked = false, LayoutInCell = true, AllowOverlap = true };
        //    Wp.SimplePosition simplePosition1 = new Wp.SimplePosition() { X = 0L, Y = 0L };

        //    Wp.HorizontalPosition horizontalPosition1 = new Wp.HorizontalPosition() { RelativeFrom = Wp.HorizontalRelativePositionValues.Column };
        //    Wp.PositionOffset positionOffset1 = new Wp.PositionOffset();
        //    positionOffset1.Text = "7559955";

        //    horizontalPosition1.Append(positionOffset1);

        //    Wp.VerticalPosition verticalPosition1 = new Wp.VerticalPosition() { RelativeFrom = Wp.VerticalRelativePositionValues.Paragraph };
        //    Wp.PositionOffset positionOffset2 = new Wp.PositionOffset();
        //    positionOffset2.Text = "-180045";

        //    verticalPosition1.Append(positionOffset2);
        //    Wp.Extent extent1 = new Wp.Extent() { Cx = 2540000L, Cy = 635000L };
        //    Wp.EffectExtent effectExtent1 = new Wp.EffectExtent() { LeftEdge = 0L, TopEdge = 0L, RightEdge = 0L, BottomEdge = 0L };
        //    Wp.WrapNone wrapNone1 = new Wp.WrapNone();
        //    Wp.DocProperties docProperties1 = new Wp.DocProperties() { Id = (UInt32Value)1U, Name = "Надпись 1" };
        //    Wp.NonVisualGraphicFrameDrawingProperties nonVisualGraphicFrameDrawingProperties1 = new Wp.NonVisualGraphicFrameDrawingProperties();

        //    A.Graphic graphic1 = new A.Graphic();
        //    graphic1.AddNamespaceDeclaration("a", "http://schemas.openxmlformats.org/drawingml/2006/main");

        //    A.GraphicData graphicData1 = new A.GraphicData() { Uri = "http://schemas.microsoft.com/office/word/2010/wordprocessingShape" };

        //    Wps.WordprocessingShape wordprocessingShape1 = new Wps.WordprocessingShape();
        //    Wps.NonVisualDrawingShapeProperties nonVisualDrawingShapeProperties1 = new Wps.NonVisualDrawingShapeProperties() { TextBox = true };

        //    Wps.ShapeProperties shapeProperties1 = new Wps.ShapeProperties();

        //    A.Transform2D transform2D1 = new A.Transform2D();
        //    A.Offset offset1 = new A.Offset() { X = 0L, Y = 0L };
        //    A.Extents extents1 = new A.Extents() { Cx = 2540000L, Cy = 635000L };

        //    transform2D1.Append(offset1);
        //    transform2D1.Append(extents1);

        //    A.PresetGeometry presetGeometry1 = new A.PresetGeometry() { Preset = A.ShapeTypeValues.Rectangle };
        //    A.AdjustValueList adjustValueList1 = new A.AdjustValueList();

        //    presetGeometry1.Append(adjustValueList1);
        //    A.NoFill noFill1 = new A.NoFill();

        //    A.Outline outline1 = new A.Outline() { Width = 6350 };
        //    A.NoFill noFill2 = new A.NoFill();

        //    outline1.Append(noFill2);
        //    A.EffectList effectList1 = new A.EffectList();

        //    A.ShapePropertiesExtensionList shapePropertiesExtensionList1 = new A.ShapePropertiesExtensionList();

        //    A.ShapePropertiesExtension shapePropertiesExtension1 = new A.ShapePropertiesExtension() { Uri = "{91240B29-F687-4F45-9708-019B960494DF}" };

        //    A14.HiddenLineProperties hiddenLineProperties1 = new A14.HiddenLineProperties() { Width = 6350 };
        //    hiddenLineProperties1.AddNamespaceDeclaration("a14", "http://schemas.microsoft.com/office/drawing/2010/main");

        //    A.SolidFill solidFill1 = new A.SolidFill();
        //    A.PresetColor presetColor1 = new A.PresetColor() { Val = A.PresetColorValues.Black };

        //    solidFill1.Append(presetColor1);

        //    hiddenLineProperties1.Append(solidFill1);

        //    shapePropertiesExtension1.Append(hiddenLineProperties1);

        //    shapePropertiesExtensionList1.Append(shapePropertiesExtension1);

        //    shapeProperties1.Append(transform2D1);
        //    shapeProperties1.Append(presetGeometry1);
        //    shapeProperties1.Append(noFill1);
        //    shapeProperties1.Append(outline1);
        //    shapeProperties1.Append(effectList1);
        //    shapeProperties1.Append(shapePropertiesExtensionList1);

        //    Wps.ShapeStyle shapeStyle1 = new Wps.ShapeStyle();

        //    A.LineReference lineReference1 = new A.LineReference() { Index = (UInt32Value)0U };
        //    A.SchemeColor schemeColor1 = new A.SchemeColor() { Val = A.SchemeColorValues.Accent1 };

        //    lineReference1.Append(schemeColor1);

        //    A.FillReference fillReference1 = new A.FillReference() { Index = (UInt32Value)0U };
        //    A.SchemeColor schemeColor2 = new A.SchemeColor() { Val = A.SchemeColorValues.Accent1 };

        //    fillReference1.Append(schemeColor2);

        //    A.EffectReference effectReference1 = new A.EffectReference() { Index = (UInt32Value)0U };
        //    A.SchemeColor schemeColor3 = new A.SchemeColor() { Val = A.SchemeColorValues.Accent1 };

        //    effectReference1.Append(schemeColor3);

        //    A.FontReference fontReference1 = new A.FontReference() { Index = A.FontCollectionIndexValues.Minor };
        //    A.SchemeColor schemeColor4 = new A.SchemeColor() { Val = A.SchemeColorValues.Dark1 };

        //    fontReference1.Append(schemeColor4);

        //    shapeStyle1.Append(lineReference1);
        //    shapeStyle1.Append(fillReference1);
        //    shapeStyle1.Append(effectReference1);
        //    shapeStyle1.Append(fontReference1);

        //    Wps.TextBoxInfo2 textBoxInfo21 = new Wps.TextBoxInfo2();

        //    TextBoxContent textBoxContent1 = new TextBoxContent();

        //    if (true)
        //    {
        //        Paragraph paragraph2 = new Paragraph() { RsidParagraphMarkRevision = "00FA5335", RsidParagraphAddition = "00FA5335", RsidParagraphProperties = "00FA5335", RsidRunAdditionDefault = "00FA5335" };

        //        ParagraphProperties paragraphProperties2 = new ParagraphProperties();
        //        SpacingBetweenLines spacingBetweenLines2 = new SpacingBetweenLines() { After = "0", Line = "240", LineRule = LineSpacingRuleValues.Auto };
        //        Justification justification2 = new Justification() { Val = JustificationValues.Right };

        //        ParagraphMarkRunProperties paragraphMarkRunProperties2 = new ParagraphMarkRunProperties();
        //        FontSize fontSize3 = new FontSize() { Val = "20" };

        //        paragraphMarkRunProperties2.Append(fontSize3);

        //        paragraphProperties2.Append(spacingBetweenLines2);
        //        paragraphProperties2.Append(justification2);
        //        paragraphProperties2.Append(paragraphMarkRunProperties2);

        //        Run run2 = new Run() { RsidRunProperties = "00FA5335" };

        //        RunProperties runProperties2 = new RunProperties();
        //        FontSize fontSize4 = new FontSize() { Val = "20" };

        //        runProperties2.Append(fontSize4);
        //        Text text1 = new Text();
        //        text1.Text = "«УТВЕРЖДАЮ»";

        //        run2.Append(runProperties2);
        //        run2.Append(text1);

        //        paragraph2.Append(paragraphProperties2);
        //        paragraph2.Append(run2);

        //        Paragraph paragraph3 = new Paragraph() { RsidParagraphMarkRevision = "00FA5335", RsidParagraphAddition = "00FA5335", RsidParagraphProperties = "00FA5335", RsidRunAdditionDefault = "00FA5335" };

        //        ParagraphProperties paragraphProperties3 = new ParagraphProperties();
        //        SpacingBetweenLines spacingBetweenLines3 = new SpacingBetweenLines() { After = "0", Line = "240", LineRule = LineSpacingRuleValues.Auto };
        //        Justification justification3 = new Justification() { Val = JustificationValues.Right };

        //        ParagraphMarkRunProperties paragraphMarkRunProperties3 = new ParagraphMarkRunProperties();
        //        FontSize fontSize5 = new FontSize() { Val = "20" };

        //        paragraphMarkRunProperties3.Append(fontSize5);

        //        paragraphProperties3.Append(spacingBetweenLines3);
        //        paragraphProperties3.Append(justification3);
        //        paragraphProperties3.Append(paragraphMarkRunProperties3);

        //        Run run3 = new Run() { RsidRunProperties = "00FA5335" };

        //        RunProperties runProperties3 = new RunProperties();
        //        FontSize fontSize6 = new FontSize() { Val = "20" };

        //        runProperties3.Append(fontSize6);
        //        Text text2 = new Text();
        //        text2.Text = "Проректор по учебной работе";

        //        run3.Append(runProperties3);
        //        run3.Append(text2);

        //        paragraph3.Append(paragraphProperties3);
        //        paragraph3.Append(run3);

        //        Paragraph paragraph4 = new Paragraph() { RsidParagraphMarkRevision = "00FA5335", RsidParagraphAddition = "00FA5335", RsidParagraphProperties = "00FA5335", RsidRunAdditionDefault = "00FA5335" };

        //        ParagraphProperties paragraphProperties4 = new ParagraphProperties();
        //        SpacingBetweenLines spacingBetweenLines4 = new SpacingBetweenLines() { After = "0", Line = "240", LineRule = LineSpacingRuleValues.Auto };
        //        Justification justification4 = new Justification() { Val = JustificationValues.Right };

        //        ParagraphMarkRunProperties paragraphMarkRunProperties4 = new ParagraphMarkRunProperties();
        //        FontSize fontSize7 = new FontSize() { Val = "20" };

        //        paragraphMarkRunProperties4.Append(fontSize7);

        //        paragraphProperties4.Append(spacingBetweenLines4);
        //        paragraphProperties4.Append(justification4);
        //        paragraphProperties4.Append(paragraphMarkRunProperties4);

        //        Run run4 = new Run() { RsidRunProperties = "00FA5335" };

        //        RunProperties runProperties4 = new RunProperties();
        //        FontSize fontSize8 = new FontSize() { Val = "20" };

        //        runProperties4.Append(fontSize8);
        //        Text text3 = new Text() { Space = SpaceProcessingModeValues.Preserve };
        //        text3.Text = "______________     ";

        //        run4.Append(runProperties4);
        //        run4.Append(text3);
        //        ProofError proofError1 = new ProofError() { Type = ProofingErrorValues.SpellStart };

        //        Run run5 = new Run() { RsidRunProperties = "00FA5335" };

        //        RunProperties runProperties5 = new RunProperties();
        //        FontSize fontSize9 = new FontSize() { Val = "20" };

        //        runProperties5.Append(fontSize9);
        //        Text text4 = new Text();
        //        text4.Text = "А.В.Синицкий";

        //        run5.Append(runProperties5);
        //        run5.Append(text4);
        //        ProofError proofError2 = new ProofError() { Type = ProofingErrorValues.SpellEnd };

        //        paragraph4.Append(paragraphProperties4);
        //        paragraph4.Append(run4);
        //        paragraph4.Append(proofError1);
        //        paragraph4.Append(run5);
        //        paragraph4.Append(proofError2);

        //        textBoxContent1.Append(paragraph2);
        //        textBoxContent1.Append(paragraph3);
        //        textBoxContent1.Append(paragraph4);
        //    }

        //    textBoxInfo21.Append(textBoxContent1);

        //    Wps.TextBodyProperties textBodyProperties1 = new Wps.TextBodyProperties() { Rotation = 0, UseParagraphSpacing = false, VerticalOverflow = A.TextVerticalOverflowValues.Overflow, HorizontalOverflow = A.TextHorizontalOverflowValues.Overflow, Vertical = A.TextVerticalValues.Horizontal, Wrap = A.TextWrappingValues.Square, LeftInset = 91440, TopInset = 45720, RightInset = 91440, BottomInset = 45720, ColumnCount = 1, ColumnSpacing = 0, RightToLeftColumns = false, FromWordArt = false, Anchor = A.TextAnchoringTypeValues.Top, AnchorCenter = false, ForceAntiAlias = false, CompatibleLineSpacing = true };

        //    A.PresetTextWrap presetTextWrap1 = new A.PresetTextWrap() { Preset = A.TextShapeValues.TextNoShape };
        //    A.AdjustValueList adjustValueList2 = new A.AdjustValueList();

        //    presetTextWrap1.Append(adjustValueList2);
        //    A.NoAutoFit noAutoFit1 = new A.NoAutoFit();

        //    textBodyProperties1.Append(presetTextWrap1);
        //    textBodyProperties1.Append(noAutoFit1);

        //    wordprocessingShape1.Append(nonVisualDrawingShapeProperties1);
        //    wordprocessingShape1.Append(shapeProperties1);
        //    wordprocessingShape1.Append(shapeStyle1);
        //    wordprocessingShape1.Append(textBoxInfo21);
        //    wordprocessingShape1.Append(textBodyProperties1);

        //    graphicData1.Append(wordprocessingShape1);

        //    graphic1.Append(graphicData1);

        //    anchor1.Append(simplePosition1);
        //    anchor1.Append(horizontalPosition1);
        //    anchor1.Append(verticalPosition1);
        //    anchor1.Append(extent1);
        //    anchor1.Append(effectExtent1);
        //    anchor1.Append(wrapNone1);
        //    anchor1.Append(docProperties1);
        //    anchor1.Append(nonVisualGraphicFrameDrawingProperties1);
        //    anchor1.Append(graphic1);

        //    drawing1.Append(anchor1);

        //    alternateContentChoice1.Append(drawing1);

        //    AlternateContentFallback alternateContentFallback1 = new AlternateContentFallback();

        //    Picture picture1 = new Picture();

        //    V.Shapetype shapetype1 = new V.Shapetype() { Id = "_x0000_t202", CoordinateSize = "21600,21600", OptionalNumber = 202, EdgePath = "m,l,21600r21600,l21600,xe" };
        //    V.Stroke stroke1 = new V.Stroke() { JoinStyle = V.StrokeJoinStyleValues.Miter };
        //    V.Path path1 = new V.Path() { AllowGradientShape = true, ConnectionPointType = Ovml.ConnectValues.Rectangle };

        //    shapetype1.Append(stroke1);
        //    shapetype1.Append(path1);

        //    V.Shape shape1 = new V.Shape() { Id = "Надпись 1", Style = "position:absolute;left:0;text-align:left;margin-left:595.25pt;margin-top:-14.2pt;width:200pt;height:50pt;z-index:251659264;visibility:visible;mso-wrap-style:square;mso-wrap-distance-left:9pt;mso-wrap-distance-top:0;mso-wrap-distance-right:9pt;mso-wrap-distance-bottom:0;mso-position-horizontal:absolute;mso-position-horizontal-relative:text;mso-position-vertical:absolute;mso-position-vertical-relative:text;v-text-anchor:top", OptionalString = "_x0000_s1026", Filled = false, Stroked = false, StrokeWeight = ".5pt", Type = "#_x0000_t202", EncodedPackage = "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\nbWxQSwUGAAAAAAQABADzAAAAaAYAAAAA\n" };
        //    V.Fill fill1 = new V.Fill() { DetectMouseClick = true };

        //    V.TextBox textBox1 = new V.TextBox();

        //    TextBoxContent textBoxContent2 = new TextBoxContent();

        //    Paragraph paragraph5 = new Paragraph() { RsidParagraphMarkRevision = "00FA5335", RsidParagraphAddition = "00FA5335", RsidParagraphProperties = "00FA5335", RsidRunAdditionDefault = "00FA5335" };

        //    ParagraphProperties paragraphProperties5 = new ParagraphProperties();
        //    SpacingBetweenLines spacingBetweenLines5 = new SpacingBetweenLines() { After = "0", Line = "240", LineRule = LineSpacingRuleValues.Auto };
        //    Justification justification5 = new Justification() { Val = JustificationValues.Right };

        //    ParagraphMarkRunProperties paragraphMarkRunProperties5 = new ParagraphMarkRunProperties();
        //    FontSize fontSize10 = new FontSize() { Val = "20" };

        //    paragraphMarkRunProperties5.Append(fontSize10);

        //    paragraphProperties5.Append(spacingBetweenLines5);
        //    paragraphProperties5.Append(justification5);
        //    paragraphProperties5.Append(paragraphMarkRunProperties5);

        //    Run run6 = new Run() { RsidRunProperties = "00FA5335" };

        //    RunProperties runProperties6 = new RunProperties();
        //    FontSize fontSize11 = new FontSize() { Val = "20" };

        //    runProperties6.Append(fontSize11);
        //    Text text5 = new Text();
        //    text5.Text = "«УТВЕРЖДАЮ»";

        //    run6.Append(runProperties6);
        //    run6.Append(text5);

        //    paragraph5.Append(paragraphProperties5);
        //    paragraph5.Append(run6);

        //    Paragraph paragraph6 = new Paragraph() { RsidParagraphMarkRevision = "00FA5335", RsidParagraphAddition = "00FA5335", RsidParagraphProperties = "00FA5335", RsidRunAdditionDefault = "00FA5335" };

        //    ParagraphProperties paragraphProperties6 = new ParagraphProperties();
        //    SpacingBetweenLines spacingBetweenLines6 = new SpacingBetweenLines() { After = "0", Line = "240", LineRule = LineSpacingRuleValues.Auto };
        //    Justification justification6 = new Justification() { Val = JustificationValues.Right };

        //    ParagraphMarkRunProperties paragraphMarkRunProperties6 = new ParagraphMarkRunProperties();
        //    FontSize fontSize12 = new FontSize() { Val = "20" };

        //    paragraphMarkRunProperties6.Append(fontSize12);

        //    paragraphProperties6.Append(spacingBetweenLines6);
        //    paragraphProperties6.Append(justification6);
        //    paragraphProperties6.Append(paragraphMarkRunProperties6);

        //    Run run7 = new Run() { RsidRunProperties = "00FA5335" };

        //    RunProperties runProperties7 = new RunProperties();
        //    FontSize fontSize13 = new FontSize() { Val = "20" };

        //    runProperties7.Append(fontSize13);
        //    Text text6 = new Text();
        //    text6.Text = "Проректор по учебной работе";

        //    run7.Append(runProperties7);
        //    run7.Append(text6);

        //    paragraph6.Append(paragraphProperties6);
        //    paragraph6.Append(run7);

        //    Paragraph paragraph7 = new Paragraph() { RsidParagraphMarkRevision = "00FA5335", RsidParagraphAddition = "00FA5335", RsidParagraphProperties = "00FA5335", RsidRunAdditionDefault = "00FA5335" };

        //    ParagraphProperties paragraphProperties7 = new ParagraphProperties();
        //    SpacingBetweenLines spacingBetweenLines7 = new SpacingBetweenLines() { After = "0", Line = "240", LineRule = LineSpacingRuleValues.Auto };
        //    Justification justification7 = new Justification() { Val = JustificationValues.Right };

        //    ParagraphMarkRunProperties paragraphMarkRunProperties7 = new ParagraphMarkRunProperties();
        //    FontSize fontSize14 = new FontSize() { Val = "20" };

        //    paragraphMarkRunProperties7.Append(fontSize14);

        //    paragraphProperties7.Append(spacingBetweenLines7);
        //    paragraphProperties7.Append(justification7);
        //    paragraphProperties7.Append(paragraphMarkRunProperties7);

        //    Run run8 = new Run() { RsidRunProperties = "00FA5335" };

        //    RunProperties runProperties8 = new RunProperties();
        //    FontSize fontSize15 = new FontSize() { Val = "20" };

        //    runProperties8.Append(fontSize15);
        //    Text text7 = new Text() { Space = SpaceProcessingModeValues.Preserve };
        //    text7.Text = "______________     ";

        //    run8.Append(runProperties8);
        //    run8.Append(text7);
        //    ProofError proofError3 = new ProofError() { Type = ProofingErrorValues.SpellStart };

        //    Run run9 = new Run() { RsidRunProperties = "00FA5335" };

        //    RunProperties runProperties9 = new RunProperties();
        //    FontSize fontSize16 = new FontSize() { Val = "20" };

        //    runProperties9.Append(fontSize16);
        //    Text text8 = new Text();
        //    text8.Text = "А.В.Синицкий";

        //    run9.Append(runProperties9);
        //    run9.Append(text8);
        //    ProofError proofError4 = new ProofError() { Type = ProofingErrorValues.SpellEnd };

        //    paragraph7.Append(paragraphProperties7);
        //    paragraph7.Append(run8);
        //    paragraph7.Append(proofError3);
        //    paragraph7.Append(run9);
        //    paragraph7.Append(proofError4);

        //    textBoxContent2.Append(paragraph5);
        //    textBoxContent2.Append(paragraph6);
        //    textBoxContent2.Append(paragraph7);

        //    textBox1.Append(textBoxContent2);

        //    shape1.Append(fill1);
        //    shape1.Append(textBox1);

        //    picture1.Append(shapetype1);
        //    picture1.Append(shape1);

        //    alternateContentFallback1.Append(picture1);

        //    alternateContent1.Append(alternateContentChoice1);
        //    alternateContent1.Append(alternateContentFallback1);

        //    run1.Append(runProperties1);
        //    run1.Append(alternateContent1);

        //    Run run10 = new Run() { RsidRunProperties = "00FA5335" };

        //    RunProperties runProperties10 = new RunProperties();
        //    FontSize fontSize17 = new FontSize() { Val = "20" };

        //    runProperties10.Append(fontSize17);
        //    Text text9 = new Text();
        //    text9.Text = "Расписание";

        //    run10.Append(runProperties10);
        //    run10.Append(text9);

        //    paragraph1.Append(paragraphProperties1);
        //    paragraph1.Append(bookmarkStart1);
        //    paragraph1.Append(bookmarkEnd1);
        //    paragraph1.Append(run1);
        //    paragraph1.Append(run10);

        //    Paragraph paragraph8 = new Paragraph() { RsidParagraphAddition = "00FA5335", RsidParagraphProperties = "00FA5335", RsidRunAdditionDefault = "00FA5335" };

        //    ParagraphProperties paragraphProperties8 = new ParagraphProperties();
        //    SpacingBetweenLines spacingBetweenLines8 = new SpacingBetweenLines() { After = "0", Line = "240", LineRule = LineSpacingRuleValues.Auto };
        //    Justification justification8 = new Justification() { Val = JustificationValues.Center };

        //    ParagraphMarkRunProperties paragraphMarkRunProperties8 = new ParagraphMarkRunProperties();
        //    FontSize fontSize18 = new FontSize() { Val = "20" };

        //    paragraphMarkRunProperties8.Append(fontSize18);

        //    paragraphProperties8.Append(spacingBetweenLines8);
        //    paragraphProperties8.Append(justification8);
        //    paragraphProperties8.Append(paragraphMarkRunProperties8);

        //    Run run11 = new Run() { RsidRunProperties = "00FA5335" };

        //    RunProperties runProperties11 = new RunProperties();
        //    FontSize fontSize19 = new FontSize() { Val = "20" };

        //    runProperties11.Append(fontSize19);
        //    Text text10 = new Text();
        //    text10.Text = "второго семестра 2013 – 2014 учебного года";

        //    run11.Append(runProperties11);
        //    run11.Append(text10);

        //    paragraph8.Append(paragraphProperties8);
        //    paragraph8.Append(run11);

        //    Paragraph paragraph9 = new Paragraph() { RsidParagraphAddition = "00FA5335", RsidParagraphProperties = "00FA5335", RsidRunAdditionDefault = "00FA5335" };

        //    ParagraphProperties paragraphProperties9 = new ParagraphProperties();
        //    SpacingBetweenLines spacingBetweenLines9 = new SpacingBetweenLines() { After = "0", Line = "240", LineRule = LineSpacingRuleValues.Auto };
        //    Justification justification9 = new Justification() { Val = JustificationValues.Center };

        //    ParagraphMarkRunProperties paragraphMarkRunProperties9 = new ParagraphMarkRunProperties();
        //    FontSize fontSize20 = new FontSize() { Val = "20" };

        //    paragraphMarkRunProperties9.Append(fontSize20);

        //    paragraphProperties9.Append(spacingBetweenLines9);
        //    paragraphProperties9.Append(justification9);
        //    paragraphProperties9.Append(paragraphMarkRunProperties9);

        //    Run run12 = new Run() { RsidRunProperties = "00FA5335" };

        //    RunProperties runProperties12 = new RunProperties();
        //    FontSize fontSize21 = new FontSize() { Val = "20" };

        //    runProperties12.Append(fontSize21);
        //    Text text11 = new Text();
        //    text11.Text = "FacultyName";

        //    run12.Append(runProperties12);
        //    run12.Append(text11);

        //    paragraph9.Append(paragraphProperties9);
        //    paragraph9.Append(run12);

        //    Paragraph paragraph10 = new Paragraph() { RsidParagraphAddition = "00FA5335", RsidParagraphProperties = "00FA5335", RsidRunAdditionDefault = "00FA5335" };

        //    ParagraphProperties paragraphProperties10 = new ParagraphProperties();
        //    SpacingBetweenLines spacingBetweenLines10 = new SpacingBetweenLines() { After = "0", Line = "240", LineRule = LineSpacingRuleValues.Auto };
        //    Justification justification10 = new Justification() { Val = JustificationValues.Center };

        //    ParagraphMarkRunProperties paragraphMarkRunProperties10 = new ParagraphMarkRunProperties();
        //    Bold bold1 = new Bold();
        //    FontSize fontSize22 = new FontSize() { Val = "28" };

        //    paragraphMarkRunProperties10.Append(bold1);
        //    paragraphMarkRunProperties10.Append(fontSize22);

        //    paragraphProperties10.Append(spacingBetweenLines10);
        //    paragraphProperties10.Append(justification10);
        //    paragraphProperties10.Append(paragraphMarkRunProperties10);

        //    Run run13 = new Run() { RsidRunProperties = "00FA5335" };

        //    RunProperties runProperties13 = new RunProperties();
        //    Bold bold2 = new Bold();
        //    FontSize fontSize23 = new FontSize() { Val = "28" };

        //    runProperties13.Append(bold2);
        //    runProperties13.Append(fontSize23);
        //    Text text12 = new Text();
        //    text12.Text = "Dow";

        //    run13.Append(runProperties13);
        //    run13.Append(text12);

        //    paragraph10.Append(paragraphProperties10);
        //    paragraph10.Append(run13);

        //    Table table1 = new Table();

        //    TableProperties tableProperties1 = new TableProperties();
        //    TableWidth tableWidth1 = new TableWidth() { Width = "15919", Type = TableWidthUnitValues.Dxa };

        //    TableBorders tableBorders1 = new TableBorders();
        //    TopBorder topBorder1 = new TopBorder() { Val = BorderValues.Single, Color = "auto", Size = (UInt32Value)4U, Space = (UInt32Value)0U };
        //    LeftBorder leftBorder1 = new LeftBorder() { Val = BorderValues.Single, Color = "auto", Size = (UInt32Value)4U, Space = (UInt32Value)0U };
        //    BottomBorder bottomBorder1 = new BottomBorder() { Val = BorderValues.Single, Color = "auto", Size = (UInt32Value)4U, Space = (UInt32Value)0U };
        //    RightBorder rightBorder1 = new RightBorder() { Val = BorderValues.Single, Color = "auto", Size = (UInt32Value)4U, Space = (UInt32Value)0U };
        //    InsideHorizontalBorder insideHorizontalBorder1 = new InsideHorizontalBorder() { Val = BorderValues.Single, Color = "auto", Size = (UInt32Value)4U, Space = (UInt32Value)0U };
        //    InsideVerticalBorder insideVerticalBorder1 = new InsideVerticalBorder() { Val = BorderValues.Single, Color = "auto", Size = (UInt32Value)4U, Space = (UInt32Value)0U };

        //    tableBorders1.Append(topBorder1);
        //    tableBorders1.Append(leftBorder1);
        //    tableBorders1.Append(bottomBorder1);
        //    tableBorders1.Append(rightBorder1);
        //    tableBorders1.Append(insideHorizontalBorder1);
        //    tableBorders1.Append(insideVerticalBorder1);
        //    TableLayout tableLayout1 = new TableLayout() { Type = TableLayoutValues.Fixed };
        //    TableLook tableLook1 = new TableLook() { Val = "0000" };

        //    tableProperties1.Append(tableWidth1);
        //    tableProperties1.Append(tableBorders1);
        //    tableProperties1.Append(tableLayout1);
        //    tableProperties1.Append(tableLook1);

        //    TableGrid tableGrid1 = new TableGrid();
        //    GridColumn gridColumn1 = new GridColumn() { Width = "1383" };

        //    int colWidth = 14536 / 2;
        //    for (int i = 0; i < 2; i++)
        //    {
        //        GridColumn gridColumn2 = new GridColumn() { Width = colWidth.ToString() };

        //        tableGrid1.Append(gridColumn2);
        //    }

        //    // Ряд имён групп

        //    TableRow tableRow1 = new TableRow() { RsidTableRowMarkRevision = "00FA5335", RsidTableRowAddition = "00FA5335", RsidTableRowProperties = "00FA5335" };

        //    TablePropertyExceptions tablePropertyExceptions1 = new TablePropertyExceptions();

        //    TableCellMarginDefault tableCellMarginDefault1 = new TableCellMarginDefault();
        //    TopMargin topMargin1 = new TopMargin() { Width = "0", Type = TableWidthUnitValues.Dxa };
        //    BottomMargin bottomMargin1 = new BottomMargin() { Width = "0", Type = TableWidthUnitValues.Dxa };

        //    tableCellMarginDefault1.Append(topMargin1);
        //    tableCellMarginDefault1.Append(bottomMargin1);

        //    tablePropertyExceptions1.Append(tableCellMarginDefault1);

        //    Dictionary<int, List<int>> plainGroupsListIds = new Dictionary<int, List<int>>();
        //    Dictionary<int, List<int>> nGroupsListIds = new Dictionary<int, List<int>>();

        //    var groupNames = new List<string>();
        //    groupNames.Add("Group1");
        //    groupNames.Add("Group2");
        //    groupNames.Add("Group3");

        //    //foreach (var group in Schedule)
        //    //{
        //    //    var groupName = _Repo.GetStudentGroup(group.Key).Name;
        //    //    groupNames.Add(groupName.Replace(" (+Н)", ""));

        //    //    //if (groupName.Contains(" (+Н)"))
        //    //    //{
        //    //    //    var plainGroupName = groupName.Replace(" (+Н)", "");
        //    //    //    var nGroupName = groupName.Replace(" (+", "(");

        //    //    //    var plainGroupId = _Repo.FindStudentGroup(plainGroupName).StudentGroupId;
        //    //    //    var plainStudentIds = _Repo.GetAllStudentsInGroups()
        //    //    //            .Where(sig => sig.StudentGroup.StudentGroupId == plainGroupId)
        //    //    //            .Select(stig => stig.Student.StudentId)
        //    //    //            .ToList();
        //    //    //    plainGroupsListIds.Add(group.Key, _Repo.GetAllStudentsInGroups()
        //    //    //            .Where(sig => plainStudentIds.Contains(sig.Student.StudentId))
        //    //    //            .Select(stig => stig.StudentGroup.StudentGroupId)
        //    //    //            .Distinct()
        //    //    //            .ToList());

        //    //    //    var nGroupId = _Repo.FindStudentGroup(nGroupName).StudentGroupId;
        //    //    //    var nStudentIds = _Repo.GetAllStudentsInGroups()
        //    //    //            .Where(sig => sig.StudentGroup.StudentGroupId == nGroupId)
        //    //    //            .Select(stig => stig.Student.StudentId)
        //    //    //            .ToList();
        //    //    //    nGroupsListIds.Add(group.Key, _Repo.GetAllStudentsInGroups()
        //    //    //            .Where(sig => nStudentIds.Contains(sig.Student.StudentId))
        //    //    //            .Select(stig => stig.StudentGroup.StudentGroupId)
        //    //    //            .Distinct()
        //    //    //            .ToList());
        //    //    //}
        //    //}

        //    TableCell tableCell1 = new TableCell();

        //    TableCellProperties tableCellProperties1 = new TableCellProperties();
        //    TableCellWidth tableCellWidth1 = new TableCellWidth() { Width = "1383", Type = TableWidthUnitValues.Dxa };
        //    Shading shading1 = new Shading() { Val = ShadingPatternValues.Clear, Color = "auto", Fill = "auto" };

        //    tableCellProperties1.Append(tableCellWidth1);
        //    tableCellProperties1.Append(shading1);

        //    Paragraph paragraph11 = new Paragraph() { RsidParagraphMarkRevision = "00FA5335", RsidParagraphAddition = "00FA5335", RsidParagraphProperties = "00FA5335", RsidRunAdditionDefault = "00FA5335" };

        //    ParagraphProperties paragraphProperties11 = new ParagraphProperties();
        //    SpacingBetweenLines spacingBetweenLines11 = new SpacingBetweenLines() { After = "0", Line = "240", LineRule = LineSpacingRuleValues.Auto };
        //    Justification justification11 = new Justification() { Val = JustificationValues.Center };

        //    ParagraphMarkRunProperties paragraphMarkRunProperties11 = new ParagraphMarkRunProperties();
        //    FontSize fontSize24 = new FontSize() { Val = "20" };

        //    paragraphMarkRunProperties11.Append(fontSize24);

        //    paragraphProperties11.Append(spacingBetweenLines11);
        //    paragraphProperties11.Append(justification11);
        //    paragraphProperties11.Append(paragraphMarkRunProperties11);

        //    Run run14 = new Run() { RsidRunProperties = "00FA5335" };

        //    RunProperties runProperties14 = new RunProperties();
        //    FontSize fontSize25 = new FontSize() { Val = "20" };

        //    runProperties14.Append(fontSize25);
        //    Text text13 = new Text();
        //    text13.Text = "Время";

        //    run14.Append(runProperties14);
        //    run14.Append(text13);

        //    paragraph11.Append(paragraphProperties11);
        //    paragraph11.Append(run14);

        //    tableCell1.Append(tableCellProperties1);
        //    tableCell1.Append(paragraph11);

        //    var HeaderCells = new List<TableCell>();

        //    for (int i = 0; i < Schedule.Count; i++)
        //    {
        //        TableCell tableCell2 = new TableCell();

        //        TableCellProperties tableCellProperties2 = new TableCellProperties();
        //        TableCellWidth tableCellWidth2 = new TableCellWidth() { Width = colWidth.ToString(), Type = TableWidthUnitValues.Dxa };
        //        Shading shading2 = new Shading() { Val = ShadingPatternValues.Clear, Color = "auto", Fill = "auto" };

        //        tableCellProperties2.Append(tableCellWidth2);
        //        tableCellProperties2.Append(shading2);

        //        Paragraph paragraph12 = new Paragraph() { RsidParagraphMarkRevision = "00FA5335", RsidParagraphAddition = "00FA5335", RsidParagraphProperties = "00FA5335", RsidRunAdditionDefault = "00FA5335" };

        //        ParagraphProperties paragraphProperties12 = new ParagraphProperties();
        //        SpacingBetweenLines spacingBetweenLines12 = new SpacingBetweenLines() { After = "0", Line = "240", LineRule = LineSpacingRuleValues.Auto };
        //        Justification justification12 = new Justification() { Val = JustificationValues.Center };

        //        ParagraphMarkRunProperties paragraphMarkRunProperties12 = new ParagraphMarkRunProperties();
        //        FontSize fontSize26 = new FontSize() { Val = "20" };

        //        paragraphMarkRunProperties12.Append(fontSize26);

        //        paragraphProperties12.Append(spacingBetweenLines12);
        //        paragraphProperties12.Append(justification12);
        //        paragraphProperties12.Append(paragraphMarkRunProperties12);

        //        Run run15 = new Run() { RsidRunProperties = "00FA5335" };

        //        RunProperties runProperties15 = new RunProperties();
        //        FontSize fontSize27 = new FontSize() { Val = "20" };

        //        runProperties15.Append(fontSize27);
        //        Text text14 = new Text();
        //        text14.Text = groupNames[i];

        //        run15.Append(runProperties15);
        //        run15.Append(text14);

        //        paragraph12.Append(paragraphProperties12);
        //        paragraph12.Append(run15);

        //        tableCell2.Append(tableCellProperties2);
        //        tableCell2.Append(paragraph12);

        //        HeaderCells.Add(tableCell2);
        //    }

        //    tableRow1.Append(tablePropertyExceptions1);
        //    tableRow1.Append(tableCell1);
        //    foreach (var tc in HeaderCells)
        //    {
        //        tableRow1.Append(tc);
        //    }
        //    // FINISH --------- Ряд имён групп

        //    var timeList = new List<string>();
        //    timeList.Add("12:22");
        //    timeList.Add("13:23");
        //    timeList.Add("14:24");
        //    foreach (var group in Schedule)
        //    {
        //        foreach (var time in group.Value.Keys)
        //        {
        //            //if (!timeList.Contains(time))
        //            //{
        //            //    timeList.Add(time);
        //            //}
        //        }
        //    }

        //    var ScheduleRows = new List<TableRow>();

        //    var timeRowIndexList = new List<int>();

        //    foreach (var time in timeList)
        //    {
        //        var TimeSchedule = new List<List<string>>();
        //        for (int i = 0; i < Schedule.Count; i++)
        //        {
        //            TimeSchedule.Add(new List<string>());
        //        }

        //        var Hour = int.Parse(time.Substring(0, 2));
        //        var Minute = int.Parse(time.Substring(3, 2));

        //        Minute += 80;

        //        while (Minute >= 60)
        //        {
        //            Hour++;
        //            Minute -= 60;
        //        }

        //        var timeString = time + " - " + Hour.ToString("D2") + ":" + Minute.ToString("D2");

        //        var columnGroupIndex = 0;
        //        foreach (var group in Schedule)
        //        {
        //            if (group.Value.ContainsKey(time))
        //            {
        //                var eventCount = group.Value[time].Count;

        //                var tfdIndex = 0;
        //                foreach (var tfdData in group.Value[time].OrderBy(tfd => tfd.Value.Item2.Select(l => 2).Min()))
        //                {
        //                    var cellText = "";
        //                    cellText += tfdData.Value.Item2[0];
        //                    var groupId = tfdData.Value.Item2[0];

        //                    if (plainGroupsListIds.ContainsKey(group.Key))
        //                    {
        //                        if (plainGroupsListIds[group.Key].Contains(groupId) && nGroupsListIds[group.Key].Contains(groupId))
        //                        {
        //                            cellText += " (+Н)";
        //                        }
        //                        if (!plainGroupsListIds[group.Key].Contains(groupId) && nGroupsListIds[group.Key].Contains(groupId))
        //                        {
        //                            cellText += " (Н)";
        //                        }
        //                    }
        //                    cellText += Environment.NewLine;
        //                    cellText += /*tfdData.Value.Item2[0].TeacherForDiscipline.Teacher.FIO +*/ Environment.NewLine;
        //                    cellText += "(" + tfdData.Value.Item1 + ")" + Environment.NewLine;

        //                    var audWeekList = tfdData.Value.Item2.ToDictionary(l => l+1, l => l+1);
        //                    var grouped = audWeekList.GroupBy(a => a.Value);

        //                    //var enumerable = grouped as List<IGrouping<string, KeyValuePair<int, string>>> ?? grouped.ToList();
        //                    //var gcount = enumerable.Count();
        //                    //if (gcount == 1)
        //                    //{
        //                    //    cellText += enumerable.ElementAt(0).Key;
        //                    //}
        //                    //else
        //                    //{
        //                    //    for (int j = 0; j < gcount; j++)
        //                    //    {
        //                    //        var jItem = enumerable.OrderBy(e => e.Select(ag => ag.Key).ToList().Min()).ElementAt(j);
        //                    //        cellText += "MojWlasnyTekst" + " - " + "MojWlasnyTekst";

        //                    //        if (j != gcount - 1)
        //                    //        {
        //                    //            cellText += Environment.NewLine;
        //                    //        }
        //                    //    }
        //                    //}

        //                    TimeSchedule[columnGroupIndex].Add(cellText);

        //                    tfdIndex++;

        //                }
        //            }

        //            columnGroupIndex++;
        //        }

        //        // ====================================================================================================================================================

        //        TableRow tableRow2 = new TableRow() { RsidTableRowMarkRevision = "00FA5335", RsidTableRowAddition = "00FA5335", RsidTableRowProperties = "00FA5335" };

        //        TablePropertyExceptions tablePropertyExceptions2 = new TablePropertyExceptions();

        //        TableCellMarginDefault tableCellMarginDefault2 = new TableCellMarginDefault();
        //        TopMargin topMargin2 = new TopMargin() { Width = "0", Type = TableWidthUnitValues.Dxa };
        //        BottomMargin bottomMargin2 = new BottomMargin() { Width = "0", Type = TableWidthUnitValues.Dxa };

        //        tableCellMarginDefault2.Append(topMargin2);
        //        tableCellMarginDefault2.Append(bottomMargin2);

        //        tablePropertyExceptions2.Append(tableCellMarginDefault2);

        //        TableCell tableCell4 = new TableCell();

        //        TableCellProperties tableCellProperties4 = new TableCellProperties();
        //        TableCellWidth tableCellWidth4 = new TableCellWidth() { Width = "1383", Type = TableWidthUnitValues.Dxa };
        //        Shading shading4 = new Shading() { Val = ShadingPatternValues.Clear, Color = "auto", Fill = "auto" };
        //        TableCellVerticalAlignment tableCellVerticalAlignment1 = new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center };

        //        tableCellProperties4.Append(tableCellWidth4);
        //        tableCellProperties4.Append(shading4);
        //        tableCellProperties4.Append(tableCellVerticalAlignment1);

        //        Paragraph paragraph14 = new Paragraph() { RsidParagraphMarkRevision = "00FA5335", RsidParagraphAddition = "00FA5335", RsidParagraphProperties = "00FA5335", RsidRunAdditionDefault = "00FA5335" };

        //        ParagraphProperties paragraphProperties14 = new ParagraphProperties();
        //        SpacingBetweenLines spacingBetweenLines14 = new SpacingBetweenLines() { After = "0", Line = "240", LineRule = LineSpacingRuleValues.Auto };
        //        Justification justification14 = new Justification() { Val = JustificationValues.Center };

        //        ParagraphMarkRunProperties paragraphMarkRunProperties14 = new ParagraphMarkRunProperties();
        //        FontSize fontSize30 = new FontSize() { Val = "20" };

        //        paragraphMarkRunProperties14.Append(fontSize30);

        //        paragraphProperties14.Append(spacingBetweenLines14);
        //        paragraphProperties14.Append(justification14);
        //        paragraphProperties14.Append(paragraphMarkRunProperties14);

        //        Run run17 = new Run() { RsidRunProperties = "00FA5335" };

        //        RunProperties runProperties17 = new RunProperties();
        //        FontSize fontSize31 = new FontSize() { Val = "20" };

        //        runProperties17.Append(fontSize31);
        //        Text text16 = new Text();
        //        text16.Text = timeString;

        //        run17.Append(runProperties17);
        //        run17.Append(text16);

        //        paragraph14.Append(paragraphProperties14);
        //        paragraph14.Append(run17);

        //        tableCell4.Append(tableCellProperties4);
        //        tableCell4.Append(paragraph14);

        //        var TimeScheduleTableCells = new List<TableCell>();

        //        for (int i = 0; i < TimeSchedule.Count; i++)
        //        {
        //            TableCell tableCell6 = new TableCell();

        //            TableCellProperties tableCellProperties6 = new TableCellProperties();
        //            TableCellWidth tableCellWidth6 = new TableCellWidth() { Width = colWidth.ToString(), Type = TableWidthUnitValues.Dxa };
        //            Shading shading6 = new Shading() { Val = ShadingPatternValues.Clear, Color = "auto", Fill = "auto" };
        //            TableCellVerticalAlignment tableCellVerticalAlignment2 = new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center };

        //            tableCellProperties6.Append(tableCellWidth6);
        //            tableCellProperties6.Append(shading6);
        //            tableCellProperties6.Append(tableCellVerticalAlignment2);

        //            if (TimeSchedule[i].Count == 0) // Empty cell
        //            {
        //                Paragraph paragraph15 = new Paragraph() { RsidParagraphMarkRevision = "00FA5335", RsidParagraphAddition = "00FA5335", RsidParagraphProperties = "00FA5335", RsidRunAdditionDefault = "00FA5335" };

        //                ParagraphProperties paragraphProperties15 = new ParagraphProperties();
        //                SpacingBetweenLines spacingBetweenLines15 = new SpacingBetweenLines() { After = "0", Line = "240", LineRule = LineSpacingRuleValues.Auto };

        //                ParagraphMarkRunProperties paragraphMarkRunProperties15 = new ParagraphMarkRunProperties();
        //                FontSize fontSize32 = new FontSize() { Val = "20" };

        //                paragraphMarkRunProperties15.Append(fontSize32);

        //                paragraphProperties15.Append(spacingBetweenLines15);
        //                paragraphProperties15.Append(paragraphMarkRunProperties15);

        //                paragraph15.Append(paragraphProperties15);

        //                tableCell6.Append(tableCellProperties6);
        //                tableCell6.Append(paragraph15);
        //            }
        //            else
        //            {
        //                Table table2 = new Table();

        //                TableProperties tableProperties2 = new TableProperties();
        //                TableWidth tableWidth2 = new TableWidth() { Width = "0", Type = TableWidthUnitValues.Auto };
        //                TableLayout tableLayout2 = new TableLayout() { Type = TableLayoutValues.Fixed };
        //                TableLook tableLook2 = new TableLook() { Val = "0000" };

        //                TableBorders tableBorders9 = new TableBorders();
        //                TopBorder topBorder9 = new TopBorder() { Val = BorderValues.None, Color = "auto", Size = (UInt32Value)4U, Space = (UInt32Value)0U };
        //                LeftBorder leftBorder9 = new LeftBorder() { Val = BorderValues.None, Color = "auto", Size = (UInt32Value)4U, Space = (UInt32Value)0U };
        //                BottomBorder bottomBorder9 = new BottomBorder() { Val = BorderValues.None, Color = "000000", Size = (UInt32Value)4U, Space = (UInt32Value)0U };
        //                RightBorder rightBorder9 = new RightBorder() { Val = BorderValues.None, Color = "auto", Size = (UInt32Value)4U, Space = (UInt32Value)0U };
        //                InsideHorizontalBorder insideHorizontalBorder9 = new InsideHorizontalBorder() { Val = BorderValues.Single, Color = "000000", Size = (UInt32Value)4U, Space = (UInt32Value)0U };
        //                InsideVerticalBorder insideVerticalBorder9 = new InsideVerticalBorder() { Val = BorderValues.None, Color = "auto", Size = (UInt32Value)4U, Space = (UInt32Value)0U };

        //                tableBorders9.Append(topBorder9);
        //                tableBorders9.Append(leftBorder9);
        //                tableBorders9.Append(bottomBorder9);
        //                tableBorders9.Append(rightBorder9);
        //                tableBorders9.Append(insideHorizontalBorder9);
        //                tableBorders9.Append(insideVerticalBorder9);

        //                tableProperties2.Append(tableWidth2);
        //                tableProperties2.Append(tableBorders9);
        //                tableProperties2.Append(tableLayout2);
        //                tableProperties2.Append(tableLook2);

        //                TableGrid tableGrid2 = new TableGrid();
        //                GridColumn gridColumn4 = new GridColumn() { Width = "7052" };

        //                tableGrid2.Append(gridColumn4);

        //                for (int j = 0; j < TimeSchedule[i].Count; j++)
        //                {
        //                    var item = TimeSchedule[i][j];
        //                    var itemLines = Regex.Split(item, "\r\n");

        //                    TableRow tableRow3 = new TableRow() { RsidTableRowMarkRevision = "00FA5335", RsidTableRowAddition = "00FA5335", RsidTableRowProperties = "00FA5335" };

        //                    TablePropertyExceptions tablePropertyExceptions3 = new TablePropertyExceptions();

        //                    TableCellMarginDefault tableCellMarginDefault3 = new TableCellMarginDefault();
        //                    TopMargin topMargin3 = new TopMargin() { Width = "0", Type = TableWidthUnitValues.Dxa };
        //                    BottomMargin bottomMargin3 = new BottomMargin() { Width = "0", Type = TableWidthUnitValues.Dxa };

        //                    tableCellMarginDefault3.Append(topMargin3);
        //                    tableCellMarginDefault3.Append(bottomMargin3);

        //                    tablePropertyExceptions3.Append(tableCellMarginDefault3);

        //                    TableCell tableCell7 = new TableCell();

        //                    TableCellProperties tableCellProperties7 = new TableCellProperties();
        //                    TableCellWidth tableCellWidth7 = new TableCellWidth() { Width = "7052", Type = TableWidthUnitValues.Dxa };
        //                    TableCellVerticalAlignment tableCellVerticalAlignment3 = new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center };

        //                    TableCellBorders tableCellBorders99 = new TableCellBorders();
        //                    BottomBorder bottomBorder99;
        //                    if (j != TimeSchedule[i].Count - 1)
        //                    {
        //                        bottomBorder99 = new BottomBorder() { Val = BorderValues.Single, Color = "auto", Size = (UInt32Value)4U, Space = (UInt32Value)0U };
        //                        tableCellBorders99.Append(bottomBorder99);
        //                    }

        //                    tableCellProperties7.Append(tableCellWidth7);
        //                    tableCellProperties7.Append(tableCellBorders99);
        //                    tableCellProperties7.Append(tableCellVerticalAlignment3);

        //                    Paragraph paragraph16 = new Paragraph() { RsidParagraphMarkRevision = "00FA5335", RsidParagraphAddition = "00FA5335", RsidParagraphProperties = "00FA5335", RsidRunAdditionDefault = "00FA5335" };

        //                    ParagraphProperties paragraphProperties16 = new ParagraphProperties();
        //                    SpacingBetweenLines spacingBetweenLines16 = new SpacingBetweenLines() { After = "0", Line = "240", LineRule = LineSpacingRuleValues.Auto };

        //                    ParagraphMarkRunProperties paragraphMarkRunProperties16 = new ParagraphMarkRunProperties();
        //                    FontSize fontSize33 = new FontSize() { Val = "20" };

        //                    paragraphMarkRunProperties16.Append(fontSize33);

        //                    paragraphProperties16.Append(spacingBetweenLines16);
        //                    paragraphProperties16.Append(paragraphMarkRunProperties16);

        //                    Run run18 = new Run() { RsidRunProperties = "00FA5335" };

        //                    RunProperties runProperties18 = new RunProperties();
        //                    FontSize fontSize34 = new FontSize() { Val = "20" };

        //                    runProperties18.Append(fontSize34);
        //                    Text text17 = new Text();
        //                    text17.Text = itemLines[0];

        //                    run18.Append(runProperties18);
        //                    run18.Append(text17);

        //                    paragraph16.Append(paragraphProperties16);
        //                    paragraph16.Append(run18);

        //                    Paragraph paragraph17 = new Paragraph() { RsidParagraphMarkRevision = "00FA5335", RsidParagraphAddition = "00FA5335", RsidParagraphProperties = "00FA5335", RsidRunAdditionDefault = "00FA5335" };

        //                    ParagraphProperties paragraphProperties17 = new ParagraphProperties();
        //                    SpacingBetweenLines spacingBetweenLines17 = new SpacingBetweenLines() { After = "0", Line = "240", LineRule = LineSpacingRuleValues.Auto };

        //                    ParagraphMarkRunProperties paragraphMarkRunProperties17 = new ParagraphMarkRunProperties();
        //                    FontSize fontSize35 = new FontSize() { Val = "20" };

        //                    paragraphMarkRunProperties17.Append(fontSize35);

        //                    paragraphProperties17.Append(spacingBetweenLines17);
        //                    paragraphProperties17.Append(paragraphMarkRunProperties17);

        //                    Run run19 = new Run() { RsidRunProperties = "00FA5335" };

        //                    RunProperties runProperties19 = new RunProperties();
        //                    FontSize fontSize36 = new FontSize() { Val = "20" };

        //                    runProperties19.Append(fontSize36);
        //                    Text text18 = new Text();
        //                    text18.Text = itemLines[1];

        //                    run19.Append(runProperties19);
        //                    run19.Append(text18);

        //                    paragraph17.Append(paragraphProperties17);
        //                    paragraph17.Append(run19);

        //                    Paragraph paragraph18 = new Paragraph() { RsidParagraphMarkRevision = "00FA5335", RsidParagraphAddition = "00FA5335", RsidParagraphProperties = "00FA5335", RsidRunAdditionDefault = "00FA5335" };

        //                    ParagraphProperties paragraphProperties18 = new ParagraphProperties();
        //                    SpacingBetweenLines spacingBetweenLines18 = new SpacingBetweenLines() { After = "0", Line = "240", LineRule = LineSpacingRuleValues.Auto };

        //                    ParagraphMarkRunProperties paragraphMarkRunProperties18 = new ParagraphMarkRunProperties();
        //                    FontSize fontSize37 = new FontSize() { Val = "20" };

        //                    paragraphMarkRunProperties18.Append(fontSize37);

        //                    paragraphProperties18.Append(spacingBetweenLines18);
        //                    paragraphProperties18.Append(paragraphMarkRunProperties18);

        //                    Run run20 = new Run() { RsidRunProperties = "00FA5335" };

        //                    RunProperties runProperties20 = new RunProperties();
        //                    FontSize fontSize38 = new FontSize() { Val = "20" };

        //                    runProperties20.Append(fontSize38);
        //                    Text text19 = new Text();
        //                    text19.Text = itemLines[2];

        //                    run20.Append(runProperties20);
        //                    run20.Append(text19);

        //                    paragraph18.Append(paragraphProperties18);
        //                    paragraph18.Append(run20);

        //                    Paragraph paragraph19 = new Paragraph() { RsidParagraphMarkRevision = "00FA5335", RsidParagraphAddition = "00FA5335", RsidParagraphProperties = "00FA5335", RsidRunAdditionDefault = "00FA5335" };

        //                    ParagraphProperties paragraphProperties19 = new ParagraphProperties();
        //                    SpacingBetweenLines spacingBetweenLines19 = new SpacingBetweenLines() { After = "0", Line = "240", LineRule = LineSpacingRuleValues.Auto };

        //                    ParagraphMarkRunProperties paragraphMarkRunProperties19 = new ParagraphMarkRunProperties();
        //                    FontSize fontSize39 = new FontSize() { Val = "20" };

        //                    paragraphMarkRunProperties19.Append(fontSize39);

        //                    paragraphProperties19.Append(spacingBetweenLines19);
        //                    paragraphProperties19.Append(paragraphMarkRunProperties19);

        //                    Run run21 = new Run() { RsidRunProperties = "00FA5335" };

        //                    RunProperties runProperties21 = new RunProperties();
        //                    FontSize fontSize40 = new FontSize() { Val = "20" };

        //                    runProperties21.Append(fontSize40);
        //                    Text text20 = new Text();
        //                    text20.Text = itemLines[3];

        //                    run21.Append(runProperties21);
        //                    run21.Append(text20);

        //                    paragraph19.Append(paragraphProperties19);
        //                    paragraph19.Append(run21);

        //                    tableCell7.Append(tableCellProperties7);
        //                    tableCell7.Append(paragraph16);
        //                    tableCell7.Append(paragraph17);
        //                    tableCell7.Append(paragraph18);
        //                    tableCell7.Append(paragraph19);

        //                    tableRow3.Append(tablePropertyExceptions3);
        //                    tableRow3.Append(tableCell7);

        //                    table2.Append(tableRow3);
        //                }

        //                table2.Append(tableProperties2);
        //                table2.Append(tableGrid2);

        //                Paragraph paragraph20 = new Paragraph() { RsidParagraphMarkRevision = "00FA5335", RsidParagraphAddition = "00FA5335", RsidParagraphProperties = "00FA5335", RsidRunAdditionDefault = "00FA5335" };

        //                ParagraphProperties paragraphProperties20 = new ParagraphProperties();
        //                SpacingBetweenLines spacingBetweenLines20 = new SpacingBetweenLines() { After = "0", Line = "240", LineRule = LineSpacingRuleValues.Auto };

        //                ParagraphMarkRunProperties paragraphMarkRunProperties20 = new ParagraphMarkRunProperties();
        //                FontSize fontSize41 = new FontSize() { Val = "20" };

        //                paragraphMarkRunProperties20.Append(fontSize41);

        //                paragraphProperties20.Append(spacingBetweenLines20);
        //                paragraphProperties20.Append(paragraphMarkRunProperties20);

        //                paragraph20.Append(paragraphProperties20);

        //                tableCell6.Append(tableCellProperties6);
        //                tableCell6.Append(table2);
        //                tableCell6.Append(paragraph20);
        //            }

        //            TimeScheduleTableCells.Add(tableCell6);
        //        }

        //        tableRow2.Append(tablePropertyExceptions2);
        //        tableRow2.Append(tableCell4);

        //        foreach (var tc in TimeScheduleTableCells)
        //        {
        //            tableRow2.Append(tc);
        //        }

        //        ScheduleRows.Add(tableRow2);
        //    }

        //    table1.Append(tableProperties1);
        //    table1.Append(tableGrid1);
        //    table1.Append(tableRow1);

        //    foreach (var tr in ScheduleRows)
        //    {
        //        table1.Append(tr);
        //    }

        //    Paragraph paragraph61 = new Paragraph() { RsidParagraphMarkRevision = "00FA5335", RsidParagraphAddition = "00FA5335", RsidParagraphProperties = "00FA5335", RsidRunAdditionDefault = "00FA5335" };

        //    ParagraphProperties paragraphProperties61 = new ParagraphProperties();
        //    SpacingBetweenLines spacingBetweenLines61 = new SpacingBetweenLines() { After = "0", Line = "240", LineRule = LineSpacingRuleValues.Auto };
        //    Justification justification19 = new Justification() { Val = JustificationValues.Center };

        //    ParagraphMarkRunProperties paragraphMarkRunProperties61 = new ParagraphMarkRunProperties();
        //    Bold bold3 = new Bold();
        //    FontSize fontSize114 = new FontSize() { Val = "20" };

        //    paragraphMarkRunProperties61.Append(bold3);
        //    paragraphMarkRunProperties61.Append(fontSize114);

        //    paragraphProperties61.Append(spacingBetweenLines61);
        //    paragraphProperties61.Append(justification19);
        //    paragraphProperties61.Append(paragraphMarkRunProperties61);

        //    paragraph61.Append(paragraphProperties61);

        //    SectionProperties sectionProperties1 = new SectionProperties() { RsidRPr = "00FA5335", RsidR = "00FA5335", RsidSect = "00FA5335" };
        //    PageSize pageSize1 = new PageSize() { Width = (UInt32Value)16838U, Height = (UInt32Value)11906U, Orient = PageOrientationValues.Landscape };
        //    PageMargin pageMargin1 = new PageMargin() { Top = 567, Right = (UInt32Value)567U, Bottom = 567, Left = (UInt32Value)567U, Header = (UInt32Value)708U, Footer = (UInt32Value)708U, Gutter = (UInt32Value)0U };
        //    Columns columns1 = new Columns() { Space = "708" };
        //    DocGrid docGrid1 = new DocGrid() { LinePitch = 360 };

        //    sectionProperties1.Append(pageSize1);
        //    sectionProperties1.Append(pageMargin1);
        //    sectionProperties1.Append(columns1);
        //    sectionProperties1.Append(docGrid1);

        //    body1.Append(paragraph1);
        //    body1.Append(paragraph8);
        //    body1.Append(paragraph9);
        //    body1.Append(paragraph10);
        //    body1.Append(table1);
        //    body1.Append(paragraph61);
        //    body1.Append(sectionProperties1);

        //    document1.Append(body1);

        //    wordDoc.MainDocumentPart.Document = document1;
        //    wordDoc.Close();
        //}
    }

}
