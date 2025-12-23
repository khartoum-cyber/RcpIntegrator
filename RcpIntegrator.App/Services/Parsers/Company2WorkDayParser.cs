using RcpIntegrator.App.Model;
using RcpIntegrator.App.Services.Interfaces;
using RcpIntegrator.App.Utilities;

namespace RcpIntegrator.App.Services.Parsers
{
    public sealed class Company2WorkDayParser : IWorkDayParser
    {
        private const string CompanyName = "Company2";

        public IEnumerable<WorkDay> Parse(Stream csvStream)
        {
            // Collect all events per employee across all dates
            var byCode = new Dictionary<string, List<(DateTime when, string type)>>();

            foreach (var row in CsvReader.ReadSemicolonSeparated(csvStream))
            {
                // ignore malformed
                if (row.Length < 4)
                    continue;

                var code = row[0].Trim();

                // ParseExact ?
                if (!DateTime.TryParse(row[1].Trim(), out var date))
                    continue;

                if (!TimeSpan.TryParse(row[2].Trim(), out var time))
                    continue;

                var type = row[3].Trim().ToUpperInvariant(); // "WE" or "WY"

                if (type != "WE" && type != "WY")
                    continue;

                var when = date.Date + time;

                if (!byCode.TryGetValue(code, out var list))
                {
                    list = new List<(DateTime when, string type)>();
                    byCode[code] = list;
                }

                list.Add((when, type));

            }


            // For each employee, sort and pair WE -> next WY (even across days)
            foreach (var (code, events) in byCode)
            {
                var ordered = events.OrderBy(e => e.when).ToList();

                DateTime? openStart = null;

                foreach (var (when, type) in ordered)
                {
                    if (type == "WE")
                    {
                        openStart = when;
                    }
                    else if (type == "WY")
                    {
                        if (openStart.HasValue && when > openStart.Value)
                        {
                            var start = openStart.Value;
                            var end = when;
                            var duration = end - start;

                            // Sanity: positive and <= 24h
                            if (duration > TimeSpan.Zero && duration <= TimeSpan.FromHours(24))
                            {
                                yield return new WorkDay(
                                    CompanyName,
                                    code,
                                    start.Date,
                                    start.TimeOfDay,
                                    end.TimeOfDay);
                            }

                            openStart = null; // close the open shift
                        }
                    }
                }
            }
        }
    }
}
