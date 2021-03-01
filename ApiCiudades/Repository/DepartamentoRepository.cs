using ApiCiudades.Repository.IRepository;
using ApiCiudades.Data;
using ApiCiudades.Models;
using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiCiudades.Repository
{
	public class DepartamentoRepository : IDepartamentoRepository
	{
		private readonly ApplicationDbContext _bd;
		public DepartamentoRepository(ApplicationDbContext bd)
		{
			_bd = bd;
		}

		public bool ActualizarDepartamento(Departamento departamento)
		{
			_bd.Departamento.Update(departamento);
			return Guardar();
		}

		public bool BorrarDepartamento(Departamento departamento)
		{
			_bd.Departamento.Remove(departamento);
			return Guardar();
		}

		public IEnumerable<Departamento> BuscarDepartamento(string nombre)
		{
			IQueryable<Departamento> query = _bd.Departamento;
			if (!string.IsNullOrEmpty(nombre))
			{
				query = query.Where(e => e.Nombre.Contains(nombre));
			}
			return query.ToList();
		}

		public bool CrearDepartamento(Departamento departamento)
		{
			_bd.Departamento.Add(departamento);
			return Guardar();
		}

		public bool ExisteDepartamento(string nombre)
		{
			bool valor = _bd.Departamento.Any(c => c.Nombre.ToLower().Trim() == nombre.ToLower().Trim());
			return valor;
		}

		public bool ExisteDepartamento(int id)
		{
			return _bd.Departamento.Any(c => c.Id == id);
		}

		public ICollection<Departamento> GetCiudadesEnDepartamentos(int CiuId)
		{
			return _bd.Departamento.Include(ci => ci.Ciudad).Where(ci => ci.ciudadId == CiuId).ToList();
		}

		public ICollection<Departamento> GetDepartamentos()
		{
			return _bd.Departamento.OrderBy(c => c.Nombre).ToList();
		}

		public Departamento GetDepartamento(int departamentoId)
		{
			return _bd.Departamento.FirstOrDefault(c => c.Id == departamentoId);
		}

		public bool Guardar()
		{
			return _bd.SaveChanges() >= 0 ? true : false;
		}
	}
}
