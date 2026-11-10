
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace QuanLySanBong
{
    public class ketnoi
    {
        // Chuoi ket noi SQL Server
        private static readonly string strKetNoi =
            ConfigurationManager
            .ConnectionStrings["QuanLySanBong"]
            .ConnectionString;

        // Tao ket noi
        public static SqlConnection GetConnection()
        {
            return new SqlConnection(strKetNoi);
        }

        // Kiem tra ket noi
        public static bool KiemTraKetNoi()
        {
            try
            {
                using (SqlConnection conn = GetConnection())
                {
                    conn.Open();
                    return conn.State == ConnectionState.Open;
                }
            }
            catch
            {
                return false;
            }
        }

        // Thuc thi cau lenh INSERT, UPDATE, DELETE
        public static int ThucThi(string sql, SqlParameter[] parameters)
        {
            using (SqlConnection conn = GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    if (parameters != null)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }

                    conn.Open();
                    return cmd.ExecuteNonQuery();
                }
            }
        }

        // Lay du lieu SELECT
        public static DataTable LayDuLieu(
            string sql,
            SqlParameter[] parameters)
        {
            DataTable dt = new DataTable();

            using (SqlConnection conn = GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    if (parameters != null)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                }
            }

            return dt;
        }

        // Lay mot gia tri tu SQL Server
        public static object LayMotGiaTri(
            string sql,
            SqlParameter[] parameters)
        {
            using (SqlConnection conn = GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    if (parameters != null)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }

                    conn.Open();
                    return cmd.ExecuteScalar();
                }
            }
        }
    }
}
