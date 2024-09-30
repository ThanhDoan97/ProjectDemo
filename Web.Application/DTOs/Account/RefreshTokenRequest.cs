using System;
using System.Collections.Generic;
using System.Text;

namespace CMC.TS.EDU.UMS.Biz.Model.Account
{
    public class RefreshTokenRequest
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
