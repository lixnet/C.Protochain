using C.Protochain.Business;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace C.Protochain.Node
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BlockBusiness.BuildGenesisBlock();

            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
            .UseKestrel()
                .UseUrls("http://localhost:3001")
                .UseStartup<Startup>()
                .Build();
    }
}
