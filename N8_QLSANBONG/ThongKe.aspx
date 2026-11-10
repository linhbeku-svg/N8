
<%@ Page Title="Thống kê báo cáo" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ThongKe.aspx.cs" Inherits="QuanLySanBong.ThongKe" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="container-fluid">

        <!-- TIÊU ĐỀ -->
        <div class="mb-4">
            <h3 class="fw-bold text-primary">
                <i class="fa-solid fa-chart-column me-2"></i>
                THỐNG KÊ VÀ BÁO CÁO
            </h3>
            <p class="text-secondary mb-0">
                Theo dõi tình hình hoạt động và doanh thu sân bóng.
            </p>
        </div>

        <!-- THÔNG BÁO -->
        <asp:Panel ID="pnlThongBao" runat="server"
            Visible="false" CssClass="alert" role="alert">
            <asp:Label ID="lblThongBao" runat="server" />
        </asp:Panel>

        <!-- BỘ LỌC THỜI GIAN -->
        <div class="card border-0 shadow-sm mb-4">
            <div class="card-body">

                <h5 class="fw-bold mb-3">
                    <i class="fa-solid fa-calendar-days me-2 text-primary"></i>
                    LỌC THỜI GIAN THỐNG KÊ
                </h5>

                <div class="row g-3 align-items-end">

                    <div class="col-md-5">
                        <label class="form-label fw-semibold">Từ ngày</label>
                        <asp:TextBox ID="txtTuNgay" runat="server"
                            TextMode="Date"
                            CssClass="form-control">
                        </asp:TextBox>
                    </div>

                    <div class="col-md-5">
                        <label class="form-label fw-semibold">Đến ngày</label>
                        <asp:TextBox ID="txtDenNgay" runat="server"
                            TextMode="Date"
                            CssClass="form-control">
                        </asp:TextBox>
                    </div>

                    <div class="col-md-2">
                        <asp:Button ID="btnThongKe" runat="server"
                            Text="Thống kê"
                            CssClass="btn btn-primary w-100"
                            OnClick="btnThongKe_Click" />
                    </div>

                </div>

                <div class="mt-3">
                    <asp:Button ID="btnHomNay" runat="server"
                        Text="Hôm nay"
                        CssClass="btn btn-outline-primary btn-sm me-2"
                        OnClick="btnHomNay_Click" />

                    <asp:Button ID="btnThangNay" runat="server"
                        Text="Tháng này"
                        CssClass="btn btn-outline-primary btn-sm me-2"
                        OnClick="btnThangNay_Click" />

                    <asp:Button ID="btnLamMoi" runat="server"
                        Text="Làm mới"
                        CssClass="btn btn-outline-secondary btn-sm"
                        OnClick="btnLamMoi_Click" />
                </div>

            </div>
        </div>

        <!-- CÁC THẺ THỐNG KÊ -->
        <div class="row g-3 mb-4">

            <div class="col-12 col-md-6 col-xl-3">
                <div class="card border-0 shadow-sm h-100">
                    <div class="card-body">
                        <div class="text-secondary mb-2">
                            <i class="fa-solid fa-futbol me-2"></i>
                            Tổng số sân bóng
                        </div>
                        <h3 class="fw-bold text-primary mb-0">
                            <asp:Label ID="lblTongSan" runat="server" Text="0" />
                        </h3>
                    </div>
                </div>
            </div>

            <div class="col-12 col-md-6 col-xl-3">
                <div class="card border-0 shadow-sm h-100">
                    <div class="card-body">
                        <div class="text-secondary mb-2">
                            <i class="fa-solid fa-users me-2"></i>
                            Tổng khách hàng
                        </div>
                        <h3 class="fw-bold text-success mb-0">
                            <asp:Label ID="lblTongKhachHang" runat="server" Text="0" />
                        </h3>
                    </div>
                </div>
            </div>

            <div class="col-12 col-md-6 col-xl-3">
                <div class="card border-0 shadow-sm h-100">
                    <div class="card-body">
                        <div class="text-secondary mb-2">
                            <i class="fa-solid fa-calendar-check me-2"></i>
                            Số lượt đặt sân
                        </div>
                        <h3 class="fw-bold text-warning mb-0">
                            <asp:Label ID="lblTongDatSan" runat="server" Text="0" />
                        </h3>
                    </div>
                </div>
            </div>

            <div class="col-12 col-md-6 col-xl-3">
                <div class="card border-0 shadow-sm h-100">
                    <div class="card-body">
                        <div class="text-secondary mb-2">
                            <i class="fa-solid fa-money-bill-wave me-2"></i>
                            Tổng doanh thu
                        </div>
                        <h3 class="fw-bold text-danger mb-0">
                            <asp:Label ID="lblDoanhThu" runat="server" Text="0 VNĐ" />
                        </h3>
                    </div>
                </div>
            </div>

        </div>

        <!-- DOANH THU THEO NGÀY -->
        <div class="card border-0 shadow-sm mb-4">

            <div class="card-header bg-white py-3">
                <h5 class="fw-bold mb-0">
                    <i class="fa-solid fa-chart-line me-2 text-primary"></i>
                    DOANH THU THEO NGÀY
                </h5>
            </div>

            <div class="card-body">

                <div class="table-responsive">

                    <asp:GridView ID="gvDoanhThu" runat="server"
                        AutoGenerateColumns="False"
                        CssClass="table table-bordered table-hover table-striped align-middle"
                        GridLines="None"
                        EmptyDataText="Không có dữ liệu doanh thu trong khoảng thời gian này.">

                        <Columns>

                            <asp:BoundField DataField="Ngay"
                                HeaderText="Ngày"
                                DataFormatString="{0:dd/MM/yyyy}" />

                            <asp:BoundField DataField="SoGiaoDich"
                                HeaderText="Số giao dịch" />

                            <asp:BoundField DataField="DoanhThu"
                                HeaderText="Doanh thu"
                                DataFormatString="{0:N0} VNĐ" />

                        </Columns>

                        <HeaderStyle CssClass="table-primary text-center" />

                    </asp:GridView>

                </div>

            </div>
        </div>

        <!-- THỐNG KÊ ĐẶT SÂN -->
        <div class="card border-0 shadow-sm mb-4">

            <div class="card-header bg-white py-3">
                <h5 class="fw-bold mb-0">
                    <i class="fa-solid fa-chart-pie me-2 text-primary"></i>
                    THỐNG KÊ TRẠNG THÁI ĐẶT SÂN
                </h5>
            </div>

            <div class="card-body">

                <div class="table-responsive">

                    <asp:GridView ID="gvTrangThaiDatSan" runat="server"
                        AutoGenerateColumns="False"
                        CssClass="table table-bordered table-hover table-striped align-middle"
                        GridLines="None"
                        EmptyDataText="Không có dữ liệu đặt sân trong khoảng thời gian này.">

                        <Columns>

                            <asp:BoundField DataField="TrangThai"
                                HeaderText="Trạng thái" />

                            <asp:BoundField DataField="SoLuong"
                                HeaderText="Số lượng phiếu đặt" />

                        </Columns>

                        <HeaderStyle CssClass="table-primary text-center" />

                    </asp:GridView>

                </div>

            </div>
        </div>

    </div>

</asp:Content>
