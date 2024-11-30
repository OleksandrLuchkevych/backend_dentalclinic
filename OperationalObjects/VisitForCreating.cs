namespace Teether.OperationalObjects
{
    public class VisitForCreating
    {
        public DateOnly Date { get; set; }
        public TimeOnly Time { get; set; }
        public string PatientUserName { get; set; }
        public string DoctorUserName { get; set; }
    }
}
