namespace gozba_na_klik.Model.Entities
{

    public enum EmployeeStatus
    {
        Active = 1,
        Suspended = 2
    }
    public class EmployeeMenagement
    {
        public int Id { get; set; }

        public int RestaurantId { get; set; }

        public Restaurant Restaurant { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string Email { get; set; }
        public string PasswordHash { get; set; }

        public string Position { get; set; }

        public EmployeeStatus Status { get; set; } = EmployeeStatus.Active;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
