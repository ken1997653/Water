using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountingNote.DBSouse
{
	public class AccountingManager
	{
		public static string GetConnectionString()
		{

			string val = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
			return val;
		}
		public static DataTable GetAccountingList(string userID)
		{
			string connStr = GetConnectionString();
			string DBCommand =
				$@"SELECT
                   ID,
                   Caption,
                   Amount,
                   ActType,
                   CreateDate,
                   Body
                   FROM Accounting
                   WHERE UserID=@userID
                   ";
			using (SqlConnection conn = new SqlConnection(connStr))
			{
				using (SqlCommand comm = new SqlCommand(DBCommand, conn))
				{
					comm.Parameters.AddWithValue("@userID", userID);

					try
					{
						conn.Open();
						var reader = comm.ExecuteReader();

						DataTable dt = new DataTable();
						dt.Load(reader);
						return dt;
					}
					catch (Exception ex)
					{
						Loggers.WriteLog(ex.Message);
						return null;
					}
				}
			}
		}
		public static bool UpdateAccounting(int id,string UserID, string Caption, int Amount, int ActType, string Body)
		{
			if (Amount < 0 || Amount > 1000000)
				throw new ArgumentException("Amount must betwrrn 0 and 1,000,000 ");
			if (ActType < 0 || ActType > 1)
				throw new ArgumentException("ActType must be 0 or 1");


			string connStr = GetConnectionString();
			string dbCommandString =
			@"UPDATE [Accounting]
              SET
            UserID = @userId
            ,Caption = @cap
            ,Amount = @amount
            ,ActType = @type
            ,CreateDate = @createDate
            ,Body = @body

             WHERE 
                 ID = @id";
			List<SqlParameter> paramList = new List<SqlParameter>();
			   paramList.Add(new SqlParameter("@userId", UserID));
			paramList.Add(new SqlParameter("@cap", Caption));
			paramList.Add(new SqlParameter("@amount", Amount));
			paramList.Add(new SqlParameter("@type", ActType));
			paramList.Add(new SqlParameter("@createDate", DateTime.Now));
			paramList.Add(new SqlParameter("@body", Body));
			paramList.Add(new SqlParameter("@id", id));


			try
			{
				int affect = DBHelper.ModifyData(connStr,dbCommandString,paramList);


				if (affect == 1)
					return true;
				else
					return false;
				//command.ExecuteNonQuery();


			}
			catch (Exception ex)
			{

				Loggers.WriteLog(ex.Message);
				return false;
			}
			    
		}

		public static void CreateAccounting(string UserID, string Caption, int Amount, int ActType, string Body)
		{
			if (Amount < 0 || Amount > 1000000)
				throw new ArgumentException("Amount must betwrrn 0 and 1,000,000 ");
			if (ActType != 0 && ActType != 1)
				throw new ArgumentException("ActType must be 0 or 1");


			string connectionString = GetConnectionString();
			string dbCommandString =
			@"INSERT INTO Accounting
            (UserID, Caption, Amount, ActType, CreateDate, Body)
             VALUES
            (@id, @cap, @amount, @type, @createDate, @body);
            ";
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				using (SqlCommand command = new SqlCommand(dbCommandString, connection))
				{
					command.Parameters.AddWithValue("@id", UserID);
					command.Parameters.AddWithValue("@cap", Caption);
					command.Parameters.AddWithValue("@amount", Amount);
					command.Parameters.AddWithValue("@type", ActType);
					command.Parameters.AddWithValue("@createDate", DateTime.Now);
					command.Parameters.AddWithValue("@body", Body);
					try
					{

						connection.Open();
						command.ExecuteNonQuery();


					}
					catch (Exception ex)
					{

						Loggers.WriteLog(ex.Message);
					}
				}
			}
		}
		public static DataRow GetAccounting(int id,string userID)
		{
			string connStr = GetConnectionString();
			string DBCommand =
				$@"SELECT
                   ID,
                   Caption,
                   Amount,
                   ActType,
                   CreateDate,
                   Body
                   FROM Accounting
                   WHERE id = @id AND UserID =@userID";
                   
			using (SqlConnection conn = new SqlConnection(connStr))
			{
				using (SqlCommand comm = new SqlCommand(DBCommand, conn))
				{
					comm.Parameters.AddWithValue("@id", id);
					comm.Parameters.AddWithValue("userID", userID);

					try
					{
						conn.Open();
						var reader = comm.ExecuteReader();

						DataTable dt = new DataTable();
						dt.Load(reader);

						if(dt.Rows.Count==0)
							return null;

						return dt.Rows[0];
					}
					catch (Exception ex)
					{
						Loggers.WriteLog(ex.Message);
						return null;
					}





				}


			}
		}
		public static void DeleteAccounting(int id)
		{
			string connStr = DBHelper.GetConnectionString();
			string DBCommand =
				$@" DELETE  [Accounting]
                   WHERE ID = @id";
			List<SqlParameter> paramList = new List<SqlParameter>();
			paramList.Add(new SqlParameter("@id", id));
		
			try
			{

		     DBHelper.ModifyData(connStr, DBCommand, paramList);
						
			}
			catch (Exception ex)
			{
						
		     Loggers.WriteLog(ex.Message);
						
			}
		}
	}
}
