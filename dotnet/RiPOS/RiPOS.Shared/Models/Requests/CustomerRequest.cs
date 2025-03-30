using RiPOS.Shared.Utilities.ValidationAttributes;
using System.ComponentModel.DataAnnotations;

namespace RiPOS.Shared.Models.Requests
{
    public class CustomerRequest
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "El nombre del cliente es requerido")]
        [MaxLength(50, ErrorMessage = "El nombre debe ser de máximo de {1} caracteres")]
        public required string Name { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "El apellido del cliente es requerido")]
        [MaxLength(50, ErrorMessage = "El apellido debe ser de máximo de {1} caracteres")]
        public required string Surname { get; set; }

        [MaxLength(50, ErrorMessage = "El apellido debe ser de máximo de {1} caracteres")]
        public string? SecondSurname { get; set; }

        [MaxLength(20, ErrorMessage = "El número de teléfono debe ser de máximo de {1} caracteres")]
        public string? PhoneNumber { get; set; }

        [MaxLength(20, ErrorMessage = "El número de celular debe ser de máximo de {1} caracteres")]
        public string? MobilePhone { get; set; }

        [MaxLength(100, ErrorMessage = "El correo electrónico debe ser de máximo de {1} caracteres")]
        [Email(AllowEmpty = true, ErrorMessage = "El correo electrónico no es válido")]
        public string? Email { get; set; }

        [MaxLength(400, ErrorMessage = "La dirección debe ser de máximo de {1} caracteres")]
        public string? Address { get; set; }

        [MaxLength(100, ErrorMessage = "La ciudad debe ser de máximo de {1} caracteres")]
        public string? City { get; set; }

        [MaxLength(10, ErrorMessage = "El código postal debe ser de máximo de {1} caracteres")]
        public string? ZipCode { get; set; }

        [MaxLength(15, ErrorMessage = "El RFC debe ser de máximo de {1} caracteres")]
        [Rfc(AllowEmpty = true, ErrorMessage = "El RFC no es válido")]
        public string? Rfc { get; set; }

        [IntegerGreaterThanZero(ErrorMessage = "El estado es requerido")]
        public int CountryStateId { get; set; }
    }
}
