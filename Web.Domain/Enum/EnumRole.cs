using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Domain.Enum
{
    public enum EnumRole
    {
        [Description("Quản trị")]
        Admin = 1,
        [Description("Người dùng")]
        User = 2

    }
}
