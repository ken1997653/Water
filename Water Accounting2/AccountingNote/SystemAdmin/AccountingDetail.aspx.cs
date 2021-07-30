using AccountingNote.DBSouse;
using AuthManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AccountingNote.SystemAdmin
{
	public partial class AccountingDetail : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
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
				this.Session["UserLoginInfo"] = null;
				Response.Redirect("/Login.aspx");
				return;
			}
			if (!this.IsPostBack)
			{
				if (this.Request.QueryString["ID"] == null)
				{
					this.btnDelete.Visible = false;
				}
				else 
				{
					this.btnDelete.Visible = true;

					string idText = this.Request.QueryString["ID"];
					int id;
					if (int.TryParse(idText, out id))
					{
						var drAccounting = AccountingManager.GetAccounting(id,currentUser.ID);

						if (drAccounting == null)
						{
							this.ltlMsg.Text = "Data doesn't exist";
							this.btnSave.Visible = false;
							this.btnDelete.Visible = false;

						}
						else
						{
							this.ddlType.SelectedValue = drAccounting["ActType"].ToString();
							this.txtAmount.Text = drAccounting["Amount"].ToString();
							this.txtCaption.Text = drAccounting["Caption"].ToString();
							this.txtDesc.Text = drAccounting["Body"].ToString();
						}


					}
					else 
					{
						this.ltlMsg.Text = "ID is required.";
						this.btnSave.Visible = false;
						this.btnDelete.Visible = false;
					}

				}
			}
		}

		protected void btnSave_Click(object sender, EventArgs e)
		{
			ltlMsg.Text = "";
			List<string> msgList = new List<string>();
			if (!this.CheckInput(out msgList))
			{
				this.ltlMsg.Text = string.Join("<br/>", msgList);
				return;
			}
			//string account = this.Session["UserLoginInfo"] as string;
			//var drUserInfo = UserInfoManager.GetUserInfoByAccount(account);

			UserInfoModel currentUser = AuthManagers.GetCurrentUser();

			if (currentUser == null)
			{
				Response.Redirect("/Login.aspx");
				return;
			}

			string userID = currentUser.ID;

			string actTypeText = this.ddlType.SelectedValue;
			string amountText = this.txtAmount.Text;
			string caption = this.txtCaption.Text;
			string body = this.txtDesc.Text;

			int amount = Convert.ToInt32(amountText);
			int actType = Convert.ToInt32(actTypeText);

			string idText = this.Request.QueryString["ID"];
			if (string.IsNullOrWhiteSpace(idText))
			{
				AccountingManager.CreateAccounting(userID, caption, amount, actType, body);
			}

			else 
			{
				int id;
				if (int.TryParse(idText, out id))
				{
					AccountingManager.UpdateAccounting(id,userID, caption, amount, actType, body);
				}
				
			}
			Response.Redirect("/SystemAdmin/AccountingList.aspx");
		}
		private bool CheckInput(out List<string> errMsgList)
		{
			List<string> msgList = new List<string>();
			if (this.ddlType.SelectedValue != "0" && this.ddlType.SelectedValue != "1")
			{
				msgList.Add("Amount is required");
			}
			else
			{
				int tempInt;
				if (!int.TryParse(this.txtAmount.Text, out tempInt))
				{
					msgList.Add("Amount must be a number.");
				}
				if (tempInt < 0 || tempInt > 1000000)
				{
					msgList.Add("Amount must between 0 and 1,000,000.");
				}
			}
			errMsgList = msgList;

			if (msgList.Count == 0)
				return true;
			else
				return false;
		}

		protected void btnDelete_Click(object sender, EventArgs e)
		{
			string idText = this.Request.QueryString["ID"];

			if (string.IsNullOrWhiteSpace(idText))
				return;

			int id;
			if (int.TryParse(idText, out id))
			{
				AccountingManager.DeleteAccounting(id);
			}
			Response.Redirect("/SystemAdmin/AccountingList.aspx");
		}
	}
}