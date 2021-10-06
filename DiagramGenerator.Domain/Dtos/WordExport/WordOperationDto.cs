using System;

namespace DiagramGenerator.Domain.Dtos.WordExport
{
    public class WordOperationDto : WordEntityDto
    {
        public int TimeInMinutes { get; set; }

        public int Employees { get; set; }

        public string Type { get; set; }
    }
}
