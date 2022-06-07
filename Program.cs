using logiWeb.Repositories;
using logiWeb.Middlewares; 

namespace logiWeb
{
    public class Program
    {
        public static void Main(string[] args)
        {
            
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddSession(); 

            builder.Services.AddControllersWithViews();
            builder.Services.AddTransient<IClienteRepository, ClienteSqlRepository> ();
            builder.Services.AddTransient<IFuncionarioRepository, FuncionarioSqlRepository> ();
            builder.Services.AddTransient<ICargoRepository, CargoSqlRepository> ();
            builder.Services.AddTransient<IStatusRepository, StatusSqlRepository> ();
            builder.Services.AddTransient<IOrdemRepository, OrdemSqlRepository> ();
            builder.Services.AddTransient<IEntregaRepository, EntregaSqlRepository> ();
            builder.Services.AddTransient<IAuthenticationRepository, IAuthenticationSqlRepository> ();
            var app = builder.Build();
            
            app.UseSession();
            
            app.UseMiddleware<AuthenticationMiddleware>();
            
            app.UseStaticFiles(); 
            
            app.MapControllerRoute(name: "default", pattern: "{controller=Login}/{action=Index}/");
            app.Run();
        }
    }
}