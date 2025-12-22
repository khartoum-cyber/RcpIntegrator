using RcpIntegrator.App.Model;

namespace RcpIntegrator.App.Services.Interfaces
{
    public interface IWorkDayParser
    {
        IEnumerable<WorkDay> Parse(Stream csvStream);
    }
}
