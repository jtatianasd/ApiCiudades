using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ApiCiudades.Models.DTO
{
	public class CiudadDTO
	{
		public int Id { get; set; }
		[Required(ErrorMessage = "El nombre es obligaotrio")]
		public string Nombre { get; set; }
	}
}
