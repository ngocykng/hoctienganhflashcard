# EnglishFlashcard3D

Ứng dụng desktop WPF học từ vựng tiếng Anh qua flashcard 3D, hỗ trợ sinh từ vựng bằng Gemini AI.

## Yêu cầu

- Windows 10/11
- Visual Studio Community 2026 trở lên
- .NET 10 SDK
- Tài khoản Google Gemini API

## Clone dự án về máy

Mở PowerShell và chạy:

```powershell
git clone https://github.com/ngocykng/hoctienganhflashcard.git
cd hoctienganhflashcard
```

Sau đó mở file solution/project trong Visual Studio.

## Cấu hình Gemini API Key

Ứng dụng đọc API key từ biến môi trường `GEMINI_API_KEY`.

### Cách 1: Cấu hình tạm thời trong PowerShell

```powershell
$env:GEMINI_API_KEY="YOUR_GEMINI_API_KEY"
```

Lệnh này chỉ có hiệu lực trong cửa sổ terminal hiện tại.

### Cách 2: Cấu hình vĩnh viễn trên Windows

Chạy PowerShell với quyền phù hợp:

```powershell
[System.Environment]::SetEnvironmentVariable("GEMINI_API_KEY", "YOUR_GEMINI_API_KEY", "User")
```

Sau đó đóng và mở lại Visual Studio để ứng dụng nhận biến môi trường mới.

## Chạy dự án

Trong thư mục dự án:

```powershell
dotnet restore
dotnet build
dotnet run
```

Hoặc mở bằng Visual Studio và nhấn **Start**.

## Cách dùng tính năng sinh từ vựng

1. Mở ứng dụng.
2. Nhập chủ đề, ví dụ: `life`, `business`, `technology`.
3. Chọn số lượng từ và mức độ.
4. Nhấn **Sinh Từ Vựng (Gemini AI)**.

## Đẩy dự án lên GitHub

Nếu đã có thay đổi mới, dùng các lệnh sau:

```powershell
git status
git add .
git commit -m "Update project"
git push origin master
```

## Lưu ý

- Không nên commit API key trực tiếp vào source code.
- Nếu bị lỗi 403/404/503 từ Gemini, hãy kiểm tra:
  - API key còn hiệu lực
  - đã bật Gemini API
  - model đang được hỗ trợ trong tài khoản của bạn

## License

Chưa thiết lập.
