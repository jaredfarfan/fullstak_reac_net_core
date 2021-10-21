using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aplicacion.Contratos;
using Aplicacion.ManejadorError;
using Dominio;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistencia;

namespace Aplicacion.Seguridad
{
    public class Login
    {
        //esta es la logica de negocioas, lo que el web api va llamar primero
        public class Ejecuta : IRequest<UsuarioData>
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }
        //hereda desde AbstractValidator, es una libreria
        public class EjecutaValidacion : AbstractValidator<Ejecuta>
        {
            public EjecutaValidacion()
            {
                //reglas de validacion
                RuleFor(x => x.Email).NotEmpty();
                RuleFor(x => x.Password).NotEmpty();
            }
        }

        public class Manejador : IRequestHandler<Ejecuta, UsuarioData>
        {
            //provienen desde el core identity, estos dos tienes que importar
            private readonly UserManager<Usuario> _userManager;
            private readonly SignInManager<Usuario> _signInManager;
            //implementacion de la interface
            private readonly IJwtGenerador _jwtGenerador;
            private readonly CursosOnlineContext _context;
            public Manejador(UserManager<Usuario> userManager, SignInManager<Usuario> signInManager, IJwtGenerador jwtGenerador, CursosOnlineContext context )
            {
                _userManager = userManager;
                _signInManager = signInManager;
                _jwtGenerador = jwtGenerador;
                _context = context;
            }
            public async Task<UsuarioData> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                //que el email exista dentro de core identity
                 var usuario = await _userManager.FindByEmailAsync(request.Email);
                if (usuario == null)
                {
                    //clase especial para manejar excepciones
                    throw new ManejadorExcepcion(HttpStatusCode.Unauthorized);
                }
                //para hacer el login
                var resultado = await _signInManager.CheckPasswordSignInAsync(usuario, request.Password, false);
                //sacar la lista de roles para pasar como parametro
                //arreglo estring de los roles, pasarlo a un list
                var resultadoRoles = await _userManager.GetRolesAsync(usuario);
                var listaRoles = new List<string>(resultadoRoles);
                
                
                
                var imagenPerfil = await _context.Documento.Where(x => x.ObjetoReferencia == new Guid(usuario.Id)).FirstOrDefaultAsync();


                if (resultado.Succeeded){

                    #region CHECAR SI HAY IMAGEN

                    if (imagenPerfil != null)
                    {
                        var imagenCliente = new ImagenGeneral
                        {
                            Data = Convert.ToBase64String(imagenPerfil.Contenido),
                            Extension = imagenPerfil.Extension,
                            Nombre = imagenPerfil.Nombre
                        };
                        return new UsuarioData
                        {
                            NombreCompleto = usuario.NombreCompleto,
                            Token = _jwtGenerador.CrearToken(usuario, listaRoles),
                            Username = usuario.UserName,
                            Email = usuario.Email,
                            ImagenPerfil = imagenCliente
                        };
                    }
                    else
                    {
                        //return usuario;
                        return new UsuarioData
                        {
                            NombreCompleto = usuario.NombreCompleto,
                            Token = _jwtGenerador.CrearToken(usuario, listaRoles),
                            Username = usuario.UserName,
                            Email = usuario.Email,
                            Imagen = null
                        };
                    }

                    #endregion

                    
                }

                throw new ManejadorExcepcion(HttpStatusCode.Unauthorized);

            }
        }
    }
}