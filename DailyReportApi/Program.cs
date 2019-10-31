using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace DailyReportApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CheckDbFolder();
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseUrls("http://localhost:5000", "http://*:5001")
                .UseStartup<Startup>();

        private static void CheckDbFolder()
        {
            if (!Directory.Exists("Data"))
            {
                try
                {
                    Directory.CreateDirectory("Data");
                    Console.WriteLine("Created \"Data\" folder.");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }

            if (!Directory.Exists("Log"))
            {
                try
                {
                    Directory.CreateDirectory("Log");
                    Console.WriteLine("Created \"Log\" folder.");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }
    }
}
