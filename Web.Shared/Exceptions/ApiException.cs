using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Shared.Exceptions
{
    public class ApiException : Exception
    {
        public int StatusCode { get; }
        public string ErrorMessage { get; }
        public ApiException(int statusCode , string errorMessage) { 
            StatusCode = statusCode;
            ErrorMessage = errorMessage;
        }
    }
}
