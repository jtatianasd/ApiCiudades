using ApiCiudades.Models;
using ApiCiudades.Models.DTO;
using ApiCiudades.Repository.IRepository;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiCiudades.Controllers
{

	[Route("api/Departamentos")]
	[ApiController]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public class DepartamentosController : Controller
	{
		private readonly IDepartamentoRepository _depRepo;
		private readonly IMapper _mapper;
		public DepartamentosController(IDepartamentoRepository depRepo,IMapper mapper)
		{
			_depRepo = depRepo;
			_mapper = mapper;
		}

		/// <summary>
		/// Obtener todos los departamentos
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		[ProducesResponseType(200, Type = typeof(List<DepartamentoDTO>))]
		[ProducesResponseType(400)]
		public IActionResult GetDepartamentos()
		{
			var listaDepartamentos = _depRepo.GetDepartamentos();
			var listaDepartamentosDTO = new List<DepartamentoDTO>();
			foreach (var lista in listaDepartamentos)
			{
				listaDepartamentosDTO.Add(_mapper.Map<DepartamentoDTO>(lista));
			}
			return Ok(listaDepartamentosDTO);
		}

		/// <summary>
		///Obtener un departamento especifico
		/// </summary>
		/// <param name="DepartamentoId"> </param>
		/// <returns></returns>
		[HttpGet("{DepartamentoId:int}", Name = "GetDepartamento")]
		[ProducesResponseType(200, Type = typeof(DepartamentoDTO))]
		[ProducesResponseType(404)]
		[ProducesDefaultResponseType]
		public IActionResult GetDepartamento(int DepartamentoId)
		{
			var itemDepartamento = _depRepo.GetDepartamento(DepartamentoId);
			if (itemDepartamento == null)
			{
				return NotFound();
			}
			else
			{
				var itemDepartamentoDTO = _mapper.Map<DepartamentoDTO>(itemDepartamento);
				return Ok(itemDepartamentoDTO);
			}

		}

		/// <summary>
		///Obtener un departamento especifico
		/// </summary>
		/// <param name="ciudadId"> </param>
		/// <returns></returns>
		[HttpGet("GetCiudadesEnDepartamentos/{ciudadId:int}")]
		[ProducesResponseType(200, Type = typeof(DepartamentoDTO))]
		[ProducesResponseType(404)]
		[ProducesDefaultResponseType]
		public IActionResult GetDepartamentosEnCategoria(int ciudadId)
		{
			var listaDepartamento = _depRepo.GetCiudadesEnDepartamentos(ciudadId);
			if (listaDepartamento == null || listaDepartamento.Count == 0)
			{
				return NotFound();
			}
			var itemDepartamento = new List<DepartamentoDTO>();
			foreach (var item in listaDepartamento)
			{
				itemDepartamento.Add(_mapper.Map<DepartamentoDTO>(item));
			}
			return Ok(itemDepartamento);
		}


		/// <summary>
		/// Obtener un departamento especifico por nombre 
		/// </summary>
		/// <param name="nombre"> </param>
		/// <returns></returns>
		[HttpGet("Buscar")]
		[ProducesResponseType(200, Type = typeof(DepartamentoDTO))]
		[ProducesResponseType(404)]
		[ProducesResponseType(500)]
		[ProducesDefaultResponseType]
		public IActionResult Buscar(string nombre)
		{
			try
			{
				var resultado = _depRepo.BuscarDepartamento(nombre);
				if (resultado.Any())
				{
					return Ok(resultado);
				}
				return NotFound();
			}
			catch (Exception)
			{

				return StatusCode(StatusCodes.Status500InternalServerError, "Error recuperando datos de la aplicacion");
			}
		}

		/// <summary>
		/// Registrar un nuevo departamento
		/// </summary>
		/// <param name="DepartamentoDTO"></param>
		/// <returns></returns>
		[HttpPost]
		[ProducesResponseType(201, Type = typeof(DepartamentoDTO))]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public IActionResult CrearDepartamento([FromForm] DepartamentoCreateDTO DepartamentoDTO)
		{
			if (DepartamentoDTO == null)
			{
				return BadRequest(ModelState);
			}
			if (_depRepo.ExisteDepartamento(DepartamentoDTO.Nombre))
			{
				ModelState.AddModelError("", "La Departamento ya existe");
				return StatusCode(404, ModelState);
			}

			var Departamento = _mapper.Map<Departamento>(DepartamentoDTO);

			if (!_depRepo.CrearDepartamento(Departamento))
			{
				ModelState.AddModelError("", $"Algo Salio mal guardando el registro{Departamento.Nombre}");
				return StatusCode(500, ModelState);
			}
			return CreatedAtRoute("GetDepartamento", new { DepartamentoId = Departamento.Id }, Departamento);
		}

		/// <summary>
		/// Actualizar un departamento existente
		/// </summary>
		/// <param name="DepartamentoId"></param>
		/// <param name="DepartamentoDTO"></param>
		/// <returns></returns>
		[HttpPatch("{DepartamentoId:int}", Name = "ActualizarDepartamento")]
		[ProducesResponseType(204)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public IActionResult ActualizarDepartamento(int DepartamentoId, [FromBody] DepartamentoDTO DepartamentoDTO)
		{
			if (DepartamentoDTO == null || DepartamentoId != DepartamentoDTO.Id)
			{
				return BadRequest(ModelState);
			}
			var Departamento = _mapper.Map<Departamento>(DepartamentoDTO);
			if (!_depRepo.ActualizarDepartamento(Departamento))
			{
				ModelState.AddModelError("", $"Algo Salio mal actualizando el registro{Departamento.Nombre}");
				return StatusCode(500, ModelState);
			}
			return NoContent();
		}

		/// <summary>
		/// Borrar un departamento existente
		/// </summary>
		/// <param name="DepartamentoId"></param>
		/// <returns></returns>
		[HttpDelete("{DepartamentoId:int}", Name = "BorrarDepartamento")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status409Conflict)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		[ProducesDefaultResponseType]
		public IActionResult BorrarDepartamento(int DepartamentoId)
		{
			if (!_depRepo.ExisteDepartamento(DepartamentoId))
			{
				return NotFound();
			}
			var Departamento = _depRepo.GetDepartamento(DepartamentoId);
			if (!_depRepo.BorrarDepartamento(Departamento))
			{
				ModelState.AddModelError("", $"Algo Salio mal borrando el registro{Departamento.Nombre}");
				return StatusCode(500, ModelState);
			}
			return NoContent();
		}
	}
}
