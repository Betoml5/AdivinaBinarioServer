using AdivinaBinarioServer.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace AdivinaBinarioServer.Services
{
    public class JuegoServer
    {
        public event EventHandler<RespuestaDTO> OnRespuestaRecibida;
        private UdpClient UDPServer  = new UdpClient(5000);
        public JuegoServer()
        {
            var hilo = new Thread(Iniciar);
            hilo.IsBackground = true;
            hilo.Start();
        }

        public void MandarFelicitacion(string ip)
        {
            var remoto = new IPEndPoint(IPAddress.Parse(ip), 5001);
            var json = JsonSerializer.Serialize("¡Felicidades acertaste!");
            var bytes = Encoding.UTF8.GetBytes(json);
                
            //server
            UDPServer.Send(bytes, bytes.Length, remoto);
           
        }



        void Iniciar()
        {
            while (true)
            {


                IPEndPoint IpRemota = new IPEndPoint(IPAddress.Any, 5000);
                byte[] buffer = UDPServer.Receive(ref IpRemota);

                RespuestaDTO respuesta = JsonSerializer.Deserialize<RespuestaDTO>(Encoding.UTF8.GetString(buffer));

                if (respuesta != null)
                {
                    // Aquí se dispara el evento pero deberia ser en el hilo principal22
                    Application.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        OnRespuestaRecibida?.Invoke(this, respuesta);

                    }));
                }

            }
        }

    }
}
