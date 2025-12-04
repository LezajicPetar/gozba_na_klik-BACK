namespace gozba_na_klik.Dtos.Employee
{
    public class EmployeeDto
    {
        public int Id { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string Email { get; set; }
        public string Position { get; set; }

        public string Status { get; set; }  // "AKTIVAN" / "SUSPENDOVAN"
    }
}
