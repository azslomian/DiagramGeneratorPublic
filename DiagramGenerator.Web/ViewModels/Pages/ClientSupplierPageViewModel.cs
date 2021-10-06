using System.Collections.Generic;

namespace DiagramGenerator.Web.ViewModels.Pages
{
    public class ClientSupplierPageViewModel
    {
        public IList<ClientViewModel> Clients { get; set; }

        public IList<SupplierViewModel> Suppliers { get; set; }
    }
}
