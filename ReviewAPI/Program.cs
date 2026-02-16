using Microsoft.AspNetCore.OData;
using Microsoft.OData.ModelBuilder;
using ReviewAPI.Entities;
using ReviewAPI.Services;
using ReviewAPI.Settings;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<ReviewDatabaseSettings>(
    builder.Configuration.GetSection("ReviewDatabase"));
builder.Services.Configure<ArticleApiSettings>(
    builder.Configuration.GetSection("ArticleApi"));

builder.Services.AddSingleton<ReviewsService>();
builder.Services.AddHttpClient<ArticlesServiceClient>();

var reviewODataBuilder = new ODataConventionModelBuilder();
reviewODataBuilder.EntitySet<Review>("Reviews");

builder.Services.AddControllers()
    .AddOData(options => options
        .Select()
        .Filter()
        .OrderBy()
        .Expand()
        .Count()
        .SetMaxTop(100)
        .AddRouteComponents("odata", reviewODataBuilder.GetEdmModel()));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
