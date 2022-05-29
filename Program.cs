using logiWeb.Repositories;

namespace logiWeb
{
    public class Program
    {
        public static void Main(string[] args)
        {
            
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllersWithViews();
            builder.Services.AddTransient<IClienteRepository, ClienteSqlRepository> ();
            builder.Services.AddTransient<IFuncionarioRepository, FuncionarioSqlRepository> ();
            builder.Services.AddTransient<ICargoRepository, CargoSqlRepository> ();
            //builder.Services.AddTransient<IStatusRepository, StatusSqlRepository> ();
            //builder.Services.AddTransient<IOrdemRepository, OrdemSqlRepository> ();
            //builder.Services.AddTransient<IEntregaRepository, EntregaSqlRepository> ();
            
            var app = builder.Build();
            
            app.UseStaticFiles(); 
            app.MapDefaultControllerRoute();
            app.Run();
        }
    }
}