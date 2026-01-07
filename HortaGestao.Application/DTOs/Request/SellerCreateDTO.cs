using System.ComponentModel.DataAnnotations;
using HortaGestao.Application.Interfaces.Services;

namespace HortaGestao.Application.DTOs.Request;

public class SellerCreateDto:ICreateUserDto
{
    [Required(ErrorMessage = "Email é obrigatório")]
    [EmailAddress(ErrorMessage = "Email inválido")]
    public string Email { get; set; }
    
    [Required(ErrorMessage = "Senha é obrigatória")]
    [StringLength(100, MinimumLength = 8, ErrorMessage = "Senha deve conter no minimo 8  caracteres")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$", 
        ErrorMessage = "Senha deve conter: letra maiúscula, letra minúscula, número e caractere especial")] 
    public string Password { get; set; }
    public string Name { get; set; }

    public string PhoneNumber { get; set; }
}