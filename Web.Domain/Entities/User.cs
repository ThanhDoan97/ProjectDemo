using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Domain.Enum;

namespace Web.Domain.Entities
{
    public class User : BaseEntity
    {
        public string Name { get; set; }    
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public EnumRole Role {  get; set; }  

    }
}
