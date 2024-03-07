using AdivinaBinarioCliente.Models.DTOs;
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

namespace AdivinaBinarioCliente.Services
{
    public  class JuegoService
    {

        private UdpClient cliente = new UdpClient(5001);
        public string Servidor { get; set; } = "0.0.0.0";

        public JuegoService()
        {
            var hilo = new Thread(Iniciar);
            hilo.IsBackground = true;
            hilo.Start();
        }

        void Iniciar()
        {
            IPEndPoint IpRemota = new IPEndPoint(IPAddress.Any, 5001);
            byte[] buffer = cliente.Receive(ref IpRemota);

            string respuesta = JsonSerializer.Deserialize<string>(Encoding.UTF8.GetString(buffer));

            if (respuesta != null)
            {
                // Aquí se dispara el evento pero deberia ser en el hilo principal22
                Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    OnFelicitacionRecibida?.Invoke(this, respuesta);

                }));
            }


        }



        public void EnviarRespuesta(RespuestaDTO respuesta)
        {
            //Enviar al SERVER
            var ipEndPoint = new IPEndPoint(IPAddress.Parse(Servidor), 5000);
            var json = JsonSerializer.Serialize(respuesta);
            byte[] bytes = Encoding.UTF8.GetBytes(json);
            cliente.Send(bytes, bytes.Length, ipEndPoint);
        }

        public event EventHandler<string> OnFelicitacionRecibida;
    }
}
