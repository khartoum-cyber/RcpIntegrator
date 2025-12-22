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


    }
}
