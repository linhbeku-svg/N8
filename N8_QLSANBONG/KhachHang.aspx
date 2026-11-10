
<%@ Page Title="Quản lý khách hàng" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="KhachHang.aspx.cs" Inherits="QuanLySanBong.KhachHang" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="container-fluid">

        <!-- TIÊU ĐỀ -->
        <div class="mb-4">
            <h3 class="fw-bold text-primary">
                <i class="fa-solid fa-users me-2"></i>
                QUẢN LÝ KHÁCH HÀNG
            </h3>

            <p class="text-secondary mb-0">
                Quản lý thông tin khách hàng sử dụng dịch vụ sân bóng.
            </p>
        </div>

        <!-- THÔNG BÁO -->
        <asp:Panel ID="pnlThongBao" runat="server"
            Visible="false"
            CssClass="alert"
            role="alert">

            <asp:Label ID="lblThongBao" runat="server" />

        </asp:Panel>

        <!-- FORM THÊM SỬA -->
        <div class="card border-0 shadow-sm mb-4">

            <div class="card-header bg-white py-3">
                <h5 class="fw-bold mb-0">
                    <asp:Label ID="lblTieuDeForm" runat="server"
                        Text="THÊM KHÁCH HÀNG" />
                </h5>
            </div>

            <div class="card-body">

                <asp:HiddenField ID="hfMaKH" runat="server" />

                <div class="row g-3">

                    <div class="col-md-6">

                        <label class="form-label fw-semibold">
                            Họ và tên
                        </label>

                        <asp:TextBox ID="txtHoTen" runat="server"
                            CssClass="form-control"
                            MaxLength="100"
                            placeholder="Nhập họ và tên khách hàng">
                        </asp:TextBox>

                    </div>

                    <div class="col-md-6">

                        <label class="form-label fw-semibold">
                            Số điện thoại
                        </label>

                        <asp:TextBox ID="txtSoDienThoai" runat="server"
                            CssClass="form-control"
                            MaxLength="15"
                            placeholder="Nhập số điện thoại">
                        </asp:TextBox>

                    </div>

                    <div class="col-md-6">

                        <label class="form-label fw-semibold">
                            Email
                        </label>

                        <asp:TextBox ID="txtEmail" runat="server"
                            CssClass="form-control"
                            MaxLength="100"
                            TextMode="Email"
                            placeholder="example@gmail.com">
                        </asp:TextBox>

                    </div>

                    <div class="col-md-6">

                        <label class="form-label fw-semibold">
                            Địa chỉ
                        </label>

                        <asp:TextBox ID="txtDiaChi" runat="server"
                            CssClass="form-control"
                            MaxLength="255"
                            placeholder="Nhập địa chỉ khách hàng">
                        </asp:TextBox>

                    </div>

                </div>

                <div class="mt-4">

                    <asp:Button ID="btnLuu" runat="server"
                        Text="Lưu khách hàng"
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

                <div class="row g-3 align-items-end">

                    <div class="col-md-9">

                        <label class="form-label fw-semibold">
                            Tìm kiếm khách hàng
                        </label>

                        <asp:TextBox ID="txtTimKiem" runat="server"
                            CssClass="form-control"
                            placeholder="Nhập họ tên hoặc số điện thoại">
                        </asp:TextBox>

                    </div>

                    <div class="col-md-3">

                        <asp:Button ID="btnTimKiem" runat="server"
                            Text="Tìm kiếm"
                            CssClass="btn btn-success w-100"
                            OnClick="btnTimKiem_Click" />

                    </div>

                </div>

            </div>

        </div>

        <!-- DANH SÁCH KHÁCH HÀNG -->
        <div class="card border-0 shadow-sm">

            <div class="card-header bg-white py-3">

                <h5 class="fw-bold mb-0">
                    <i class="fa-solid fa-list me-2 text-primary"></i>
                    DANH SÁCH KHÁCH HÀNG
                </h5>

            </div>

            <div class="card-body">

                <div class="table-responsive">

                    <asp:GridView ID="gvKhachHang" runat="server"
                        AutoGenerateColumns="False"
                        DataKeyNames="MaKH"
                        CssClass="table table-bordered table-hover table-striped align-middle"
                        GridLines="None"
                        EmptyDataText="Chưa có khách hàng nào."
                        OnRowCommand="gvKhachHang_RowCommand">

                        <Columns>

                            <asp:BoundField DataField="MaKH"
                                HeaderText="Mã KH" />

                            <asp:BoundField DataField="HoTen"
                                HeaderText="Họ và tên" />

                            <asp:BoundField DataField="SoDienThoai"
                                HeaderText="Số điện thoại" />

                            <asp:BoundField DataField="Email"
                                HeaderText="Email"
                                NullDisplayText="Chưa cập nhật" />

                            <asp:BoundField DataField="DiaChi"
                                HeaderText="Địa chỉ"
                                NullDisplayText="Chưa cập nhật" />

                            <asp:BoundField DataField="NgayTao"
                                HeaderText="Ngày tạo"
                                DataFormatString="{0:dd/MM/yyyy HH:mm}" />

                            <asp:TemplateField HeaderText="Thao tác">

                                <ItemTemplate>

                                    <asp:LinkButton ID="btnSua" runat="server"
                                        CommandName="SuaKH"
                                        CommandArgument='<%# Eval("MaKH") %>'
                                        CssClass="btn btn-warning btn-sm me-1"
                                        CausesValidation="false">

                                        <i class="fa-solid fa-pen-to-square"></i>
                                        Sửa

                                    </asp:LinkButton>

                                    <asp:LinkButton ID="btnXoa" runat="server"
                                        CommandName="XoaKH"
                                        CommandArgument='<%# Eval("MaKH") %>'
                                        CssClass="btn btn-danger btn-sm"
                                        CausesValidation="false"
                                        OnClientClick="return confirm('Bạn có chắc chắn muốn xóa khách hàng này không?');">

                                        <i class="fa-solid fa-trash"></i>
                                        Xóa

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
