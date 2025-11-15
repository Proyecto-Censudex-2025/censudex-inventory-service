using censudex_inventory_service_api.src.Consumer;
using censudex_inventory_service_api.src.Controller;
using censudex_inventory_service_api.src.Messages;
using censudex_inventory_service_api.src.Repository;
using censudex_inventory_service_api.src.Service;
using DotNetEnv;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

///Carga las variables de entorno desde el archivo .env
Env.Load();

///Inicializa la conexión a Supabase utilizando las variables de entorno
var url = Environment.GetEnvironmentVariable("SUPABASE_URL");
var key = Environment.GetEnvironmentVariable("SUPABASE_API_KEY");

///Configura las opciones de Supabase, incluyendo la conexión en tiempo real
var options = new Supabase.SupabaseOptions
{
    AutoConnectRealtime = true
};
///Crea el cliente de Supabase y lo inicializa
var supabase = new Supabase.Client(url, key, options);
await supabase.InitializeAsync();

///Configura los servicios de la aplicación
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
if (app.Environment.IsDevelopment()) {}
app.UseHttpsRedirection();
app.MapGrpcService<ProductGrpcService>();

app.Run();

