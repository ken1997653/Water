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
	class DBHelper
	{
		public static string GetConnectionString()
		{
			//string val = ConfigurationManager.AppSettings["ConnectionString"];
			string val =
				ConfigurationManager.ConnectionStrings
				["DefaultConnection"].ConnectionString;
			return val;
		}
		public static DataTable ReadDataTable(string connStr, string DBCommand, List<SqlParameter> list)
		{
			using (SqlConnection conn = new SqlConnection(connStr))
			{
				using (SqlCommand comm = new SqlCommand(DBCommand, conn))
				{
					comm.Parameters.AddRange(list.ToArray());

										
						conn.Open();
						var reader = comm.ExecuteReader();

						DataTable dt = new DataTable();
						dt.Load(reader);
						return dt;

				}
			}
		}

		public static DataRow ReadDataRow(string connStr, string DBCommand, List<SqlParameter> list)
		{
			using (SqlConnection conn = new SqlConnection(connStr))
			{
				using (SqlCommand comm = new SqlCommand(DBCommand, conn))
				{
					comm.Parameters.AddRange(list.ToArray());


					conn.Open();
					var reader = comm.ExecuteReader();

					DataTable dt = new DataTable();
					dt.Load(reader);

					if (dt.Rows.Count == 0)
						return null;
						return dt.Rows[0];

				}
			}
		}
		public static int ModifyData(string connStr, string DBCommand, List<SqlParameter> paramList)
		{
			using (SqlConnection conn = new SqlConnection(connStr))
			{
				using (SqlCommand comm = new SqlCommand(DBCommand, conn))
				{
					comm.Parameters.AddRange(paramList.ToArray());

					conn.Open();
					int effectRowsCount = comm.ExecuteNonQuery();
				
					return effectRowsCount;

				}
			}
		}
	}
}
