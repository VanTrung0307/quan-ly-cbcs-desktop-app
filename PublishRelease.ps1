#requires -Version 5.1
<#
.SYNOPSIS
    Tao goi cap nhat QuanLyCBCS va tu dong sinh latest.json.

.DESCRIPTION
    - Khong publish project thay Visual Studio.
    - Nhan thu muc Publish da duoc Visual Studio tao.
    - Tao ZIP voi noi dung nam truc tiep o thu muc goc cua ZIP.
    - Tinh SHA-256 tu file ZIP.
    - Tao latest.json dung schema ma GitHubUpdateService dang doc.
    - Kiem tra EXE, ZIP, SHA-256, JSON va cac tep database local.
    - Khong upload GitHub va khong tao GitHub Release.

.EXAMPLE
    .\PublishRelease.ps1 -PublishDirectory "E:\QuanLyCBCS_Release_v3.1.0"
#>

[CmdletBinding()]
param(
    [Parameter()]
    [string]$PublishDirectory,

    [Parameter()]
    [string]$Version,

    [Parameter()]
    [string]$OutputDirectory,

    [Parameter()]
    [string]$ReleaseNotes,

    [Parameter()]
    [switch]$Mandatory
)

Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

function Write-Section {
    param([string]$Message)
    Write-Host ""
    Write-Host ("=" * 64) -ForegroundColor DarkCyan
    Write-Host ("  " + $Message) -ForegroundColor Cyan
    Write-Host ("=" * 64) -ForegroundColor DarkCyan
}

function Read-RequiredValue {
    param(
        [string]$Prompt,
        [string]$CurrentValue
    )

    if (-not [string]::IsNullOrWhiteSpace($CurrentValue)) {
        return $CurrentValue.Trim()
    }

    do {
        $value = Read-Host $Prompt
    } while ([string]::IsNullOrWhiteSpace($value))

    return $value.Trim()
}

function Get-VersionFromFolderName {
    param([string]$FolderName)

    # Ho tro: QuanLyCBCS_Release_v3.1.0, Release_3.1.0, v3.1.0
    $match = [regex]::Match(
        $FolderName,
        '(?<!\d)v?(\d+\.\d+\.\d+(?:\.\d+)?)(?!\d)',
        [System.Text.RegularExpressions.RegexOptions]::IgnoreCase
    )

    if ($match.Success) {
        return $match.Groups[1].Value
    }

    return $null
}

function Test-SemanticVersion {
    param([string]$Value)

    if ($Value -notmatch '^\d+\.\d+\.\d+(?:\.\d+)?$') {
        throw "Version '$Value' khong hop le. Dinh dang duoc chap nhan: 3.1.0 hoac 3.1.0.1."
    }
}

