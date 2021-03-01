using ApiCiudades.Models;
using ApiCiudades.Models.DTO;
using AutoMapper;

namespace ApiCiudades.CiudadesMapper
{
	public class CiudadesMapper : Profile
	{
		public CiudadesMapper()
		{
			CreateMap<Ciudad, CiudadDTO>().ReverseMap();
			CreateMap<Departamento, DepartamentoDTO>().ReverseMap();
			CreateMap<Departamento, DepartamentoCreateDTO>().ReverseMap();
			CreateMap<Departamento, DepartamentoUpdateDTO>().ReverseMap();
			CreateMap<Usuario, UsuarioDTO>().ReverseMap();
			CreateMap<Usuario, UsuarioAuthDTO>().ReverseMap();
			CreateMap<Usuario, UsuarioAuthLoginDTO>().ReverseMap();
		}
	}
}
