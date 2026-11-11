
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DangNhap.aspx.cs" Inherits="QuanLySanBong.DangNhap" %>

<!DOCTYPE html>
<html lang="vi">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />

    <title>Đăng nhập - Quản lý sân bóng</title>

    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css"
          rel="stylesheet" />

    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.5.2/css/all.min.css"
          rel="stylesheet" />
</head>

<body class="bg-primary bg-gradient">

    <form id="form1" runat="server">

        <div class="container min-vh-100 d-flex align-items-center justify-content-center py-4">

            <div class="card shadow-lg border-0 rounded-4 w-100"
                 style="max-width: 430px;">

                <div class="card-body p-4 p-md-5">

                    <!-- LOGO -->
                    <div class="text-center mb-4">

                        <div class="text-primary mb-3">
                            <i class="fa-solid fa-futbol fa-4x"></i>
                        </div>

                        <h4 class="fw-bold text-primary">
                            QUẢN LÝ SÂN BÓNG
                        </h4>

                        <p class="text-secondary mb-0">
                            Đăng nhập để sử dụng hệ thống
                        </p>

                    </div>

                    <!-- THÔNG BÁO -->
                    <asp:Panel ID="pnlThongBao" runat="server"
                        Visible="false"
                        CssClass="alert alert-danger"
                        role="alert">

                        <i class="fa-solid fa-circle-exclamation me-2"></i>

                        <asp:Label ID="lblThongBao" runat="server" />

                    </asp:Panel>

                    <!-- TÊN ĐĂNG NHẬP -->
                    <div class="mb-3">

                        <label class="form-label fw-semibold">
                            Tên đăng nhập
                        </label>

                        <div class="input-group">

                            <span class="input-group-text">
                                <i class="fa-solid fa-user"></i>
                            </span>

                            <asp:TextBox ID="txtTenDangNhap" runat="server"
                                CssClass="form-control"
                                placeholder="Nhập tên đăng nhập"
                                MaxLength="50">
                            </asp:TextBox>

                        </div>

                    </div>

                    <!-- MẬT KHẨU -->
                    <div class="mb-4">

                        <label class="form-label fw-semibold">
                            Mật khẩu
                        </label>

                        <div class="input-group">

                            <span class="input-group-text">
                                <i class="fa-solid fa-lock"></i>
                            </span>

                            <asp:TextBox ID="txtMatKhau" runat="server"
                                TextMode="Password"
                                CssClass="form-control"
                                placeholder="Nhập mật khẩu"
                                MaxLength="255">
                            </asp:TextBox>

                        </div>

                    </div>

                    <!-- NÚT ĐĂNG NHẬP -->
                    <asp:Button ID="btnDangNhap" runat="server"
                        Text="ĐĂNG NHẬP"
                        CssClass="btn btn-primary w-100 fw-bold py-2"
                        OnClick="btnDangNhap_Click" />

                    <div class="text-center mt-4">
                        <small class="text-secondary">
                            © 2026 - Hệ thống quản lý sân bóng
                        </small>
                    </div>

                </div>

            </div>

        </div>

    </form>

    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js"></script>

</body>
</html>
