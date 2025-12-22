using RcpIntegrator.App.Model;
using RcpIntegrator.App.Services.Interfaces;
using RcpIntegrator.App.Utilities;

namespace RcpIntegrator.App.Services.Parsers
{
    public sealed class Company1WorkDayParser : IWorkDayParser
    {
        private const string CompanyName = "Company1";

        public IEnumerable<WorkDay> Parse(Stream csvStream)
        {
            foreach (var row in CsvReader.ReadSemicolonSeparated(csvStream))
            {
                // ignore malformed
                if (row.Length < 5)
                    continue;

                var employeeCode = row[0].Trim();
                if (string.IsNullOrEmpty(employeeCode)) 
                    continue;

                // ParseExact ?
                if (!DateTime.TryParse(row[1].Trim(), out var date))
                    continue;
                if (!TimeSpan.TryParse(row[2].Trim(), out var entryTime))
                    continue;
                if (!TimeSpan.TryParse(row[3].Trim(), out var exitTime))
                    continue;

                if (exitTime < entryTime) 
                    continue;

                yield return new WorkDay(CompanyName, employeeCode, date, entryTime, exitTime);
            }
        }
    }
}
