# AI Product Manager Workspace

تطبيق ASP.NET Core Minimal API لتحليل طلبات العملاء وتحويلها إلى تذاكر تطوير واستراتيجية تسويقية.

## التشغيل

```bash
dotnet run
```

افتح العنوان الذي يظهر في الطرفية، أو استخدم:

```text
http://localhost:5000
```

## API

`POST /api/analyze`

```json
{
  "prompt": "تطبيق توصيل طلبات",
  "marketingFocus": "UVP"
}
```
