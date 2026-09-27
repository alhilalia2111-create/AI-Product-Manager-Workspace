using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapPost("/api/analyze", async (HttpContext context, AnalysisRequest request) =>
{
    if (string.IsNullOrWhiteSpace(request.Prompt))
    {
        return Results.BadRequest(new { error = "الرجاء إدخال طلب العميل أولاً" });
    }

    var tickets = new[]
    {
        new Ticket("PROD-001", "تطوير واجهة مستخدم تفاعلية (Frontend UI)", "🔴 High", $"بناء واجهة مخصصة فائقة السرعة ومتجاوبة لطلب '{request.Prompt.Trim()}' لتقديم تجربة استخدام ممتازة."),
        new Ticket("PROD-002", "هندسة البنية التحتية الخلفية (Backend Services)", "🟡 Medium", "تأسيس قواعد البيانات وبناء واجهات API وتأمين بوابات الدفع وحفظ سجلات العمليات."),
        new Ticket("PROD-003", "قابلية التوسع والتتبع (Observability & Scale)", "🟢 Medium", "إضافة مراقبة الأداء وتسجيل الأخطاء ومتابعة الأحداث التشغيلية لضمان استقرار الخدمة.")
    };

    var marketingStrategy = request.MarketingFocus switch
    {
        "Risk Reversal" => "إضافة ضمان استرجاع الأموال لمدة 14 يوماً بنقرة واحدة لتقليل مخاطر العميل وزيادة الثقة.",
        "Viral Loops" => "تفعيل نظام إحالة تلقائي يمنح العميل وصديقه رصيداً فورياً عند مشاركة التطبيق.",
        "UVP" => "عرض الميزة التنافسية الفريدة بوضوح خلال الثواني الأولى لكسب ثقة الزائر فوراً.",
        _ => "استراتيجية نمو افتراضية مخصصة بناءً على الطلب المعطى."
    };

    return Results.Ok(new AnalysisResponse(
        tickets,
        marketingStrategy,
        Random.Shared.Next(91, 99)));
});

app.MapGet("/", async context =>
{
    context.Response.ContentType = "text/html; charset=utf-8";
    await context.Response.WriteAsync(HtmlPage);
});

app.Run();

public sealed record AnalysisRequest(string Prompt = "", string MarketingFocus = "");
public sealed record Ticket(string Id, string Title, string Priority, string Description);
public sealed record AnalysisResponse(Ticket[] Tickets, string MarketingStrategy, int SuccessRate);

