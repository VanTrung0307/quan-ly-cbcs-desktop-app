<div align="center">

# QUẢN LÝ CÁN BỘ, CHIẾN SĨ (CBCS)

**Ứng dụng desktop hỗ trợ quản lý hồ sơ CBCS và hoạt động công tác**

Ứng dụng Windows Forms phát triển bằng C#, sử dụng Microsoft SQL Server để lưu trữ dữ liệu.

</div>

---

## 1. Giới thiệu

**Quản lý CBCS** là ứng dụng desktop hỗ trợ quản lý tập trung thông tin cán bộ, chiến sĩ và các hoạt động công tác. Ứng dụng tổ chức các nghiệp vụ theo từng màn hình, đồng thời cung cấp các chức năng báo cáo, quản trị tài khoản, phân quyền, sao lưu/phục hồi dữ liệu và cập nhật phần mềm.

## 2. Các nhóm chức năng

### 2.1. Đăng nhập và trang chủ
- Đăng nhập bằng tài khoản được cấp.
- Hiển thị trang chủ và các thông tin tổng quan theo quyền truy cập.

### 2.2. Quản lý cán bộ, chiến sĩ
- Xem và tra cứu danh sách CBCS.
- Thêm, chỉnh sửa và quản lý thông tin hồ sơ CBCS.
- Xem thông tin chi tiết của từng CBCS.

### 2.3. Quản lý công tác
- Quản lý danh sách công tác.
- Xem và chỉnh sửa thông tin chi tiết công tác.
- Phân công CBCS tham gia công tác.
- Theo dõi lịch sử tham gia công tác.

### 2.4. Thống kê và báo cáo
- Dashboard công tác và bảng điều khiển quản trị.
- Thống kê công tác CBCS.
- Báo cáo quân số.
- Xem trước báo cáo trước khi thực hiện thao tác tiếp theo.

### 2.5. Nhật ký hệ thống
- Theo dõi lịch sử thao tác được ghi nhận trong hệ thống.

### 2.6. Quản lý tài khoản và bảo mật
- Xem danh sách tài khoản.
- Thêm hoặc chỉnh sửa thông tin tài khoản.
- Đổi mật khẩu.
- Thiết lập quyền truy cập theo tài khoản.

### 2.7. Sao lưu và phục hồi dữ liệu
- Chọn thư mục lưu trữ và tạo bản sao lưu cơ sở dữ liệu.
- Xem danh sách bản sao lưu, thời gian, dung lượng và đường dẫn.
- Chọn file sao lưu để phục hồi.
- Theo dõi lịch sử thao tác sao lưu, phục hồi và xóa bản sao lưu.

**Lưu ý:** Phục hồi dữ liệu có thể ghi đè cơ sở dữ liệu hiện tại. Cần xác minh đúng file sao lưu và sao lưu trạng thái hiện tại trước khi phục hồi.

### 2.8. Cập nhật ứng dụng
- Kiểm tra phiên bản hiện tại và phiên bản mới.
- Xem nội dung thay đổi của bản cập nhật.
- Thiết lập các tùy chọn kiểm tra/thông báo cập nhật.
- Tải gói cập nhật và hiển thị tiến trình tải, xác minh.

### 2.9. Chức năng đang phát triển
- **Nghỉ phép:** tab hiện hiển thị thông báo chức năng sẽ được cập nhật sau. Nghiệp vụ quản lý nghỉ phép chưa được xem là chức năng hoàn thiện của phiên bản hiện tại.

## 3. Hình ảnh giao diện

Các ảnh giao diện đầy đủ được lưu trong thư mục [`assets/`](assets/). Ảnh được sắp xếp theo nhóm nghiệp vụ; tên file được đánh số để dễ đối chiếu.

### 3.1. Đăng nhập và trang chủ

<p align="center">
  <img src="assets/01-login.jpg" width="900" alt="Màn hình đăng nhập"/>
</p>

<p align="center">
  <img src="assets/02-home-dashboard.jpg" width="900" alt="Trang chủ và dashboard tổng quan"/>
</p>

### 3.2. Dashboard công tác và bảng điều khiển quản trị

<p align="center">
  <img src="assets/03-mission-dashboard.jpg" width="900" alt="Dashboard công tác"/>
</p>

<p align="center">
  <img src="assets/04-admin-dashboard.jpg" width="900" alt="Bảng điều khiển quản trị"/>
</p>

### 3.3. Quản lý CBCS

<p align="center">
  <img src="assets/05-personnel-list.jpg" width="900" alt="Danh sách CBCS"/>
</p>

<p align="center">
  <img src="assets/06-personnel-detail.jpg" width="900" alt="Thông tin chi tiết CBCS"/>
</p>

### 3.4. Quản lý công tác và phân công CBCS

<p align="center">
  <img src="assets/07-mission-list.jpg" width="900" alt="Danh sách công tác"/>
</p>

<p align="center">
  <img src="assets/08-mission-detail.jpg" width="900" alt="Thông tin chi tiết công tác"/>
</p>

<p align="center">
  <img src="assets/09-mission-assignment.jpg" width="900" alt="Phân công CBCS"/>
</p>

<p align="center">
  <img src="assets/14-personnel-mission-history.jpg" width="900" alt="Lịch sử công tác CBCS"/>
</p>

### 3.5. Nhật ký hệ thống

<p align="center">
  <img src="assets/10-audit-log.jpg" width="900" alt="Nhật ký hệ thống"/>
