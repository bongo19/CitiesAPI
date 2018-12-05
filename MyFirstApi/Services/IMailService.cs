using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstApi.Services
{
    public interface IMailService
    {
        void SendMessage(string subject, string message);
    }
}
