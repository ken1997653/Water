
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AccountingNote.DBSouse;

namespace AccountingNote
{
	public partial class Login : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (this.Session["UserLoginInfo"] != null)
			{
				this.plcLogin.Visible = false;
				Response.Redirect("/SystemAdmin/UserInfo.aspx");
			}
			else
			{
				this.plcLogin.Visible = true;
			}
		}

		protected void btnLogin_Click(object sender, EventArgs e)
		{
	

			string inp_Account = this.txtAccount.Text;
			string inp_PWD = this.txtPwd.Text;

			if (string.IsNullOrWhiteSpace(inp_Account) || string.IsNullOrWhiteSpace(inp_PWD))
			{
				this.ltlMsg.Text = "Account / PWD is required.";
				return;
			}
			DataRow dr = UserInfoManager.GetUserInfoByAccount(inp_Account);


			if (dr!=null&&string.Compare(dr["PWD"].ToString(),inp_PWD)==0)
			{
				this.Session["UserLoginInfo"] = dr["Account"];
				Response.Redirect("/SystemAdmin/UserInfo.aspx");
			}
			else
			{
				this.ltlMsg.Text = "Login fail. Plesae check Account/PWD.";
				return;
			}
			
		}
	}
}