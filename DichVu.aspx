
<%@ Page Title="Quản lý dịch vụ" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DichVu.aspx.cs" Inherits="QuanLySanBong.DichVu" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="container-fluid">

        <!-- TIÊU ĐỀ -->
        <div class="mb-4">
            <h3 class="fw-bold text-primary">
                <i class="fa-solid fa-bell-concierge me-2"></i>
                QUẢN LÝ DỊCH VỤ
            </h3>
            <p class="text-secondary mb-0">
                Quản lý các dịch vụ cung cấp cho khách hàng tại sân bóng.
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
                        Text="THÊM DỊCH VỤ" />
                </h5>
            </div>

            <div class="card-body">

                <asp:HiddenField ID="hfMaDV" runat="server" />

                <div class="row g-3">

                    <div class="col-md-6">
                        <label class="form-label fw-semibold">Tên dịch vụ</label>

                        <asp:TextBox ID="txtTenDV" runat="server"
                            CssClass="form-control"
                            MaxLength="100"
                            placeholder="Ví dụ: Nước suối">
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

                    <div class="col-md-6">
                        <label class="form-label fw-semibold">Đơn vị tính</label>

                        <asp:DropDownList ID="ddlDonViTinh" runat="server"
                            CssClass="form-select">
                            <asp:ListItem Value="Chai">Chai</asp:ListItem>
                            <asp:ListItem Value="Lon">Lon</asp:ListItem>
                            <asp:ListItem Value="Cái">Cái</asp:ListItem>
                            <asp:ListItem Value="Bộ">Bộ</asp:ListItem>
                            <asp:ListItem Value="Lượt">Lượt</asp:ListItem>
                            <asp:ListItem Value="Phần">Phần</asp:ListItem>
                        </asp:DropDownList>
                    </div>

                    <div class="col-md-6">
                        <label class="form-label fw-semibold">Trạng thái</label>

                        <asp:DropDownList ID="ddlTrangThai" runat="server"
                            CssClass="form-select">
                            <asp:ListItem Value="1">Đang kinh doanh</asp:ListItem>
                            <asp:ListItem Value="0">Ngừng kinh doanh</asp:ListItem>
                        </asp:DropDownList>
                    </div>

                </div>

                <div class="mt-4">

                    <asp:Button ID="btnLuu" runat="server"
                        Text="Lưu dịch vụ"
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
                            Tìm kiếm dịch vụ
                        </label>

                        <asp:TextBox ID="txtTimKiem" runat="server"
                            CssClass="form-control"
                            placeholder="Nhập tên dịch vụ cần tìm">
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

        <!-- DANH SÁCH DỊCH VỤ -->
        <div class="card border-0 shadow-sm">

            <div class="card-header bg-white py-3">
                <h5 class="fw-bold mb-0">
                    <i class="fa-solid fa-list me-2 text-primary"></i>
                    DANH SÁCH DỊCH VỤ
                </h5>
            </div>

            <div class="card-body">

                <div class="table-responsive">

                    <asp:GridView ID="gvDichVu" runat="server"
                        AutoGenerateColumns="False"
                        DataKeyNames="MaDV"
                        CssClass="table table-bordered table-hover table-striped align-middle"
                        GridLines="None"
                        EmptyDataText="Chưa có dịch vụ nào."
                        OnRowCommand="gvDichVu_RowCommand">

                        <Columns>

                            <asp:BoundField DataField="MaDV"
                                HeaderText="Mã dịch vụ" />

                            <asp:BoundField DataField="TenDV"
                                HeaderText="Tên dịch vụ" />

                            <asp:BoundField DataField="DonGia"
                                HeaderText="Đơn giá"
                                DataFormatString="{0:N0} VNĐ" />

                            <asp:BoundField DataField="DonViTinh"
                                HeaderText="Đơn vị tính" />

                            <asp:TemplateField HeaderText="Trạng thái">
                                <ItemTemplate>

                                    <asp:Label ID="lblTrangThai" runat="server"
                                        Text='<%# Convert.ToInt32(Eval("TrangThai")) == 1 ? "Đang kinh doanh" : "Ngừng kinh doanh" %>'
                                        CssClass='<%# Convert.ToInt32(Eval("TrangThai")) == 1 ? "badge bg-success" : "badge bg-danger" %>' />

                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Thao tác">
                                <ItemTemplate>

                                    <asp:LinkButton ID="btnSua" runat="server"
                                        CommandName="SuaDV"
                                        CommandArgument='<%# Eval("MaDV") %>'
                                        CssClass="btn btn-warning btn-sm me-1"
                                        CausesValidation="false">
                                        <i class="fa-solid fa-pen-to-square"></i>
                                        Sửa
                                    </asp:LinkButton>

                                    <asp:LinkButton ID="btnXoa" runat="server"
                                        CommandName="XoaDV"
                                        CommandArgument='<%# Eval("MaDV") %>'
                                        CssClass="btn btn-danger btn-sm"
                                        CausesValidation="false"
                                        OnClientClick="return confirm('Bạn có chắc chắn muốn xóa dịch vụ này không?');">
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
