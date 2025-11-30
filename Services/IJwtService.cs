using washit.models;

namespace washit.services
{
    public interface IJwtService
    {
        string GenerateToken(User user);
    }
}