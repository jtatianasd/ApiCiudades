using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiCiudades.Models
{
	public class Departamento
	{
		[Key]
		public int Id { get; set; }
		public string Nombre { get; set; }

		//Llave foranea con la tabla Ciudad
		public int ciudadId { get; set; }
		[ForeignKey("ciudadId")]
		public Ciudad Ciudad { get; set; }
	}
}
