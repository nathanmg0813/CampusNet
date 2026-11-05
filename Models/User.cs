namespace CampusNet.Models
{
    public class User
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public UserRole Role { get; set; }

        public User(string id, string name, UserRole role)
        {
            Id = id;
            Name = name;
            Role = role;
        }

        public override string ToString()
        {
            return $"[{Id}] {Name} ({Role})";
        }
    }
}