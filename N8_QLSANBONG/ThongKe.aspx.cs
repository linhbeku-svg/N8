
using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

namespace QuanLySanBong
{
    public partial class ThongKe : System.Web.UI.Page
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
                DateTime homNay = DateTime.Today;
                DateTime dauThang = new DateTime(
                    homNay.Year, homNay.Month, 1);

                txtTuNgay.Text = dauThang.ToString("yyyy-MM-dd");
                txtDenNgay.Text = homNay.ToString("yyyy-MM-dd");

                LoadThongKe();
            }
        }

        // KIỂM TRA VÀ LẤY KHOẢNG THỜI GIAN
        private bool LayKhoangThoiGian(
            out DateTime tuNgay, out DateTime denNgay)
        {
            tuNgay = DateTime.MinValue;
            denNgay = DateTime.MinValue;

            if (!DateTime.TryParseExact(
                txtTuNgay.Text,
                "yyyy-MM-dd",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out tuNgay))
            {
                HienThongBao("Vui lòng chọn ngày bắt đầu hợp lệ.", "warning");
                return false;
            }

            if (!DateTime.TryParseExact(
                txtDenNgay.Text,
                "yyyy-MM-dd",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out denNgay))
            {
                HienThongBao("Vui lòng chọn ngày kết thúc hợp lệ.", "warning");
                return false;
            }

            if (tuNgay > denNgay)
            {
                HienThongBao(
                    "Ngày bắt đầu không được lớn hơn ngày kết thúc.",
                    "warning");
                return false;
            }

            return true;
        }

        // TẢI TOÀN BỘ THỐNG KÊ
        private void LoadThongKe()
        {
            DateTime tuNgay;
            DateTime denNgay;

            if (!LayKhoangThoiGian(out tuNgay, out denNgay))
                return;

            pnlThongBao.Visible = false;

            try
            {
                LoadTongSoSan();
                LoadTongKhachHang();
                LoadTongDatSan(tuNgay, denNgay);
                LoadDoanhThu(tuNgay, denNgay);
                LoadDoanhThuTheoNgay(tuNgay, denNgay);
                LoadTrangThaiDatSan(tuNgay, denNgay);
            }
            catch (Exception ex)
            {
                HienThongBao(
                    "Không thể tải thống kê: " + ex.Message,
                    "danger");
            }
        }

        // 1. TỔNG SỐ SÂN BÓNG
        private void LoadTongSoSan()
        {
            string sql = @"
                SELECT COUNT(*)
                FROM SANBONG";

            object ketQua = ketnoi.LayMotGiaTri(sql, null);

            lblTongSan.Text = Convert.ToInt32(ketQua).ToString("N0");
        }

        // 2. TỔNG SỐ KHÁCH HÀNG
        private void LoadTongKhachHang()
        {
            string sql = @"
                SELECT COUNT(*)
                FROM KHACHHANG";

            object ketQua = ketnoi.LayMotGiaTri(sql, null);

            lblTongKhachHang.Text =
                Convert.ToInt32(ketQua).ToString("N0");
        }

        // 3. SỐ LƯỢT ĐẶT SÂN TRONG KHOẢNG THỜI GIAN
        private void LoadTongDatSan(DateTime tuNgay, DateTime denNgay)
        {
            string sql = @"
                SELECT COUNT(*)
                FROM DATSAN
                WHERE NgayDat >= @TuNgay
                  AND NgayDat < @DenNgay";

            SqlParameter[] parameters =
            {
                new SqlParameter("@TuNgay", SqlDbType.DateTime)
                {
                    Value = tuNgay.Date
                },
                new SqlParameter("@DenNgay", SqlDbType.DateTime)
                {
                    Value = denNgay.Date.AddDays(1)
                }
            };

            object ketQua = ketnoi.LayMotGiaTri(sql, parameters);

            lblTongDatSan.Text =
                Convert.ToInt32(ketQua).ToString("N0");
        }

        // 4. TỔNG DOANH THU TRONG KHOẢNG THỜI GIAN
        // Chỉ tính các giao dịch thanh toán thành công.
        private void LoadDoanhThu(DateTime tuNgay, DateTime denNgay)
        {
            string sql = @"
                SELECT ISNULL(SUM(SoTien), 0)
                FROM THANHTOAN
                WHERE TrangThai = N'ThanhCong'
                  AND NgayThanhToan >= @TuNgay
                  AND NgayThanhToan < @DenNgay";

            SqlParameter[] parameters =
            {
                new SqlParameter("@TuNgay", SqlDbType.DateTime)
                {
                    Value = tuNgay.Date
                },
                new SqlParameter("@DenNgay", SqlDbType.DateTime)
                {
                    Value = denNgay.Date.AddDays(1)
                }
            };

            object ketQua = ketnoi.LayMotGiaTri(sql, parameters);

            decimal doanhThu = Convert.ToDecimal(ketQua);

            lblDoanhThu.Text =
                doanhThu.ToString("N0") + " VNĐ";
        }

        // 5. THỐNG KÊ DOANH THU TỪNG NGÀY
        private void LoadDoanhThuTheoNgay(
            DateTime tuNgay, DateTime denNgay)
        {
            string sql = @"
                SELECT
                    CAST(NgayThanhToan AS DATE) AS Ngay,
                    COUNT(*) AS SoGiaoDich,
                    SUM(SoTien) AS DoanhThu
                FROM THANHTOAN
                WHERE TrangThai = N'ThanhCong'
                  AND NgayThanhToan >= @TuNgay
                  AND NgayThanhToan < @DenNgay
                GROUP BY CAST(NgayThanhToan AS DATE)
                ORDER BY CAST(NgayThanhToan AS DATE) DESC";

            SqlParameter[] parameters =
            {
                new SqlParameter("@TuNgay", SqlDbType.DateTime)
                {
                    Value = tuNgay.Date
                },
                new SqlParameter("@DenNgay", SqlDbType.DateTime)
                {
                    Value = denNgay.Date.AddDays(1)
                }
            };

            DataTable dt = ketnoi.LayDuLieu(sql, parameters);

            gvDoanhThu.DataSource = dt;
            gvDoanhThu.DataBind();
        }

        // 6. THỐNG KÊ TRẠNG THÁI ĐẶT SÂN
        private void LoadTrangThaiDatSan(
            DateTime tuNgay, DateTime denNgay)
        {
            string sql = @"
                SELECT
                    CASE TrangThai
                        WHEN 'ChoXacNhan' THEN N'Chờ xác nhận'
                        WHEN 'DaXacNhan' THEN N'Đã xác nhận'
                        WHEN 'DangSuDung' THEN N'Đang sử dụng'
                        WHEN 'HoanThanh' THEN N'Hoàn thành'
                        WHEN 'DaHuy' THEN N'Đã hủy'
                        ELSE TrangThai
                    END AS TrangThai,
                    COUNT(*) AS SoLuong
                FROM DATSAN
                WHERE NgayDat >= @TuNgay
                  AND NgayDat < @DenNgay
                GROUP BY TrangThai
                ORDER BY TrangThai";

            SqlParameter[] parameters =
            {
                new SqlParameter("@TuNgay", SqlDbType.DateTime)
                {
                    Value = tuNgay.Date
                },
                new SqlParameter("@DenNgay", SqlDbType.DateTime)
                {
                    Value = denNgay.Date.AddDays(1)
                }
            };

            DataTable dt = ketnoi.LayDuLieu(sql, parameters);

            gvTrangThaiDatSan.DataSource = dt;
            gvTrangThaiDatSan.DataBind();
        }

        // NÚT THỐNG KÊ
        protected void btnThongKe_Click(object sender, EventArgs e)
        {
            LoadThongKe();
        }

        // THỐNG KÊ HÔM NAY
        protected void btnHomNay_Click(object sender, EventArgs e)
        {
            DateTime homNay = DateTime.Today;

            txtTuNgay.Text = homNay.ToString("yyyy-MM-dd");
            txtDenNgay.Text = homNay.ToString("yyyy-MM-dd");

            LoadThongKe();
        }

        // THỐNG KÊ THÁNG NÀY
        protected void btnThangNay_Click(object sender, EventArgs e)
        {
            DateTime homNay = DateTime.Today;

            DateTime dauThang = new DateTime(
                homNay.Year, homNay.Month, 1);

            txtTuNgay.Text = dauThang.ToString("yyyy-MM-dd");
            txtDenNgay.Text = homNay.ToString("yyyy-MM-dd");

            LoadThongKe();
        }

        // LÀM MỚI
        protected void btnLamMoi_Click(object sender, EventArgs e)
        {
            DateTime homNay = DateTime.Today;

            DateTime dauThang = new DateTime(
                homNay.Year, homNay.Month, 1);

            txtTuNgay.Text = dauThang.ToString("yyyy-MM-dd");
            txtDenNgay.Text = homNay.ToString("yyyy-MM-dd");

            LoadThongKe();
        }

        // HIỂN THỊ THÔNG BÁO
        private void HienThongBao(string noiDung, string loai)
        {
            pnlThongBao.CssClass = "alert alert-" + loai;
            lblThongBao.Text = noiDung;
            pnlThongBao.Visible = true;
        }
    }
}
