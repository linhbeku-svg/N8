
using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

namespace QuanLySanBong
{
    public partial class BangGia : System.Web.UI.Page
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
                LoadDanhSachSan();
                LoadDanhSachBangGia();
            }
        }

        // Tải danh sách sân vào DropDownList
        private void LoadDanhSachSan()
        {
            try
            {
                string sql = @"
                    SELECT MaSan, TenSan
                    FROM SANBONG
                    ORDER BY TenSan";

                DataTable dt = ketnoi.LayDuLieu(sql, null);

                ddlMaSan.DataSource = dt;
                ddlMaSan.DataTextField = "TenSan";
                ddlMaSan.DataValueField = "MaSan";
                ddlMaSan.DataBind();

                ddlMaSan.Items.Insert(0,
                    new System.Web.UI.WebControls.ListItem(
                        "-- Chọn sân bóng --", ""));
            }
            catch (Exception ex)
            {
                HienThongBao("Không thể tải danh sách sân: " + ex.Message, "danger");
            }
        }

        // Hiển thị danh sách bảng giá
        private void LoadDanhSachBangGia()
        {
            try
            {
                string sql = @"
                    SELECT bg.MaGia, bg.MaSan, sb.TenSan,
                           bg.KhungGio, bg.DonGia,
                           bg.NgayApDung, bg.NgayKetThuc
                    FROM BANGGIA bg
                    INNER JOIN SANBONG sb ON bg.MaSan = sb.MaSan
                    WHERE (@TenSan = N''
                           OR sb.TenSan LIKE N'%' + @TenSan + N'%')
                    ORDER BY bg.MaGia DESC";

                SqlParameter[] parameters =
                {
                    new SqlParameter("@TenSan", txtTimKiem.Text.Trim())
                };

                DataTable dt = ketnoi.LayDuLieu(sql, parameters);

                gvBangGia.DataSource = dt;
                gvBangGia.DataBind();
            }
            catch (Exception ex)
            {
                HienThongBao("Không thể tải danh sách bảng giá: " + ex.Message, "danger");
            }
        }

        // Thêm hoặc cập nhật bảng giá
        protected void btnLuu_Click(object sender, EventArgs e)
        {
            pnlThongBao.Visible = false;

            if (string.IsNullOrWhiteSpace(ddlMaSan.SelectedValue))
            {
                HienThongBao("Vui lòng chọn sân bóng.", "warning");
                return;
            }

            string khungGio = txtKhungGio.Text.Trim();

            if (string.IsNullOrWhiteSpace(khungGio))
            {
                HienThongBao("Vui lòng nhập khung giờ.", "warning");
                return;
            }

            decimal donGia;

            if (!decimal.TryParse(
                txtDonGia.Text.Trim(),
                NumberStyles.Number,
                CultureInfo.InvariantCulture,
                out donGia))
            {
                HienThongBao("Đơn giá không hợp lệ.", "warning");
                return;
            }

            if (donGia <= 0)
            {
                HienThongBao("Đơn giá phải lớn hơn 0.", "warning");
                return;
            }

            DateTime ngayApDung;

            if (!DateTime.TryParseExact(
                txtNgayApDung.Text,
                "yyyy-MM-dd",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out ngayApDung))
            {
                HienThongBao("Vui lòng chọn ngày áp dụng hợp lệ.", "warning");
                return;
            }

            object ngayKetThuc = DBNull.Value;

            if (!string.IsNullOrWhiteSpace(txtNgayKetThuc.Text))
            {
                DateTime ngayKT;

                if (!DateTime.TryParseExact(
                    txtNgayKetThuc.Text,
                    "yyyy-MM-dd",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out ngayKT))
                {
                    HienThongBao("Ngày kết thúc không hợp lệ.", "warning");
                    return;
                }

                if (ngayKT.Date < ngayApDung.Date)
                {
                    HienThongBao("Ngày kết thúc phải bằng hoặc sau ngày áp dụng.", "warning");
                    return;
                }

                ngayKetThuc = ngayKT.Date;
            }

            try
            {
                if (string.IsNullOrWhiteSpace(hfMaGia.Value))
                {
                    // THÊM BẢNG GIÁ
                    string sql = @"
                        INSERT INTO BANGGIA
                            (MaSan, KhungGio, DonGia, NgayApDung, NgayKetThuc)
                        VALUES
                            (@MaSan, @KhungGio, @DonGia, @NgayApDung, @NgayKetThuc)";

                    SqlParameter[] parameters =
                    {
                        new SqlParameter("@MaSan", Convert.ToInt32(ddlMaSan.SelectedValue)),
                        new SqlParameter("@KhungGio", khungGio),
                        new SqlParameter("@DonGia", SqlDbType.Decimal)
                        {
                            Precision = 12,
                            Scale = 2,
                            Value = donGia
                        },
                        new SqlParameter("@NgayApDung", SqlDbType.Date)
                        {
                            Value = ngayApDung.Date
                        },
                        new SqlParameter("@NgayKetThuc", SqlDbType.Date)
                        {
                            Value = ngayKetThuc
                        }
                    };

                    ketnoi.ThucThi(sql, parameters);

                    HienThongBao("Thêm bảng giá thành công.", "success");
                }
                else
                {
                    // CẬP NHẬT BẢNG GIÁ
                    string sql = @"
                        UPDATE BANGGIA
                        SET MaSan = @MaSan,
                            KhungGio = @KhungGio,
                            DonGia = @DonGia,
                            NgayApDung = @NgayApDung,
                            NgayKetThuc = @NgayKetThuc
                        WHERE MaGia = @MaGia";

                    SqlParameter[] parameters =
                    {
                        new SqlParameter("@MaSan", Convert.ToInt32(ddlMaSan.SelectedValue)),
                        new SqlParameter("@KhungGio", khungGio),
                        new SqlParameter("@DonGia", SqlDbType.Decimal)
                        {
                            Precision = 12,
                            Scale = 2,
                            Value = donGia
                        },
                        new SqlParameter("@NgayApDung", SqlDbType.Date)
                        {
                            Value = ngayApDung.Date
                        },
                        new SqlParameter("@NgayKetThuc", SqlDbType.Date)
                        {
                            Value = ngayKetThuc
                        },
                        new SqlParameter("@MaGia", Convert.ToInt32(hfMaGia.Value))
                    };

                    int ketQua = ketnoi.ThucThi(sql, parameters);

                    if (ketQua > 0)
                        HienThongBao("Cập nhật bảng giá thành công.", "success");
                    else
                        HienThongBao("Không tìm thấy bảng giá cần cập nhật.", "warning");
                }

                LamMoiForm();
                LoadDanhSachBangGia();
            }
            catch (Exception ex)
            {
                HienThongBao("Lỗi lưu bảng giá: " + ex.Message, "danger");
            }
        }

        // Xử lý nút Sửa và Xóa
        protected void gvBangGia_RowCommand(object sender,
            System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            if (e.CommandName == "SuaGia")
            {
                int maGia = Convert.ToInt32(e.CommandArgument);
                LoadThongTinBangGia(maGia);
            }
            else if (e.CommandName == "XoaGia")
            {
                int maGia = Convert.ToInt32(e.CommandArgument);
                XoaBangGia(maGia);
            }
        }

        // Lấy thông tin bảng giá để sửa
        private void LoadThongTinBangGia(int maGia)
        {
            try
            {
                string sql = @"
                    SELECT MaGia, MaSan, KhungGio,
                           DonGia, NgayApDung, NgayKetThuc
                    FROM BANGGIA
                    WHERE MaGia = @MaGia";

                SqlParameter[] parameters =
                {
                    new SqlParameter("@MaGia", maGia)
                };

                DataTable dt = ketnoi.LayDuLieu(sql, parameters);

                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];

                    hfMaGia.Value = row["MaGia"].ToString();

                    string maSan = row["MaSan"].ToString();

                    if (ddlMaSan.Items.FindByValue(maSan) != null)
                        ddlMaSan.SelectedValue = maSan;

                    txtKhungGio.Text = row["KhungGio"].ToString();

                    txtDonGia.Text = Convert.ToDecimal(row["DonGia"])
                        .ToString(CultureInfo.InvariantCulture);

                    txtNgayApDung.Text = Convert.ToDateTime(row["NgayApDung"])
                        .ToString("yyyy-MM-dd");

                    if (row["NgayKetThuc"] == DBNull.Value)
                        txtNgayKetThuc.Text = "";
                    else
                        txtNgayKetThuc.Text = Convert.ToDateTime(row["NgayKetThuc"])
                            .ToString("yyyy-MM-dd");

                    lblTieuDeForm.Text = "CẬP NHẬT BẢNG GIÁ";
                    btnLuu.Text = "Cập nhật";

                    HienThongBao("Đã tải thông tin bảng giá. Bạn có thể chỉnh sửa.", "info");
                }
                else
                {
                    HienThongBao("Không tìm thấy bảng giá.", "warning");
                }
            }
            catch (Exception ex)
            {
                HienThongBao("Lỗi lấy thông tin bảng giá: " + ex.Message, "danger");
            }
        }

        // Xóa bảng giá
        private void XoaBangGia(int maGia)
        {
            try
            {
                string sql = @"
                    DELETE FROM BANGGIA
                    WHERE MaGia = @MaGia";

                SqlParameter[] parameters =
                {
                    new SqlParameter("@MaGia", maGia)
                };

                int ketQua = ketnoi.ThucThi(sql, parameters);

                if (ketQua > 0)
                    HienThongBao("Xóa bảng giá thành công.", "success");
                else
                    HienThongBao("Không tìm thấy bảng giá cần xóa.", "warning");

                LamMoiForm();
                LoadDanhSachBangGia();
            }
            catch (SqlException ex)
            {
                if (ex.Number == 547)
                {
                    HienThongBao(
                        "Không thể xóa bảng giá vì đang được dữ liệu khác tham chiếu.",
                        "danger");
                }
                else
                {
                    HienThongBao("Lỗi SQL khi xóa bảng giá: " + ex.Message, "danger");
                }
            }
            catch (Exception ex)
            {
                HienThongBao("Lỗi xóa bảng giá: " + ex.Message, "danger");
            }
        }

        // Tìm kiếm bảng giá
        protected void btnTimKiem_Click(object sender, EventArgs e)
        {
            LoadDanhSachBangGia();
        }

        // Làm mới biểu mẫu
        protected void btnLamMoi_Click(object sender, EventArgs e)
        {
            LamMoiForm();
            pnlThongBao.Visible = false;
            LoadDanhSachBangGia();
        }

        private void LamMoiForm()
        {
            hfMaGia.Value = "";

            if (ddlMaSan.Items.Count > 0)
                ddlMaSan.SelectedIndex = 0;

            txtKhungGio.Text = "";
            txtDonGia.Text = "";
            txtNgayApDung.Text = "";
            txtNgayKetThuc.Text = "";

            lblTieuDeForm.Text = "THÊM BẢNG GIÁ";
            btnLuu.Text = "Lưu bảng giá";
        }

        // Hiển thị thông báo
        private void HienThongBao(string noiDung, string loai)
        {
            pnlThongBao.CssClass = "alert alert-" + loai;
            lblThongBao.Text = noiDung;
            pnlThongBao.Visible = true;
        }
    }
}
