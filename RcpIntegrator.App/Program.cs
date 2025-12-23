using RcpIntegrator.App.Services;
using RcpIntegrator.App.Services.Parsers;

namespace RcpIntegrator.App
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("RCP Integrator App");

            var baseDir = AppContext.BaseDirectory;

            var rcp1Path = Path.Combine(baseDir, "rcp1.csv");
            var rcp2Path = Path.Combine(baseDir, "rcp2.csv");

            using var fs1 = File.OpenRead(rcp1Path);
            using var fs2 = File.OpenRead(rcp2Path);

            var service = new WorkDayService();
            var workDays = service.LoadUnique(
                (fs1, new Company1WorkDayParser()), (fs2, new Company2WorkDayParser()));

            // Print
            foreach (var wd in workDays.OrderBy(w => w.Company).ThenBy(w => w.EmployeeCode).ThenBy(w => w.Date))
            {
                Console.WriteLine(wd);
            }
        }
    }
}