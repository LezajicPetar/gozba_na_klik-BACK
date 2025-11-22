using gozba_na_klik.Model;
using gozba_na_klik.Repository;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.WebUtilities;


namespace gozba_na_klik.Service
{
    public class UserTokenService
    {
        private readonly UserTokenRepository _repo;

        public UserTokenService(UserTokenRepository repo)
        {
            _repo = repo;
        }

        public string GenerateRawToken()
        {
            return Guid.NewGuid().ToString("N");
        }
    }
}
