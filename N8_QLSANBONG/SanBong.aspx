
<%@ Page Title="Quản lý sân bóng" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SanBong.aspx.cs" Inherits="QuanLySanBong.SanBong" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="container-fluid">

        <!-- TIÊU ĐỀ -->
        <div class="d-flex justify-content-between align-items-center mb-4">
            <div>
                <h3 class="fw-bold text-primary">
                    <i class="fa-solid fa-futbol me-2"></i>
                    QUẢN LÝ SÂN BÓNG
                </h3>
                <p class="text-secondary mb-0">
                    Quản lý thông tin và tình trạng các sân bóng.
                </p>
            </div>

            <span class="badge bg-primary fs-6">
                Quản lý sân
            </span>
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
                        Text="THÊM SÂN BÓNG" />
                </h5>
            </div>

            <div class="card-body">

                <asp:HiddenField ID="hfMaSan" runat="server" />

                <div class="row g-3">

                    <div class="col-md-6">

                        <label class="form-label fw-semibold">
                            Tên sân bóng
                        </label>

                        <asp:TextBox ID="txtTenSan" runat="server"
                            CssClass="form-control"
                            MaxLength="100"
                            placeholder="Nhập tên sân bóng">
                        </asp:TextBox>

                    </div>

                    <div class="col-md-6">

                        <label class="form-label fw-semibold">
                            Loại sân
                        </label>

                        <asp:DropDownList ID="ddlLoaiSan" runat="server"
                            CssClass="form-select">

                            <asp:ListItem Value="Sân 5">Sân 5 người</asp:ListItem>
                            <asp:ListItem Value="Sân 7">Sân 7 người</asp:ListItem>
                            <asp:ListItem Value="Sân 11">Sân 11 người</asp:ListItem>

                        </asp:DropDownList>

                    </div>

                    <div class="col-md-6">

                        <label class="form-label fw-semibold">
                            Trạng thái
                        </label>

                        <asp:DropDownList ID="ddlTrangThai" runat="server"
                            CssClass="form-select">

                            <asp:ListItem Value="Trong">Trống</asp:ListItem>
                            <asp:ListItem Value="DangSuDung">Đang sử dụng</asp:ListItem>
                            <asp:ListItem Value="BaoTri">Bảo trì</asp:ListItem>
                            <asp:ListItem Value="NgungHoatDong">Ngừng hoạt động</asp:ListItem>

                        </asp:DropDownList>

                    </div>

                    <div class="col-md-6">

                        <label class="form-label fw-semibold">
                            Mô tả
                        </label>

                        <asp:TextBox ID="txtMoTa" runat="server"
                            CssClass="form-control"
                            MaxLength="255"
                            placeholder="Nhập mô tả sân bóng">
                        </asp:TextBox>

                    </div>

                </div>

                <div class="mt-4">

                    <asp:Button ID="btnLuu" runat="server"
                        Text="Lưu thông tin"
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

        <!-- TÌM KIẾM VÀ LỌC -->
        <div class="card border-0 shadow-sm mb-4">

            <div class="card-body">

                <div class="row g-3 align-items-end">

                    <div class="col-md-6">

                        <label class="form-label fw-semibold">
                            Tìm kiếm sân bóng
                        </label>

                        <asp:TextBox ID="txtTimKiem" runat="server"
                            CssClass="form-control"
                            placeholder="Nhập tên sân cần tìm">
                        </asp:TextBox>

                    </div>

                    <div class="col-md-4">

                        <label class="form-label fw-semibold">
                            Lọc theo trạng thái
                        </label>

                        <asp:DropDownList ID="ddlLocTrangThai" runat="server"
                            CssClass="form-select">

                            <asp:ListItem Value="">Tất cả trạng thái</asp:ListItem>
                            <asp:ListItem Value="Trong">Trống</asp:ListItem>
                            <asp:ListItem Value="DangSuDung">Đang sử dụng</asp:ListItem>
                            <asp:ListItem Value="BaoTri">Bảo trì</asp:ListItem>
                            <asp:ListItem Value="NgungHoatDong">Ngừng hoạt động</asp:ListItem>

                        </asp:DropDownList>

                    </div>

                    <div class="col-md-2">

                        <asp:Button ID="btnTimKiem" runat="server"
                            Text="Tìm kiếm"
                            CssClass="btn btn-success w-100"
                            OnClick="btnTimKiem_Click" />

                    </div>

                </div>

            </div>

        </div>

        <!-- DANH SÁCH SÂN -->
        <div class="card border-0 shadow-sm">

            <div class="card-header bg-white py-3">
                <h5 class="fw-bold mb-0">
                    <i class="fa-solid fa-list me-2 text-primary"></i>
                    DANH SÁCH SÂN BÓNG
                </h5>
            </div>

            <div class="card-body">

                <div class="table-responsive">

                    <asp:GridView ID="gvSanBong" runat="server"
                        AutoGenerateColumns="False"
                        DataKeyNames="MaSan"
                        CssClass="table table-bordered table-hover table-striped align-middle"
                        GridLines="None"
                        EmptyDataText="Không có sân bóng nào."
                        OnRowCommand="gvSanBong_RowCommand">

                        <Columns>

                            <asp:BoundField DataField="MaSan"
                                HeaderText="Mã sân" />

                            <asp:BoundField DataField="TenSan"
                                HeaderText="Tên sân" />

                            <asp:BoundField DataField="LoaiSan"
                                HeaderText="Loại sân" />

                            <asp:TemplateField HeaderText="Trạng thái">
                                <ItemTemplate>

                                    <asp:Label ID="lblTrangThai" runat="server"
                                        Text='<%# Eval("TrangThai").ToString() == "Trong" ? "Trống" : Eval("TrangThai").ToString() == "DangSuDung" ? "Đang sử dụng" : Eval("TrangThai").ToString() == "BaoTri" ? "Bảo trì" : "Ngừng hoạt động" %>'
                                        CssClass='<%# Eval("TrangThai").ToString() == "Trong" ? "badge bg-success" : Eval("TrangThai").ToString() == "DangSuDung" ? "badge bg-primary" : Eval("TrangThai").ToString() == "BaoTri" ? "badge bg-warning text-dark" : "badge bg-danger" %>' />

                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:BoundField DataField="MoTa"
                                HeaderText="Mô tả" />

                            <asp:TemplateField HeaderText="Thao tác">
                                <ItemTemplate>

                                    <asp:LinkButton ID="btnSua" runat="server"
                                        CommandName="SuaSan"
                                        CommandArgument='<%# Eval("MaSan") %>'
                                        CssClass="btn btn-warning btn-sm me-1"
                                        CausesValidation="false">
                                        <i class="fa-solid fa-pen-to-square"></i>
                                        Sửa
                                    </asp:LinkButton>

                                    <asp:LinkButton ID="btnXoa" runat="server"
                                        CommandName="XoaSan"
                                        CommandArgument='<%# Eval("MaSan") %>'
                                        CssClass="btn btn-danger btn-sm"
                                        CausesValidation="false"
                                        OnClientClick="return confirm('Bạn có chắc chắn muốn xóa sân bóng này không?');">
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
