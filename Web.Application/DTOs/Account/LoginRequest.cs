using System.ComponentModel.DataAnnotations;

namespace CMC.TS.EDU.UMS.Biz.Model.Account
{
    public class LoginRequest
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    public class AzureRedeemRequest
    {
        [Required]
        public string Tenant { get; set; }
        [Required]
        public string Client_ID { get; set; }
        [Required]
        public string Code { get; set; }
        [Required]
        public string Redirect_Uri { get; set; }
        [Required]
        public string Grant_Type { get; set; }
        public string Client_Secret { get; set; }
        public string Scope { get; set; }
        public string Code_Verifier { get; set; }

        public string DeviceID { get; set; }
        public string NotiID { get; set; }
        public bool IgnoreLogging { get; set; }
    }
    public class AzureRedeemResponse
    {
        public string Token_Type { get; set; }
        public string Scope { get; set; }
        public string Expires_In { get; set; }
        public string Ext_Expires_In { get; set; }
        public string Access_Token { get; set; }
    }
}