</p>

### 3.6. Thống kê và báo cáo

<p align="center">
  <img src="assets/13-mission-statistics.jpg" width="900" alt="Thống kê công tác CBCS"/>
</p>

<p align="center">
  <img src="assets/11-personnel-report.jpg" width="900" alt="Báo cáo quân số"/>
</p>

<p align="center">
  <img src="assets/12-report-preview.jpg" width="900" alt="Xem trước báo cáo"/>
</p>

### 3.7. Tài khoản, mật khẩu và phân quyền

<p align="center">
  <img src="assets/15-user-list.jpg" width="900" alt="Danh sách tài khoản"/>
</p>

<p align="center">
  <img src="assets/16-user-detail.jpg" width="900" alt="Thông tin tài khoản"/>
</p>

<p align="center">
  <img src="assets/17-change-password.jpg" width="900" alt="Đổi mật khẩu"/>
</p>

<p align="center">
  <img src="assets/18-permission-management.jpg" width="900" alt="Quản lý phân quyền"/>
</p>

### 3.8. Sao lưu và phục hồi dữ liệu

<p align="center">
  <img src="assets/19-backup-restore.jpg" width="900" alt="Sao lưu và phục hồi dữ liệu"/>
</p>

### 3.9. Cập nhật ứng dụng

<p align="center">
  <img src="assets/21-app-update.jpg" width="900" alt="Màn hình cập nhật ứng dụng"/>
</p>

<p align="center">
  <img src="assets/22-update-download-progress.png" width="900" alt="Tiến trình tải và xác minh gói cập nhật"/>
</p>

### 3.10. Tab Nghỉ phép

<p align="center">
  <img src="assets/20-leave-coming-soon.jpg" width="900" alt="Thông báo chức năng Nghỉ phép sẽ được cập nhật sau"/>
</p>

Tab Nghỉ phép hiện là màn hình thông báo đang phát triển, chưa cung cấp nghiệp vụ quản lý nghỉ phép hoàn chỉnh.

## 4. Công nghệ sử dụng

- **Ngôn ngữ lập trình:** C#
- **Nền tảng giao diện:** Windows Forms (WinForms)
- **Nền tảng chạy ứng dụng:** .NET 8
- **Cơ sở dữ liệu:** Microsoft SQL Server
- **Môi trường phát triển:** Visual Studio
- **Nguồn phát hành cập nhật:** GitHub Releases theo cấu hình dự án.

## 5. Yêu cầu triển khai

- Máy tính sử dụng hệ điều hành Windows tương thích với bản publish.
- .NET 8 Runtime phù hợp nếu bản publish không đóng gói sẵn runtime.
- Máy chủ SQL Server hoạt động và cho phép máy chạy ứng dụng kết nối.
- Cơ sở dữ liệu được tạo hoặc khôi phục theo cấu trúc mà ứng dụng yêu cầu.
- Chuỗi kết nối cơ sở dữ liệu được cấu hình chính xác.
- Tài khoản đăng nhập hợp lệ và được cấp quyền phù hợp.

## 6. Build và chạy ứng dụng

1. Mở solution của dự án bằng Visual Studio.
2. Kiểm tra cấu hình build, package phụ thuộc và chuỗi kết nối SQL Server.
3. Chọn cấu hình `Release`.
4. Thực hiện **Build → Rebuild Solution**.
5. Kiểm tra kết nối SQL Server và cơ sở dữ liệu.
6. Chạy ứng dụng từ Visual Studio hoặc sử dụng thư mục publish đã tạo.

Đường dẫn publish và cách đóng gói runtime cần căn cứ vào cấu hình thực tế của solution.

## 7. Lưu ý về dữ liệu và bảo mật

- Không đưa mật khẩu SQL Server, mật khẩu tài khoản hoặc chuỗi kết nối có thông tin xác thực lên GitHub.
- Không công khai dữ liệu CBCS thật, dữ liệu cá nhân hoặc file sao lưu cơ sở dữ liệu.
- Cấp quyền tài khoản theo đúng phạm vi công việc.
- Trước khi phục hồi cơ sở dữ liệu, cần xác minh file sao lưu và cân nhắc sao lưu trạng thái hiện tại.
- Không tắt ứng dụng khi quy trình cập nhật đang yêu cầu chờ hoàn tất.

## 8. Cấu trúc thư mục

```text
QuanLyCBCS/
├── README.md
└── assets/
    ├── 01-login.jpg
    ├── 02-home-dashboard.jpg
    ├── 03-mission-dashboard.jpg
    ├── 04-admin-dashboard.jpg
    ├── 05-personnel-list.jpg
    ├── 06-personnel-detail.jpg
    ├── 07-mission-list.jpg
    ├── 08-mission-detail.jpg
    ├── 09-mission-assignment.jpg
    ├── 10-audit-log.jpg
    ├── 11-personnel-report.jpg
    ├── 12-report-preview.jpg
    ├── 13-mission-statistics.jpg
    ├── 14-personnel-mission-history.jpg
    ├── 15-user-list.jpg
    ├── 16-user-detail.jpg
    ├── 17-change-password.jpg
    ├── 18-permission-management.jpg
    ├── 19-backup-restore.jpg
    ├── 20-leave-coming-soon.jpg
    ├── 21-app-update.jpg
    └── 22-update-download-progress.png
```

---

<div align="center">

**QUẢN LÝ CBCS — DESKTOP APPLICATION**

</div>
