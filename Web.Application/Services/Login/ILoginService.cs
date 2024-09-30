using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Application.Services.Login
{
    public interface ILoginService
    {

        bool IsValidUserCredentials(string userName, string password);
    }
}
