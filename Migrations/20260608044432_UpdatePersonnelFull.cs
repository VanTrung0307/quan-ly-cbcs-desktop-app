using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuanLyCBCS.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePersonnelFull : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CapBacHam",
                table: "Personnel",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ChucVu",
                table: "Personnel",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ChuyenNganhDaoTao",
                table: "Personnel",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Personnel",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "DangVien",
                table: "Personnel",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "GioiTinh",
                table: "Personnel",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "HKTTCu",
                table: "Personnel",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HKTTMoi",
                table: "Personnel",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "HeSoLuong",
                table: "Personnel",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "NgayBoNhiem",
                table: "Personnel",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "NgayCapCCCD",
                table: "Personnel",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "NgayNhanCapBac",
                table: "Personnel",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "NgayVaoDangChinhThuc",
                table: "Personnel",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "NgayVaoDangDuBi",
                table: "Personnel",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NoiSinh",
                table: "Personnel",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "QueQuanCu",
                table: "Personnel",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "QueQuanMoi",
                table: "Personnel",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ThangNamNhanCongTac",
                table: "Personnel",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ThangNamTotNghiep",
                table: "Personnel",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ThangNamVaoNganh",
                table: "Personnel",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TinhCu",
                table: "Personnel",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TinhMoi",
                table: "Personnel",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TrinhDoChuyenMon",
                table: "Personnel",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TrinhDoLyLuanChinhTri",
                table: "Personnel",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TrinhDoNgoaiNgu",
                table: "Personnel",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TrinhDoTinHoc",
                table: "Personnel",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TrinhDoVanHoa",
                table: "Personnel",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Personnel",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ViTriCongTac",
                table: "Personnel",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CapBacHam",
                table: "Personnel");

            migrationBuilder.DropColumn(
                name: "ChucVu",
                table: "Personnel");

            migrationBuilder.DropColumn(
                name: "ChuyenNganhDaoTao",
                table: "Personnel");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Personnel");

            migrationBuilder.DropColumn(
                name: "DangVien",
                table: "Personnel");

            migrationBuilder.DropColumn(
                name: "GioiTinh",
                table: "Personnel");

            migrationBuilder.DropColumn(
                name: "HKTTCu",
                table: "Personnel");

            migrationBuilder.DropColumn(
                name: "HKTTMoi",
                table: "Personnel");

            migrationBuilder.DropColumn(
                name: "HeSoLuong",
                table: "Personnel");

            migrationBuilder.DropColumn(
                name: "NgayBoNhiem",
                table: "Personnel");

            migrationBuilder.DropColumn(
                name: "NgayCapCCCD",
                table: "Personnel");

            migrationBuilder.DropColumn(
                name: "NgayNhanCapBac",
                table: "Personnel");

            migrationBuilder.DropColumn(
                name: "NgayVaoDangChinhThuc",
                table: "Personnel");

            migrationBuilder.DropColumn(
                name: "NgayVaoDangDuBi",
                table: "Personnel");

            migrationBuilder.DropColumn(
                name: "NoiSinh",
                table: "Personnel");

            migrationBuilder.DropColumn(
                name: "QueQuanCu",
                table: "Personnel");

            migrationBuilder.DropColumn(
                name: "QueQuanMoi",
                table: "Personnel");

            migrationBuilder.DropColumn(
                name: "ThangNamNhanCongTac",
                table: "Personnel");

            migrationBuilder.DropColumn(
                name: "ThangNamTotNghiep",
                table: "Personnel");

            migrationBuilder.DropColumn(
                name: "ThangNamVaoNganh",
                table: "Personnel");

            migrationBuilder.DropColumn(
                name: "TinhCu",
                table: "Personnel");

            migrationBuilder.DropColumn(
                name: "TinhMoi",
                table: "Personnel");

            migrationBuilder.DropColumn(
                name: "TrinhDoChuyenMon",
                table: "Personnel");

            migrationBuilder.DropColumn(
                name: "TrinhDoLyLuanChinhTri",
                table: "Personnel");

            migrationBuilder.DropColumn(
                name: "TrinhDoNgoaiNgu",
                table: "Personnel");

            migrationBuilder.DropColumn(
                name: "TrinhDoTinHoc",
                table: "Personnel");

            migrationBuilder.DropColumn(
                name: "TrinhDoVanHoa",
                table: "Personnel");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Personnel");

            migrationBuilder.DropColumn(
                name: "ViTriCongTac",
                table: "Personnel");
        }
    }
}
