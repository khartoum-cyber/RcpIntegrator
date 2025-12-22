using RcpIntegrator.App.Services.Parsers;

namespace RcpIntegrator.Tests
{
    public class Company1WorkDayParserTests
    {
        [Test]
        public void Parse_ValidRow_YieldsWorkDay()
        {
            var csv = "EMP001; 2025-12-19; 08:00; 16:00; note\n";
            using var ms = CsvHelpers.ToStream(csv);

            var parser = new Company1WorkDayParser();

            var result = parser.Parse(ms).ToList();

            Assert.That(result, Has.Count.EqualTo(1));
            var wd = result[0];

            Assert.That(wd.Company, Is.EqualTo("Company1"));
            Assert.That(wd.EmployeeCode, Is.EqualTo("EMP001"));
            Assert.That(wd.Date.Date, Is.EqualTo(new DateTime(2025, 12, 19)));
            Assert.That(wd.EntryTime, Is.EqualTo(TimeSpan.FromHours(8)));
            Assert.That(wd.ExitTime, Is.EqualTo(TimeSpan.FromHours(16)));
        }
    }
}
