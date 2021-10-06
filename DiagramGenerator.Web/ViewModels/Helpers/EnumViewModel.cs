using System.ComponentModel.DataAnnotations;

namespace DiagramGenerator.Web.ViewModels.Helpers
{
    public class EnumViewModel
    {
        [Required]
        public int Value { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
