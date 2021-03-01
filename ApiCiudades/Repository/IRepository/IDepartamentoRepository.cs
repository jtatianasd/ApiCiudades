using ApiCiudades.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiCiudades.Repository.IRepository
{
	public interface IDepartamentoRepository
	{
		ICollection<Departamento> GetDepartamentos();
		ICollection<Departamento> GetCiudadesEnDepartamentos(int CiuId);
		Departamento GetDepartamento(int departamentoId);
		bool ExisteDepartamento(string nombre);
		IEnumerable<Departamento> BuscarDepartamento(string nombre);
		bool ExisteDepartamento(int id);
		bool CrearDepartamento(Departamento departamento);
		bool ActualizarDepartamento(Departamento departamento);
		bool BorrarDepartamento(Departamento departamento);
		bool Guardar();
	}
}
