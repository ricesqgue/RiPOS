using RiPOS.Shared.Utilities.ValidationAttributes;
using System.ComponentModel.DataAnnotations;

namespace RiPOS.Shared.Models.Requests
{
    public class UserRequest
    {
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "El nombre es requerido")]
        [MaxLength(50, ErrorMessage = "El nombre debe ser de máximo de {1} caracteres")]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "El apellido es requerido")]
        [MaxLength(50, ErrorMessage = "El apellido debe ser de máximo de {1} caracteres")]
        public string Surname { get; set; }

        [MaxLength(50, ErrorMessage = "El apellido debe ser de máximo de {1} caracteres")]
        public string SecondSurname { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "El nombre de usuario es requerido")]
        [MaxLength(50, ErrorMessage = "El nombre de usuario debe ser de máximo de {1} caracteres")]
        public string Username { get; set; }

        [Email(AllowEmpty = true, ErrorMessage = "El correo electrónico no es válido")]
        [MaxLength(100, ErrorMessage = "El nombre debe ser de máximo de {1} caracteres")]
        public string Email { get; set; }

        [MaxLength(25, ErrorMessage = "El número de teléfono debe ser de máximo de {1} caracteres")]
        public string PhoneNumber { get; set; }

        [MaxLength(25, ErrorMessage = "El número de celular debe ser de máximo de {1} caracteres")]
        public string MobilePhone { get; set; }

        [RolesInEnum]
        public List<int> RoleIds { get; set; }
    }
}
