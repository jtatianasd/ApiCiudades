using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ApiCiudades.Models.DTO
{
	public class DepartamentoUpdateDTO
	{
		public int Id { get; set; }
		[Required(ErrorMessage = "El nombre es obligatorio")]
		public string Nombre { get; set; }

		//Llave foranea con la tabla Ciudad
		public int ciudadId { get; set; }
		public Ciudad Ciudad { get; set; }
	}
}
