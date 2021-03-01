using ApiCiudades.Data;
using ApiCiudades.Models;
using ApiCiudades.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiCiudades.Repository
{
	public class CiudadRepository : ICiudadRepository
	{
		private readonly ApplicationDbContext _bd;
		public CiudadRepository(ApplicationDbContext bd)
		{
			_bd = bd;
		}
		public bool ActualizarCiudad(Ciudad ciudad)
		{
			_bd.Ciudad.Update(ciudad);
			return Guardar();
		}

		public bool BorrarCiudad(Ciudad ciudad)
		{
			_bd.Ciudad.Remove(ciudad);
			return Guardar();
		}

		public bool CrearCiudad(Ciudad ciudad)
		{
			_bd.Ciudad.Add(ciudad);
			return Guardar();
		}

		public bool ExisteCiudad(string nombre)
		{
			bool valor = _bd.Ciudad.Any(c => c.Nombre.ToLower().Trim() == nombre.ToLower().Trim());
			return valor;
		}

		public bool ExisteCiudad(int id)
		{
			return _bd.Ciudad.Any(c => c.Id == id);
		}

		public Ciudad GetCiudad(int CiudadId)
		{
			return _bd.Ciudad.FirstOrDefault(c => c.Id == CiudadId);
		}

		public ICollection<Ciudad> GetCiudades()
		{
			return _bd.Ciudad.OrderBy(c => c.Nombre).ToList();
		}

		public bool Guardar()
		{
			return _bd.SaveChanges() >= 0 ? true : false;
		}
	}
}
