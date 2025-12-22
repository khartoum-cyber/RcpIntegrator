using RcpIntegrator.App.Services;
using RcpIntegrator.App.Services.Parsers;

namespace RcpIntegrator.App
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            using var fs1 = File.OpenRead("rcp1.csv");
            //using var fs2 = File.OpenRead("rcp2.csv");

            var service = new WorkDayService();
            var workDays = service.LoadUnique(
                fs1, new Company1WorkDayParser());

            // Print
            foreach (var wd in workDays.OrderBy(w => w.Company).ThenBy(w => w.EmployeeCode).ThenBy(w => w.Date))
            {
                Console.WriteLine(wd.EmployeeCode);
            }
        }
    }
}