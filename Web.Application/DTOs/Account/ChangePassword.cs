namespace CMC.TS.EDU.UMS.Biz.Model.Account
{
    public class ChangePassword
    {
        public string CurrentPass { get; set; }
        public string NewPass { get; set; }
    }

    public class ForgotPassword
    {
        public string Email { get; set; }
    }
}
