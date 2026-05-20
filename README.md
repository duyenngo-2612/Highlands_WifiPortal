# Highlands_WifiPortal
# Highlands Wifi Portal

## Giới thiệu

Highlands Wifi Portal là hệ thống quản lý và xác thực truy cập WiFi dành cho cửa hàng/café theo mô hình captive portal. Dự án hỗ trợ người dùng đăng nhập để truy cập Internet, quản lý OTP xác thực, và cung cấp các chức năng quản trị hệ thống.

## Chức năng chính

### Người dùng

* Đăng nhập truy cập WiFi
* Xác thực bằng OTP
* Kiểm tra trạng thái đăng nhập
* Đăng xuất khỏi hệ thống

### Quản trị viên

* Quản lý người dùng
* Quản lý phiên đăng nhập WiFi
* Theo dõi lịch sử truy cập
* Quản lý OTP và xác thực
* Quản lý API hệ thống

## Công nghệ sử dụng

### Backend

* ASP.NET Core
* Entity Framework Core
* SQL Server

### Công cụ phát triển

* Visual Studio 2022
* Git & GitHub


## Yêu cầu hệ thống

* .NET 8 SDK hoặc mới hơn
* SQL Server
* Visual Studio 2022
* Git

## Hướng dẫn cài đặt

### 1. Clone project

```bash
git clone https://github.com/duyenngo-2612/Highlands_WifiPortal.git
```

### 2. Mở project

Mở file solution bằng Visual Studio.

### 3. Cấu hình database

Chỉnh sửa chuỗi kết nối trong file:

```bash
appsettings.json
```

Ví dụ:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=.;Database=HighlandsWifiPortal;Trusted_Connection=True;TrustServerCertificate=True"
}
```

### 4. Chạy migration

Mở Package Manager Console:

```bash
Update-Database
```

### 5. Chạy project

```bash
Ctrl + F5
```

Hoặc:

```bash
dotnet run
```

## Kiểm thử hệ thống

Sử dụng Postman để kiểm thử API.

Các bước:

1. Đăng nhập để lấy JWT Token
2. Gắn token vào Authorization Header
3. Gửi request tới API

## Quy trình Git

### Tạo nhánh mới

```bash
git checkout -b Dev
```

### Commit code

```bash
git add .
git commit -m "your message"
```

### Push code

```bash
git push origin Dev
```

### Merge vào master/main

```bash
git checkout master
git merge Dev
git push origin master
```

## Thành viên thực hiện

* Ngô Thị Thu Duyên
* Tăng Khánh Nhi
* Lê Thị Hồng Nhã
* Lê Nguyễn Khánh Trình


Dự án được sử dụng cho mục đích học tập và nghiên cứu.
