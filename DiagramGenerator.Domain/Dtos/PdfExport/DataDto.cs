using System;
using System.Collections.Generic;
using System.Text;

namespace DiagramGenerator.Domain.Dtos.PdfExport
{
    public class DataDto
    {
        public List<string> Data { get; set; }

        public DataDto()
        {
            Data = new List<string>();
        }
    }
}
