
using System;
using System.Data;
using System.Data.SqlClient;

namespace QuanLySanBong
{
    public partial class KhachHang : System.Web.UI.Page
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
            }
        }

        // HIỂN THỊ DANH SÁCH KHÁCH HÀNG
        private void LoadDanhSachKhachHang()
        {
            try
            {
                string sql = @"
                    SELECT MaKH, HoTen, SoDienThoai, Email, DiaChi, NgayTao
                    FROM KHACHHANG
                    WHERE
                        (@TuKhoa = N''
                        OR HoTen LIKE N'%' + @TuKhoa + N'%'
                        OR SoDienThoai LIKE '%' + @TuKhoa + '%')
                    ORDER BY MaKH DESC";

                SqlParameter[] parameters =
                {
                    new SqlParameter("@TuKhoa", txtTimKiem.Text.Trim())
                };

                DataTable dt = ketnoi.LayDuLieu(sql, parameters);

                gvKhachHang.DataSource = dt;
                gvKhachHang.DataBind();
            }
            catch (Exception ex)
            {
                HienThongBao("Không thể tải danh sách khách hàng: " + ex.Message, "danger");
            }
        }

        // THÊM HOẶC CẬP NHẬT KHÁCH HÀNG
        protected void btnLuu_Click(object sender, EventArgs e)
        {
            pnlThongBao.Visible = false;

            string hoTen = txtHoTen.Text.Trim();
            string soDienThoai = txtSoDienThoai.Text.Trim();
            string email = txtEmail.Text.Trim();
            string diaChi = txtDiaChi.Text.Trim();

            if (string.IsNullOrWhiteSpace(hoTen))
            {
                HienThongBao("Vui lòng nhập họ và tên khách hàng.", "warning");
                return;
            }

            if (string.IsNullOrWhiteSpace(soDienThoai))
            {
                HienThongBao("Vui lòng nhập số điện thoại.", "warning");
                return;
            }

            if (soDienThoai.Length > 15)
            {
                HienThongBao("Số điện thoại không được vượt quá 15 ký tự.", "warning");
                return;
            }

            if (!string.IsNullOrWhiteSpace(email) &&
                !System.Text.RegularExpressions.Regex.IsMatch(
                    email,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                HienThongBao("Địa chỉ email không hợp lệ.", "warning");
                return;
            }

            try
            {
                if (string.IsNullOrWhiteSpace(hfMaKH.Value))
                {
                    // THÊM KHÁCH HÀNG
                    string sql = @"
                        INSERT INTO KHACHHANG
                            (HoTen, SoDienThoai, Email, DiaChi)
                        VALUES
                            (@HoTen, @SoDienThoai, @Email, @DiaChi)";

                    SqlParameter[] parameters =
                    {
                        new SqlParameter("@HoTen", hoTen),
                        new SqlParameter("@SoDienThoai", soDienThoai),
                        new SqlParameter("@Email",
                            string.IsNullOrWhiteSpace(email)
                                ? (object)DBNull.Value : email),
                        new SqlParameter("@DiaChi",
                            string.IsNullOrWhiteSpace(diaChi)
                                ? (object)DBNull.Value : diaChi)
                    };

                    ketnoi.ThucThi(sql, parameters);

                    HienThongBao("Thêm khách hàng thành công.", "success");
                }
                else
                {
                    // CẬP NHẬT KHÁCH HÀNG
                    string sql = @"
                        UPDATE KHACHHANG
                        SET HoTen = @HoTen,
                            SoDienThoai = @SoDienThoai,
                            Email = @Email,
                            DiaChi = @DiaChi
                        WHERE MaKH = @MaKH";

                    SqlParameter[] parameters =
                    {
                        new SqlParameter("@HoTen", hoTen),
                        new SqlParameter("@SoDienThoai", soDienThoai),
                        new SqlParameter("@Email",
                            string.IsNullOrWhiteSpace(email)
                                ? (object)DBNull.Value : email),
                        new SqlParameter("@DiaChi",
                            string.IsNullOrWhiteSpace(diaChi)
                                ? (object)DBNull.Value : diaChi),
                        new SqlParameter("@MaKH", Convert.ToInt32(hfMaKH.Value))
                    };

                    int ketQua = ketnoi.ThucThi(sql, parameters);

                    if (ketQua > 0)
                        HienThongBao("Cập nhật khách hàng thành công.", "success");
                    else
                        HienThongBao("Không tìm thấy khách hàng cần cập nhật.", "warning");
                }

                LamMoiForm();
                LoadDanhSachKhachHang();
            }
            catch (Exception ex)
            {
                HienThongBao("Lỗi lưu thông tin khách hàng: " + ex.Message, "danger");
            }
        }

        // XỬ LÝ NÚT SỬA VÀ XÓA
        protected void gvKhachHang_RowCommand(object sender,
            System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            if (e.CommandName == "SuaKH")
            {
                int maKH = Convert.ToInt32(e.CommandArgument);
                LoadThongTinKhachHang(maKH);
            }
            else if (e.CommandName == "XoaKH")
            {
                int maKH = Convert.ToInt32(e.CommandArgument);
                XoaKhachHang(maKH);
            }
        }

        // LẤY THÔNG TIN KHÁCH HÀNG ĐỂ SỬA
        private void LoadThongTinKhachHang(int maKH)
        {
            try
            {
                string sql = @"
                    SELECT MaKH, HoTen, SoDienThoai, Email, DiaChi
                    FROM KHACHHANG
                    WHERE MaKH = @MaKH";

                SqlParameter[] parameters =
                {
                    new SqlParameter("@MaKH", maKH)
                };

                DataTable dt = ketnoi.LayDuLieu(sql, parameters);

                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];

                    hfMaKH.Value = row["MaKH"].ToString();
                    txtHoTen.Text = row["HoTen"].ToString();
                    txtSoDienThoai.Text = row["SoDienThoai"].ToString();

                    txtEmail.Text = row["Email"] == DBNull.Value
                        ? "" : row["Email"].ToString();

                    txtDiaChi.Text = row["DiaChi"] == DBNull.Value
                        ? "" : row["DiaChi"].ToString();

                    lblTieuDeForm.Text = "CẬP NHẬT KHÁCH HÀNG";
                    btnLuu.Text = "Cập nhật";

                    HienThongBao("Đã tải thông tin khách hàng. Bạn có thể chỉnh sửa.", "info");
                }
                else
                {
                    HienThongBao("Không tìm thấy khách hàng.", "warning");
                }
            }
            catch (Exception ex)
            {
                HienThongBao("Lỗi lấy thông tin khách hàng: " + ex.Message, "danger");
            }
        }

        // XÓA KHÁCH HÀNG
        private void XoaKhachHang(int maKH)
        {
            try
            {
                string sql = @"
                    DELETE FROM KHACHHANG
                    WHERE MaKH = @MaKH";

                SqlParameter[] parameters =
                {
                    new SqlParameter("@MaKH", maKH)
                };

                int ketQua = ketnoi.ThucThi(sql, parameters);

                if (ketQua > 0)
                    HienThongBao("Xóa khách hàng thành công.", "success");
                else
                    HienThongBao("Không tìm thấy khách hàng cần xóa.", "warning");

                LamMoiForm();
                LoadDanhSachKhachHang();
            }
            catch (SqlException ex)
            {
                if (ex.Number == 547)
                {
                    HienThongBao(
                        "Không thể xóa khách hàng này vì đã có thông tin đặt sân liên quan.",
                        "danger");
                }
                else
                {
                    HienThongBao("Lỗi SQL khi xóa khách hàng: " + ex.Message, "danger");
                }
            }
            catch (Exception ex)
            {
                HienThongBao("Lỗi xóa khách hàng: " + ex.Message, "danger");
            }
        }

        // TÌM KIẾM KHÁCH HÀNG
        protected void btnTimKiem_Click(object sender, EventArgs e)
        {
            LoadDanhSachKhachHang();
        }

        // LÀM MỚI FORM
        protected void btnLamMoi_Click(object sender, EventArgs e)
        {
            LamMoiForm();
            pnlThongBao.Visible = false;
            LoadDanhSachKhachHang();
        }

        private void LamMoiForm()
        {
            hfMaKH.Value = "";

            txtHoTen.Text = "";
            txtSoDienThoai.Text = "";
            txtEmail.Text = "";
            txtDiaChi.Text = "";

            lblTieuDeForm.Text = "THÊM KHÁCH HÀNG";
            btnLuu.Text = "Lưu khách hàng";
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
