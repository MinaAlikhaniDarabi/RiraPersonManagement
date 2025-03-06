
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc(options =>
{
    options.Interceptors.Add<ExceptionInterceptor>(); 
});
var app = builder.Build();

app.MapGrpcService<PersonManagementService>();
app.Run();
