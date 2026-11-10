
<%@ Page Title="Quản lý tài khoản" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="TaiKhoan.aspx.cs" Inherits="QuanLySanBong.TaiKhoan" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="container-fluid">

        <!-- TIÊU ĐỀ -->
        <div class="mb-4">
            <h3 class="fw-bold text-primary">
                <i class="fa-solid fa-users-gear me-2"></i>
                QUẢN LÝ TÀI KHOẢN
            </h3>
            <p class="text-secondary mb-0">
                Quản lý tài khoản đăng nhập và phân quyền người sử dụng hệ thống.
            </p>
        </div>

        <!-- THÔNG BÁO -->
        <asp:Panel ID="pnlThongBao" runat="server"
            Visible="false" CssClass="alert" role="alert">
            <asp:Label ID="lblThongBao" runat="server" />
        </asp:Panel>

        <!-- FORM THÊM SỬA -->
        <div class="card border-0 shadow-sm mb-4">

            <div class="card-header bg-white py-3">
                <h5 class="fw-bold mb-0">
                    <i class="fa-solid fa-user-plus me-2 text-primary"></i>
                    THÔNG TIN TÀI KHOẢN
                </h5>
            </div>

            <div class="card-body">

                <asp:HiddenField ID="hfMaTK" runat="server" />

                <div class="row g-3">

                    <div class="col-md-6">
                        <label class="form-label fw-semibold">Tên đăng nhập</label>
                        <asp:TextBox ID="txtTenDangNhap" runat="server"
                            CssClass="form-control"
                            MaxLength="50"
                            placeholder="Nhập tên đăng nhập">
                        </asp:TextBox>
                    </div>

                    <div class="col-md-6">
                        <label class="form-label fw-semibold">Mật khẩu</label>
                        <asp:TextBox ID="txtMatKhau" runat="server"
                            CssClass="form-control"
                            TextMode="Password"
                            MaxLength="255"
                            placeholder="Nhập mật khẩu">
                        </asp:TextBox>
                        <div class="form-text">
                            Khi sửa tài khoản, để trống nếu không muốn đổi mật khẩu.
                        </div>
                    </div>

                    <div class="col-md-6">
                        <label class="form-label fw-semibold">Họ và tên</label>
                        <asp:TextBox ID="txtHoTen" runat="server"
                            CssClass="form-control"
                            MaxLength="100"
                            placeholder="Nhập họ và tên">
                        </asp:TextBox>
                    </div>

                    <div class="col-md-6">
                        <label class="form-label fw-semibold">Số điện thoại</label>
                        <asp:TextBox ID="txtSoDienThoai" runat="server"
                            CssClass="form-control"
                            MaxLength="15"
                            placeholder="Nhập số điện thoại">
                        </asp:TextBox>
                    </div>

                    <div class="col-md-6">
                        <label class="form-label fw-semibold">Email</label>
                        <asp:TextBox ID="txtEmail" runat="server"
                            CssClass="form-control"
                            TextMode="Email"
                            MaxLength="100"
                            placeholder="Nhập địa chỉ email">
                        </asp:TextBox>
                    </div>

                    <div class="col-md-6">
                        <label class="form-label fw-semibold">Quyền tài khoản</label>
                        <asp:DropDownList ID="ddlQuyen" runat="server"
                            CssClass="form-select">
                            <asp:ListItem Value="NhanVien">Nhân viên</asp:ListItem>
                            <asp:ListItem Value="QuanLy">Quản lý</asp:ListItem>
                            <asp:ListItem Value="Admin">Admin</asp:ListItem>
                        </asp:DropDownList>
                    </div>

                </div>

                <div class="mt-4">

                    <asp:Button ID="btnLuu" runat="server"
                        Text="Lưu tài khoản"
                        CssClass="btn btn-primary me-2"
                        OnClick="btnLuu_Click" />

                    <asp:Button ID="btnLamMoi" runat="server"
                        Text="Làm mới"
                        CssClass="btn btn-secondary"
                        OnClick="btnLamMoi_Click"
                        CausesValidation="false" />

                </div>

            </div>
        </div>

        <!-- TÌM KIẾM -->
        <div class="card border-0 shadow-sm mb-4">
            <div class="card-body">

                <div class="row g-2">

                    <div class="col-md-10">
                        <asp:TextBox ID="txtTimKiem" runat="server"
                            CssClass="form-control"
                            placeholder="Nhập tên đăng nhập, họ tên hoặc số điện thoại">
                        </asp:TextBox>
                    </div>

                    <div class="col-md-2">
                        <asp:Button ID="btnTimKiem" runat="server"
                            Text="Tìm kiếm"
                            CssClass="btn btn-primary w-100"
                            OnClick="btnTimKiem_Click" />
                    </div>

                </div>

            </div>
        </div>

        <!-- DANH SÁCH TÀI KHOẢN -->
        <div class="card border-0 shadow-sm">

            <div class="card-header bg-white py-3">
                <h5 class="fw-bold mb-0">
                    <i class="fa-solid fa-list me-2 text-primary"></i>
                    DANH SÁCH TÀI KHOẢN
                </h5>
            </div>

            <div class="card-body">

                <div class="table-responsive">

                    <asp:GridView ID="gvTaiKhoan" runat="server"
                        AutoGenerateColumns="False"
                        CssClass="table table-bordered table-hover table-striped align-middle"
                        GridLines="None"
                        DataKeyNames="MaTK"
                        OnRowCommand="gvTaiKhoan_RowCommand"
                        EmptyDataText="Không tìm thấy tài khoản nào.">

                        <Columns>

                            <asp:BoundField DataField="MaTK"
                                HeaderText="Mã tài khoản" />

                            <asp:BoundField DataField="TenDangNhap"
                                HeaderText="Tên đăng nhập" />

                            <asp:BoundField DataField="HoTen"
                                HeaderText="Họ và tên" />

                            <asp:BoundField DataField="SoDienThoai"
                                HeaderText="Số điện thoại" />

                            <asp:BoundField DataField="Email"
                                HeaderText="Email" />

                            <asp:TemplateField HeaderText="Quyền">
                                <ItemTemplate>
                                    <%# Eval("Quyen").ToString() == "Admin" ? "Quản trị viên" :
                                        (Eval("Quyen").ToString() == "QuanLy" ? "Quản lý" : "Nhân viên") %>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Trạng thái">
                                <ItemTemplate>
                                    <asp:Label ID="lblTrangThai" runat="server"
                                        Text='<%# Convert.ToInt32(Eval("TrangThai")) == 1 ? "Hoạt động" : "Đã khóa" %>'
                                        CssClass='<%# Convert.ToInt32(Eval("TrangThai")) == 1 ? "badge text-bg-success" : "badge text-bg-danger" %>'>
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Thao tác">
                                <ItemTemplate>

                                    <asp:LinkButton ID="btnSua" runat="server"
                                        CommandName="SuaTaiKhoan"
                                        CommandArgument='<%# Eval("MaTK") %>'
                                        CssClass="btn btn-sm btn-warning me-1"
                                        CausesValidation="false">
                                        <i class="fa-solid fa-pen-to-square"></i> Sửa
                                    </asp:LinkButton>

                                    <asp:LinkButton ID="btnKhoa" runat="server"
                                        CommandName="DoiTrangThai"
                                        CommandArgument='<%# Eval("MaTK") %>'
                                        CssClass='<%# Convert.ToInt32(Eval("TrangThai")) == 1 ? "btn btn-sm btn-danger" : "btn btn-sm btn-success" %>'
                                        CausesValidation="false"
                                        OnClientClick="return confirm('Bạn có chắc muốn thay đổi trạng thái tài khoản này?');">
                                        <%# Convert.ToInt32(Eval("TrangThai")) == 1 ? "Khóa" : "Mở khóa" %>
                                    </asp:LinkButton>

                                </ItemTemplate>
                            </asp:TemplateField>

                        </Columns>

                        <HeaderStyle CssClass="table-primary text-center" />

                    </asp:GridView>

                </div>

            </div>
        </div>

    </div>

</asp:Content>
