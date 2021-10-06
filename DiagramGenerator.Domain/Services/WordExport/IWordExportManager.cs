using DiagramGenerator.DataAccess.Model;
using DocumentFormat.OpenXml.Packaging;

namespace DiagramGenerator.Domain.Services.Interfaces
{
    public interface IWordExportManager
    {
        void Export(Diagram diagram, WordprocessingDocument wordDoc);
    }
}