namespace RcpIntegrator.App.Model
{
    public class WorkDay
    {
        public string Company { get; }
        public string EmployeeCode { get; }
        public DateTime Date { get; }
        public TimeSpan EntryTime { get; }
        public TimeSpan ExitTime { get; }

        public WorkDay(string company, string employeeCode, DateTime date, TimeSpan entry, TimeSpan exit)
        {
            Company = company;
            EmployeeCode = employeeCode;
            Date = date;
            EntryTime = entry;
            ExitTime = exit;
        }
    }
}
