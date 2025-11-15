using censudex_inventory_service_api.src.Consumer;
using censudex_inventory_service_api.src.Controller;
using censudex_inventory_service_api.src.Messages;
using censudex_inventory_service_api.src.Repository;
using censudex_inventory_service_api.src.Service;
using DotNetEnv;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);
Env.Load();
var url = Environment.GetEnvironmentVariable("SUPABASE_URL");
var key = Environment.GetEnvironmentVariable("SUPABASE_API_KEY");

var options = new Supabase.SupabaseOptions
{
    AutoConnectRealtime = true
};
var supabase = new Supabase.Client(url, key, options);
await supabase.InitializeAsync();

builder.Services.AddSingleton<Supabase.Client>(supabase);
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddGrpc();
builder.Services.AddMassTransit(x =>
    {
    x.AddConsumer<OrderCreatedConsumer>();
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });
        cfg.ReceiveEndpoint("order-created-queue", e =>
        {
            e.ConfigureConsumer<OrderCreatedConsumer>(context);
        });
        cfg.Send<OrderFailedStockMessage>(e =>
        {
        e.UseRoutingKeyFormatter(context => "order.failed.stock");
        });
        cfg.Send<StockLowMessage>(e =>
        {
        e.UseRoutingKeyFormatter(context => "stock.low");
        });
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
}

app.UseHttpsRedirection();
app.MapGrpcService<ProductGrpcService>();

app.Run();

