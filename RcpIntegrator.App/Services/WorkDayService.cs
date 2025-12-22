using RcpIntegrator.App.Model;
using RcpIntegrator.App.Services.Interfaces;

namespace RcpIntegrator.App.Services
{
    public sealed class WorkDayService
    {
        public IReadOnlyCollection<WorkDay> LoadUnique(Stream stream, IWorkDayParser parser)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));
            if (parser == null) throw new ArgumentNullException(nameof(parser));

            var unique = new Dictionary<(string company, string code, DateTime date), WorkDay>();

            foreach (var wd in parser.Parse(stream))
            {
                var key = (wd.Company, wd.EmployeeCode, wd.Date);

                unique.TryAdd(key, wd);
            }

            return unique.Values.ToList();
        }
    }
}