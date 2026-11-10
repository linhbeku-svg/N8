
<%@ Page Language="C#" AutoEventWireup="true"
    CodeBehind="KiemTraKetNoi.aspx.cs"
    Inherits="QuanLySanBong.KiemTraKetNoi" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <meta charset="utf-8" />
    <title>Kiểm tra kết nối SQL Server</title>
    <style>
        body {
            font-family: Arial;
            background: #f1f5f9;
            padding: 50px;
        }

        .container {
            width: 450px;
            margin: auto;
            padding: 30px;
            background: white;
            border-radius: 10px;
            text-align: center;
        }

        .btn {
            padding: 12px 25px;
            background: #198754;
            color: white;
            border: none;
            border-radius: 5px;
            cursor: pointer;
        }

        .result {
            display: block;
            margin-top: 20px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <h2>KIỂM TRA KẾT NỐI DATABASE</h2>

            <asp:Button ID="btnKiemTra"
                runat="server"
                Text="Kiểm tra kết nối"
                CssClass="btn"
                OnClick="btnKiemTra_Click" />

            <asp:Label ID="lblKetQua"
                runat="server"
                CssClass="result" />
        </div>
    </form>
</body>
</html>
