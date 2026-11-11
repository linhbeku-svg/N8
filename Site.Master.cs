
using System;

namespace QuanLySanBong
{
    public partial class SiteMaster : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Kiểm tra đăng nhập
            if (Session["MaTK"] == null)
            {
                Response.Redirect("~/DangNhap.aspx");
                return;
            }

            if (!IsPostBack)
            {
                lblHoTen.Text = Session["HoTen"] != null
                    ? Session["HoTen"].ToString()
                    : Session["TenDangNhap"].ToString();

                lblQuyen.Text = Session["Quyen"] != null
                    ? Session["Quyen"].ToString()
                    : "Người dùng";
            }
        }

        // Đăng xuất
        protected void btnDangXuat_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();

            Response.Redirect("~/DangNhap.aspx");
        }
    }
}
