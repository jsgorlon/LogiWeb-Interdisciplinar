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

            var app = builder.Build();
            
            app.UseStaticFiles(); 
            app.MapDefaultControllerRoute();
            app.Run();
        }
    }
}