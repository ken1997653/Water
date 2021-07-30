﻿using AccountingNote.DBSouse;
using AuthManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AccountingNote.SystemAdmin
{
	public partial class AccountingList : System.Web.UI.Page
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

			DataTable dt = AccountingManager.GetAccountingList(currentUser.ID);
			if (dt.Rows.Count > 0)
			{
				this.gvAccountingList.DataSource = dt;
				this.gvAccountingList.DataBind();
				
			}
			else
			{
				this.gvAccountingList.Visible = false;
				this.plcNoData.Visible = true;
			}
			
		
		}

		protected void BtnCreate_Click(object sender, EventArgs e)
		{
			Response.Redirect("/SystemAdmin/AccountingDetail.aspx");
		}

		protected void gvAccountingList_RowDataBound(object sender, GridViewRowEventArgs e)
		{
			var row = e.Row;

			if (row.RowType == DataControlRowType.DataRow)
			{
				Label lbl = row.FindControl("lblActType") as Label;
				var dr = row.DataItem as DataRowView;
				int actType = dr.Row.Field<int>("ActType");

				if (actType == 0)
				{
					lbl.Text = "支出";

				}
				else 
				{
					lbl.Text = "收入";

				}
				if (dr.Row.Field<int>("Amount") > 1500)
				{
					lbl.ForeColor = Color.Red;
				}

			}
		}
	}

}