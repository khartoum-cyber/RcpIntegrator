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
            Company = company ?? throw new ArgumentNullException(nameof(company));
            EmployeeCode = employeeCode ?? throw new ArgumentNullException(nameof(employeeCode));
            Date = date;
            EntryTime = entry;
            ExitTime = exit;

            if (exit < entry)
                throw new ArgumentException("ExitTime must be >= EntryTime.");
        }

        public TimeSpan Duration => ExitTime - EntryTime;

        public override string ToString() =>  $"{Company} - {EmployeeCode}  {Date:yyyy-MM-dd}  {EntryTime} - {ExitTime} ({Duration})";
    }
}
