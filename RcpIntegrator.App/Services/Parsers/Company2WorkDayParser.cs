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
            // Group events: (code, date) -> list of (time, type)
            var eventsByKey = new Dictionary<(string code, DateTime date), List<(TimeSpan time, string type)>>();

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

                var key = (code, date.Date);

                if (!eventsByKey.ContainsKey(key))
                {
                    eventsByKey[key] = new List<(TimeSpan time, string type)>();
                }

                eventsByKey[key].Add((time, type));
            }

            // Build WorkDay per valid pair (one WE and one WY)
            foreach (var kvp in eventsByKey)
            {
                var key = kvp.Key;
                var events = kvp.Value;
                var entryEvent = events.FirstOrDefault(e => e.type == "WE");
                var exitEvent = events.FirstOrDefault(e => e.type == "WY");

                if (exitEvent.time < entryEvent.time) 
                    continue;

                // Must have both entry and exit
                if (entryEvent.type == "WE" && exitEvent.type == "WY")
                {
                    yield return new WorkDay(
                        CompanyName,
                        key.code,
                        key.date,
                        entryEvent.time,
                        exitEvent.time);
                }
            }
        }

    }
}
