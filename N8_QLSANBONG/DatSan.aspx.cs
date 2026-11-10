
using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

namespace QuanLySanBong
{
    public partial class DatSan : System.Web.UI.Page
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
                LoadDanhSachKhachHang();
                LoadDanhSachSan();
                LoadDanhSachDatSan();
            }
        }

        // TẢI DANH SÁCH KHÁCH HÀNG
        private void LoadDanhSachKhachHang()
        {
            try
            {
                string sql = @"
                    SELECT MaKH, HoTen, SoDienThoai
                    FROM KHACHHANG
                    ORDER BY HoTen";

                DataTable dt = ketnoi.LayDuLieu(sql, null);

                ddlKhachHang.DataSource = dt;
                ddlKhachHang.DataTextField = "HoTen";
                ddlKhachHang.DataValueField = "MaKH";
                ddlKhachHang.DataBind();

                ddlKhachHang.Items.Insert(0,
                    new System.Web.UI.WebControls.ListItem(
                        "-- Chọn khách hàng --", ""));
            }
            catch (Exception ex)
            {
                HienThongBao("Không thể tải danh sách khách hàng: " + ex.Message, "danger");
            }
        }

        // TẢI DANH SÁCH SÂN BÓNG
        private void LoadDanhSachSan()
        {
            try
            {
                string sql = @"
                    SELECT MaSan, TenSan
                    FROM SANBONG
                    WHERE TrangThai NOT IN (N'BaoTri', N'NgungHoatDong')
                    ORDER BY TenSan";

                DataTable dt = ketnoi.LayDuLieu(sql, null);

                ddlSanBong.DataSource = dt;
                ddlSanBong.DataTextField = "TenSan";
                ddlSanBong.DataValueField = "MaSan";
                ddlSanBong.DataBind();

                ddlSanBong.Items.Insert(0,
                    new System.Web.UI.WebControls.ListItem(
                        "-- Chọn sân bóng --", ""));
            }
            catch (Exception ex)
            {
                HienThongBao("Không thể tải danh sách sân bóng: " + ex.Message, "danger");
            }
        }

        // HIỂN THỊ DANH SÁCH ĐẶT SÂN
        private void LoadDanhSachDatSan()
        {
            try
            {
                string sql = @"
                    SELECT
                        ds.MaDatSan,
                        kh.HoTen,
                        kh.SoDienThoai,
                        sb.TenSan,
                        ds.NgayThue,
                        CONVERT(VARCHAR(5), ds.GioBatDau, 108) AS GioBatDau,
                        CONVERT(VARCHAR(5), ds.GioKetThuc, 108) AS GioKetThuc,
                        ds.TienCoc,
                        ds.TrangThai
                    FROM DATSAN ds
                    INNER JOIN KHACHHANG kh ON ds.MaKH = kh.MaKH
                    INNER JOIN SANBONG sb ON ds.MaSan = sb.MaSan
                    WHERE
                        (@TuKhoa = N''
                        OR kh.HoTen LIKE N'%' + @TuKhoa + N'%'
                        OR sb.TenSan LIKE N'%' + @TuKhoa + N'%'
                        OR kh.SoDienThoai LIKE '%' + @TuKhoa + '%')
                    ORDER BY ds.MaDatSan DESC";

                SqlParameter[] parameters =
                {
                    new SqlParameter("@TuKhoa", txtTimKiem.Text.Trim())
                };

                DataTable dt = ketnoi.LayDuLieu(sql, parameters);

                gvDatSan.DataSource = dt;
                gvDatSan.DataBind();
            }
            catch (Exception ex)
            {
                HienThongBao("Không thể tải danh sách đặt sân: " + ex.Message, "danger");
            }
        }

        // THÊM HOẶC CẬP NHẬT PHIẾU ĐẶT SÂN
        protected void btnLuu_Click(object sender, EventArgs e)
        {
            pnlThongBao.Visible = false;

            if (string.IsNullOrWhiteSpace(ddlKhachHang.SelectedValue))
            {
                HienThongBao("Vui lòng chọn khách hàng.", "warning");
                return;
            }

            if (string.IsNullOrWhiteSpace(ddlSanBong.SelectedValue))
            {
                HienThongBao("Vui lòng chọn sân bóng.", "warning");
                return;
            }

            DateTime ngayThue;

            if (!DateTime.TryParseExact(
                txtNgayThue.Text,
                "yyyy-MM-dd",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out ngayThue))
            {
                HienThongBao("Vui lòng chọn ngày thuê hợp lệ.", "warning");
                return;
            }

            TimeSpan gioBatDau;
            TimeSpan gioKetThuc;

            if (!TimeSpan.TryParseExact(
                txtGioBatDau.Text,
                @"hh\:mm",
                CultureInfo.InvariantCulture,
                out gioBatDau))
            {
                HienThongBao("Giờ bắt đầu không hợp lệ.", "warning");
                return;
            }

            if (!TimeSpan.TryParseExact(
                txtGioKetThuc.Text,
                @"hh\:mm",
                CultureInfo.InvariantCulture,
                out gioKetThuc))
            {
                HienThongBao("Giờ kết thúc không hợp lệ.", "warning");
                return;
            }

            if (gioKetThuc <= gioBatDau)
            {
                HienThongBao("Giờ kết thúc phải sau giờ bắt đầu.", "warning");
                return;
            }

            decimal tienCoc = 0;

            if (!string.IsNullOrWhiteSpace(txtTienCoc.Text))
            {
                if (!decimal.TryParse(
                    txtTienCoc.Text.Trim(),
                    NumberStyles.Number,
                    CultureInfo.InvariantCulture,
                    out tienCoc))
                {
                    HienThongBao("Tiền cọc không hợp lệ.", "warning");
                    return;
                }
            }

            if (tienCoc < 0)
            {
                HienThongBao("Tiền cọc không được nhỏ hơn 0.", "warning");
                return;
            }

            string trangThai = ddlTrangThai.SelectedValue;

            int maSan = Convert.ToInt32(ddlSanBong.SelectedValue);
            int maKH = Convert.ToInt32(ddlKhachHang.SelectedValue);

            int maDatSan = 0;

            if (!string.IsNullOrWhiteSpace(hfMaDatSan.Value))
                maDatSan = Convert.ToInt32(hfMaDatSan.Value);

            try
            {
                // KIỂM TRA TRÙNG LỊCH ĐẶT SÂN
                if (trangThai != "DaHuy")
                {
                    if (KiemTraTrungLich(
                        maSan, ngayThue, gioBatDau, gioKetThuc, maDatSan))
                    {
                        HienThongBao(
                            "Sân bóng đã có lịch đặt trong khoảng thời gian này. Vui lòng chọn giờ khác.",
                            "warning");
                        return;
                    }
                }

                if (maDatSan == 0)
                {
                    // THÊM PHIẾU ĐẶT SÂN
                    string sql = @"
                        INSERT INTO DATSAN
                            (MaKH, MaSan, MaTK, NgayThue,
                             GioBatDau, GioKetThuc, TienCoc, TrangThai)
                        VALUES
                            (@MaKH, @MaSan, @MaTK, @NgayThue,
                             @GioBatDau, @GioKetThuc, @TienCoc, @TrangThai)";

                    SqlParameter[] parameters =
                    {
                        new SqlParameter("@MaKH", maKH),
                        new SqlParameter("@MaSan", maSan),
                        new SqlParameter("@MaTK", Convert.ToInt32(Session["MaTK"])),
                        new SqlParameter("@NgayThue", SqlDbType.Date)
                        {
                            Value = ngayThue.Date
                        },
                        new SqlParameter("@GioBatDau", SqlDbType.Time)
                        {
                            Value = gioBatDau
                        },
                        new SqlParameter("@GioKetThuc", SqlDbType.Time)
                        {
                            Value = gioKetThuc
                        },
                        new SqlParameter("@TienCoc", SqlDbType.Decimal)
                        {
                            Precision = 12,
                            Scale = 2,
                            Value = tienCoc
                        },
                        new SqlParameter("@TrangThai", trangThai)
                    };

                    ketnoi.ThucThi(sql, parameters);

                    HienThongBao("Thêm phiếu đặt sân thành công.", "success");
                }
                else
                {
                    // CẬP NHẬT PHIẾU ĐẶT SÂN
                    string sql = @"
                        UPDATE DATSAN
                        SET MaKH = @MaKH,
                            MaSan = @MaSan,
                            NgayThue = @NgayThue,
                            GioBatDau = @GioBatDau,
                            GioKetThuc = @GioKetThuc,
                            TienCoc = @TienCoc,
                            TrangThai = @TrangThai
                        WHERE MaDatSan = @MaDatSan";

                    SqlParameter[] parameters =
                    {
                        new SqlParameter("@MaKH", maKH),
                        new SqlParameter("@MaSan", maSan),
                        new SqlParameter("@NgayThue", SqlDbType.Date)
                        {
                            Value = ngayThue.Date
                        },
                        new SqlParameter("@GioBatDau", SqlDbType.Time)
                        {
                            Value = gioBatDau
                        },
                        new SqlParameter("@GioKetThuc", SqlDbType.Time)
                        {
                            Value = gioKetThuc
                        },
                        new SqlParameter("@TienCoc", SqlDbType.Decimal)
                        {
                            Precision = 12,
                            Scale = 2,
                            Value = tienCoc
                        },
                        new SqlParameter("@TrangThai", trangThai),
                        new SqlParameter("@MaDatSan", maDatSan)
                    };

                    int ketQua = ketnoi.ThucThi(sql, parameters);

                    if (ketQua > 0)
                        HienThongBao("Cập nhật phiếu đặt sân thành công.", "success");
                    else
                        HienThongBao("Không tìm thấy phiếu đặt sân cần cập nhật.", "warning");
                }

                LamMoiForm();
                LoadDanhSachDatSan();
            }
            catch (Exception ex)
            {
                HienThongBao("Lỗi lưu phiếu đặt sân: " + ex.Message, "danger");
            }
        }

        // KIỂM TRA SÂN CÓ BỊ TRÙNG LỊCH HAY KHÔNG
        private bool KiemTraTrungLich(
            int maSan,
            DateTime ngayThue,
            TimeSpan gioBatDau,
            TimeSpan gioKetThuc,
            int maDatSan)
        {
            string sql = @"
                SELECT COUNT(*)
                FROM DATSAN
                WHERE MaSan = @MaSan
                    AND NgayThue = @NgayThue
                    AND TrangThai <> N'DaHuy'
                    AND GioBatDau < @GioKetThuc
                    AND GioKetThuc > @GioBatDau
                    AND (@MaDatSan = 0 OR MaDatSan <> @MaDatSan)";

            SqlParameter[] parameters =
            {
                new SqlParameter("@MaSan", maSan),
                new SqlParameter("@NgayThue", SqlDbType.Date)
                {
                    Value = ngayThue.Date
                },
                new SqlParameter("@GioBatDau", SqlDbType.Time)
                {
                    Value = gioBatDau
                },
                new SqlParameter("@GioKetThuc", SqlDbType.Time)
                {
                    Value = gioKetThuc
                },
                new SqlParameter("@MaDatSan", maDatSan)
            };

            object ketQua = ketnoi.LayMotGiaTri(sql, parameters);

            return Convert.ToInt32(ketQua) > 0;
        }

        // XỬ LÝ NÚT SỬA
        protected void gvDatSan_RowCommand(
            object sender,
            System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            if (e.CommandName == "SuaDatSan")
            {
                int maDatSan = Convert.ToInt32(e.CommandArgument);
                LoadThongTinDatSan(maDatSan);
            }
        }

        // LẤY THÔNG TIN PHIẾU ĐẶT SÂN
        private void LoadThongTinDatSan(int maDatSan)
        {
            try
            {
                string sql = @"
                    SELECT MaDatSan, MaKH, MaSan, NgayThue,
                           GioBatDau, GioKetThuc, TienCoc, TrangThai
                    FROM DATSAN
                    WHERE MaDatSan = @MaDatSan";

                SqlParameter[] parameters =
                {
                    new SqlParameter("@MaDatSan", maDatSan)
                };

                DataTable dt = ketnoi.LayDuLieu(sql, parameters);

                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];

                    hfMaDatSan.Value = row["MaDatSan"].ToString();

                    string maKH = row["MaKH"].ToString();
                    string maSan = row["MaSan"].ToString();

                    if (ddlKhachHang.Items.FindByValue(maKH) != null)
                        ddlKhachHang.SelectedValue = maKH;

                    if (ddlSanBong.Items.FindByValue(maSan) != null)
                        ddlSanBong.SelectedValue = maSan;

                    txtNgayThue.Text = Convert.ToDateTime(row["NgayThue"])
                        .ToString("yyyy-MM-dd");

                    txtGioBatDau.Text = ((TimeSpan)row["GioBatDau"])
                        .ToString(@"hh\:mm");

                    txtGioKetThuc.Text = ((TimeSpan)row["GioKetThuc"])
                        .ToString(@"hh\:mm");

                    txtTienCoc.Text = Convert.ToDecimal(row["TienCoc"])
                        .ToString(CultureInfo.InvariantCulture);

                    ddlTrangThai.SelectedValue = row["TrangThai"].ToString();

                    lblTieuDeForm.Text = "CẬP NHẬT PHIẾU ĐẶT SÂN";
                    btnLuu.Text = "Cập nhật đặt sân";

                    HienThongBao(
                        "Đã tải thông tin phiếu đặt sân. Bạn có thể chỉnh sửa.",
                        "info");
                }
                else
                {
                    HienThongBao("Không tìm thấy phiếu đặt sân.", "warning");
                }
            }
            catch (Exception ex)
            {
                HienThongBao(
                    "Lỗi lấy thông tin đặt sân: " + ex.Message,
                    "danger");
            }
        }

        // TÌM KIẾM
        protected void btnTimKiem_Click(object sender, EventArgs e)
        {
            LoadDanhSachDatSan();
        }

        // LÀM MỚI FORM
        protected void btnLamMoi_Click(object sender, EventArgs e)
        {
            LamMoiForm();
            pnlThongBao.Visible = false;
            LoadDanhSachDatSan();
        }

        private void LamMoiForm()
        {
            hfMaDatSan.Value = "";

            if (ddlKhachHang.Items.Count > 0)
                ddlKhachHang.SelectedIndex = 0;

            if (ddlSanBong.Items.Count > 0)
                ddlSanBong.SelectedIndex = 0;

            txtNgayThue.Text = "";
            txtGioBatDau.Text = "";
            txtGioKetThuc.Text = "";
            txtTienCoc.Text = "0";

            ddlTrangThai.SelectedValue = "ChoXacNhan";

            lblTieuDeForm.Text = "THÊM PHIẾU ĐẶT SÂN";
            btnLuu.Text = "Lưu phiếu đặt sân";
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
