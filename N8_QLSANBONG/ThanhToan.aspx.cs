using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

namespace QuanLySanBong
{
    public partial class ThanhToan : System.Web.UI.Page
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
                LoadDanhSachHoaDon();
                LoadLichSuThanhToan();
                LamMoiForm();
            }
        }

        // 1. HIỂN THỊ DANH SÁCH HÓA ĐƠN CÒN NỢ
        private void LoadDanhSachHoaDon()
        {
            try
            {
                string sql = @"
                    SELECT
                        H.MaHD,
                        N'Hóa đơn #' + CONVERT(NVARCHAR(20), H.MaHD)
                            + N' - ' + K.HoTen AS TenHienThi,
                        H.TongTien,
                        ISNULL(SUM(
                            CASE
                                WHEN T.TrangThai = N'ThanhCong'
                                THEN T.SoTien
                                ELSE 0
                            END
                        ), 0) AS DaThanhToan
                    FROM HOADON H
                    INNER JOIN DATSAN D ON H.MaDatSan = D.MaDatSan
                    INNER JOIN KHACHHANG K ON D.MaKH = K.MaKH
                    LEFT JOIN THANHTOAN T ON H.MaHD = T.MaHD
                    GROUP BY H.MaHD, K.HoTen, H.TongTien
                    HAVING H.TongTien >
                        ISNULL(SUM(
                            CASE
                                WHEN T.TrangThai = N'ThanhCong'
                                THEN T.SoTien
                                ELSE 0
                            END
                        ), 0)
                    ORDER BY H.MaHD DESC";

                DataTable dt = ketnoi.LayDuLieu(sql, null);

                ddlHoaDon.DataSource = dt;
                ddlHoaDon.DataTextField = "TenHienThi";
                ddlHoaDon.DataValueField = "MaHD";
                ddlHoaDon.DataBind();

                ddlHoaDon.Items.Insert(
                    0,
                    new System.Web.UI.WebControls.ListItem(
                        "-- Chọn hóa đơn --", ""));
            }
            catch (Exception ex)
            {
                HienThongBao(
                    "Không thể tải danh sách hóa đơn: " + ex.Message,
                    "danger");
            }
        }

        // 2. CHỌN HÓA ĐƠN
        protected void ddlHoaDon_SelectedIndexChanged(
            object sender, EventArgs e)
        {
            pnlThongBao.Visible = false;

            if (string.IsNullOrWhiteSpace(ddlHoaDon.SelectedValue))
            {
                XoaThongTinHoaDon();
                return;
            }

            try
            {
                int maHD = Convert.ToInt32(ddlHoaDon.SelectedValue);
                HienThongTinHoaDon(maHD);
            }
            catch (Exception ex)
            {
                HienThongBao(
                    "Lỗi tải thông tin hóa đơn: " + ex.Message,
                    "danger");
            }
        }

        // 3. HIỂN THỊ THÔNG TIN HÓA ĐƠN
        private void HienThongTinHoaDon(int maHD)
        {
            string sql = @"
                SELECT
                    H.TongTien,
                    ISNULL(SUM(
                        CASE
                            WHEN T.TrangThai = N'ThanhCong'
                            THEN T.SoTien
                            ELSE 0
                        END
                    ), 0) AS DaThanhToan
                FROM HOADON H
                LEFT JOIN THANHTOAN T ON H.MaHD = T.MaHD
                WHERE H.MaHD = @MaHD
                GROUP BY H.TongTien";

            SqlParameter[] parameters =
            {
                new SqlParameter("@MaHD", maHD)
            };

            DataTable dt = ketnoi.LayDuLieu(sql, parameters);

            if (dt.Rows.Count == 0)
            {
                XoaThongTinHoaDon();
                HienThongBao("Không tìm thấy hóa đơn.", "warning");
                return;
            }

            decimal tongTien =
                Convert.ToDecimal(dt.Rows[0]["TongTien"]);

            decimal daThanhToan =
                Convert.ToDecimal(dt.Rows[0]["DaThanhToan"]);

            decimal conPhaiTra = tongTien - daThanhToan;

            if (conPhaiTra < 0)
                conPhaiTra = 0;

            txtTongTien.Text =
                tongTien.ToString(CultureInfo.InvariantCulture);

            txtDaThanhToan.Text =
                daThanhToan.ToString(CultureInfo.InvariantCulture);

            txtConPhaiTra.Text =
                conPhaiTra.ToString(CultureInfo.InvariantCulture);

            txtSoTien.Text =
                conPhaiTra.ToString(CultureInfo.InvariantCulture);
        }

        // 4. XÁC NHẬN THANH TOÁN
        protected void btnThanhToan_Click(object sender, EventArgs e)
        {
            pnlThongBao.Visible = false;

            if (string.IsNullOrWhiteSpace(ddlHoaDon.SelectedValue))
            {
                HienThongBao(
                    "Vui lòng chọn hóa đơn cần thanh toán.",
                    "warning");
                return;
            }

            decimal soTien;

            if (!decimal.TryParse(
                txtSoTien.Text.Trim(),
                NumberStyles.Number,
                CultureInfo.InvariantCulture,
                out soTien))
            {
                HienThongBao(
                    "Số tiền thanh toán không hợp lệ.",
                    "warning");
                return;
            }

            if (soTien <= 0)
            {
                HienThongBao(
                    "Số tiền thanh toán phải lớn hơn 0.",
                    "warning");
                return;
            }

            int maHD = Convert.ToInt32(ddlHoaDon.SelectedValue);

            string phuongThuc = ddlPhuongThuc.SelectedValue;
            string maGiaoDich = txtMaGiaoDich.Text.Trim();

            SqlConnection conn = null;
            SqlTransaction transaction = null;

            try
            {
                conn = ketnoi.GetConnection();
                conn.Open();

                transaction = conn.BeginTransaction(
                    System.Data.IsolationLevel.Serializable);

                // LẤY TỔNG TIỀN HÓA ĐƠN
                string sqlHoaDon = @"
                    SELECT TongTien
                    FROM HOADON WITH (UPDLOCK, HOLDLOCK)
                    WHERE MaHD = @MaHD";

                decimal tongTien;

                using (SqlCommand cmd = new SqlCommand(
                    sqlHoaDon, conn, transaction))
                {
                    cmd.Parameters.Add("@MaHD", SqlDbType.Int).Value = maHD;

                    object ketQua = cmd.ExecuteScalar();

                    if (ketQua == null || ketQua == DBNull.Value)
                        throw new Exception("Hóa đơn không tồn tại.");

                    tongTien = Convert.ToDecimal(ketQua);
                }

                // TÍNH SỐ TIỀN ĐÃ THANH TOÁN
                string sqlDaThanhToan = @"
                    SELECT ISNULL(SUM(SoTien), 0)
                    FROM THANHTOAN
                    WHERE MaHD = @MaHD
                      AND TrangThai = N'ThanhCong'";

                decimal daThanhToan;

                using (SqlCommand cmd = new SqlCommand(
                    sqlDaThanhToan, conn, transaction))
                {
                    cmd.Parameters.Add("@MaHD", SqlDbType.Int).Value = maHD;

                    daThanhToan =
                        Convert.ToDecimal(cmd.ExecuteScalar());
                }

                decimal conPhaiTra = tongTien - daThanhToan;

                if (conPhaiTra <= 0)
                    throw new Exception(
                        "Hóa đơn đã được thanh toán đủ.");

                if (soTien > conPhaiTra)
                    throw new Exception(
                        "Số tiền thanh toán vượt quá số tiền còn phải trả.");

                // THÊM GIAO DỊCH
                string sqlThem = @"
                    INSERT INTO THANHTOAN
                    (
                        MaHD,
                        SoTien,
                        PhuongThuc,
                        MaGiaoDich,
                        TrangThai
                    )
                    VALUES
                    (
                        @MaHD,
                        @SoTien,
                        @PhuongThuc,
                        @MaGiaoDich,
                        N'ThanhCong'
                    )";

                using (SqlCommand cmd = new SqlCommand(
                    sqlThem, conn, transaction))
                {
                    cmd.Parameters.Add("@MaHD", SqlDbType.Int).Value = maHD;

                    SqlParameter pSoTien =
                        cmd.Parameters.Add("@SoTien", SqlDbType.Decimal);

                    pSoTien.Precision = 12;
                    pSoTien.Scale = 2;
                    pSoTien.Value = soTien;

                    cmd.Parameters.Add(
                        "@PhuongThuc", SqlDbType.NVarChar, 30)
                        .Value = phuongThuc;

                    cmd.Parameters.Add(
                        "@MaGiaoDich", SqlDbType.VarChar, 100)
                        .Value = string.IsNullOrWhiteSpace(maGiaoDich)
                            ? (object)DBNull.Value
                            : maGiaoDich;

                    cmd.ExecuteNonQuery();
                }

                // CẬP NHẬT TRẠNG THÁI HÓA ĐƠN
                decimal tongDaThanhToan = daThanhToan + soTien;

                string trangThaiHoaDon =
                    tongDaThanhToan >= tongTien
                    ? "DaThanhToan"
                    : "ThanhToanMotPhan";

                string sqlCapNhat = @"
                    UPDATE HOADON
                    SET TrangThai = @TrangThai
                    WHERE MaHD = @MaHD";

                using (SqlCommand cmd = new SqlCommand(
                    sqlCapNhat, conn, transaction))
                {
                    cmd.Parameters.Add(
                        "@TrangThai", SqlDbType.NVarChar, 30)
                        .Value = trangThaiHoaDon;

                    cmd.Parameters.Add("@MaHD", SqlDbType.Int).Value = maHD;

                    cmd.ExecuteNonQuery();
                }

                transaction.Commit();

                HienThongBao(
                    trangThaiHoaDon == "DaThanhToan"
                    ? "Thanh toán thành công. Hóa đơn đã được thanh toán đủ."
                    : "Đã ghi nhận thanh toán một phần. Hóa đơn vẫn còn số tiền phải trả.",
                    "success");

                LoadDanhSachHoaDon();
                LoadLichSuThanhToan();
                XoaThongTinHoaDon();

                if (ddlHoaDon.Items.Count > 0)
                    ddlHoaDon.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                if (transaction != null)
                {
                    try
                    {
                        transaction.Rollback();
                    }
                    catch
                    {
                    }
                }

                HienThongBao(
                    "Không thể thực hiện thanh toán: " + ex.Message,
                    "danger");
            }
            finally
            {
                if (transaction != null)
                    transaction.Dispose();

                if (conn != null)
                    conn.Dispose();
            }
        }

        // 5. HIỂN THỊ LỊCH SỬ THANH TOÁN
        private void LoadLichSuThanhToan()
        {
            try
            {
                string sql = @"
                    SELECT
                        T.MaTT,
                        T.MaHD,
                        K.HoTen,
                        T.NgayThanhToan,
                        T.SoTien,
                        T.PhuongThuc,
                        T.MaGiaoDich,
                        T.TrangThai
                    FROM THANHTOAN T
                    INNER JOIN HOADON H ON T.MaHD = H.MaHD
                    INNER JOIN DATSAN D ON H.MaDatSan = D.MaDatSan
                    INNER JOIN KHACHHANG K ON D.MaKH = K.MaKH
                    ORDER BY T.MaTT DESC";

                DataTable dt = ketnoi.LayDuLieu(sql, null);

                gvThanhToan.DataSource = dt;
                gvThanhToan.DataBind();
            }
            catch (Exception ex)
            {
                HienThongBao(
                    "Lỗi tải lịch sử thanh toán: " + ex.Message,
                    "danger");
            }
        }

        // 6. XÓA THÔNG TIN HÓA ĐƠN TRÊN FORM
        private void XoaThongTinHoaDon()
        {
            txtTongTien.Text = "";
            txtDaThanhToan.Text = "";
            txtConPhaiTra.Text = "";
            txtSoTien.Text = "";
            txtMaGiaoDich.Text = "";
        }

        // 7. LÀM MỚI FORM
        private void LamMoiForm()
        {
            XoaThongTinHoaDon();

            if (ddlHoaDon.Items.Count > 0)
                ddlHoaDon.SelectedIndex = 0;

            if (ddlPhuongThuc.Items.Count > 0)
                ddlPhuongThuc.SelectedIndex = 0;
        }

        protected void btnLamMoi_Click(object sender, EventArgs e)
        {
            pnlThongBao.Visible = false;

            LoadDanhSachHoaDon();
            LoadLichSuThanhToan();
            LamMoiForm();
        }

        // 8. HIỂN THỊ THÔNG BÁO
        private void HienThongBao(string noiDung, string loai)
        {
            pnlThongBao.CssClass = "alert alert-" + loai;
            lblThongBao.Text = noiDung;
            pnlThongBao.Visible = true;
        }
    }
}
