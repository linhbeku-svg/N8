
using System;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace QuanLySanBong
{
    public partial class TaiKhoan : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // KIỂM TRA ĐĂNG NHẬP
            if (Session["MaTK"] == null)
            {
                Response.Redirect("~/DangNhap.aspx");
                return;
            }

            // CHỈ ADMIN ĐƯỢC QUẢN LÝ TÀI KHOẢN
            if (Session["Quyen"] == null ||
                Session["Quyen"].ToString() != "Admin")
            {
                Response.Redirect("~/Default.aspx");
                return;
            }

            if (!IsPostBack)
            {
                LoadDanhSachTaiKhoan();
                LamMoiForm();
            }
        }

        // 1. HIỂN THỊ DANH SÁCH TÀI KHOẢN
        private void LoadDanhSachTaiKhoan()
        {
            try
            {
                string sql = @"
                    SELECT
                        MaTK,
                        TenDangNhap,
                        HoTen,
                        SoDienThoai,
                        Email,
                        Quyen,
                        TrangThai
                    FROM TAIKHOAN
                    WHERE
                        @TuKhoa = N''
                        OR TenDangNhap LIKE @TimKiem
                        OR HoTen LIKE @TimKiem
                        OR SoDienThoai LIKE @TimKiem
                    ORDER BY MaTK DESC";

                string tuKhoa = txtTimKiem.Text.Trim();

                SqlParameter[] parameters =
                {
                    new SqlParameter("@TuKhoa", SqlDbType.NVarChar, 100)
                    {
                        Value = tuKhoa
                    },
                    new SqlParameter("@TimKiem", SqlDbType.NVarChar, 120)
                    {
                        Value = "%" + tuKhoa + "%"
                    }
                };

                DataTable dt = ketnoi.LayDuLieu(sql, parameters);

                gvTaiKhoan.DataSource = dt;
                gvTaiKhoan.DataBind();
            }
            catch (Exception ex)
            {
                HienThongBao(
                    "Không thể tải danh sách tài khoản: " + ex.Message,
                    "danger");
            }
        }

        // 2. LƯU TÀI KHOẢN: THÊM MỚI HOẶC CẬP NHẬT
        protected void btnLuu_Click(object sender, EventArgs e)
        {
            pnlThongBao.Visible = false;

            string tenDangNhap = txtTenDangNhap.Text.Trim();
            string matKhau = txtMatKhau.Text;
            string hoTen = txtHoTen.Text.Trim();
            string soDienThoai = txtSoDienThoai.Text.Trim();
            string email = txtEmail.Text.Trim();
            string quyen = ddlQuyen.SelectedValue;

            // KIỂM TRA DỮ LIỆU
            if (string.IsNullOrWhiteSpace(tenDangNhap))
            {
                HienThongBao("Vui lòng nhập tên đăng nhập.", "warning");
                return;
            }

            if (string.IsNullOrWhiteSpace(hoTen))
            {
                HienThongBao("Vui lòng nhập họ và tên.", "warning");
                return;
            }

            if (tenDangNhap.Length > 50)
            {
                HienThongBao("Tên đăng nhập không được vượt quá 50 ký tự.", "warning");
                return;
            }

            if (hoTen.Length > 100)
            {
                HienThongBao("Họ tên không được vượt quá 100 ký tự.", "warning");
                return;
            }

            if (soDienThoai.Length > 15)
            {
                HienThongBao("Số điện thoại không được vượt quá 15 ký tự.", "warning");
                return;
            }

            if (email.Length > 100)
            {
                HienThongBao("Email không được vượt quá 100 ký tự.", "warning");
                return;
            }

            if (!string.IsNullOrWhiteSpace(email) &&
                !Regex.IsMatch(email,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                HienThongBao("Địa chỉ email không hợp lệ.", "warning");
                return;
            }

            if (quyen != "Admin" &&
                quyen != "QuanLy" &&
                quyen != "NhanVien")
            {
                HienThongBao("Quyền tài khoản không hợp lệ.", "warning");
                return;
            }

            try
            {
                // THÊM TÀI KHOẢN MỚI
                if (string.IsNullOrWhiteSpace(hfMaTK.Value))
                {
                    if (string.IsNullOrWhiteSpace(matKhau))
                    {
                        HienThongBao(
                            "Vui lòng nhập mật khẩu cho tài khoản mới.",
                            "warning");
                        return;
                    }

                    if (matKhau.Length > 255)
                    {
                        HienThongBao(
                            "Mật khẩu không được vượt quá 255 ký tự.",
                            "warning");
                        return;
                    }

                    string sql = @"
                        INSERT INTO TAIKHOAN
                        (
                            TenDangNhap,
                            MatKhau,
                            HoTen,
                            SoDienThoai,
                            Email,
                            Quyen,
                            TrangThai
                        )
                        VALUES
                        (
                            @TenDangNhap,
                            @MatKhau,
                            @HoTen,
                            @SoDienThoai,
                            @Email,
                            @Quyen,
                            1
                        )";

                    SqlParameter[] parameters =
                    {
                        new SqlParameter("@TenDangNhap", SqlDbType.VarChar, 50)
                        {
                            Value = tenDangNhap
                        },
                        new SqlParameter("@MatKhau", SqlDbType.VarChar, 255)
                        {
                            Value = matKhau
                        },
                        new SqlParameter("@HoTen", SqlDbType.NVarChar, 100)
                        {
                            Value = hoTen
                        },
                        new SqlParameter("@SoDienThoai", SqlDbType.VarChar, 15)
                        {
                            Value = string.IsNullOrWhiteSpace(soDienThoai)
                                ? (object)DBNull.Value : soDienThoai
                        },
                        new SqlParameter("@Email", SqlDbType.VarChar, 100)
                        {
                            Value = string.IsNullOrWhiteSpace(email)
                                ? (object)DBNull.Value : email
                        },
                        new SqlParameter("@Quyen", SqlDbType.VarChar, 20)
                        {
                            Value = quyen
                        }
                    };

                    ketnoi.ThucThi(sql, parameters);

                    HienThongBao(
                        "Thêm tài khoản mới thành công.",
                        "success");
                }
                else
                {
                    // CẬP NHẬT TÀI KHOẢN
                    int maTK;

                    if (!int.TryParse(hfMaTK.Value, out maTK))
                    {
                        HienThongBao("Mã tài khoản không hợp lệ.", "warning");
                        return;
                    }

                    // Không cho phép tự thay đổi quyền của tài khoản đang đăng nhập
                    int maTKDangNhap = Convert.ToInt32(Session["MaTK"]);

                    if (maTK == maTKDangNhap &&
                        quyen != "Admin")
                    {
                        HienThongBao(
                            "Không thể tự hạ quyền Admin của tài khoản đang đăng nhập.",
                            "warning");
                        return;
                    }

                    string sql;

                    if (string.IsNullOrWhiteSpace(matKhau))
                    {
                        sql = @"
                            UPDATE TAIKHOAN
                            SET
                                HoTen = @HoTen,
                                SoDienThoai = @SoDienThoai,
                                Email = @Email,
                                Quyen = @Quyen
                            WHERE MaTK = @MaTK";
                    }
                    else
                    {
                        if (matKhau.Length > 255)
                        {
                            HienThongBao(
                                "Mật khẩu không được vượt quá 255 ký tự.",
                                "warning");
                            return;
                        }

                        sql = @"
                            UPDATE TAIKHOAN
                            SET
                                MatKhau = @MatKhau,
                                HoTen = @HoTen,
                                SoDienThoai = @SoDienThoai,
                                Email = @Email,
                                Quyen = @Quyen
                            WHERE MaTK = @MaTK";
                    }

                    SqlParameter[] parameters =
                    {
                        new SqlParameter("@MaTK", SqlDbType.Int)
                        {
                            Value = maTK
                        },
                        new SqlParameter("@HoTen", SqlDbType.NVarChar, 100)
                        {
                            Value = hoTen
                        },
                        new SqlParameter("@SoDienThoai", SqlDbType.VarChar, 15)
                        {
                            Value = string.IsNullOrWhiteSpace(soDienThoai)
                                ? (object)DBNull.Value : soDienThoai
                        },
                        new SqlParameter("@Email", SqlDbType.VarChar, 100)
                        {
                            Value = string.IsNullOrWhiteSpace(email)
                                ? (object)DBNull.Value : email
                        },
                        new SqlParameter("@Quyen", SqlDbType.VarChar, 20)
                        {
                            Value = quyen
                        }
                    };

                    if (!string.IsNullOrWhiteSpace(matKhau))
                    {
                        SqlParameter pMatKhau =
                            new SqlParameter("@MatKhau", SqlDbType.VarChar, 255);

                        pMatKhau.Value = matKhau;

                        Array.Resize(ref parameters, parameters.Length + 1);
                        parameters[parameters.Length - 1] = pMatKhau;
                    }

                    int soDong = ketnoi.ThucThi(sql, parameters);

                    if (soDong == 0)
                    {
                        HienThongBao("Không tìm thấy tài khoản cần sửa.", "warning");
                        return;
                    }

                    HienThongBao(
                        "Cập nhật tài khoản thành công.",
                        "success");
                }

                LamMoiForm();
                LoadDanhSachTaiKhoan();
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2627 || ex.Number == 2601)
                {
                    HienThongBao(
                        "Tên đăng nhập đã tồn tại. Vui lòng chọn tên khác.",
                        "warning");
                }
                else
                {
                    HienThongBao(
                        "Lỗi cơ sở dữ liệu: " + ex.Message,
                        "danger");
                }
            }
            catch (Exception ex)
            {
                HienThongBao(
                    "Không thể lưu tài khoản: " + ex.Message,
                    "danger");
            }
        }

        // 3. XỬ LÝ NÚT SỬA VÀ KHÓA/MỞ KHÓA
        protected void gvTaiKhoan_RowCommand(
            object sender,
            System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            if (e.CommandArgument == null)
                return;

            int maTK;

            if (!int.TryParse(e.CommandArgument.ToString(), out maTK))
                return;

            if (e.CommandName == "SuaTaiKhoan")
            {
                LoadThongTinSua(maTK);
            }
            else if (e.CommandName == "DoiTrangThai")
            {
                DoiTrangThaiTaiKhoan(maTK);
            }
        }

        // 4. ĐƯA THÔNG TIN TÀI KHOẢN LÊN FORM
        private void LoadThongTinSua(int maTK)
        {
            try
            {
                string sql = @"
                    SELECT *
                    FROM TAIKHOAN
                    WHERE MaTK = @MaTK";

                SqlParameter[] parameters =
                {
                    new SqlParameter("@MaTK", maTK)
                };

                DataTable dt = ketnoi.LayDuLieu(sql, parameters);

                if (dt.Rows.Count == 0)
                {
                    HienThongBao("Không tìm thấy tài khoản.", "warning");
                    return;
                }

                DataRow row = dt.Rows[0];

                hfMaTK.Value = row["MaTK"].ToString();

                txtTenDangNhap.Text = row["TenDangNhap"].ToString();
                txtTenDangNhap.Enabled = false;

                // Không hiển thị lại mật khẩu cũ
                txtMatKhau.Text = "";

                txtHoTen.Text = row["HoTen"].ToString();
                txtSoDienThoai.Text =
                    row["SoDienThoai"] == DBNull.Value
                    ? "" : row["SoDienThoai"].ToString();

                txtEmail.Text =
                    row["Email"] == DBNull.Value
                    ? "" : row["Email"].ToString();

                ddlQuyen.SelectedValue = row["Quyen"].ToString();

                btnLuu.Text = "Cập nhật tài khoản";

                HienThongBao(
                    "Đang chỉnh sửa tài khoản: " +
                    row["TenDangNhap"].ToString(),
                    "info");
            }
            catch (Exception ex)
            {
                HienThongBao(
                    "Không thể lấy thông tin tài khoản: " + ex.Message,
                    "danger");
            }
        }

        // 5. KHÓA HOẶC MỞ KHÓA TÀI KHOẢN
        private void DoiTrangThaiTaiKhoan(int maTK)
        {
            try
            {
                int maTKDangNhap = Convert.ToInt32(Session["MaTK"]);

                if (maTK == maTKDangNhap)
                {
                    HienThongBao(
                        "Bạn không thể khóa tài khoản đang đăng nhập.",
                        "warning");
                    return;
                }

                string sql = @"
                    UPDATE TAIKHOAN
                    SET TrangThai =
                        CASE
                            WHEN TrangThai = 1 THEN 0
                            ELSE 1
                        END
                    WHERE MaTK = @MaTK";

                SqlParameter[] parameters =
                {
                    new SqlParameter("@MaTK", maTK)
                };

                int soDong = ketnoi.ThucThi(sql, parameters);

                if (soDong > 0)
                {
                    HienThongBao(
                        "Đã thay đổi trạng thái tài khoản thành công.",
                        "success");
                }
                else
                {
                    HienThongBao(
                        "Không tìm thấy tài khoản cần thay đổi.",
                        "warning");
                }

                LoadDanhSachTaiKhoan();
            }
            catch (Exception ex)
            {
                HienThongBao(
                    "Không thể thay đổi trạng thái tài khoản: " + ex.Message,
                    "danger");
            }
        }

        // 6. TÌM KIẾM
        protected void btnTimKiem_Click(object sender, EventArgs e)
        {
            LoadDanhSachTaiKhoan();
        }

        // 7. LÀM MỚI FORM
        protected void btnLamMoi_Click(object sender, EventArgs e)
        {
            pnlThongBao.Visible = false;

            LamMoiForm();

            txtTimKiem.Text = "";

            LoadDanhSachTaiKhoan();
        }

        private void LamMoiForm()
        {
            hfMaTK.Value = "";

            txtTenDangNhap.Text = "";
            txtTenDangNhap.Enabled = true;

            txtMatKhau.Text = "";
            txtHoTen.Text = "";
            txtSoDienThoai.Text = "";
            txtEmail.Text = "";

            if (ddlQuyen.Items.Count > 0)
                ddlQuyen.SelectedValue = "NhanVien";

            btnLuu.Text = "Lưu tài khoản";
        }

        // 8. THÔNG BÁO
        private void HienThongBao(string noiDung, string loai)
        {
            pnlThongBao.CssClass = "alert alert-" + loai;
            lblThongBao.Text = noiDung;
            pnlThongBao.Visible = true;
        }
    }
}
