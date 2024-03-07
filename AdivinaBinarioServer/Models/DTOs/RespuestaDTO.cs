using AdivinaBinarioCliente.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdivinaBinarioServer.Models.DTOs
{
    public class RespuestaDTO
    {
        public int Respuesta { get; set; }
        public string NombreJugador { get; set; }
        public string IPJugador { get; set; }


    }
}
