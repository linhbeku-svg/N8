
using System;

namespace QuanLySanBong
{
    public partial class KiemTraKetNoi : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnKiemTra_Click(object sender, EventArgs e)
        {
            try
            {
                if (ketnoi.KiemTraKetNoi())
                {
                    lblKetQua.ForeColor =
                        System.Drawing.Color.Green;

                    lblKetQua.Text =
                        "KẾT NỐI SQL SERVER THÀNH CÔNG!";
                }
                else
                {
                    lblKetQua.ForeColor =
                        System.Drawing.Color.Red;

                    lblKetQua.Text =
                        "KẾT NỐI SQL SERVER THẤT BẠI!";
                }
            }
            catch (Exception ex)
            {
                lblKetQua.ForeColor =
                    System.Drawing.Color.Red;

                lblKetQua.Text =
                    "Lỗi: " + Server.HtmlEncode(ex.Message);
            }
        }
    }
}
