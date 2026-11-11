
using System;
using System.Data;
using System.Data.SqlClient;

namespace QuanLySanBong
{
    public partial class SanBong : System.Web.UI.Page
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
            }
        }

        // Hiển thị danh sách sân bóng
        private void LoadDanhSachSan()
        {
            try
            {
                string sql = @"
                    SELECT MaSan, TenSan, LoaiSan, TrangThai, MoTa
                    FROM SANBONG
                    WHERE (@TenSan = N'' OR TenSan LIKE N'%' + @TenSan + N'%')
                    AND (@TrangThai = N'' OR TrangThai = @TrangThai)
                    ORDER BY MaSan DESC";

                SqlParameter[] parameters =
                {
                    new SqlParameter("@TenSan", txtTimKiem.Text.Trim()),
                    new SqlParameter("@TrangThai", ddlLocTrangThai.SelectedValue)
                };

                DataTable dt = ketnoi.LayDuLieu(sql, parameters);

                gvSanBong.DataSource = dt;
                gvSanBong.DataBind();
            }
            catch (Exception ex)
            {
                HienThongBao("Không thể tải danh sách sân bóng: " + ex.Message, "danger");
            }
        }

        // Thêm hoặc cập nhật sân bóng
        protected void btnLuu_Click(object sender, EventArgs e)
        {
            pnlThongBao.Visible = false;

            string tenSan = txtTenSan.Text.Trim();
            string loaiSan = ddlLoaiSan.SelectedValue;
            string trangThai = ddlTrangThai.SelectedValue;
            string moTa = txtMoTa.Text.Trim();

            if (string.IsNullOrWhiteSpace(tenSan))
            {
                HienThongBao("Vui lòng nhập tên sân bóng.", "warning");
                return;
            }

            try
            {
                if (string.IsNullOrWhiteSpace(hfMaSan.Value))
                {
                    // THÊM SÂN BÓNG
                    string sql = @"
                        INSERT INTO SANBONG
                            (TenSan, LoaiSan, TrangThai, MoTa)
                        VALUES
                            (@TenSan, @LoaiSan, @TrangThai, @MoTa)";

                    SqlParameter[] parameters =
                    {
                        new SqlParameter("@TenSan", tenSan),
                        new SqlParameter("@LoaiSan", loaiSan),
                        new SqlParameter("@TrangThai", trangThai),
                        new SqlParameter("@MoTa",
                            string.IsNullOrWhiteSpace(moTa)
                                ? (object)DBNull.Value
                                : moTa)
                    };

                    ketnoi.ThucThi(sql, parameters);

                    HienThongBao("Thêm sân bóng thành công.", "success");
                }
                else
                {
                    // CẬP NHẬT SÂN BÓNG
                    string sql = @"
                        UPDATE SANBONG
                        SET TenSan = @TenSan,
                            LoaiSan = @LoaiSan,
                            TrangThai = @TrangThai,
                            MoTa = @MoTa
                        WHERE MaSan = @MaSan";

                    SqlParameter[] parameters =
                    {
                        new SqlParameter("@TenSan", tenSan),
                        new SqlParameter("@LoaiSan", loaiSan),
                        new SqlParameter("@TrangThai", trangThai),
                        new SqlParameter("@MoTa",
                            string.IsNullOrWhiteSpace(moTa)
                                ? (object)DBNull.Value
                                : moTa),
                        new SqlParameter("@MaSan", Convert.ToInt32(hfMaSan.Value))
                    };

                    int ketQua = ketnoi.ThucThi(sql, parameters);

                    if (ketQua > 0)
                        HienThongBao("Cập nhật sân bóng thành công.", "success");
                    else
                        HienThongBao("Không tìm thấy sân bóng cần cập nhật.", "warning");
                }

                LamMoiForm();
                LoadDanhSachSan();
            }
            catch (Exception ex)
            {
                HienThongBao("Lỗi lưu sân bóng: " + ex.Message, "danger");
            }
        }

        // Xử lý nút Sửa và Xóa trong GridView
        protected void gvSanBong_RowCommand(object sender,
            System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            if (e.CommandName == "SuaSan")
            {
                int maSan = Convert.ToInt32(e.CommandArgument);
                LoadThongTinSan(maSan);
            }
            else if (e.CommandName == "XoaSan")
            {
                int maSan = Convert.ToInt32(e.CommandArgument);
                XoaSanBong(maSan);
            }
        }

        // Lấy thông tin sân bóng để sửa
        private void LoadThongTinSan(int maSan)
        {
            try
            {
                string sql = @"
                    SELECT MaSan, TenSan, LoaiSan, TrangThai, MoTa
                    FROM SANBONG
                    WHERE MaSan = @MaSan";

                SqlParameter[] parameters =
                {
                    new SqlParameter("@MaSan", maSan)
                };

                DataTable dt = ketnoi.LayDuLieu(sql, parameters);

                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];

                    hfMaSan.Value = row["MaSan"].ToString();
                    txtTenSan.Text = row["TenSan"].ToString();
                    txtMoTa.Text = row["MoTa"] == DBNull.Value
                        ? ""
                        : row["MoTa"].ToString();

                    ddlLoaiSan.SelectedValue = row["LoaiSan"].ToString();
                    ddlTrangThai.SelectedValue = row["TrangThai"].ToString();

                    lblTieuDeForm.Text = "CẬP NHẬT SÂN BÓNG";
                    btnLuu.Text = "Cập nhật";

                    HienThongBao("Đã tải thông tin sân bóng. Bạn có thể chỉnh sửa.", "info");
                }
                else
                {
                    HienThongBao("Không tìm thấy sân bóng.", "warning");
                }
            }
            catch (Exception ex)
            {
                HienThongBao("Lỗi lấy thông tin sân bóng: " + ex.Message, "danger");
            }
        }

        // Xóa sân bóng
        private void XoaSanBong(int maSan)
        {
            try
            {
                string sql = @"
                    DELETE FROM SANBONG
                    WHERE MaSan = @MaSan";

                SqlParameter[] parameters =
                {
                    new SqlParameter("@MaSan", maSan)
                };

                int ketQua = ketnoi.ThucThi(sql, parameters);

                if (ketQua > 0)
                    HienThongBao("Xóa sân bóng thành công.", "success");
                else
                    HienThongBao("Không tìm thấy sân bóng cần xóa.", "warning");

                LamMoiForm();
                LoadDanhSachSan();
            }
            catch (SqlException ex)
            {
                if (ex.Number == 547)
                {
                    HienThongBao(
                        "Không thể xóa sân bóng này vì đã có dữ liệu đặt sân hoặc bảng giá liên quan.",
                        "danger");
                }
                else
                {
                    HienThongBao("Lỗi SQL khi xóa sân bóng: " + ex.Message, "danger");
                }
            }
            catch (Exception ex)
            {
                HienThongBao("Lỗi xóa sân bóng: " + ex.Message, "danger");
            }
        }

        // Tìm kiếm
        protected void btnTimKiem_Click(object sender, EventArgs e)
        {
            LoadDanhSachSan();
        }

        // Làm mới biểu mẫu
        protected void btnLamMoi_Click(object sender, EventArgs e)
        {
            LamMoiForm();
            pnlThongBao.Visible = false;
            LoadDanhSachSan();
        }

        private void LamMoiForm()
        {
            hfMaSan.Value = "";
            txtTenSan.Text = "";
            txtMoTa.Text = "";

            ddlLoaiSan.SelectedIndex = 0;
            ddlTrangThai.SelectedValue = "Trong";

            lblTieuDeForm.Text = "THÊM SÂN BÓNG";
            btnLuu.Text = "Lưu thông tin";
        }

        // Hiển thị thông báo Bootstrap
        private void HienThongBao(string noiDung, string loai)
        {
            pnlThongBao.CssClass = "alert alert-" + loai;
            lblThongBao.Text = noiDung;
            pnlThongBao.Visible = true;
        }
    }
}
