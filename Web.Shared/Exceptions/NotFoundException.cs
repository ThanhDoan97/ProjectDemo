using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Shared.Exceptions
{
    public class NotFoundException : ApiException
    {
        public NotFoundException(string message) : base(404, message)
        {
        }
    }
}
