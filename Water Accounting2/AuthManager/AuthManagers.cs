using AccountingNote.DBSouse;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace AuthManager
{
    public class AuthManagers
    {
        public static bool IsLogined()
        {
            if (HttpContext.Current.Session["UserLoginInfo"] == null)
                return false;
            else
                return true;
        }
        public static UserInfoModel GetCurrentUser()
        {
            string account = HttpContext.Current.Session["UserLoginInfo"] as string;

            if (account == null)
                return null;

            DataRow dr = UserInfoManager.GetUserInfoByAccount(account);

            if (dr == null)
            {
                HttpContext.Current.Session["UserLoginINfo"] = null;
                return null;
            }
            UserInfoModel model = new UserInfoModel();
            model.ID = dr["ID"].ToString();
            model.Account = dr["Account"].ToString();
            model.Name = dr["Name"].ToString();
            model.Email = dr["Email"].ToString();

            return model;

        }
        public static void Logout()
        {
            HttpContext.Current.Session["UserLoginINfo"] = null;
        }

        public static bool TryLogin(string account, string pwd, out string errmsg)
        {
            if (string.IsNullOrWhiteSpace(account) || string.IsNullOrWhiteSpace(pwd))
            {
                errmsg = "Account/Password is required";
                return false;
            }
            DataRow dr = UserInfoManager.GetUserInfoByAccount(account);
            if (dr == null)
            {
                errmsg = $"Account:{account} doesn't exist";
                return false;
            }
            if (string.Compare(dr["PWD"].ToString(), pwd) == 0)
            {
                HttpContext.Current.Session["UserLoginINfo"] = dr["Account"].ToString();
                errmsg = string.Empty;
                return true;
            }
            else 
            {
                errmsg = "Login faile, please check Account/password.";
                   return false;
            }
        }
    }

}
