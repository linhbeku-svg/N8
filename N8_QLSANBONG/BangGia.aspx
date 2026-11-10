
<%@ Page Title="Quản lý bảng giá" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="BangGia.aspx.cs" Inherits="QuanLySanBong.BangGia" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="container-fluid">

        <!-- TIÊU ĐỀ -->
        <div class="mb-4">
            <h3 class="fw-bold text-primary">
                <i class="fa-solid fa-money-bill-wave me-2"></i>
                QUẢN LÝ BẢNG GIÁ
            </h3>
            <p class="text-secondary mb-0">
                Quản lý giá thuê sân bóng theo khung giờ và thời gian áp dụng.
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
                    <asp:Label ID="lblTieuDeForm" runat="server"
                        Text="THÊM BẢNG GIÁ" />
                </h5>
            </div>

            <div class="card-body">

                <asp:HiddenField ID="hfMaGia" runat="server" />

                <div class="row g-3">

                    <div class="col-md-6">
                        <label class="form-label fw-semibold">Sân bóng</label>

                        <asp:DropDownList ID="ddlMaSan" runat="server"
                            CssClass="form-select">
                        </asp:DropDownList>
                    </div>

                    <div class="col-md-6">
                        <label class="form-label fw-semibold">Khung giờ</label>

                        <asp:TextBox ID="txtKhungGio" runat="server"
                            CssClass="form-control"
                            MaxLength="50"
                            placeholder="Ví dụ: 06:00 - 08:00">
                        </asp:TextBox>
                    </div>

                    <div class="col-md-6">
                        <label class="form-label fw-semibold">Đơn giá (VNĐ)</label>

                        <asp:TextBox ID="txtDonGia" runat="server"
                            CssClass="form-control"
                            TextMode="Number"
                            placeholder="Nhập đơn giá">
                        </asp:TextBox>
                    </div>

                    <div class="col-md-3">
                        <label class="form-label fw-semibold">Ngày áp dụng</label>

                        <asp:TextBox ID="txtNgayApDung" runat="server"
                            CssClass="form-control"
                            TextMode="Date">
                        </asp:TextBox>
                    </div>

                    <div class="col-md-3">
                        <label class="form-label fw-semibold">
                            Ngày kết thúc
                        </label>

                        <asp:TextBox ID="txtNgayKetThuc" runat="server"
                            CssClass="form-control"
                            TextMode="Date">
                        </asp:TextBox>

                        <div class="form-text">
                            Có thể để trống nếu chưa xác định ngày kết thúc.
                        </div>
                    </div>

                </div>

                <div class="mt-4">

                    <asp:Button ID="btnLuu" runat="server"
                        Text="Lưu bảng giá"
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
                            Tìm kiếm theo tên sân
                        </label>

                        <asp:TextBox ID="txtTimKiem" runat="server"
                            CssClass="form-control"
                            placeholder="Nhập tên sân bóng cần tìm">
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

        <!-- DANH SÁCH BẢNG GIÁ -->
        <div class="card border-0 shadow-sm">

            <div class="card-header bg-white py-3">
                <h5 class="fw-bold mb-0">
                    <i class="fa-solid fa-list me-2 text-primary"></i>
                    DANH SÁCH BẢNG GIÁ
                </h5>
            </div>

            <div class="card-body">

                <div class="table-responsive">

                    <asp:GridView ID="gvBangGia" runat="server"
                        AutoGenerateColumns="False"
                        DataKeyNames="MaGia"
                        CssClass="table table-bordered table-hover table-striped align-middle"
                        GridLines="None"
                        EmptyDataText="Chưa có bảng giá nào."
                        OnRowCommand="gvBangGia_RowCommand">

                        <Columns>

                            <asp:BoundField DataField="MaGia"
                                HeaderText="Mã giá" />

                            <asp:BoundField DataField="TenSan"
                                HeaderText="Tên sân" />

                            <asp:BoundField DataField="KhungGio"
                                HeaderText="Khung giờ" />

                            <asp:BoundField DataField="DonGia"
                                HeaderText="Đơn giá"
                                DataFormatString="{0:N0} VNĐ" />

                            <asp:BoundField DataField="NgayApDung"
                                HeaderText="Ngày áp dụng"
                                DataFormatString="{0:dd/MM/yyyy}" />

                            <asp:BoundField DataField="NgayKetThuc"
                                HeaderText="Ngày kết thúc"
                                DataFormatString="{0:dd/MM/yyyy}"
                                NullDisplayText="Không giới hạn" />

                            <asp:TemplateField HeaderText="Thao tác">
                                <ItemTemplate>

                                    <asp:LinkButton ID="btnSua" runat="server"
                                        CommandName="SuaGia"
                                        CommandArgument='<%# Eval("MaGia") %>'
                                        CssClass="btn btn-warning btn-sm me-1"
                                        CausesValidation="false">
                                        <i class="fa-solid fa-pen-to-square"></i>
                                        Sửa
                                    </asp:LinkButton>

                                    <asp:LinkButton ID="btnXoa" runat="server"
                                        CommandName="XoaGia"
                                        CommandArgument='<%# Eval("MaGia") %>'
                                        CssClass="btn btn-danger btn-sm"
                                        CausesValidation="false"
                                        OnClientClick="return confirm('Bạn có chắc chắn muốn xóa bảng giá này không?');">
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
