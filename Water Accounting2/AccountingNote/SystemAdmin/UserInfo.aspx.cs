using AccountingNote.DBSouse;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AuthManager;

namespace AccountingNote.SystemAdmin
{
	public partial class UserInfo : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!this.IsPostBack)
			{
				if (!AuthManagers.IsLogined())
				{
					Response.Redirect("/Login.aspx");
					return;
				}
				UserInfoModel currentUser = AuthManagers.GetCurrentUser();

				//string account = this.Session["UserLoginInfo"] as string;
				//DataRow dr = UserInfoManager.GetUserInfoByAccount(account);
				if (currentUser == null)
				{
					
					Response.Redirect("/Login.aspx");
					return;
				}

				this.ltlAccount.Text = currentUser.Account;
				this.ltlName.Text = currentUser.Name;
				this.ltlEmail.Text = currentUser.Email;
			}

	    }

		protected void btnLogout_Click(object sender, EventArgs e)
		{
			AuthManagers.Logout();
			Response.Redirect("/Login.aspx");
		}
	}
}