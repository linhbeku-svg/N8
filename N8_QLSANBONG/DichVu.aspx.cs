
using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

namespace QuanLySanBong
{
    public partial class DichVu : System.Web.UI.Page
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
                LoadDanhSachDichVu();
            }
        }

        // HIỂN THỊ DANH SÁCH DỊCH VỤ
        private void LoadDanhSachDichVu()
        {
            try
            {
                string sql = @"
                    SELECT MaDV, TenDV, DonGia, DonViTinh, TrangThai
                    FROM DICHVU
                    WHERE @TenDV = N''
                       OR TenDV LIKE N'%' + @TenDV + N'%'
                    ORDER BY MaDV DESC";

                SqlParameter[] parameters =
                {
                    new SqlParameter("@TenDV", txtTimKiem.Text.Trim())
                };

                DataTable dt = ketnoi.LayDuLieu(sql, parameters);

                gvDichVu.DataSource = dt;
                gvDichVu.DataBind();
            }
            catch (Exception ex)
            {
                HienThongBao(
                    "Không thể tải danh sách dịch vụ: " + ex.Message,
                    "danger");
            }
        }

        // THÊM HOẶC CẬP NHẬT DỊCH VỤ
        protected void btnLuu_Click(object sender, EventArgs e)
        {
            pnlThongBao.Visible = false;

            string tenDV = txtTenDV.Text.Trim();
            string donViTinh = ddlDonViTinh.SelectedValue;

            if (string.IsNullOrWhiteSpace(tenDV))
            {
                HienThongBao("Vui lòng nhập tên dịch vụ.", "warning");
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

            try
            {
                if (string.IsNullOrWhiteSpace(hfMaDV.Value))
                {
                    // THÊM DỊCH VỤ
                    string sql = @"
                        INSERT INTO DICHVU
                            (TenDV, DonGia, DonViTinh, TrangThai)
                        VALUES
                            (@TenDV, @DonGia, @DonViTinh, @TrangThai)";

                    SqlParameter[] parameters =
                    {
                        new SqlParameter("@TenDV", tenDV),
                        new SqlParameter("@DonGia", SqlDbType.Decimal)
                        {
                            Precision = 12,
                            Scale = 2,
                            Value = donGia
                        },
                        new SqlParameter("@DonViTinh", donViTinh),
                        new SqlParameter("@TrangThai",
                            Convert.ToByte(ddlTrangThai.SelectedValue))
                    };

                    ketnoi.ThucThi(sql, parameters);

                    HienThongBao("Thêm dịch vụ thành công.", "success");
                }
                else
                {
                    // CẬP NHẬT DỊCH VỤ
                    string sql = @"
                        UPDATE DICHVU
                        SET TenDV = @TenDV,
                            DonGia = @DonGia,
                            DonViTinh = @DonViTinh,
                            TrangThai = @TrangThai
                        WHERE MaDV = @MaDV";

                    SqlParameter[] parameters =
                    {
                        new SqlParameter("@TenDV", tenDV),
                        new SqlParameter("@DonGia", SqlDbType.Decimal)
                        {
                            Precision = 12,
                            Scale = 2,
                            Value = donGia
                        },
                        new SqlParameter("@DonViTinh", donViTinh),
                        new SqlParameter("@TrangThai",
                            Convert.ToByte(ddlTrangThai.SelectedValue)),
                        new SqlParameter("@MaDV", Convert.ToInt32(hfMaDV.Value))
                    };

                    int ketQua = ketnoi.ThucThi(sql, parameters);

                    if (ketQua > 0)
                        HienThongBao("Cập nhật dịch vụ thành công.", "success");
                    else
                        HienThongBao("Không tìm thấy dịch vụ cần cập nhật.", "warning");
                }

                LamMoiForm();
                LoadDanhSachDichVu();
            }
            catch (Exception ex)
            {
                HienThongBao("Lỗi lưu dịch vụ: " + ex.Message, "danger");
            }
        }

        // XỬ LÝ NÚT SỬA VÀ XÓA
        protected void gvDichVu_RowCommand(
            object sender,
            System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            if (e.CommandName == "SuaDV")
            {
                int maDV = Convert.ToInt32(e.CommandArgument);
                LoadThongTinDichVu(maDV);
            }
            else if (e.CommandName == "XoaDV")
            {
                int maDV = Convert.ToInt32(e.CommandArgument);
                XoaDichVu(maDV);
            }
        }

        // LẤY THÔNG TIN DỊCH VỤ ĐỂ SỬA
        private void LoadThongTinDichVu(int maDV)
        {
            try
            {
                string sql = @"
                    SELECT MaDV, TenDV, DonGia, DonViTinh, TrangThai
                    FROM DICHVU
                    WHERE MaDV = @MaDV";

                SqlParameter[] parameters =
                {
                    new SqlParameter("@MaDV", maDV)
                };

                DataTable dt = ketnoi.LayDuLieu(sql, parameters);

                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];

                    hfMaDV.Value = row["MaDV"].ToString();
                    txtTenDV.Text = row["TenDV"].ToString();

                    txtDonGia.Text = Convert.ToDecimal(row["DonGia"])
                        .ToString(CultureInfo.InvariantCulture);

                    ddlDonViTinh.SelectedValue = row["DonViTinh"].ToString();
                    ddlTrangThai.SelectedValue = row["TrangThai"].ToString();

                    lblTieuDeForm.Text = "CẬP NHẬT DỊCH VỤ";
                    btnLuu.Text = "Cập nhật";

                    HienThongBao(
                        "Đã tải thông tin dịch vụ. Bạn có thể chỉnh sửa.",
                        "info");
                }
                else
                {
                    HienThongBao("Không tìm thấy dịch vụ.", "warning");
                }
            }
            catch (Exception ex)
            {
                HienThongBao(
                    "Lỗi lấy thông tin dịch vụ: " + ex.Message,
                    "danger");
            }
        }

        // XÓA DỊCH VỤ
        private void XoaDichVu(int maDV)
        {
            try
            {
                string sql = @"
                    DELETE FROM DICHVU
                    WHERE MaDV = @MaDV";

                SqlParameter[] parameters =
                {
                    new SqlParameter("@MaDV", maDV)
                };

                int ketQua = ketnoi.ThucThi(sql, parameters);

                if (ketQua > 0)
                    HienThongBao("Xóa dịch vụ thành công.", "success");
                else
                    HienThongBao("Không tìm thấy dịch vụ cần xóa.", "warning");

                LamMoiForm();
                LoadDanhSachDichVu();
            }
            catch (SqlException ex)
            {
                if (ex.Number == 547)
                {
                    HienThongBao(
                        "Không thể xóa dịch vụ vì dịch vụ đã được sử dụng trong hóa đơn. Bạn có thể chuyển sang trạng thái ngừng kinh doanh.",
                        "danger");
                }
                else
                {
                    HienThongBao(
                        "Lỗi SQL khi xóa dịch vụ: " + ex.Message,
                        "danger");
                }
            }
            catch (Exception ex)
            {
                HienThongBao("Lỗi xóa dịch vụ: " + ex.Message, "danger");
            }
        }

        // TÌM KIẾM
        protected void btnTimKiem_Click(object sender, EventArgs e)
        {
            LoadDanhSachDichVu();
        }

        // LÀM MỚI FORM
        protected void btnLamMoi_Click(object sender, EventArgs e)
        {
            LamMoiForm();
            pnlThongBao.Visible = false;
            LoadDanhSachDichVu();
        }

        private void LamMoiForm()
        {
            hfMaDV.Value = "";

            txtTenDV.Text = "";
            txtDonGia.Text = "";

            ddlDonViTinh.SelectedIndex = 0;
            ddlTrangThai.SelectedValue = "1";

            lblTieuDeForm.Text = "THÊM DỊCH VỤ";
            btnLuu.Text = "Lưu dịch vụ";
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
