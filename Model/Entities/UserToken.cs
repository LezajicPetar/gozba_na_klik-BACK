namespace gozba_na_klik.Model.Entities
{
    public enum UserTokenType
    {
        Activation = 1,
        ResetPassword = 2
    }

    public class UserToken
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }

        public string TokenHash { get; set; }
        public UserTokenType Type { get; set; }

        public DateTime ExpiresAt { get; set; }
        public DateTime? UsedAt { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
