using ArticleAPI.Services;
using ArticleAPI.Settings;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<ArticleDatabaseSettings>(
    builder.Configuration.GetSection("ArticleDatabase"));
builder.Services.Configure<ReviewApiSettings>(
    builder.Configuration.GetSection("ReviewApi"));

builder.Services.AddSingleton<ArticlesService>();
builder.Services.AddHttpClient<ReviewsServiceClient>();

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
