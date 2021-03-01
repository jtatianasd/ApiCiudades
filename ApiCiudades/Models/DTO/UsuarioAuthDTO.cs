using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ApiCiudades.Models.DTO
{
	public class UsuarioAuthDTO
	{
		[Key]
		public int Id { get; set; }

		[Required(ErrorMessage = "El usuario es obligatorio")]
		public string Usuario { get; set; }

		[Required(ErrorMessage = "El password es obligatorio")]
		[StringLength(10, MinimumLength = 4, ErrorMessage = "La contraseña debe estar entre 4 y 10 caracteres")]
		public string Password { get; set; }
	}
}
