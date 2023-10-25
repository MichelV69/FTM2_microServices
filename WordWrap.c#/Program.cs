using System.Collections;
using WordWrap;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
Hashtable apiInfo = new();
 apiInfo.Add("version", 1.0);
 apiInfo.Add("title", "WordWrap");
 apiInfo.Add("description", "converts WOT into Tidy WOT");

var wp = new Tidy();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    var v = apiInfo["version"];
    var t = apiInfo["title"];
    var d = apiInfo["description"];
     c.SwaggerDoc($"v{v}", new OpenApiInfo { Title = $"{t} API", Description = $"{d}", Version = $"v{v}" });
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
   var v = apiInfo["version"];
   var t = apiInfo["title"];
   var d = apiInfo["description"];
   c.SwaggerEndpoint("/swagger/v1/swagger.json", $"{t} API V{v}");
});

app.MapGet("/", () => "Welcome to " + apiInfo["title"]);
// app.MapGet("/RollDice/{dice_string}", (string dice_string) => aDiceTower.RollDice(dice_string));
app.MapGet("/WordWrap/{text_blob}/{wrap_col}", (string text_blob, int wrap_col) => wp.doWordWrap(text_blob, wrap_col)  );
app.Run();
