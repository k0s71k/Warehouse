using Microsoft.EntityFrameworkCore;
using Warehouse.Server.Data;
using Warehouse.Server.Services;
using Warehouse.Server.Services.Interfaces;

namespace Warehouse.Server;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddCors();
        builder.Services.AddControllers();
        builder.Services.AddDbContext<WarehouseContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

        builder.Services.AddLogging(config =>
        {
            config.AddConsole();
            config.AddDebug();
        });

        builder.Services.AddScoped<IBalanceService, BalanceService>();
        builder.Services.AddScoped<IClientService, ClientService>();
        builder.Services.AddScoped<IMeasureUnitService, MeasureUnitService>();
        builder.Services.AddScoped<IResourceService, ResourceService>();
        builder.Services.AddScoped<IReceiveDocumentService, ReceiveDocumentService>();
        builder.Services.AddScoped<ISendDocumentService, SendDocumentService>();

        var app = builder.Build();

        app.UseDefaultFiles();
        app.MapStaticAssets();

        if (builder.Environment.IsDevelopment())
            app.UseCors(x => x
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed(o => true)
                .AllowCredentials());

        app.UseAuthorization();

        app.Services.CreateScope();

        app.MapControllers();

        app.MapFallbackToFile("/index.html");

        app.Run();
    }
}