try {
    Write-Section "QUAN LY CBCS - TAO GOI PHAT HANH"

    # 1. Xac dinh thu muc Publish.
    if ([string]::IsNullOrWhiteSpace($PublishDirectory)) {
        $PublishDirectory = Read-Host "Nhap duong dan thu muc Publish (chua QuanLyCBCS.exe)"
    }

    if ([string]::IsNullOrWhiteSpace($PublishDirectory)) {
        throw "Chua cung cap thu muc Publish."
    }

    $PublishDirectory = [System.IO.Path]::GetFullPath($PublishDirectory.Trim('"'))

    if (-not (Test-Path -LiteralPath $PublishDirectory -PathType Container)) {
        throw "Khong tim thay thu muc Publish: $PublishDirectory"
    }

    $exePath = Join-Path $PublishDirectory "QuanLyCBCS.exe"
    if (-not (Test-Path -LiteralPath $exePath -PathType Leaf)) {
        throw "Thu muc Publish khong co QuanLyCBCS.exe: $PublishDirectory"
    }

    # Chan viec vo tinh dong goi database local / backup database.
    $databaseFiles = @(
        Get-ChildItem -LiteralPath $PublishDirectory -File -Recurse |
        Where-Object {
            $_.Extension -in @(".mdf", ".ldf", ".bak")
        }
    )

    if ($databaseFiles.Count -gt 0) {
        $names = ($databaseFiles | ForEach-Object { $_.FullName }) -join "`n"
        throw "Phat hien tep database/backup trong thu muc Publish. Hay loai bo truoc khi dong goi:`n$names"
    }

    # 2. Xac dinh version.
    if ([string]::IsNullOrWhiteSpace($Version)) {
        $Version = Get-VersionFromFolderName (Split-Path $PublishDirectory -Leaf)
    }

    if ([string]::IsNullOrWhiteSpace($Version)) {
        $Version = Read-Host "Khong doc duoc version tu ten thu muc. Nhap version (vi du 3.1.0)"
    }

    $Version = $Version.Trim()
    Test-SemanticVersion $Version

    # 3. Xac dinh noi luu ZIP va latest.json.
    if ([string]::IsNullOrWhiteSpace($OutputDirectory)) {
        $OutputDirectory = Split-Path $PublishDirectory -Parent
    }

    $OutputDirectory = [System.IO.Path]::GetFullPath($OutputDirectory.Trim('"'))

    if (-not (Test-Path -LiteralPath $OutputDirectory -PathType Container)) {
        New-Item -ItemType Directory -Path $OutputDirectory -Force | Out-Null
    }

    $packageFileName = "QuanLyCBCS_Release_v$Version.zip"
    $zipPath = Join-Path $OutputDirectory $packageFileName
    $manifestPath = Join-Path $PSScriptRoot "latest.json"

    Write-Host "Thu muc Publish : $PublishDirectory"
    Write-Host "Phien ban       : $Version"
    Write-Host "File ZIP        : $zipPath"
    Write-Host "Manifest        : $manifestPath"

    # 4. Nhan thong tin phat hanh.
    $ReleaseNotes = Read-RequiredValue `
        -Prompt "Nhap ghi chu phat hanh (release notes)" `
        -CurrentValue $ReleaseNotes

    $isMandatory = [bool]$Mandatory
    if (-not $Mandatory.IsPresent) {
        $mandatoryAnswer = Read-Host "Day co phai ban cap nhat bat buoc khong? (y/N)"
        $isMandatory = $mandatoryAnswer -match '^(y|yes)$'
    }

    # 5. Xac nhan ghi de file cu neu co.
    foreach ($existingPath in @($zipPath, $manifestPath)) {
        if (Test-Path -LiteralPath $existingPath -PathType Leaf) {
            $answer = Read-Host "File da ton tai: $existingPath. Ghi de? (y/N)"
            if ($answer -notmatch '^(y|yes)$') {
                throw "Da dung de tranh ghi de file phat hanh hien co."
            }
        }
    }

    # 6. Tao ZIP. IncludeBaseDirectory = false de EXE nam o root ZIP.
    Write-Section "DANG DONG GOI ZIP"

    Add-Type -AssemblyName System.IO.Compression
    Add-Type -AssemblyName System.IO.Compression.FileSystem

    if (Test-Path -LiteralPath $zipPath) {
        Remove-Item -LiteralPath $zipPath -Force
    }

    [System.IO.Compression.ZipFile]::CreateFromDirectory(
        $PublishDirectory,
        $zipPath,
        [System.IO.Compression.CompressionLevel]::Optimal,
        $false
    )

    if (-not (Test-Path -LiteralPath $zipPath -PathType Leaf)) {
        throw "Khong tao duoc file ZIP."
    }

    # 7. Kiem tra cau truc ZIP: EXE phai nam ngay tai thu muc goc.
    $archive = [System.IO.Compression.ZipFile]::OpenRead($zipPath)
    try {
        $rootExe = $archive.Entries |
            Where-Object {
                $_.FullName -eq "QuanLyCBCS.exe"
            } |
            Select-Object -First 1

        if ($null -eq $rootExe) {
            throw "Cau truc ZIP khong hop le: QuanLyCBCS.exe khong nam o thu muc goc."
        }

        if ($archive.Entries.Count -eq 0) {
            throw "File ZIP khong co noi dung."
        }
    }
    finally {
        $archive.Dispose()
    }

    # 8. Tinh SHA-256 tu ZIP vua tao.
    Write-Section "TINH SHA-256"

    $zipInfo = Get-Item -LiteralPath $zipPath
    if ($zipInfo.Length -le 0) {
        throw "File ZIP co dung luong bang 0."
    }

    if ($zipInfo.Length -gt 500MB) {
        throw "File ZIP vuot qua gioi han 500 MB cua dich vu cap nhat."
    }

    $sha256 = (Get-FileHash -LiteralPath $zipPath -Algorithm SHA256).Hash.ToUpperInvariant()

    if ($sha256 -notmatch '^[0-9A-F]{64}$') {
        throw "Khong tao duoc ma SHA-256 hop le."
    }

    # 9. Tao latest.json dung schema cua UpdateManifest.
    Write-Section "TAO LATEST.JSON"

    $manifest = [ordered]@{
        version         = $Version
        packageFileName = $packageFileName
        sha256          = $sha256
        releaseNotes    = $ReleaseNotes
        publishedAtUtc  = [DateTime]::UtcNow.ToString(
            "yyyy-MM-ddTHH:mm:ssZ",
            [Globalization.CultureInfo]::InvariantCulture
        )
        mandatory       = $isMandatory
    }

    $json = $manifest | ConvertTo-Json -Depth 5
    $utf8NoBom = New-Object System.Text.UTF8Encoding($false)

    $manifestTempPath = "$manifestPath.tmp"
    [System.IO.File]::WriteAllText($manifestTempPath, $json, $utf8NoBom)

    # Doc lai JSON de xac nhan cu phap va cac truong bat buoc.
    $manifestCheck = Get-Content -LiteralPath $manifestTempPath -Raw -Encoding UTF8 |
        ConvertFrom-Json

    if ($manifestCheck.version -ne $Version -or
        $manifestCheck.packageFileName -ne $packageFileName -or
        $manifestCheck.sha256 -ne $sha256 -or
        [string]::IsNullOrWhiteSpace($manifestCheck.releaseNotes)) {
        throw "Du lieu latest.json sau khi tao khong khop."
    }

    if ((Get-Item -LiteralPath $manifestTempPath).Length -gt 1MB) {
        throw "latest.json vuot qua gioi han 1 MB."
    }

    Move-Item -LiteralPath $manifestTempPath -Destination $manifestPath -Force

    # 10. Tinh lai SHA-256 doc lap va doi chieu.
    $verifiedHash = (Get-FileHash -LiteralPath $zipPath -Algorithm SHA256).Hash.ToUpperInvariant()
    if ($verifiedHash -ne $sha256) {
        throw "SHA-256 kiem tra lai khong trung khop."
    }

    # 11. Bao cao ket qua.
    Write-Section "HOAN TAT - KIEM TRA THANH CONG"

    [PSCustomObject]@{
        Version         = $Version
        Package         = $packageFileName
        PackageSizeMB   = [Math]::Round($zipInfo.Length / 1MB, 2)
        SHA256          = $sha256
        Manifest        = $manifestPath
        PublishedAtUtc  = $manifest.publishedAtUtc
        Mandatory       = $isMandatory
        RootExeVerified = $true
        HashVerified    = $true
    } | Format-List

    Write-Host "Hai file can upload len GitHub Release:" -ForegroundColor Green
    Write-Host "1. $zipPath" -ForegroundColor White
    Write-Host "2. $manifestPath" -ForegroundColor White
    Write-Host ""
    Write-Host "Luu y: Script chua upload va chua tao GitHub Release." -ForegroundColor Yellow
}
catch {
    Write-Host ""
    Write-Host ("PHAT HANH THAT BAI: " + $_.Exception.Message) -ForegroundColor Red
    exit 1
}
