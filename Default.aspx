
<%@ Page Title="Trang chủ" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="QuanLySanBong._Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <!-- TIÊU ĐỀ -->
    <div class="container-fluid">

        <div class="d-flex justify-content-between align-items-center mb-4">
            <div>
                <h3 class="fw-bold text-primary">
                    <i class="fa-solid fa-gauge-high me-2"></i>
                    TỔNG QUAN HỆ THỐNG
                </h3>

                <p class="text-secondary mb-0">
                    Chào mừng bạn đến với hệ thống quản lý sân bóng.
                </p>
            </div>

            <span class="badge bg-primary fs-6">
                Trang chủ
            </span>
        </div>

        <!-- CÁC THẺ THỐNG KÊ -->
        <div class="row g-3 mb-4">

            <!-- TỔNG SÂN -->
            <div class="col-12 col-sm-6 col-xl-3">

                <div class="card border-0 shadow-sm h-100">
                    <div class="card-body">

                        <div class="d-flex justify-content-between align-items-center">

                            <div>
                                <p class="text-secondary mb-2">
                                    Tổng số sân bóng
                                </p>

                                <h2 class="fw-bold text-primary mb-0">
                                    <asp:Label ID="lblTongSan" runat="server"
                                        Text="0" />
                                </h2>
                            </div>

                            <div class="text-primary">
                                <i class="fa-solid fa-futbol fa-3x"></i>
                            </div>

                        </div>

                    </div>
                </div>

            </div>

            <!-- KHÁCH HÀNG -->
            <div class="col-12 col-sm-6 col-xl-3">

                <div class="card border-0 shadow-sm h-100">
                    <div class="card-body">

                        <div class="d-flex justify-content-between align-items-center">

                            <div>
                                <p class="text-secondary mb-2">
                                    Tổng khách hàng
                                </p>

                                <h2 class="fw-bold text-success mb-0">
                                    <asp:Label ID="lblTongKhachHang" runat="server"
                                        Text="0" />
                                </h2>
                            </div>

                            <div class="text-success">
                                <i class="fa-solid fa-users fa-3x"></i>
                            </div>

                        </div>

                    </div>
                </div>

            </div>

            <!-- ĐẶT SÂN -->
            <div class="col-12 col-sm-6 col-xl-3">

                <div class="card border-0 shadow-sm h-100">
                    <div class="card-body">

                        <div class="d-flex justify-content-between align-items-center">

                            <div>
                                <p class="text-secondary mb-2">
                                    Tổng phiếu đặt sân
                                </p>

                                <h2 class="fw-bold text-warning mb-0">
                                    <asp:Label ID="lblTongDatSan" runat="server"
                                        Text="0" />
                                </h2>
                            </div>

                            <div class="text-warning">
                                <i class="fa-solid fa-calendar-check fa-3x"></i>
                            </div>

                        </div>

                    </div>
                </div>

            </div>

            <!-- DOANH THU -->
            <div class="col-12 col-sm-6 col-xl-3">

                <div class="card border-0 shadow-sm h-100">
                    <div class="card-body">

                        <div class="d-flex justify-content-between align-items-center">

                            <div>
                                <p class="text-secondary mb-2">
                                    Tổng doanh thu
                                </p>

                                <h4 class="fw-bold text-danger mb-0">
                                    <asp:Label ID="lblDoanhThu" runat="server"
                                        Text="0 VNĐ" />
                                </h4>
                            </div>

                            <div class="text-danger">
                                <i class="fa-solid fa-money-bill-wave fa-3x"></i>
                            </div>

                        </div>

                    </div>
                </div>

            </div>

        </div>

        <!-- TRUY CẬP NHANH -->
        <div class="card border-0 shadow-sm mb-4">

            <div class="card-header bg-white py-3">
                <h5 class="fw-bold mb-0">
                    <i class="fa-solid fa-bolt text-warning me-2"></i>
                    TRUY CẬP NHANH
                </h5>
            </div>

            <div class="card-body">

                <div class="row g-3">

                    <div class="col-6 col-md-3">
                        <a href="SanBong.aspx"
                           class="btn btn-outline-primary w-100 py-3">
                            <i class="fa-solid fa-futbol fa-2x d-block mb-2"></i>
                            Quản lý sân bóng
                        </a>
                    </div>

                    <div class="col-6 col-md-3">
                        <a href="DatSan.aspx"
                           class="btn btn-outline-success w-100 py-3">
                            <i class="fa-solid fa-calendar-check fa-2x d-block mb-2"></i>
                            Quản lý đặt sân
                        </a>
                    </div>

                    <div class="col-6 col-md-3">
                        <a href="KhachHang.aspx"
                           class="btn btn-outline-info w-100 py-3">
                            <i class="fa-solid fa-users fa-2x d-block mb-2"></i>
                            Khách hàng
                        </a>
                    </div>

                    <div class="col-6 col-md-3">
                        <a href="BangGia.aspx"
                           class="btn btn-outline-warning w-100 py-3">
                            <i class="fa-solid fa-tags fa-2x d-block mb-2"></i>
                            Quản lý bảng giá
                        </a>
                    </div>

                    <div class="col-6 col-md-3">
                        <a href="DichVu.aspx"
                           class="btn btn-outline-secondary w-100 py-3">
                            <i class="fa-solid fa-bell-concierge fa-2x d-block mb-2"></i>
                            Quản lý dịch vụ
                        </a>
                    </div>

                    <div class="col-6 col-md-3">
                        <a href="HoaDon.aspx"
                           class="btn btn-outline-danger w-100 py-3">
                            <i class="fa-solid fa-file-invoice fa-2x d-block mb-2"></i>
                            Quản lý hóa đơn
                        </a>
                    </div>

                    <div class="col-6 col-md-3">
                        <a href="ThanhToan.aspx"
                           class="btn btn-outline-dark w-100 py-3">
                            <i class="fa-solid fa-credit-card fa-2x d-block mb-2"></i>
                            Thanh toán
                        </a>
                    </div>

                    <div class="col-6 col-md-3">
                        <a href="ThongKe.aspx"
                           class="btn btn-outline-primary w-100 py-3">
                            <i class="fa-solid fa-chart-column fa-2x d-block mb-2"></i>
                            Xem thống kê
                        </a>
                    </div>

                </div>

            </div>

        </div>

        <!-- DANH SÁCH ĐẶT SÂN MỚI NHẤT -->
        <div class="card border-0 shadow-sm">

            <div class="card-header bg-white py-3">

                <div class="d-flex justify-content-between align-items-center">

                    <h5 class="fw-bold mb-0">
                        <i class="fa-solid fa-clock text-primary me-2"></i>
                        PHIẾU ĐẶT SÂN MỚI NHẤT
                    </h5>

                    <a href="DatSan.aspx" class="btn btn-primary btn-sm">
                        Xem tất cả
                        <i class="fa-solid fa-arrow-right ms-1"></i>
                    </a>

                </div>

            </div>

            <div class="card-body">

                <div class="table-responsive">

                    <asp:GridView ID="gvDatSanMoi" runat="server"
                        AutoGenerateColumns="False"
                        CssClass="table table-bordered table-hover table-striped align-middle"
                        GridLines="None"
                        EmptyDataText="Chưa có phiếu đặt sân."
                        ShowHeaderWhenEmpty="true">

                        <Columns>

                            <asp:BoundField DataField="MaDatSan"
                                HeaderText="Mã đặt sân" />

                            <asp:BoundField DataField="HoTen"
                                HeaderText="Khách hàng" />

                            <asp:BoundField DataField="TenSan"
                                HeaderText="Sân bóng" />

                            <asp:BoundField DataField="NgayThue"
                                HeaderText="Ngày thuê"
                                DataFormatString="{0:dd/MM/yyyy}" />

                            <asp:BoundField DataField="GioBatDau"
                                HeaderText="Giờ bắt đầu" />

                            <asp:BoundField DataField="GioKetThuc"
                                HeaderText="Giờ kết thúc" />

                            <asp:BoundField DataField="TrangThai"
                                HeaderText="Trạng thái" />

                        </Columns>

                        <HeaderStyle CssClass="table-primary text-center" />

                    </asp:GridView>

                </div>

            </div>

        </div>

    </div>

</asp:Content>
