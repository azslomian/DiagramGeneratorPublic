using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiagramGenerator.Web.Services
{
    public interface IMailService
    {
        void SendMessage(string to, string name);
    }
}
