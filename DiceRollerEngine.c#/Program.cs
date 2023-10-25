using DiceRollerEngine;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
var API_Version = 1.0;
var aDiceTower = new DiceBag();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
     c.SwaggerDoc($"v{API_Version}", new OpenApiInfo { Title = "DiceRollerEngine API", Description = "Virtual Shiny Math Rocks", Version = $"v{API_Version}" });
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
   c.SwaggerEndpoint("/swagger/v1/swagger.json", $"DiceRollerEngine API V{API_Version}");
});

app.MapGet("/", () => "Hello World!");
app.MapGet("/RollDice/{dice_string}", (string dice_string) => aDiceTower.RollDice(dice_string));

app.Run();
