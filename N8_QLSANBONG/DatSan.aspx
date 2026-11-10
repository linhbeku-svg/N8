
<%@ Page Title="Quản lý đặt sân" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DatSan.aspx.cs" Inherits="QuanLySanBong.DatSan" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="container-fluid">

        <!-- TIÊU ĐỀ -->
        <div class="mb-4">
            <h3 class="fw-bold text-primary">
                <i class="fa-solid fa-calendar-check me-2"></i>
                QUẢN LÝ ĐẶT SÂN
            </h3>
            <p class="text-secondary mb-0">
                Quản lý lịch đặt sân và thông tin thuê sân bóng.
            </p>
        </div>

        <!-- THÔNG BÁO -->
        <asp:Panel ID="pnlThongBao" runat="server"
            Visible="false" CssClass="alert" role="alert">
            <asp:Label ID="lblThongBao" runat="server" />
        </asp:Panel>

        <!-- FORM ĐẶT SÂN -->
        <div class="card border-0 shadow-sm mb-4">

            <div class="card-header bg-white py-3">
                <h5 class="fw-bold mb-0">
                    <asp:Label ID="lblTieuDeForm" runat="server"
                        Text="THÊM PHIẾU ĐẶT SÂN" />
                </h5>
            </div>

            <div class="card-body">

                <asp:HiddenField ID="hfMaDatSan" runat="server" />

                <div class="row g-3">

                    <div class="col-md-6">
                        <label class="form-label fw-semibold">Khách hàng</label>
                        <asp:DropDownList ID="ddlKhachHang" runat="server"
                            CssClass="form-select">
                        </asp:DropDownList>
                    </div>

                    <div class="col-md-6">
                        <label class="form-label fw-semibold">Sân bóng</label>
                        <asp:DropDownList ID="ddlSanBong" runat="server"
                            CssClass="form-select">
                        </asp:DropDownList>
                    </div>

                    <div class="col-md-6">
                        <label class="form-label fw-semibold">Ngày thuê</label>
                        <asp:TextBox ID="txtNgayThue" runat="server"
                            CssClass="form-control"
                            TextMode="Date">
                        </asp:TextBox>
                    </div>

                    <div class="col-md-3">
                        <label class="form-label fw-semibold">Giờ bắt đầu</label>
                        <asp:TextBox ID="txtGioBatDau" runat="server"
                            CssClass="form-control"
                            TextMode="Time">
                        </asp:TextBox>
                    </div>

                    <div class="col-md-3">
                        <label class="form-label fw-semibold">Giờ kết thúc</label>
                        <asp:TextBox ID="txtGioKetThuc" runat="server"
                            CssClass="form-control"
                            TextMode="Time">
                        </asp:TextBox>
                    </div>

                    <div class="col-md-6">
                        <label class="form-label fw-semibold">Tiền cọc (VNĐ)</label>
                        <asp:TextBox ID="txtTienCoc" runat="server"
                            CssClass="form-control"
                            TextMode="Number"
                            placeholder="Nhập tiền cọc">
                        </asp:TextBox>
                    </div>

                    <div class="col-md-6">
                        <label class="form-label fw-semibold">Trạng thái</label>
                        <asp:DropDownList ID="ddlTrangThai" runat="server"
                            CssClass="form-select">
                            <asp:ListItem Value="ChoXacNhan">Chờ xác nhận</asp:ListItem>
                            <asp:ListItem Value="DaXacNhan">Đã xác nhận</asp:ListItem>
                            <asp:ListItem Value="DangSuDung">Đang sử dụng</asp:ListItem>
                            <asp:ListItem Value="HoanThanh">Hoàn thành</asp:ListItem>
                            <asp:ListItem Value="DaHuy">Đã hủy</asp:ListItem>
                        </asp:DropDownList>
                    </div>

                </div>

                <div class="mt-4">
                    <asp:Button ID="btnLuu" runat="server"
                        Text="Lưu phiếu đặt sân"
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
                            Tìm kiếm phiếu đặt sân
                        </label>
                        <asp:TextBox ID="txtTimKiem" runat="server"
                            CssClass="form-control"
                            placeholder="Nhập tên khách hàng hoặc tên sân">
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

        <!-- DANH SÁCH ĐẶT SÂN -->
        <div class="card border-0 shadow-sm">
            <div class="card-header bg-white py-3">
                <h5 class="fw-bold mb-0">
                    <i class="fa-solid fa-list me-2 text-primary"></i>
                    DANH SÁCH PHIẾU ĐẶT SÂN
                </h5>
            </div>

            <div class="card-body">
                <div class="table-responsive">

                    <asp:GridView ID="gvDatSan" runat="server"
                        AutoGenerateColumns="False"
                        DataKeyNames="MaDatSan"
                        CssClass="table table-bordered table-hover table-striped align-middle"
                        GridLines="None"
                        EmptyDataText="Chưa có phiếu đặt sân nào."
                        OnRowCommand="gvDatSan_RowCommand">

                        <Columns>

                            <asp:BoundField DataField="MaDatSan"
                                HeaderText="Mã đặt sân" />

                            <asp:BoundField DataField="HoTen"
                                HeaderText="Khách hàng" />

                            <asp:BoundField DataField="SoDienThoai"
                                HeaderText="Số điện thoại" />

                            <asp:BoundField DataField="TenSan"
                                HeaderText="Sân bóng" />

                            <asp:BoundField DataField="NgayThue"
                                HeaderText="Ngày thuê"
                                DataFormatString="{0:dd/MM/yyyy}" />

                            <asp:BoundField DataField="GioBatDau"
                                HeaderText="Giờ bắt đầu" />

                            <asp:BoundField DataField="GioKetThuc"
                                HeaderText="Giờ kết thúc" />

                            <asp:BoundField DataField="TienCoc"
                                HeaderText="Tiền cọc"
                                DataFormatString="{0:N0} VNĐ" />

                            <asp:TemplateField HeaderText="Trạng thái">
                                <ItemTemplate>
                                    <asp:Label ID="lblTrangThai" runat="server"
                                        Text='<%# Eval("TrangThai").ToString() == "ChoXacNhan" ? "Chờ xác nhận" : Eval("TrangThai").ToString() == "DaXacNhan" ? "Đã xác nhận" : Eval("TrangThai").ToString() == "DangSuDung" ? "Đang sử dụng" : Eval("TrangThai").ToString() == "HoanThanh" ? "Hoàn thành" : "Đã hủy" %>'
                                        CssClass='<%# Eval("TrangThai").ToString() == "ChoXacNhan" ? "badge bg-warning text-dark" : Eval("TrangThai").ToString() == "DaXacNhan" ? "badge bg-primary" : Eval("TrangThai").ToString() == "DangSuDung" ? "badge bg-info text-dark" : Eval("TrangThai").ToString() == "HoanThanh" ? "badge bg-success" : "badge bg-danger" %>' />
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Thao tác">
                                <ItemTemplate>

                                    <asp:LinkButton ID="btnSua" runat="server"
                                        CommandName="SuaDatSan"
                                        CommandArgument='<%# Eval("MaDatSan") %>'
                                        CssClass="btn btn-warning btn-sm"
                                        CausesValidation="false">
                                        <i class="fa-solid fa-pen-to-square"></i>
                                        Sửa
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
