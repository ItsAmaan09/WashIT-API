using Dapper;
using washit.models;
using washit.utility;

namespace washit.repository
{

    public class UserRepository : IUserRepository
    {
        private readonly DB _db;

        public UserRepository(DB db)
        {
            _db = db;
        }
        public async Task<User?> GetUserAsync(string userName, string password)
        {
            using var conn = _db.CreateConnection();

            string sql = @"
            SELECT Id, UserName, PasswordHash, PasswordSalt
            FROM Users
            WHERE UserName = @UserName
            ";

            var userRecord = await conn.QueryFirstOrDefaultAsync<UserDO>(sql, new { UserName = userName });

            if (userRecord == null) return null;

            bool isValid = CryptoHelper.VerifyPassword(password, userRecord.PasswordHash, userRecord.PasswordSalt);
            if (!isValid) return null;
            return new User
            {
                Id = userRecord.Id,
                UserName = userRecord.UserName
            };
        }

        public async Task<User?> GetUserByNameAsync(string userName)
        {
            using var conn = _db.CreateConnection();

            string sql = @"
        SELECT Id, UserName
        FROM Users
        WHERE UserName = @UserName
    ";

            return await conn.QueryFirstOrDefaultAsync<User>(sql, new { UserName = userName });
        }


        public async Task<int> RegisterUserAsync(string userName, string password)
        {
            using var conn = _db.CreateConnection();

            // generate hash + salt
            CryptoHelper.CreatePasswordHash(password, out byte[] hash, out byte[] salt);

            string sql = @"
            INSERT INTO Users (UserName, PasswordHash, PasswordSalt)
            VALUES (@UserName, @PasswordHash, @PasswordSalt);

            SELECT SCOPE_IDENTITY();
        ";

            return await conn.ExecuteScalarAsync<int>(sql, new
            {
                UserName = userName,
                PasswordHash = hash,
                PasswordSalt = salt
            });
        }
    }
}