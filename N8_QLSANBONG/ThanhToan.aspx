
<%@ Page Title="Quản lý thanh toán" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ThanhToan.aspx.cs" Inherits="QuanLySanBong.ThanhToan" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="container-fluid">

        <div class="mb-4">
            <h3 class="fw-bold text-primary">
                <i class="fa-solid fa-money-bill-transfer me-2"></i>
                QUẢN LÝ THANH TOÁN
            </h3>
            <p class="text-secondary mb-0">
                Quản lý giao dịch và lịch sử thanh toán hóa đơn.
            </p>
        </div>

        <asp:Panel ID="pnlThongBao" runat="server"
            Visible="false" CssClass="alert" role="alert">
            <asp:Label ID="lblThongBao" runat="server" />
        </asp:Panel>

        <!-- THÔNG TIN THANH TOÁN -->
        <div class="card border-0 shadow-sm mb-4">

            <div class="card-header bg-white py-3">
                <h5 class="fw-bold mb-0">THỰC HIỆN THANH TOÁN</h5>
            </div>

            <div class="card-body">

                <div class="row g-3">

                    <div class="col-md-6">
                        <label class="form-label fw-semibold">Hóa đơn cần thanh toán</label>

                        <asp:DropDownList ID="ddlHoaDon" runat="server"
                            CssClass="form-select"
                            AutoPostBack="true"
                            OnSelectedIndexChanged="ddlHoaDon_SelectedIndexChanged">
                        </asp:DropDownList>
                    </div>

                    <div class="col-md-6">
                        <label class="form-label fw-semibold">Tổng tiền hóa đơn (VNĐ)</label>

                        <asp:TextBox ID="txtTongTien" runat="server"
                            CssClass="form-control"
                            ReadOnly="true">
                        </asp:TextBox>
                    </div>

                    <div class="col-md-6">
                        <label class="form-label fw-semibold">Đã thanh toán (VNĐ)</label>

                        <asp:TextBox ID="txtDaThanhToan" runat="server"
                            CssClass="form-control"
                            ReadOnly="true">
                        </asp:TextBox>
                    </div>

                    <div class="col-md-6">
                        <label class="form-label fw-semibold">Còn phải thanh toán (VNĐ)</label>

                        <asp:TextBox ID="txtConPhaiTra" runat="server"
                            CssClass="form-control fw-bold text-danger"
                            ReadOnly="true">
                        </asp:TextBox>
                    </div>

                    <div class="col-md-6">
                        <label class="form-label fw-semibold">Số tiền thanh toán (VNĐ)</label>

                        <asp:TextBox ID="txtSoTien" runat="server"
                            CssClass="form-control"
                            TextMode="Number"
                            placeholder="Nhập số tiền khách thanh toán">
                        </asp:TextBox>
                    </div>

                    <div class="col-md-6">
                        <label class="form-label fw-semibold">Phương thức thanh toán</label>

                        <asp:DropDownList ID="ddlPhuongThuc" runat="server"
                            CssClass="form-select">
                            <asp:ListItem Value="TienMat">Tiền mặt</asp:ListItem>
                            <asp:ListItem Value="ChuyenKhoan">Chuyển khoản</asp:ListItem>
                            <asp:ListItem Value="ViDienTu">Ví điện tử</asp:ListItem>
                        </asp:DropDownList>
                    </div>

                    <div class="col-md-12">
                        <label class="form-label fw-semibold">Mã giao dịch</label>

                        <asp:TextBox ID="txtMaGiaoDich" runat="server"
                            CssClass="form-control"
                            MaxLength="100"
                            placeholder="Nhập mã giao dịch nếu có">
                        </asp:TextBox>

                        <div class="form-text">
                            Có thể để trống nếu thanh toán bằng tiền mặt.
                        </div>
                    </div>

                </div>

                <div class="mt-4">

                    <asp:Button ID="btnThanhToan" runat="server"
                        Text="Xác nhận thanh toán"
                        CssClass="btn btn-primary me-2"
                        OnClick="btnThanhToan_Click"
                        OnClientClick="return confirm('Bạn xác nhận ghi nhận giao dịch thanh toán này?');" />

                    <asp:Button ID="btnLamMoi" runat="server"
                        Text="Làm mới"
                        CssClass="btn btn-secondary"
                        OnClick="btnLamMoi_Click"
                        CausesValidation="false" />

                </div>

            </div>
        </div>

        <!-- LỊCH SỬ THANH TOÁN -->
        <div class="card border-0 shadow-sm">

            <div class="card-header bg-white py-3">
                <h5 class="fw-bold mb-0">
                    <i class="fa-solid fa-clock-rotate-left me-2 text-primary"></i>
                    LỊCH SỬ THANH TOÁN
                </h5>
            </div>

            <div class="card-body">

                <div class="table-responsive">

                    <asp:GridView ID="gvThanhToan" runat="server"
                        AutoGenerateColumns="False"
                        CssClass="table table-bordered table-hover table-striped align-middle"
                        GridLines="None"
                        EmptyDataText="Chưa có giao dịch thanh toán nào.">

                        <Columns>

                            <asp:BoundField DataField="MaTT"
                                HeaderText="Mã thanh toán" />

                            <asp:BoundField DataField="MaHD"
                                HeaderText="Mã hóa đơn" />

                            <asp:BoundField DataField="HoTen"
                                HeaderText="Khách hàng" />

                            <asp:BoundField DataField="NgayThanhToan"
                                HeaderText="Ngày thanh toán"
                                DataFormatString="{0:dd/MM/yyyy HH:mm}" />

                            <asp:BoundField DataField="SoTien"
                                HeaderText="Số tiền"
                                DataFormatString="{0:N0} VNĐ" />

                            <asp:TemplateField HeaderText="Phương thức">
                                <ItemTemplate>
                                    <%# Eval("PhuongThuc").ToString() == "TienMat" ? "Tiền mặt" :
                                        (Eval("PhuongThuc").ToString() == "ChuyenKhoan" ? "Chuyển khoản" : "Ví điện tử") %>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:BoundField DataField="MaGiaoDich"
                                HeaderText="Mã giao dịch" />

                            <asp:BoundField DataField="TrangThai"
                                HeaderText="Trạng thái giao dịch" />

                        </Columns>

                        <HeaderStyle CssClass="table-primary text-center" />

                    </asp:GridView>

                </div>

            </div>
        </div>

    </div>

</asp:Content>
