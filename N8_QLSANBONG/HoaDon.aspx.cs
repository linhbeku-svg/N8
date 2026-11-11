
using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

namespace QuanLySanBong
{
    public partial class HoaDon : System.Web.UI.Page
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
                LoadDanhSachDatSan();
                LoadDanhSachHoaDon();
                LamMoiForm();
            }
        }

        // DANH SÁCH ĐẶT SÂN CHƯA CÓ HÓA ĐƠN
        private void LoadDanhSachDatSan()
        {
            string sql = @"
                SELECT D.MaDatSan,
                       K.HoTen,
                       S.TenSan,
                       D.NgayThue,
                       D.GioBatDau,
                       D.GioKetThuc
                FROM DATSAN D
                INNER JOIN KHACHHANG K ON D.MaKH = K.MaKH
                INNER JOIN SANBONG S ON D.MaSan = S.MaSan
                WHERE NOT EXISTS
                (
                    SELECT 1
                    FROM HOADON H
                    WHERE H.MaDatSan = D.MaDatSan
                )
                AND D.TrangThai <> N'DaHuy'
                ORDER BY D.MaDatSan DESC";

            DataTable dt = ketnoi.LayDuLieu(sql, null);

            ddlDatSan.DataSource = dt;
            ddlDatSan.DataTextField = "HoTen";
            ddlDatSan.DataValueField = "MaDatSan";
            ddlDatSan.DataBind();

            ddlDatSan.Items.Insert(0,
                new System.Web.UI.WebControls.ListItem(
                    "-- Chọn phiếu đặt sân --", ""));
        }

        // TÍNH TIỀN SÂN THEO BẢNG GIÁ
        private decimal TinhTienSan(int maDatSan)
        {
            string sql = @"
                SELECT TOP 1
                    B.DonGia *
                    (
                        DATEDIFF(MINUTE, D.GioBatDau, D.GioKetThuc) / 60.0
                    )
                FROM DATSAN D
                INNER JOIN BANGGIA B ON B.MaSan = D.MaSan
                WHERE D.MaDatSan = @MaDatSan
                  AND D.NgayThue >= B.NgayApDung
                  AND (B.NgayKetThuc IS NULL OR D.NgayThue <= B.NgayKetThuc)
                  AND (
                      B.KhungGio = '00:00-23:59'
                      OR B.KhungGio = 'Ca ngay'
                  )
                ORDER BY B.NgayApDung DESC";

            SqlParameter[] parameters =
            {
                new SqlParameter("@MaDatSan", maDatSan)
            };

            object ketQua = ketnoi.LayMotGiaTri(sql, parameters);

            if (ketQua == null || ketQua == DBNull.Value)
            {
                // Nếu không có bảng giá khớp cả ngày,
                // kiểm tra các bảng giá còn hiệu lực của sân.
                string sql2 = @"
                    SELECT TOP 1
                        B.DonGia *
                        (
                            DATEDIFF(MINUTE, D.GioBatDau, D.GioKetThuc) / 60.0
                        )
                    FROM DATSAN D
                    INNER JOIN BANGGIA B ON B.MaSan = D.MaSan
                    WHERE D.MaDatSan = @MaDatSan
                      AND D.NgayThue >= B.NgayApDung
                      AND (B.NgayKetThuc IS NULL OR D.NgayThue <= B.NgayKetThuc)
                    ORDER BY B.NgayApDung DESC";

                ketQua = ketnoi.LayMotGiaTri(sql2, parameters);
            }

            if (ketQua == null || ketQua == DBNull.Value)
                throw new Exception("Sân này chưa có bảng giá phù hợp với ngày thuê.");

            return Convert.ToDecimal(ketQua);
        }

        // CHỌN PHIẾU ĐẶT SÂN
        protected void ddlDatSan_SelectedIndexChanged(object sender, EventArgs e)
        {
            pnlThongBao.Visible = false;

            if (string.IsNullOrWhiteSpace(ddlDatSan.SelectedValue))
            {
                txtTienSan.Text = "";
                txtTongTien.Text = "";
                return;
            }

            try
            {
                int maDatSan = Convert.ToInt32(ddlDatSan.SelectedValue);
                decimal tienSan = TinhTienSan(maDatSan);

                txtTienSan.Text = tienSan.ToString("0.##", CultureInfo.InvariantCulture);
                TinhTongTien(sender, e);
            }
            catch (Exception ex)
            {
                HienThongBao(ex.Message, "warning");
            }
        }

        // TÍNH TỔNG TIỀN
        protected void TinhTongTien(object sender, EventArgs e)
        {
            decimal tienSan = 0;
            decimal tienDichVu = 0;
            decimal giamGia = 0;

            decimal.TryParse(txtTienSan.Text, NumberStyles.Number,
                CultureInfo.InvariantCulture, out tienSan);

            decimal.TryParse(txtTienDichVu.Text, NumberStyles.Number,
                CultureInfo.InvariantCulture, out tienDichVu);

            decimal.TryParse(txtGiamGia.Text, NumberStyles.Number,
                CultureInfo.InvariantCulture, out giamGia);

            decimal tongTien = tienSan + tienDichVu - giamGia;

            if (tongTien < 0)
                tongTien = 0;

            txtTongTien.Text = tongTien.ToString("0.##",
                CultureInfo.InvariantCulture);
        }

        // LẬP HOẶC CẬP NHẬT HÓA ĐƠN
        protected void btnLuu_Click(object sender, EventArgs e)
        {
            pnlThongBao.Visible = false;

            if (string.IsNullOrWhiteSpace(hfMaHD.Value) &&
                string.IsNullOrWhiteSpace(ddlDatSan.SelectedValue))
            {
                HienThongBao("Vui lòng chọn phiếu đặt sân.", "warning");
                return;
            }

            decimal tienDichVu;
            decimal giamGia;

            if (!decimal.TryParse(txtTienDichVu.Text.Trim(),
                NumberStyles.Number, CultureInfo.InvariantCulture,
                out tienDichVu))
            {
                HienThongBao("Tiền dịch vụ không hợp lệ.", "warning");
                return;
            }

            if (!decimal.TryParse(txtGiamGia.Text.Trim(),
                NumberStyles.Number, CultureInfo.InvariantCulture,
                out giamGia))
            {
                HienThongBao("Giảm giá không hợp lệ.", "warning");
                return;
            }

            if (tienDichVu < 0 || giamGia < 0)
            {
                HienThongBao("Tiền dịch vụ và giảm giá không được âm.", "warning");
                return;
            }

            try
            {
                decimal tienSan;

                if (!decimal.TryParse(txtTienSan.Text,
                    NumberStyles.Number, CultureInfo.InvariantCulture,
                    out tienSan))
                {
                    HienThongBao("Không xác định được tiền sân.", "warning");
                    return;
                }

                decimal tongTien = tienSan + tienDichVu - giamGia;

                if (tongTien < 0)
                {
                    HienThongBao("Giảm giá không được lớn hơn tổng tiền.", "warning");
                    return;
                }

                if (string.IsNullOrWhiteSpace(hfMaHD.Value))
                {
                    // KIỂM TRA HÓA ĐƠN ĐÃ TỒN TẠI
                    string sqlKiemTra = @"
                        SELECT COUNT(*)
                        FROM HOADON
                        WHERE MaDatSan = @MaDatSan";

                    object tonTai = ketnoi.LayMotGiaTri(sqlKiemTra,
                        new SqlParameter[]
                        {
                            new SqlParameter("@MaDatSan",
                                Convert.ToInt32(ddlDatSan.SelectedValue))
                        });

                    if (Convert.ToInt32(tonTai) > 0)
                    {
                        HienThongBao("Phiếu đặt sân này đã có hóa đơn.", "warning");
                        LoadDanhSachDatSan();
                        return;
                    }

                    // TẠO HÓA ĐƠN
                    string sql = @"
                        INSERT INTO HOADON
                        (
                            MaDatSan, MaTK, TienSan,
                            TienDichVu, GiamGia, TongTien, TrangThai
                        )
                        VALUES
                        (
                            @MaDatSan, @MaTK, @TienSan,
                            @TienDichVu, @GiamGia, @TongTien, @TrangThai
                        )";

                    SqlParameter[] parameters =
                    {
                        new SqlParameter("@MaDatSan",
                            Convert.ToInt32(ddlDatSan.SelectedValue)),
                        new SqlParameter("@MaTK",
                            Convert.ToInt32(Session["MaTK"])),
                        TaoTien("@TienSan", tienSan),
                        TaoTien("@TienDichVu", tienDichVu),
                        TaoTien("@GiamGia", giamGia),
                        TaoTien("@TongTien", tongTien),
                        new SqlParameter("@TrangThai",
                            ddlTrangThai.SelectedValue)
                    };

                    ketnoi.ThucThi(sql, parameters);

                    HienThongBao("Lập hóa đơn thành công.", "success");
                }
                else
                {
                    // CẬP NHẬT HÓA ĐƠN
                    int maHD = Convert.ToInt32(hfMaHD.Value);

                    string sqlKiemTra = @"
                        SELECT COUNT(*)
                        FROM THANHTOAN
                        WHERE MaHD = @MaHD
                          AND TrangThai = N'ThanhCong'";

                    object daThanhToan = ketnoi.LayMotGiaTri(
                        sqlKiemTra,
                        new SqlParameter[]
                        {
                            new SqlParameter("@MaHD", maHD)
                        });

                    if (Convert.ToInt32(daThanhToan) > 0)
                    {
                        HienThongBao(
                            "Hóa đơn đã có giao dịch thanh toán thành công. Không thể sửa hóa đơn tại đây.",
                            "warning");
                        return;
                    }

                    string sql = @"
                        UPDATE HOADON
                        SET TienSan = @TienSan,
                            TienDichVu = @TienDichVu,
                            GiamGia = @GiamGia,
                            TongTien = @TongTien,
                            TrangThai = @TrangThai
                        WHERE MaHD = @MaHD";

                    SqlParameter[] parameters =
                    {
                        TaoTien("@TienSan", tienSan),
                        TaoTien("@TienDichVu", tienDichVu),
                        TaoTien("@GiamGia", giamGia),
                        TaoTien("@TongTien", tongTien),
                        new SqlParameter("@TrangThai",
                            ddlTrangThai.SelectedValue),
                        new SqlParameter("@MaHD", maHD)
                    };

                    ketnoi.ThucThi(sql, parameters);

                    HienThongBao("Cập nhật hóa đơn thành công.", "success");
                }

                LamMoiForm();
                LoadDanhSachDatSan();
                LoadDanhSachHoaDon();
            }
            catch (Exception ex)
            {
                HienThongBao("Lỗi lưu hóa đơn: " + ex.Message, "danger");
            }
        }

        // TẠO THAM SỐ TIỀN
        private SqlParameter TaoTien(string ten, decimal giaTri)
        {
            return new SqlParameter(ten, SqlDbType.Decimal)
            {
                Precision = 12,
                Scale = 2,
                Value = giaTri
            };
        }

        // HIỂN THỊ DANH SÁCH HÓA ĐƠN
        private void LoadDanhSachHoaDon()
        {
            try
            {
                string sql = @"
                    SELECT H.MaHD, H.MaDatSan, K.HoTen, S.TenSan,
                           H.NgayLap, H.TienSan, H.TienDichVu,
                           H.GiamGia, H.TongTien, H.TrangThai
                    FROM HOADON H
                    INNER JOIN DATSAN D ON H.MaDatSan = D.MaDatSan
                    INNER JOIN KHACHHANG K ON D.MaKH = K.MaKH
                    INNER JOIN SANBONG S ON D.MaSan = S.MaSan
                    WHERE
                        CAST(H.MaHD AS NVARCHAR(20)) LIKE N'%' + @TuKhoa + N'%'
                        OR K.HoTen LIKE N'%' + @TuKhoa + N'%'
                        OR S.TenSan LIKE N'%' + @TuKhoa + N'%'
                    ORDER BY H.MaHD DESC";

                SqlParameter[] parameters =
                {
                    new SqlParameter("@TuKhoa", txtTimKiem.Text.Trim())
                };

                DataTable dt = ketnoi.LayDuLieu(sql, parameters);

                gvHoaDon.DataSource = dt;
                gvHoaDon.DataBind();
            }
            catch (Exception ex)
            {
                HienThongBao("Lỗi tải danh sách hóa đơn: " + ex.Message, "danger");
            }
        }

        // SỬA HÓA ĐƠN
        protected void gvHoaDon_RowCommand(
            object sender,
            System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            if (e.CommandName == "SuaHD")
            {
                int maHD = Convert.ToInt32(e.CommandArgument);
                LoadThongTinHoaDon(maHD);
            }
        }

        private void LoadThongTinHoaDon(int maHD)
        {
            try
            {
                string sql = @"
                    SELECT *
                    FROM HOADON
                    WHERE MaHD = @MaHD";

                DataTable dt = ketnoi.LayDuLieu(sql,
                    new SqlParameter[]
                    {
                        new SqlParameter("@MaHD", maHD)
                    });

                if (dt.Rows.Count == 0)
                {
                    HienThongBao("Không tìm thấy hóa đơn.", "warning");
                    return;
                }

                DataRow row = dt.Rows[0];

                hfMaHD.Value = row["MaHD"].ToString();

                string maDatSan = row["MaDatSan"].ToString();

                if (ddlDatSan.Items.FindByValue(maDatSan) == null)
                {
                    ddlDatSan.Items.Add(
                        new System.Web.UI.WebControls.ListItem(
                            "Phiếu đặt sân #" + maDatSan, maDatSan));
                }

                ddlDatSan.SelectedValue = maDatSan;

                txtTienSan.Text = Convert.ToDecimal(row["TienSan"])
                    .ToString(CultureInfo.InvariantCulture);

                txtTienDichVu.Text = Convert.ToDecimal(row["TienDichVu"])
                    .ToString(CultureInfo.InvariantCulture);

                txtGiamGia.Text = Convert.ToDecimal(row["GiamGia"])
                    .ToString(CultureInfo.InvariantCulture);

                txtTongTien.Text = Convert.ToDecimal(row["TongTien"])
                    .ToString(CultureInfo.InvariantCulture);

                ddlTrangThai.SelectedValue = row["TrangThai"].ToString();

                lblTieuDeForm.Text = "CẬP NHẬT HÓA ĐƠN";
                btnLuu.Text = "Cập nhật hóa đơn";

                HienThongBao("Đã tải thông tin hóa đơn.", "info");
            }
            catch (Exception ex)
            {
                HienThongBao("Lỗi tải hóa đơn: " + ex.Message, "danger");
            }
        }

        // TÌM KIẾM
        protected void btnTimKiem_Click(object sender, EventArgs e)
        {
            LoadDanhSachHoaDon();
        }

        // LÀM MỚI
        protected void btnLamMoi_Click(object sender, EventArgs e)
        {
            LamMoiForm();
            pnlThongBao.Visible = false;
            LoadDanhSachDatSan();
            LoadDanhSachHoaDon();
        }

        private void LamMoiForm()
        {
            hfMaHD.Value = "";

            if (ddlDatSan.Items.Count > 0)
                ddlDatSan.SelectedIndex = 0;

            txtTienSan.Text = "";
            txtTienDichVu.Text = "0";
            txtGiamGia.Text = "0";
            txtTongTien.Text = "";

            ddlTrangThai.SelectedValue = "ChuaThanhToan";

            lblTieuDeForm.Text = "LẬP HÓA ĐƠN";
            btnLuu.Text = "Lập hóa đơn";
        }

        // THÔNG BÁO
        private void HienThongBao(string noiDung, string loai)
        {
            pnlThongBao.CssClass = "alert alert-" + loai;
            lblThongBao.Text = noiDung;
            pnlThongBao.Visible = true;
        }
    }
}
