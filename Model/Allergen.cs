namespace gozba_na_klik.Model
{
    public class Allergen
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<UserAllergen> UserAllergens { get; set; } = new List<UserAllergen>();
    }
}
