using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace PUTSWeb
{
  public class Program
  {
    public static void Main(string[] args)
    {
      CreateWebHostBuilder(args).Build().Run();
    }

    public static IWebHostBuilder CreateWebHostBuilder(string[] args)
    {
      var p = System.Reflection.Assembly.GetEntryAssembly().Location;
      p = p.Substring(0, p.LastIndexOf(@"\") + 1);

      return WebHost.CreateDefaultBuilder(args)
          .UseContentRoot(p)
          .UseStartup<Startup>();
    }
  }
}
