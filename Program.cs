using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Hosting;

namespace PaymentRecive.API
{
    public class Program
    {
        public static string Namespace = typeof(Startup).Namespace;
        public static string AppName = Namespace.Substring(Namespace.LastIndexOf('.', Namespace.LastIndexOf('.') - 1) + 1);
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
            .ConfigureKestrel(options =>
            {
                options.ListenAnyIP(81, listenOptions =>
                listenOptions.Protocols = HttpProtocols.Http1);

                options.ListenAnyIP(5001, listenOptions =>
                        listenOptions.Protocols = HttpProtocols.Http2);
            })
            .UseStartup<Startup>();
    }
}
