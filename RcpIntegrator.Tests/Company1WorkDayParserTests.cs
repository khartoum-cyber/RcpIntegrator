using RcpIntegrator.App.Model;
using RcpIntegrator.App.Services.Parsers;
using NUnit.Framework;

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

        [Test]
        public void Parse_MixedValidInvalid_OnlyValidReturned()
        {

            var csv = string.Join("\n", new[]
            {
                "EMP001; 2025-12-19; 08:00; 16:00; ok",
                "EMP001; not-a-date; 08:00; 16:00; bad",
                "EMP002; 2025-12-20; 09:00; 17:00; ok",
                "EMP003; 2025-12-21; 09:00; not-a-time; bad",
            }) + "\n";

            using var ms = CsvHelpers.ToStream(csv);
            var parser = new Company1WorkDayParser();

            var result = parser.Parse(ms).ToList();

            Assert.Multiple(() =>
            {
                Assert.That(result.Count, Is.EqualTo(2));

                // All returned items must be from Company1
                Assert.That(result, Has.All.Matches<WorkDay>(wd => wd.Company == "Company1"));

                // Collection should contain EMP001 and EMP002
                Assert.That(result, Has.Some.Matches<WorkDay>(wd => wd.EmployeeCode == "EMP001"));
                Assert.That(result, Has.Some.Matches<WorkDay>(wd => wd.EmployeeCode == "EMP002"));
            });
        }

    }


}
