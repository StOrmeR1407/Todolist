using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadBot
{
    internal class Sql
    {
        static string strCon = @"Data Source=STORMER;Initial Catalog=Banhang;Integrated Security=True";
        public static string timMaKH(string maKH)
        {
            string kq = "";
            try
            {
                using (SqlConnection cn = new SqlConnection(strCon))
                {
                    cn.Open();
                    using (SqlCommand cm = cn.CreateCommand())
                    {
                        cm.CommandText = "TIM_MAKH";
                        cm.CommandType = CommandType.StoredProcedure;
                        cm.Parameters.Add("@maKH", SqlDbType.NVarChar, 50).Value = maKH;
                        object obj = cm.ExecuteScalar(); //lấy col1 of row1
                        if (obj != null)
                            kq = (string)obj;
                        else
                            kq = $"không có sv nào tên: {maKH}";
                    }
                }
            }
            catch (Exception ex)
            {
                kq += $"Error: {ex.Message}";
            }
            return kq;
        }
    }
}
