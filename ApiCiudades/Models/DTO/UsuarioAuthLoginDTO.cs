using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ApiCiudades.Models.DTO
{
	public class UsuarioAuthLoginDTO
	{
		[Required(ErrorMessage = "El usuario es obligatorio")]
		public string Usuario { get; set; }

		[Required(ErrorMessage = "El password es obligatorio")]
		public string Password { get; set; }
	}
}
