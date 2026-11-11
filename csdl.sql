
-- =========================================
-- 1. TAO DATABASE
-- =========================================
CREATE DATABASE QuanLySanBong;
GO

USE QuanLySanBong;
GO

-- =========================================
-- 2. BANG TAI KHOAN
-- =========================================
CREATE TABLE TAIKHOAN (
    MaTK INT IDENTITY(1,1) PRIMARY KEY,
    TenDangNhap VARCHAR(50) NOT NULL UNIQUE,
    MatKhau VARCHAR(255) NOT NULL,
    HoTen NVARCHAR(100) NOT NULL,
    SoDienThoai VARCHAR(15),
    Email VARCHAR(100),
    Quyen VARCHAR(20) NOT NULL
        CHECK (Quyen IN ('Admin','QuanLy','NhanVien')),
    TrangThai TINYINT DEFAULT 1
);
GO

-- =========================================
-- 3. BANG KHACH HANG
-- =========================================
CREATE TABLE KHACHHANG (
    MaKH INT IDENTITY(1,1) PRIMARY KEY,
    HoTen NVARCHAR(100) NOT NULL,
    SoDienThoai VARCHAR(15) NOT NULL,
    Email VARCHAR(100),
    DiaChi NVARCHAR(255),
    NgayTao DATETIME DEFAULT GETDATE()
);
GO

-- =========================================
-- 4. BANG SAN BONG
-- =========================================
CREATE TABLE SANBONG (
    MaSan INT IDENTITY(1,1) PRIMARY KEY,
    TenSan NVARCHAR(100) NOT NULL,
    LoaiSan NVARCHAR(30) NOT NULL,
    TrangThai NVARCHAR(30) DEFAULT N'Trong'
        CHECK (TrangThai IN (
            N'Trong',
            N'DangSuDung',
            N'BaoTri',
            N'NgungHoatDong'
        )),
    MoTa NVARCHAR(255)
);
GO

-- =========================================
-- 5. BANG GIA
-- =========================================
CREATE TABLE BANGGIA (
    MaGia INT IDENTITY(1,1) PRIMARY KEY,
    MaSan INT NOT NULL,
    KhungGio VARCHAR(50) NOT NULL,
    DonGia DECIMAL(12,2) NOT NULL,
    NgayApDung DATE NOT NULL,
    NgayKetThuc DATE,
    FOREIGN KEY (MaSan) REFERENCES SANBONG(MaSan)
);
GO

-- =========================================
-- 6. BANG DAT SAN
-- =========================================
CREATE TABLE DATSAN (
    MaDatSan INT IDENTITY(1,1) PRIMARY KEY,
    MaKH INT NOT NULL,
    MaSan INT NOT NULL,
    MaTK INT NOT NULL,
    NgayDat DATETIME DEFAULT GETDATE(),
    NgayThue DATE NOT NULL,
    GioBatDau TIME NOT NULL,
    GioKetThuc TIME NOT NULL,
    TienCoc DECIMAL(12,2) DEFAULT 0,
    TrangThai NVARCHAR(30) DEFAULT N'ChoXacNhan'
        CHECK (TrangThai IN (
            N'ChoXacNhan',
            N'DaXacNhan',
            N'DangSuDung',
            N'HoanThanh',
            N'DaHuy'
        )),

    FOREIGN KEY (MaKH) REFERENCES KHACHHANG(MaKH),
    FOREIGN KEY (MaSan) REFERENCES SANBONG(MaSan),
    FOREIGN KEY (MaTK) REFERENCES TAIKHOAN(MaTK),

    CHECK (GioKetThuc > GioBatDau)
);
GO

-- =========================================
-- 7. BANG DICH VU
-- =========================================
CREATE TABLE DICHVU (
    MaDV INT IDENTITY(1,1) PRIMARY KEY,
    TenDV NVARCHAR(100) NOT NULL,
    DonGia DECIMAL(12,2) NOT NULL,
    DonViTinh NVARCHAR(30),
    TrangThai TINYINT DEFAULT 1
);
GO

-- =========================================
-- 8. BANG HOA DON
-- =========================================
CREATE TABLE HOADON (
    MaHD INT IDENTITY(1,1) PRIMARY KEY,
    MaDatSan INT NOT NULL UNIQUE,
    MaTK INT NOT NULL,
    NgayLap DATETIME DEFAULT GETDATE(),
    TienSan DECIMAL(12,2) DEFAULT 0,
    TienDichVu DECIMAL(12,2) DEFAULT 0,
    GiamGia DECIMAL(12,2) DEFAULT 0,
    TongTien DECIMAL(12,2) DEFAULT 0,
    TrangThai NVARCHAR(30) DEFAULT N'ChuaThanhToan'
        CHECK (TrangThai IN (
            N'ChuaThanhToan',
            N'ThanhToanMotPhan',
            N'DaThanhToan'
        )),

    FOREIGN KEY (MaDatSan) REFERENCES DATSAN(MaDatSan),
    FOREIGN KEY (MaTK) REFERENCES TAIKHOAN(MaTK)
);
GO

-- =========================================
-- 9. BANG CHI TIET HOA DON
-- =========================================
CREATE TABLE CHITIETHOADON (
    MaHD INT NOT NULL,
    MaDV INT NOT NULL,
    SoLuong INT NOT NULL,
    DonGia DECIMAL(12,2) NOT NULL,
    ThanhTien DECIMAL(12,2) NOT NULL,

    PRIMARY KEY (MaHD, MaDV),
    FOREIGN KEY (MaHD) REFERENCES HOADON(MaHD),
    FOREIGN KEY (MaDV) REFERENCES DICHVU(MaDV),

    CHECK (SoLuong > 0)
);
GO

