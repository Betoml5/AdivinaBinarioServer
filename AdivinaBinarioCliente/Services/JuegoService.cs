using AdivinaBinarioCliente.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AdivinaBinarioCliente.Services
{
    public  class JuegoService
    {

        private UdpClient cliente = new UdpClient();

        public string Servidor { get; set; } = "0.0.0.0";

        public void EnviarRespuesta(RespuestaDTO respuesta)
        {
            var ipEndPoint = new IPEndPoint(IPAddress.Parse(Servidor), 5000);
            var json = JsonSerializer.Serialize(respuesta);
            byte[] bytes = Encoding.UTF8.GetBytes(json);
            cliente.Send(bytes, bytes.Length, ipEndPoint);
        }

    }
}
