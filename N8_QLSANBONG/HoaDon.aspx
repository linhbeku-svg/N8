
<%@ Page Title="Quản lý hóa đơn" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="HoaDon.aspx.cs" Inherits="QuanLySanBong.HoaDon" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="container-fluid">

        <div class="mb-4">
            <h3 class="fw-bold text-primary">
                <i class="fa-solid fa-file-invoice-dollar me-2"></i>
                QUẢN LÝ HÓA ĐƠN
            </h3>
            <p class="text-secondary mb-0">
                Lập và quản lý hóa đơn thanh toán sân bóng.
            </p>
        </div>

        <asp:Panel ID="pnlThongBao" runat="server"
            Visible="false" CssClass="alert" role="alert">
            <asp:Label ID="lblThongBao" runat="server" />
        </asp:Panel>

        <!-- FORM LẬP HÓA ĐƠN -->
        <div class="card border-0 shadow-sm mb-4">

            <div class="card-header bg-white py-3">
                <h5 class="fw-bold mb-0">
                    <asp:Label ID="lblTieuDeForm" runat="server"
                        Text="LẬP HÓA ĐƠN" />
                </h5>
            </div>

            <div class="card-body">

                <asp:HiddenField ID="hfMaHD" runat="server" />

                <div class="row g-3">

                    <div class="col-md-6">
                        <label class="form-label fw-semibold">Phiếu đặt sân</label>

                        <asp:DropDownList ID="ddlDatSan" runat="server"
                            CssClass="form-select"
                            AutoPostBack="true"
                            OnSelectedIndexChanged="ddlDatSan_SelectedIndexChanged">
                        </asp:DropDownList>
                    </div>

                    <div class="col-md-6">
                        <label class="form-label fw-semibold">Tiền sân (VNĐ)</label>

                        <asp:TextBox ID="txtTienSan" runat="server"
                            CssClass="form-control"
                            ReadOnly="true">
                        </asp:TextBox>
                    </div>

                    <div class="col-md-6">
                        <label class="form-label fw-semibold">Tiền dịch vụ (VNĐ)</label>

                        <asp:TextBox ID="txtTienDichVu" runat="server"
                            CssClass="form-control"
                            TextMode="Number"
                            AutoPostBack="true"
                            OnTextChanged="TinhTongTien">
                        </asp:TextBox>
                    </div>

                    <div class="col-md-6">
                        <label class="form-label fw-semibold">Giảm giá (VNĐ)</label>

                        <asp:TextBox ID="txtGiamGia" runat="server"
                            CssClass="form-control"
                            TextMode="Number"
                            AutoPostBack="true"
                            OnTextChanged="TinhTongTien">
                        </asp:TextBox>
                    </div>

                    <div class="col-md-6">
                        <label class="form-label fw-semibold">Tổng tiền (VNĐ)</label>

                        <asp:TextBox ID="txtTongTien" runat="server"
                            CssClass="form-control fw-bold text-primary"
                            ReadOnly="true">
                        </asp:TextBox>
                    </div>

                    <div class="col-md-6">
                        <label class="form-label fw-semibold">Trạng thái hóa đơn</label>

                        <asp:DropDownList ID="ddlTrangThai" runat="server"
                            CssClass="form-select">
                            <asp:ListItem Value="ChuaThanhToan">Chưa thanh toán</asp:ListItem>
                            <asp:ListItem Value="ThanhToanMotPhan">Thanh toán một phần</asp:ListItem>
                            <asp:ListItem Value="DaThanhToan">Đã thanh toán</asp:ListItem>
                        </asp:DropDownList>
                    </div>

                </div>

                <div class="mt-4">

                    <asp:Button ID="btnLuu" runat="server"
                        Text="Lập hóa đơn"
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
                        <label class="form-label fw-semibold">Tìm kiếm hóa đơn</label>

                        <asp:TextBox ID="txtTimKiem" runat="server"
                            CssClass="form-control"
                            placeholder="Nhập mã hóa đơn, tên khách hàng hoặc tên sân">
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

        <!-- DANH SÁCH HÓA ĐƠN -->
        <div class="card border-0 shadow-sm">

            <div class="card-header bg-white py-3">
                <h5 class="fw-bold mb-0">
                    <i class="fa-solid fa-list me-2 text-primary"></i>
                    DANH SÁCH HÓA ĐƠN
                </h5>
            </div>

            <div class="card-body">

                <div class="table-responsive">

                    <asp:GridView ID="gvHoaDon" runat="server"
                        AutoGenerateColumns="False"
                        DataKeyNames="MaHD"
                        CssClass="table table-bordered table-hover table-striped align-middle"
                        GridLines="None"
                        EmptyDataText="Chưa có hóa đơn nào."
                        OnRowCommand="gvHoaDon_RowCommand">

                        <Columns>

                            <asp:BoundField DataField="MaHD"
                                HeaderText="Mã hóa đơn" />

                            <asp:BoundField DataField="MaDatSan"
                                HeaderText="Mã đặt sân" />

                            <asp:BoundField DataField="HoTen"
                                HeaderText="Khách hàng" />

                            <asp:BoundField DataField="TenSan"
                                HeaderText="Sân bóng" />

                            <asp:BoundField DataField="NgayLap"
                                HeaderText="Ngày lập"
                                DataFormatString="{0:dd/MM/yyyy HH:mm}" />

                            <asp:BoundField DataField="TienSan"
                                HeaderText="Tiền sân"
                                DataFormatString="{0:N0} VNĐ" />

                            <asp:BoundField DataField="TienDichVu"
                                HeaderText="Tiền dịch vụ"
                                DataFormatString="{0:N0} VNĐ" />

                            <asp:BoundField DataField="GiamGia"
                                HeaderText="Giảm giá"
                                DataFormatString="{0:N0} VNĐ" />

                            <asp:BoundField DataField="TongTien"
                                HeaderText="Tổng tiền"
                                DataFormatString="{0:N0} VNĐ" />

                            <asp:TemplateField HeaderText="Trạng thái">
                                <ItemTemplate>
                                    <asp:Label ID="lblTrangThai" runat="server"
                                        Text='<%# Eval("TrangThai").ToString() == "DaThanhToan" ? "Đã thanh toán" : (Eval("TrangThai").ToString() == "ThanhToanMotPhan" ? "Thanh toán một phần" : "Chưa thanh toán") %>'
                                        CssClass='<%# Eval("TrangThai").ToString() == "DaThanhToan" ? "badge bg-success" : (Eval("TrangThai").ToString() == "ThanhToanMotPhan" ? "badge bg-warning text-dark" : "badge bg-danger") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Thao tác">
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnSua" runat="server"
                                        CommandName="SuaHD"
                                        CommandArgument='<%# Eval("MaHD") %>'
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
