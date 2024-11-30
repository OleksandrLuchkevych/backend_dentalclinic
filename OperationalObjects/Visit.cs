using System.ComponentModel.DataAnnotations.Schema;

namespace Teether.OperationalObjects
{
    [Table(nameof(Visit))]
    public class Visit
    {
        public long Id { get; set; }
        public DateOnly Date { get; set; }
        public TimeOnly Time { get; set; }
        public User Patient { get; set; }
        public User Doctor { get; set; }
    }
}
