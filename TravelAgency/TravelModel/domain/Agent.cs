namespace TravelModel.domain;
public class Agent : Entity<long>
{
    public string Name { get; set; }
    public string Password { get; set; }

    public Agent(string name, string password)
    {
        Name = name;
        Password = password;
    }

    public override string ToString()
    {
        return "Name: " + Name + " Password: " + Password;
    }
}