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

builder.Services.AddControllers();
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
