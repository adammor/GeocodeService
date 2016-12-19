using T = GeocodeService.Modules.StreetAddress;
using System;
using System.Reflection;
namespace RESTService.Modules
{
    class Program
    {     
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Service is running ...");
                RestEL.Modules.Startup.Initialize<T>(Assembly.GetExecutingAssembly().GetName().Name);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}