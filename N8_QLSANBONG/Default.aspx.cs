
using System;
using System.Data;

namespace QuanLySanBong
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["MaTK"] == null)
            {
                Response.Redirect("~/DangNhap.aspx");
                return;
            }

            if (!IsPostBack)
            {
                LoadTongQuan();
                LoadDatSanMoi();
            }
        }

        // Lấy dữ liệu thống kê
        private void LoadTongQuan()
        {
            try
            {
                // Tổng số sân bóng
                string sqlSan = @"
                    SELECT COUNT(*)
                    FROM SANBONG";

                object tongSan = ketnoi.LayMotGiaTri(sqlSan, null);

                lblTongSan.Text = Convert.ToInt32(tongSan).ToString();

                // Tổng số khách hàng
                string sqlKhachHang = @"
                    SELECT COUNT(*)
                    FROM KHACHHANG";

                object tongKhachHang =
                    ketnoi.LayMotGiaTri(sqlKhachHang, null);

                lblTongKhachHang.Text =
                    Convert.ToInt32(tongKhachHang).ToString();

                // Tổng số phiếu đặt sân
                string sqlDatSan = @"
                    SELECT COUNT(*)
                    FROM DATSAN";

                object tongDatSan =
                    ketnoi.LayMotGiaTri(sqlDatSan, null);

                lblTongDatSan.Text =
                    Convert.ToInt32(tongDatSan).ToString();

                // Tổng doanh thu từ thanh toán thành công
                string sqlDoanhThu = @"
                    SELECT ISNULL(SUM(SoTien), 0)
                    FROM THANHTOAN
                    WHERE TrangThai = N'ThanhCong'";

                object doanhThu =
                    ketnoi.LayMotGiaTri(sqlDoanhThu, null);

                lblDoanhThu.Text =
                    Convert.ToDecimal(doanhThu).ToString("N0") + " VNĐ";
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(
                    "Lỗi thống kê trang chủ: " + ex.Message);
            }
        }

        // Lấy 10 phiếu đặt sân mới nhất
        private void LoadDatSanMoi()
        {
            try
            {
                string sql = @"
                    SELECT TOP 10
                        ds.MaDatSan,
                        kh.HoTen,
                        sb.TenSan,
                        ds.NgayThue,
                        CONVERT(VARCHAR(5), ds.GioBatDau, 108) AS GioBatDau,
                        CONVERT(VARCHAR(5), ds.GioKetThuc, 108) AS GioKetThuc,
                        ds.TrangThai
                    FROM DATSAN ds
                    INNER JOIN KHACHHANG kh
                        ON ds.MaKH = kh.MaKH
                    INNER JOIN SANBONG sb
                        ON ds.MaSan = sb.MaSan
                    ORDER BY ds.MaDatSan DESC";

                DataTable dt = ketnoi.LayDuLieu(sql, null);

                gvDatSanMoi.DataSource = dt;
                gvDatSanMoi.DataBind();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(
                    "Lỗi tải danh sách đặt sân: " + ex.Message);
            }
        }
    }
}
