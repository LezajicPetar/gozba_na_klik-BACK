namespace gozba_na_klik.Model.Entities
{
    public class Allergen
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<UserAllergen> UserAllergens { get; set; } = new List<UserAllergen>();
    }
}
