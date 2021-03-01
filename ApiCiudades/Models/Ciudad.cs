using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace ApiCiudades.Models
{
	public class Ciudad
	{
		[Key]
		public int Id { get; set; }
		public string Nombre { get; set; }

	}
}
