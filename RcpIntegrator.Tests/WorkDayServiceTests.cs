using RcpIntegrator.App.Model;
using RcpIntegrator.App.Services;
using RcpIntegrator.App.Services.Interfaces;

namespace RcpIntegrator.Tests
{
    class FakeParser : IWorkDayParser
    {
        private readonly IEnumerable<WorkDay> _items;
        public FakeParser(IEnumerable<WorkDay> items) => _items = items;
        public IEnumerable<WorkDay> Parse(Stream csvStream) => _items;
    }

    public class WorkDayServiceTests
    {
        [Test]
        public void LoadUnique_NullStream_Throws()
        {
            var service = new WorkDayService();
            var parser = new FakeParser(Array.Empty<WorkDay>());

            Assert.Throws<ArgumentNullException>(() => service.LoadUnique(null!, parser));
        }

        [Test]
        public void LoadUnique_NullParser_Throws()
        {
            var service = new WorkDayService();
            using var ms = new MemoryStream();

            Assert.Throws<ArgumentNullException>(() => service.LoadUnique(ms, null!));
        }

        [Test]
        public void LoadUnique_DuplicatesByEmpCodeAndDate_FirstWins()
        {
            var date = new DateTime(2025, 12, 19);
            var w1 = new WorkDay("Company1", "EMP001", date, TimeSpan.FromHours(8), TimeSpan.FromHours(16));
            var w2 = new WorkDay("Company1", "EMP001", date, TimeSpan.FromHours(9), TimeSpan.FromHours(17)); // duplicate key

            var parser = new FakeParser(new[] { w1, w2 });
            var service = new WorkDayService();
            using var ms = new MemoryStream();

            var result = service.LoadUnique(ms, parser).ToList();

            Assert.That(result, Has.Exactly(1).Items);
            Assert.That(w1, Is.SameAs(result[0])); // TryAdd keeps first occurrence
        }

        [Test]
        public void LoadUnique_DistinctDates_AreAllKept()
        {
            var d1 = new DateTime(2025, 12, 19);
            var d2 = new DateTime(2025, 12, 20);

            var items = new[]
            {
                new WorkDay("Company1", "EMP001", d1, TimeSpan.FromHours(8), TimeSpan.FromHours(16)),
                new WorkDay("Company1", "EMP001", d2, TimeSpan.FromHours(8), TimeSpan.FromHours(16)),
            };

            var parser = new FakeParser(items);
            var service = new WorkDayService();
            using var ms = new MemoryStream();

            var result = service.LoadUnique(ms, parser).OrderBy(w => w.Date).ToList();

            Assert.That(result, Has.Exactly(2).Items);
            Assert.That(d1, Is.EqualTo(result[0].Date));
            Assert.That(d2, Is.EqualTo(result[1].Date));
        }

    }
}