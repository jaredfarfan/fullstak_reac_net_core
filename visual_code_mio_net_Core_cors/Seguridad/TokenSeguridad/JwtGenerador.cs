using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Aplicacion.Contratos;
using Dominio;
using Microsoft.IdentityModel.Tokens;

namespace Seguridad
{
    public class JwtGenerador : IJwtGenerador
    {
        public string CrearToken(Usuario usuario,List<string> roles)
        {
            //claims= es la data del usuario que quiero compartir con los clientes
             var claims = new List<Claim>{
                new Claim(JwtRegisteredClaimNames.NameId, usuario.UserName)
            };

            if(roles!=null){
                foreach (var rol in roles)
                {
                    //cuando se crea un cleam se define que tipo de cleams es, en este caso rol
                    claims.Add(new Claim(ClaimTypes.Role,rol));
                }
            }

            //paabra secreta que va sesencriptar a futuro el token
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Mi palabra secreta"));
            var credenciales = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescripcion = new SecurityTokenDescriptor {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(30),
                SigningCredentials = credenciales
            };
            //escribir el token
            var tokenManejador = new JwtSecurityTokenHandler();
            var token = tokenManejador.CreateToken(tokenDescripcion);
            //devolver el string
            return tokenManejador.WriteToken(token);
        }
    }
}