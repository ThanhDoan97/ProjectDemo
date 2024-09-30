using System;
using System.Collections.Generic;
using System.Text;

namespace CMC.TS.EDU.UMS.Biz.Model.Account
{
    public class LoginResult
    {
        public long UserID { get; set; }
        public string UserName { get; set; }
        public string Fullname { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Avatar { get; set; }

        public long RoleID { get; set; }
        public string Role { get; set; }
        public string RoleName { get; set; }

        public long DepartmentID { get; set; }
        public string Department { get; set; }
        public string DepartmentName { get; set; }

        public string OriginalUserName { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public string OrgCode { get; set; }

        public long? KhoaID { get; set; }
        public string MaKhoa { get; set; }
        public string TenKhoa { get; set; }


        public long? BoMonID { get; set; }
        public string MaBoMon { get; set; }
        public string TenBoMon { get; set; }

        public bool IsAdmin { get; set; }
        public bool IsSystem { get; set; }
      //  public List<Model.Permission.MenuView> Permissions { get; set; }
        public List<string> Owners { get; set; }

    }

    public class LoginStudenResult
    {
        public long UserID { get; set; }
        public string UserName { get; set; }
        public string Fullname { get; set; }
        public string Avatar { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public string OrgCode { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsSystem { get; set; }
        public string OriginalUserName { get; set; }
    }

    public class LoginIntegrationResult
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