const string HtmlPage = """
<!DOCTYPE html>
<html lang="ar" dir="rtl">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>AI Product Manager Workspace</title>
    <style>
        :root { --bg:#0d1117; --panel:#161b22; --text:#c9d1d9; --muted:#8b949e; --border:#30363d; --p1:#4f46e5; --p2:#06b6d4; }
        * { box-sizing:border-box; }
        body { margin:0; background:var(--bg); color:var(--text); font-family:"Segoe UI",Tahoma,sans-serif; padding:40px 20px; }
        .container { max-width:1100px; margin:auto; }
        .title { font-size:clamp(2rem,4vw,3rem); font-weight:800; background:linear-gradient(45deg,var(--p1),var(--p2)); -webkit-background-clip:text; -webkit-text-fill-color:transparent; }
        .muted { color:var(--muted); margin-bottom:30px; }
        .grid { display:grid; grid-template-columns:repeat(auto-fit,minmax(250px,1fr)); gap:20px; }
        .field { margin-bottom:20px; } label { display:block; margin-bottom:8px; font-weight:600; }
        select, textarea { width:100%; padding:12px; border:1px solid var(--border); border-radius:10px; background:var(--panel); color:var(--text); font-size:1rem; }
        textarea { min-height:120px; resize:vertical; }
        button { border:0; border-radius:10px; padding:12px 22px; font-weight:700; cursor:pointer; color:white; }
        .primary { background:linear-gradient(45deg,var(--p1),var(--p2)); }
        .secondary { background:transparent; border:1px solid var(--p2); color:var(--p2); margin-bottom:20px; }
        .hidden { display:none; }
        .alert, .card { padding:20px; border-radius:12px; margin:15px 0; }
        .alert { background:#12351d; border:1px solid #238636; color:#b7f3c9; }
        .card { background:var(--panel); border:1px solid var(--border); border-right:5px solid #238636; }
        .marketing { border-right-color:#a855f7; background:#1f192c; }
    </style>
</head>
<body>
<main class="container">
    <h1 class="title">🚀 شاشة إدارة وتفكيك المنتجات الذكية</h1>
    <p class="muted">تشغيل فوري أوتوماتيكي بنظرية الكود البرمجي الواحد الشامل</p>
    <div class="grid">
        <div class="field"><label for="framework">إستراتيجية هندسة المتطلبات:</label><select id="framework"><option>PRD Framework (Agile)</option><option>User Story Mapping</option><option>Jobs-To-Be-Done (JTBD)</option></select></div>
        <div class="field"><label for="marketingFocus">محرك اليقين التسويقي:</label><select id="marketingFocus"><option value="Risk Reversal">تقليل المخاطر (Risk Reversal)</option><option value="Viral Loops">النمو الفيروسي (Viral Loops)</option><option value="UVP">عرض القيمة الفريد (UVP)</option></select></div>
    </div>
    <div class="field"><label for="prompt">📥 طلب العميل الخام:</label><textarea id="prompt" placeholder="مثال: تطبيق توصيل طلبات..."></textarea></div>
    <button class="primary" onclick="processRequirement()">⚡ تفكيك الطلب وإعداد الإستراتيجية</button>
    <section id="resultArea" class="hidden">
        <div class="alert">✅ تم التحليل بنجاح!</div>
        <button class="secondary" onclick="downloadReport()">📥 تحميل التقرير (Markdown)</button>
        <div class="card marketing"><h3>🎯 إستراتيجية التسويق</h3><p id="mktStrategyText"></p><strong>معدل اليقين المتوقع: <span id="successRateText"></span>%</strong></div>
        <h2>📋 التذاكر والمتطلبات التقنية</h2><div id="ticketsContainer"></div>
    </section>
</main>
<script>
let lastData = null;
async function processRequirement() {
    const prompt = document.getElementById('prompt').value.trim();
    if (!prompt) return alert('⚠️ يرجى كتابة طلب العميل أولاً');
    try {
        const response = await fetch('/api/analyze', { method:'POST', headers:{'Content-Type':'application/json'}, body:JSON.stringify({ prompt, marketingFocus:document.getElementById('marketingFocus').value }) });
        const data = await response.json();
        if (!response.ok) throw new Error(data.error || 'حدث خطأ أثناء التحليل');
        lastData = data;
        document.getElementById('mktStrategyText').textContent = data.marketingStrategy;
        document.getElementById('successRateText').textContent = data.successRate;
        const container = document.getElementById('ticketsContainer'); container.replaceChildren();
        data.tickets.forEach(t => {
            const card = document.createElement('div'); card.className='card';
            const title = document.createElement('h3'); title.textContent=`🆔 ${t.id} - ${t.title}`;
            const priority = document.createElement('p'); priority.innerHTML='<strong>الأولوية:</strong> '; priority.append(t.priority);
            const desc = document.createElement('p'); desc.textContent=t.description;
            card.append(title, priority, desc); container.append(card);
        });
        document.getElementById('resultArea').classList.remove('hidden');
    } catch (error) { alert(error.message || 'حدث خطأ غير متوقع'); }
}
function downloadReport() {
    if (!lastData) return;
    let md=`# 🚀 AI Product Management Report\n\n## 📥 الطلب الأساسي:\n${document.getElementById('prompt').value.trim()}\n\n## 📋 تذاكر العمل الفنية:\n`;
    lastData.tickets.forEach(t => md+=`### 🆔 ${t.id} - ${t.title}\n* الأولوية: ${t.priority}\n* الوصف: ${t.description}\n\n`);
    md+=`## 🎯 الخطة التسويقية:\n* إطار العمل: ${document.getElementById('framework').value}\n* معدل اليقين: ${lastData.successRate}%\n* التفاصيل: ${lastData.marketingStrategy}`;
    const url=URL.createObjectURL(new Blob([md],{type:'text/markdown;charset=utf-8'})); const a=document.createElement('a'); a.href=url; a.download='Product_Report.md'; a.click(); URL.revokeObjectURL(url);
}
</script>
</body>
</html>
""";
