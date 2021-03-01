using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiCiudades.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiCiudades.Data
{
	public class ApplicationDbContext:DbContext
	{
		public ApplicationDbContext()
		{

		}
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
		{
		}
		public DbSet<Ciudad> Ciudad { get; set; }
		public DbSet<Departamento> Departamento { get; set; }
		public DbSet<Usuario> Usuario { get; set; }
	}
}
