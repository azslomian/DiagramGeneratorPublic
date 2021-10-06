using DiagramGenerator.Domain.Dtos.Abstract;
using System.Collections.Generic;

namespace DiagramGenerator.DataAccess.Model
{
    public class DiagramDto : EntityDto
    {
        public ProcessDto Process { get; set; }

        public IList<InputDto> Inputs {get; set; }

        public IList<OutputDto> Outputs { get; set; }

        public IList<MethodDto> Methods { get; set; }

        public IList<RequirementDto> Requirements { get; set; }

        public IList<CriterionDto> Criteria { get; set; }
    }
}
