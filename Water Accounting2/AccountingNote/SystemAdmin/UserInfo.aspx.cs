using AccountingNote.DBSouse;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AccountingNote.SystemAdmin
{
	public partial class UserInfo : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (this.Session["UserLoginInfo"] == null)
			{
				Response.Redirect("/Login.aspx");
				return;
			}
			string account = this.Session["UserLoginInfo"] as string;
			DataRow dr = UserInfoManager.GetUserInfoByAccount(account);
			if (dr == null)
			{
				this.Session["UserLoginInfo"] = null;
				Response.Redirect("/Login.aspx");
				return;
			}
			this.ltlAccount.Text = dr["Account"].ToString();
			this.ltlName.Text = dr["Name"].ToString();
			this.ltlEmail.Text = dr["Email"].ToString();
		}

		protected void btnLogout_Click(object sender, EventArgs e)
		{
			this.Session["UserLoginInfo"] = null;
			Response.Redirect("/Login.aspx");
		}
	}
}