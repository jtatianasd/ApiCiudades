using ApiCiudades.Models;
using ApiCiudades.Models.DTO;
using ApiCiudades.Repository.IRepository;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ApiCiudades.Controllers
{
	[Route("api/Usuarios")]
	[ApiController]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public class UsuariosController : Controller
	{
		private readonly IUsuarioRepository _userRepo;
		private readonly IMapper _mapper;
		private readonly IConfiguration _config;
		public UsuariosController(IUsuarioRepository userRepo, IMapper mapper, IConfiguration config)
		{
			_userRepo = userRepo;
			_mapper = mapper;
			_config = config;
		}

		/// <summary>
		/// Obtener todos los usuarios
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		[ProducesResponseType(200, Type = typeof(List<UsuarioDTO>))]
		[ProducesResponseType(400)]
		public IActionResult GetUsuarios()
		{
			var listaUsuarios = _userRepo.GetUsuarios();
			var listaUsuariosDTO = new List<UsuarioDTO>();
			foreach (var lista in listaUsuarios)
			{
				listaUsuariosDTO.Add(_mapper.Map<UsuarioDTO>(lista));
			}
			return Ok(listaUsuariosDTO);
		}

		/// <summary>
		///Obtener un usuario especifico
		/// </summary>
		/// <param name="UsuarioId"> </param>
		/// <returns></returns>
		[HttpGet("{UsuarioId:int}", Name = "GetUsuario")]
		[ProducesResponseType(200, Type = typeof(UsuarioDTO))]
		[ProducesResponseType(404)]
		[ProducesDefaultResponseType]
		public IActionResult GetUsuario(int UsuarioId)
		{
			var itemUsuario = _userRepo.GetUsuario(UsuarioId);
			if (itemUsuario == null)
			{
				return NotFound();
			}
			else
			{
				var itemUsuarioDTO = _mapper.Map<UsuarioDTO>(itemUsuario);
				return Ok(itemUsuarioDTO);
			}
		}
		/// <summary>
		/// Registrar un nuevo usuario
		/// </summary>
		/// <param name="usuarioAuthDTO"></param>
		/// <returns></returns>
		[HttpPost("Registro")]
		[ProducesResponseType(201, Type = typeof(UsuarioAuthDTO))]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public IActionResult Registro(UsuarioAuthDTO usuarioAuthDTO)
		{
			usuarioAuthDTO.Usuario = usuarioAuthDTO.Usuario.ToLower();
			if (_userRepo.ExisteUsuario(usuarioAuthDTO.Usuario))
			{
				return BadRequest("El usuario ya existe");
			}
			var usuarioACrear = new Usuario
			{
				UsuarioA = usuarioAuthDTO.Usuario
			};

			var usuarioCreado = _userRepo.Registro(usuarioACrear, usuarioAuthDTO.Password);
			return Ok(usuarioCreado);
		}

		/// <summary>
		/// Login de un usuario
		/// </summary>
		/// <param name="usuarioAuthLoginDTO"></param>
		/// <returns></returns>
		[HttpPost("Login")]
		[ProducesResponseType(201, Type = typeof(UsuarioAuthLoginDTO))]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public IActionResult Login(UsuarioAuthLoginDTO usuarioAuthLoginDTO)
		{
			var usuarioDesdeRepo = _userRepo.Login(usuarioAuthLoginDTO.Usuario, usuarioAuthLoginDTO.Password);
			if (usuarioDesdeRepo == null)
			{
				return Unauthorized();
			}
			var claims = new[]
			{
				 new Claim(ClaimTypes.NameIdentifier,usuarioDesdeRepo.Id.ToString()),
				new Claim(ClaimTypes.Name, usuarioDesdeRepo.UsuarioA.ToString())
			};

			//Generacion de token
			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));
			var credenciales = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(claims),
				Expires = DateTime.Now.AddDays(1),
				SigningCredentials = credenciales
			};

			var tokenHandler = new JwtSecurityTokenHandler();
			var token = tokenHandler.CreateToken(tokenDescriptor);
			return Ok(new
			{
				token = tokenHandler.WriteToken(token)
			});
		}
	}
}
