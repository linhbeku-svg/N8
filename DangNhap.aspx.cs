
using System;
using System.Data.SqlClient;

namespace QuanLySanBong
{
    public partial class DangNhap : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["MaTK"] != null)
            {
                Response.Redirect("~/Default.aspx");
            }
        }

        protected void btnDangNhap_Click(object sender, EventArgs e)
        {
            pnlThongBao.Visible = false;

            string tenDangNhap = txtTenDangNhap.Text.Trim();
            string matKhau = txtMatKhau.Text;

            if (string.IsNullOrWhiteSpace(tenDangNhap))
            {
                HienThongBao("Vui lòng nhập tên đăng nhập.");
                return;
            }

            if (string.IsNullOrWhiteSpace(matKhau))
            {
                HienThongBao("Vui lòng nhập mật khẩu.");
                return;
            }

            try
            {
                string sql = @"
                    SELECT MaTK, TenDangNhap, HoTen, Quyen
                    FROM TAIKHOAN
                    WHERE TenDangNhap = @TenDangNhap
                    AND MatKhau = @MatKhau
                    AND TrangThai = 1";

                using (SqlConnection conn = ketnoi.GetConnection())
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@TenDangNhap", tenDangNhap);
                    cmd.Parameters.AddWithValue("@MatKhau", matKhau);

                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Session["MaTK"] = Convert.ToInt32(reader["MaTK"]);
                            Session["TenDangNhap"] = reader["TenDangNhap"].ToString();
                            Session["HoTen"] = reader["HoTen"].ToString();
                            Session["Quyen"] = reader["Quyen"].ToString();

                            Response.Redirect("~/Default.aspx");
                            return;
                        }
                        else
                        {
                            HienThongBao(
                                "Tên đăng nhập hoặc mật khẩu không đúng, hoặc tài khoản đã bị khóa.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                HienThongBao(
                    "Đăng nhập thất bại. Vui lòng kiểm tra kết nối cơ sở dữ liệu.");
            }
        }

        private void HienThongBao(string noiDung)
        {
            lblThongBao.Text = noiDung;
            pnlThongBao.Visible = true;
        }
    }
}
