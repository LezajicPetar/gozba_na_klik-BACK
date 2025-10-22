namespace gozba_na_klik.Model.Entities
{
    public class UserAllergen
    {
        public User User { get; set; }
        public int UserId { get; set; }

        public Allergen Allergen { get; set; }
        public int AllergenId { get; set; }
    }
}
