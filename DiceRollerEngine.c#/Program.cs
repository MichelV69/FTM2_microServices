using System.Collections;
using DiceRollerEngine;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
Hashtable apiInfo = new();
 apiInfo.Add("version", 1.0);
 apiInfo.Add("title", "DiceRollerEngine");
 apiInfo.Add("description", "Virtual Shiny Math Rock");

var aDiceTower = new DiceBag();


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
app.MapGet("/RollDice/{dice_string}", (string dice_string) => aDiceTower.RollDice(dice_string));
app.MapGet("/SearchStringForRolls/{string_to_search}", (string string_to_search) => aDiceTower.SearchStringForRolls(string_to_search));
app.Run();