-- =========================================
-- 10. BANG THANH TOAN
-- =========================================
CREATE TABLE THANHTOAN (
    MaTT INT IDENTITY(1,1) PRIMARY KEY,
    MaHD INT NOT NULL,
    NgayThanhToan DATETIME DEFAULT GETDATE(),
    SoTien DECIMAL(12,2) NOT NULL,
    PhuongThuc NVARCHAR(30) NOT NULL
        CHECK (PhuongThuc IN (
            N'TienMat',
            N'ChuyenKhoan',
            N'ViDienTu'
        )),
    MaGiaoDich VARCHAR(100),
    TrangThai NVARCHAR(30) DEFAULT N'ThanhCong'
        CHECK (TrangThai IN (
            N'ThanhCong',
            N'ThatBai',
            N'ChoXuLy'
        )),

    FOREIGN KEY (MaHD) REFERENCES HOADON(MaHD)
);
GO

-- =========================================
-- 11. DU LIEU MAU TAI KHOAN
-- =========================================
INSERT INTO TAIKHOAN
(TenDangNhap, MatKhau, HoTen, SoDienThoai, Email, Quyen)
VALUES
('admin', 'admin123', N'Quan tri vien', '0900000001',
 'admin@gmail.com', 'Admin'),
('quanly', '123456', N'Nguyen Van Quan', '0900000002',
 'quanly@gmail.com', 'QuanLy'),
('nhanvien1', '123456', N'Tran Van Nam', '0900000003',
 'nhanvien@gmail.com', 'NhanVien');
GO

-- =========================================
-- 12. DU LIEU MAU KHACH HANG
-- =========================================
INSERT INTO KHACHHANG
(HoTen, SoDienThoai, Email, DiaChi)
VALUES
(N'Nguyen Van An', '0911111111', 'an@gmail.com', N'Long Xuyen'),
(N'Tran Van Binh', '0922222222', 'binh@gmail.com', N'Chau Doc'),
(N'Le Van Cuong', '0933333333', 'cuong@gmail.com', N'An Giang');
GO

-- =========================================
-- 13. DU LIEU MAU SAN BONG
-- =========================================
INSERT INTO SANBONG (TenSan, LoaiSan, TrangThai, MoTa)
VALUES
(N'San so 1', N'San 5', N'Trong', N'San co nhan tao'),
(N'San so 2', N'San 7', N'Trong', N'San co nhan tao'),
(N'San so 3', N'San 5', N'Trong', N'San co mai che'),
(N'San so 4', N'San 11', N'BaoTri', N'San dang bao tri');
GO

-- =========================================
-- 14. DU LIEU MAU BANG GIA
-- =========================================
INSERT INTO BANGGIA
(MaSan, KhungGio, DonGia, NgayApDung)
VALUES
(1, '06:00-17:00', 200000, '20260101'),
(1, '17:00-22:00', 300000, '20260101'),
(2, '06:00-17:00', 400000, '20260101'),
(2, '17:00-22:00', 500000, '20260101'),
(3, '06:00-22:00', 250000, '20260101');
GO

-- =========================================
-- 15. DU LIEU MAU DICH VU
-- =========================================
INSERT INTO DICHVU (TenDV, DonGia, DonViTinh)
VALUES
(N'Nuoc suoi', 10000, N'Chai'),
(N'Nuoc ngot', 15000, N'Lon'),
(N'Thue ao bib', 50000, N'Bo'),
(N'Thue bong', 30000, N'Qua');
GO

-- =========================================
-- 16. DU LIEU MAU DAT SAN
-- =========================================
INSERT INTO DATSAN
(MaKH, MaSan, MaTK, NgayThue, GioBatDau,
 GioKetThuc, TienCoc, TrangThai)
VALUES
(1, 1, 3, '20261110', '18:00', '19:30',
 100000, N'DaXacNhan'),
(2, 2, 3, '20261111', '19:00', '20:00',
 200000, N'ChoXacNhan');
GO

-- =========================================
-- 17. DU LIEU MAU HOA DON
-- =========================================
INSERT INTO HOADON
(MaDatSan, MaTK, TienSan, TienDichVu,
 GiamGia, TongTien, TrangThai)
VALUES
(1, 3, 450000, 25000, 0, 475000, N'ThanhToanMotPhan');
GO

-- =========================================
-- 18. DU LIEU MAU CHI TIET HOA DON
-- =========================================
INSERT INTO CHITIETHOADON
(MaHD, MaDV, SoLuong, DonGia, ThanhTien)
VALUES
(1, 1, 1, 10000, 10000),
(1, 2, 1, 15000, 15000);
GO

-- =========================================
-- 19. DU LIEU MAU THANH TOAN
-- =========================================
INSERT INTO THANHTOAN
(MaHD, SoTien, PhuongThuc, TrangThai)
VALUES
(1, 100000, N'TienMat', N'ThanhCong');
GO

-- =========================================
-- 20. VIEW THONG KE DOANH THU THEO NGAY
-- =========================================
CREATE VIEW V_THONGKEDOANHTHU AS
SELECT
    CAST(NgayThanhToan AS DATE) AS Ngay,
    SUM(SoTien) AS DoanhThu
FROM THANHTOAN
WHERE TrangThai = N'ThanhCong'
GROUP BY CAST(NgayThanhToan AS DATE);
GO

-- =========================================
-- 21. KIEM TRA DU LIEU
-- =========================================
SELECT * FROM TAIKHOAN;
SELECT * FROM KHACHHANG;
SELECT * FROM SANBONG;
SELECT * FROM BANGGIA;
SELECT * FROM DATSAN;
SELECT * FROM DICHVU;
SELECT * FROM HOADON;
SELECT * FROM CHITIETHOADON;
SELECT * FROM THANHTOAN;

SELECT * FROM V_THONGKEDOANHTHU;
GO
