# Quản lý CBCS --- Ứng dụng quản lý cán bộ, chiến sĩ Cảnh sát cơ động

Ứng dụng desktop xây dựng bằng **C# Windows Forms (WinForms)** nhằm hỗ
trợ quản lý hồ sơ cán bộ, chiến sĩ (CBCS), quản lý công tác và phân công
nhân sự, theo dõi tài khoản người dùng, phân quyền và lập báo cáo.

> **Trạng thái dự án:** Các ảnh bên dưới ghi lại giao diện đã thiết kế
> trong Visual Studio. Chúng không tự chứng minh rằng mọi nghiệp vụ đã
> được kiểm thử hoàn chỉnh. Hãy cập nhật trạng thái và thông tin kỹ
> thuật sau khi kiểm thử bản phát hành.

## Mục lục

-   [Tổng quan](#tổng-quan)
-   [Các chức năng chính](#các-chức-năng-chính)
-   [Ảnh giao diện](#ảnh-giao-diện)
-   [Công nghệ](#công-nghệ)
-   [Yêu cầu môi trường](#yêu-cầu-môi-trường)
-   [Cấu hình và chạy dự án](#cấu-hình-và-chạy-dự-án)
-   [Cơ sở dữ liệu và an toàn dữ
    liệu](#cơ-sở-dữ-liệu-và-an-toàn-dữ-liệu)
-   [Định hướng phát triển](#định-hướng-phát-triển)
-   [Lưu ý khi công khai repository](#lưu-ý-khi-công-khai-repository)

## Tổng quan

**Quản lý CBCS** là ứng dụng quản lý nội bộ với giao diện Windows Forms.
Các màn hình hiện có tập trung vào những nhóm nghiệp vụ:

-   Quản lý hồ sơ CBCS.
-   Quản lý công tác và phân công CBCS.
-   Thống kê, báo cáo quân số và lịch sử công tác.
-   Quản lý tài khoản, vai trò và quyền truy cập.
-   Nhật ký hoạt động và trung tâm thông báo.
-   Sao lưu, phục hồi cơ sở dữ liệu và xuất dữ liệu Excel từ các màn
    hình có hỗ trợ.

## Các chức năng chính

### 1. Đăng nhập và khởi tạo

-   Màn hình đăng nhập bằng tên tài khoản và mật khẩu.
-   Màn hình khởi tạo ứng dụng.
-   Giao diện nhận diện phần mềm.

### 2. Quản lý hồ sơ CBCS

-   Danh sách CBCS và khu vực tìm kiếm/lọc.
-   Các thao tác thêm, sửa và xóa hồ sơ.
-   Khu vực thống kê quân số.
-   Các nút hỗ trợ xuất Excel, sao lưu và phục hồi dữ liệu.

### 3. Quản lý công tác và phân công

-   Danh sách công tác.
-   Thêm hoặc chỉnh sửa thông tin công tác.
-   Dashboard tổng hợp tình hình công tác.
-   Màn hình phân công CBCS vào từng công tác.
-   Màn hình thống kê và lịch sử công tác CBCS.

### 4. Tài khoản và phân quyền

-   Danh sách tài khoản người dùng.
-   Tạo hoặc chỉnh sửa tài khoản.
-   Đổi mật khẩu.
-   Thiết lập quyền theo vai trò.
-   Các thao tác quản trị tài khoản như đặt lại mật khẩu hoặc mở khóa,
    tùy theo quyền được cấp.

### 5. Nhật ký, thông báo và báo cáo

-   Nhật ký hoạt động người dùng.
-   Trung tâm thông báo.
-   Dashboard quản trị.
-   Báo cáo quân số.
-   Màn hình xem báo cáo và thống kê công tác.

## Ảnh giao diện

Ảnh được lưu trong thư mục [`assets/`](assets/). Các liên kết ảnh dùng
đường dẫn tương đối để hiển thị trực tiếp trên GitHub.

### Đăng nhập và khởi tạo

  -----------------------------------------------------------------------
  Đăng nhập hệ thống                  Khởi tạo hệ thống
  ----------------------------------- -----------------------------------
  ![Đăng nhập hệ                      ![Khởi tạo hệ
  thống](assets/login.png)            thống](assets/loading.png)

  -----------------------------------------------------------------------

### Quản lý hồ sơ CBCS

![Danh sách CBCS](assets/personnel-list.png)

*Màn hình danh sách CBCS với khu vực tìm kiếm, lọc, thống kê và các thao
tác quản lý.*

### Quản lý công tác

  -----------------------------------------------------------------------
  Danh sách công tác                  Thông tin công tác
  ----------------------------------- -----------------------------------
  ![Quản lý công                      ![Thông tin công
  tác](assets/mission-list.png)       tác](assets/mission-detail.png)

  -----------------------------------------------------------------------

![Phân công CBCS](assets/mission-assignment.png)

*Màn hình phân công chia CBCS thành hai nhóm: chưa phân công và đã phân
công.*

  ----------------------------------------------------------------------------------
  Dashboard công tác                   Lịch sử công tác CBCS
  ------------------------------------ ---------------------------------------------
  ![Dashboard công                     ![Lịch sử công tác
  tác](assets/mission-dashboard.png)   CBCS](assets/personnel-mission-history.png)

  ----------------------------------------------------------------------------------

![Thống kê công tác CBCS](assets/personnel-mission-statistics.png)

### Tài khoản và phân quyền

  -----------------------------------------------------------------------
  Quản lý tài khoản                   Thông tin tài khoản
  ----------------------------------- -----------------------------------
  ![Quản lý tài                       ![Thông tin tài
  khoản](assets/user-list.png)        khoản](assets/user-detail.png)

  -----------------------------------------------------------------------

  -----------------------------------------------------------------------
  Quản lý phân quyền                  Đổi mật khẩu
  ----------------------------------- -----------------------------------
  ![Quản lý phân                      ![Đổi mật
  quyền](assets/permissions.png)      khẩu](assets/change-password.png)

  -----------------------------------------------------------------------

### Quản trị, nhật ký và báo cáo

![Bảng điều khiển quản trị](assets/admin-dashboard.png)

  -----------------------------------------------------------------------
  Nhật ký hệ thống                    Trung tâm thông báo
  ----------------------------------- -----------------------------------
  ![Nhật ký hệ                        ![Trung tâm thông
  thống](assets/audit-log.png)        báo](assets/notifications.png)

  -----------------------------------------------------------------------

  -----------------------------------------------------------------------
  Báo cáo quân số                     Xem báo cáo
  ----------------------------------- -----------------------------------
  ![Báo cáo quân                      ![Xem báo
  số](assets/personnel-report.png)    cáo](assets/report-viewer.png)

  -----------------------------------------------------------------------

## Công nghệ

Thông tin dưới đây dựa trên bối cảnh phát triển đã trao đổi; hãy đối
chiếu lại với cấu hình hiện tại của solution trước khi phát hành.

-   **Ngôn ngữ:** C#
-   **Giao diện desktop:** Windows Forms (WinForms)
-   **IDE:** Microsoft Visual Studio
-   **Cơ sở dữ liệu:** Microsoft SQL Server
-   **Dữ liệu và báo cáo:** có các màn hình hỗ trợ xuất Excel, sao lưu
    và phục hồi cơ sở dữ liệu.

> Nếu repository có dùng Entity Framework, Guna UI hoặc thư viện Excel
> cụ thể, hãy ghi đúng phiên bản/package đang có trong solution và file
> cấu hình NuGet.

## Yêu cầu môi trường

Môi trường dự kiến để mở và chạy dự án:

-   Windows.
-   Visual Studio với workload **.NET desktop development**.
-   Phiên bản .NET Framework đúng với cấu hình của project (kiểm tra tại
    **Project Properties → Application**).
-   Microsoft SQL Server Database Engine nếu ứng dụng cần kết nối cơ sở
    dữ liệu SQL Server.
-   SQL Server Management Studio (SSMS) là công cụ quản trị cơ sở dữ
    liệu, không thay thế SQL Server Database Engine.

## Cấu hình và chạy dự án

1.  Clone repository về máy.
2.  Mở file solution (`.sln`) bằng Visual Studio.
3.  Kiểm tra Target Framework trong **Project → Properties →
    Application**.
4.  Kiểm tra chuỗi kết nối trong cấu hình ứng dụng, chẳng hạn
    `App.config`; không mặc định dùng tên server của máy phát triển.
5.  Tạo hoặc phục hồi database theo cấu trúc mà phiên bản mã nguồn hiện
    tại yêu cầu.
6.  Đặt project WinForms làm **Startup Project**.
7.  Build solution, xử lý hết lỗi build rồi chạy thử bằng `F5`.

Ví dụ chuỗi kết nối chỉ để minh họa --- hãy thay tên server và database
theo môi trường thực tế:

``` xml
<connectionStrings>
  <add name="AppDbContext"
       connectionString="Data Source=YOUR_SQL_SERVER;Initial Catalog=YOUR_DATABASE;Integrated Security=True;TrustServerCertificate=True"
       providerName="System.Data.SqlClient" />
</connectionStrings>
```

Tên `AppDbContext`, provider và thuộc tính kết nối trong ví dụ phải được
điều chỉnh cho khớp với cấu hình thực tế của dự án. Không đăng tải mật
khẩu cơ sở dữ liệu hoặc thông tin xác thực thật lên GitHub.

## Cơ sở dữ liệu và an toàn dữ liệu

-   Sao lưu database trước khi cập nhật cấu trúc hoặc chạy thử chức năng
    phục hồi.
-   Kiểm tra bản backup có thể phục hồi thành công trên một database thử
    nghiệm.
-   Không đưa dữ liệu CBCS thật, tài khoản, mật khẩu, khóa bí mật hoặc
    file backup chứa dữ liệu nội bộ lên repository công khai.
-   Với triển khai nhiều máy, cần cấu hình SQL Server và quyền truy cập
    mạng một cách phù hợp; không nên để lộ cổng cơ sở dữ liệu ra
    Internet công cộng.
-   Kiểm tra quyền truy cập ở cả giao diện và tầng xử lý nghiệp vụ,
    không chỉ ẩn nút trên màn hình.

## Định hướng phát triển

-   Hoàn thiện kiểm thử các luồng thêm/sửa/xóa và phân công CBCS.
-   Kiểm tra tính nhất quán giữa dashboard, danh sách và báo cáo.
-   Hoàn thiện thông báo lỗi và xử lý trường hợp database chưa có dữ
    liệu.
-   Kiểm thử phân quyền theo từng vai trò.
-   Hoàn thiện quy trình publish/cài đặt và cập nhật ứng dụng.
-   Bổ sung tài liệu cấu hình database, hướng dẫn người dùng và hướng
    dẫn sao lưu/phục hồi.

## Lưu ý khi công khai repository

Trước khi đặt repository ở chế độ Public:

1.  Kiểm tra ảnh chụp màn hình để loại bỏ tên người dùng, dữ liệu cá
    nhân hoặc thông tin nội bộ không muốn công khai.
2.  Kiểm tra `App.config`, file `.json`, script SQL và lịch sử Git để
    bảo đảm không chứa mật khẩu hoặc thông tin kết nối nhạy cảm.
3.  Không đưa database sản xuất, file `.bak`, dữ liệu CBCS thật hoặc
    thông tin tài khoản lên GitHub.
4.  Chỉ ghi "đã kiểm thử" hoặc "đã phát hành" đối với chức năng đã được
    kiểm tra thực tế.

------------------------------------------------------------------------

**Tên dự án:** Quản lý CBCS\
**Mô tả ngắn:** Ứng dụng desktop quản lý hồ sơ CBCS, công tác, phân
công, tài khoản, phân quyền và báo cáo bằng C# WinForms.
