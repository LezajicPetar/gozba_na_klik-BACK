namespace gozba_na_klik.Model.Entities
{
    public class WorkTime
    {
        public int Id { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }
        public int DayOfWeek { get; set; }
        public TimeOnly Start { get; set; }
        public TimeOnly End { get; set; }
    }
}
