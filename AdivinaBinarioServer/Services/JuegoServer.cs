﻿using AdivinaBinarioServer.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace AdivinaBinarioServer.Services
{
    public class JuegoServer
    {
        public event EventHandler<RespuestaDTO> OnRespuestaRecibida;

        public JuegoServer()
        {
            var hilo = new Thread(Iniciar);
            hilo.IsBackground = true;
            hilo.Start();
        }


        void Iniciar()
        {
            UdpClient UDPServer = new UdpClient(5000);
            IPEndPoint IpRemota = new IPEndPoint(IPAddress.Any, 5000);
            byte[] buffer = UDPServer.Receive(ref IpRemota);

            RespuestaDTO respuesta = JsonSerializer.Deserialize<RespuestaDTO>(Encoding.UTF8.GetString(buffer));

            if(respuesta != null)
            {
                // Aquí se dispara el evento pero deberia ser en el hilo principal22
                OnRespuestaRecibida?.Invoke(this, respuesta);
            }
            

        }

    }
}