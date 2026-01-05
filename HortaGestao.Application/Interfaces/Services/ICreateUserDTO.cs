namespace HortaGestao.Application.Interfaces.Services;

public interface ICreateUserDto
{
    string Email { get; }
    string Password { get; }
    string Name { get; }

}