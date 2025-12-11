namespace DDDPractice.Application.Interfaces;

public interface ICreateUserDTO
{
    string Email { get; set; }
    string Password { get; set; }
    string Name { get; set; }
}