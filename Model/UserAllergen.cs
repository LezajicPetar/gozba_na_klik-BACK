namespace gozba_na_klik.Model
{
    public class UserAllergen
    {
        public int Id { get; set; }

        public User User { get; set; }
        public int UserId { get; set; }

        public Allergen Allergen { get; set; }
        public int AllergenId { get; set; }
    }
}
