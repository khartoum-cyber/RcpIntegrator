using RcpIntegrator.App.Model;
using RcpIntegrator.App.Services.Interfaces;

namespace RcpIntegrator.App.Services
{
    public sealed class WorkDayService
    {
        public IReadOnlyCollection<WorkDay> LoadUnique(params (Stream stream, IWorkDayParser parser)[] sources)
        {
            if (sources == null) 
                throw new ArgumentNullException(nameof(sources));

            var unique = new Dictionary<(string company, int code, DateTime date), WorkDay>();

            foreach (var (stream, parser) in sources)
            {
                if (stream == null)
                    throw new ArgumentNullException(nameof(stream));
                if (parser == null)
                    throw new ArgumentNullException(nameof(parser));

                foreach (var wd in parser.Parse(stream))
                {
                    var key = (wd.Company, wd.EmployeeCode, wd.Date);

                    unique.TryAdd(key, wd);
                }
            }

            return unique.Values.ToList();
        }
    }
}