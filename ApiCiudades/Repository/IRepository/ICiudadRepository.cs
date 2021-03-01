using ApiCiudades.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiCiudades.Repository.IRepository
{
	public interface ICiudadRepository
	{
		ICollection<Ciudad> GetCiudades();
		Ciudad GetCiudad(int CiudadId);
		bool ExisteCiudad(string nombre);
		bool ExisteCiudad(int id);
		bool CrearCiudad(Ciudad ciudad);
		bool ActualizarCiudad(Ciudad ciudad);
		bool BorrarCiudad(Ciudad ciudad);
		bool Guardar();
	}
}
