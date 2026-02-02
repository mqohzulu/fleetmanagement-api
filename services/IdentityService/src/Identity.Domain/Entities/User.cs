using System;

namespace Identity.Domain.Entities
{
    public class User
    {
        public Guid Id { get; private set; }
        public string Email { get; private set; } = null!;
        public string Name { get; private set; } = null!;
        public string PasswordHash { get; private set; } = null!;
        public DateTime CreatedAt { get; private set; }

        private User() { }

        public User(string email, string name, string passwordHash)
        {
            Id = Guid.NewGuid();
            Email = email;
            Name = name;
            PasswordHash = passwordHash;
            CreatedAt = DateTime.UtcNow;
        }

        public void SetPasswordHash(string hash) => PasswordHash = hash;
        public void SetName(string name) => Name = name;
    }
}
