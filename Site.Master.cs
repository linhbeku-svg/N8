
using System;

namespace QuanLySanBong
{
    public partial class SiteMaster : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Kiểm tra người dùng đã đăng nhập chưa
            if (Session["MaTK"] == null)
            {
                Response.Redirect("~/DangNhap.aspx");
                return;
            }

            if (!IsPostBack)
            {
                // Hiển thị họ tên
                if (Session["HoTen"] != null)
                {
                    lblHoTen.Text = Session["HoTen"].ToString();
                }
                else
                {
                    lblHoTen.Text = Session["TenDangNhap"].ToString();
                }

                // Hiển thị tên quyền được lưu trong Session
                if (Session["Quyen"] != null)
                {
                    lblQuyen.Text = Session["Quyen"].ToString();
                }
                else
                {
                    lblQuyen.Text = "Người dùng";
                }
            }
        }

        protected void btnDangXuat_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();

            Response.Redirect("~/DangNhap.aspx");
        }
    }
}
