using DiagramGenerator.DataAccess.Model;
using Syncfusion.Pdf;

namespace DiagramGenerator.Domain.Services.Interfaces
{
    public interface IPdfExportManager
    {
        PdfDocument Export(Diagram diagram);
    }
}