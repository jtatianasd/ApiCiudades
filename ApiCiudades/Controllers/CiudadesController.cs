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
	[Route("api/Ciudades")]
	[ApiController]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public class CiudadesController : Controller
	{
		private readonly ICiudadRepository _ciuRepo;
		private readonly IMapper _mapper;
		public CiudadesController(ICiudadRepository ciuRepo, IMapper mapper)
		{
			_ciuRepo = ciuRepo;
			_mapper = mapper;
		}
		/// <summary>
		/// Obtener todas las ciudades
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		[ProducesResponseType(200, Type = typeof(List<CiudadDTO>))]
		[ProducesResponseType(400)]
		public IActionResult GetCategorias()
		{
			var listaCategorias = _ciuRepo.GetCiudades();
			var listaCategoriasDTO = new List<CiudadDTO>();
			foreach (var lista in listaCategorias)
			{
				listaCategoriasDTO.Add(_mapper.Map<CiudadDTO>(lista));
			}
			return Ok(listaCategoriasDTO);
		}

		/// <summary>
		///Obtener una ciudad individual
		/// </summary>
		/// <param name="ciudadId"> </param>
		/// <returns></returns>
		[HttpGet("{ciudadId:int}", Name = "GetCiudad")]
		[ProducesResponseType(200, Type = typeof(CiudadDTO))]
		[ProducesResponseType(404)]
		[ProducesDefaultResponseType]
		public IActionResult GetCiudad(int ciudadId)
		{
			var itemCategoria = _ciuRepo.GetCiudad(ciudadId);
			if (itemCategoria == null)
			{
				return NotFound();
			}
			else
			{
				var itemCiudadDTO = _mapper.Map<CiudadDTO>(itemCategoria);
				return Ok(itemCiudadDTO);
			}

		}
		/// <summary>
		/// Crear una nueva ciudad
		/// </summary>
		/// <param name="ciudadDTO"></param>
		/// <returns></returns>
		[HttpPost]
		[ProducesResponseType(201, Type = typeof(CiudadDTO))]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public IActionResult CrearCiudad([FromBody] CiudadDTO ciudadDTO)
		{
			if (ciudadDTO == null)
			{
				return BadRequest(ModelState);
			}
			if (_ciuRepo.ExisteCiudad(ciudadDTO.Nombre))
			{
				ModelState.AddModelError("", "La ciudad ya existe");
				return StatusCode(404, ModelState);
			}

			var ciudad = _mapper.Map<Ciudad>(ciudadDTO);
			if (!_ciuRepo.CrearCiudad(ciudad))
			{
				ModelState.AddModelError("", $"Algo Salio mal guardando el registro{ciudad.Nombre}");
				return StatusCode(500, ModelState);
			}
			return CreatedAtRoute("GetCiudad", new { ciudadId = ciudad.Id }, ciudad);
		}

		/// <summary>
		/// Actualizar una ciudad existente
		/// </summary>
		/// <param name="ciudadId"></param>
		/// <param name="ciudadDTO"></param>
		/// <returns></returns>
		[HttpPatch("{ciudadId:int}", Name = "ActualizarCiudad")]
		[ProducesResponseType(204)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public IActionResult ActualizarCiudad(int ciudadId, [FromBody] CiudadDTO ciudadDTO)
		{
			if (ciudadDTO == null || ciudadId != ciudadDTO.Id)
			{
				return BadRequest(ModelState);
			}
			var ciudad = _mapper.Map<Ciudad>(ciudadDTO);
			if (!_ciuRepo.ActualizarCiudad(ciudad))
			{
				ModelState.AddModelError("", $"Algo Salio mal actualizando el registro{ciudad.Nombre}");
				return StatusCode(500, ModelState);
			}
			return NoContent();
		}

		/// <summary>
		/// Borrar una ciudad existente
		/// </summary>
		/// <param name="ciudadId"></param>
		/// <returns></returns>
		[HttpDelete("{ciudadId:int}", Name = "BorrarCiudad")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status409Conflict)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		[ProducesDefaultResponseType]
		public IActionResult BorrarCiudad(int ciudadId)
		{
			if (!_ciuRepo.ExisteCiudad(ciudadId))
			{
				return NotFound();
			}
			var ciudad = _ciuRepo.GetCiudad(ciudadId);
			if (!_ciuRepo.BorrarCiudad(ciudad))
			{
				ModelState.AddModelError("", $"Algo Salio mal borrando el registro{ciudad.Nombre}");
				return StatusCode(500, ModelState);
			}
			return NoContent();
		}
	}
}
