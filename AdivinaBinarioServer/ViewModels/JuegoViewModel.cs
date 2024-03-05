using AdivinaBinarioServer.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AdivinaBinarioServer.ViewModels 
{ 
    public class JuegoViewModel : INotifyPropertyChanged
    {
        JuegoServer servidor = new JuegoServer();
        ObservableCollection<string> JugadoresConRespuestaCorrecta { get; set; } = new ObservableCollection<string>();
        public string NumeroBinario { get; set; }


        public string Ip { get; set; } = "0.0.0.0";


        public JuegoViewModel()
        {
            NumeroBinario = GenerarNumeroBinarioRandom();
            var ips = Dns.GetHostAddresses(Dns.GetHostName());
            Ip = ips.FirstOrDefault(ip => ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).ToString();
            servidor.OnRespuestaRecibida += Servidor_OnRespuestaRecibida    ;
        }

        private bool ValidarNumero(int numeroDecimal, string numeroBinario)
        {
            // Convierte el número decimal a binario y luego compara con el número binario dado
            string binario = Convert.ToString(numeroDecimal, 2);
            return binario == numeroBinario;
        }

        private string GenerarNumeroBinarioRandom()
        {
            Random rand = new Random();
            string numeroBinario = "";

            for (int i = 0; i < 5; i++)
            {
                // Genera un dígito binario aleatorio (0 o 1) y lo agrega al número binario
                int digito = rand.Next(2);
                numeroBinario += digito;
            }

            return numeroBinario;
        }

        private void Servidor_OnRespuestaRecibida(object sender, Models.DTOs.RespuestaDTO e)
        {
            var EsCorrecto = ValidarNumero(e.Respuesta, NumeroBinario);
            if (EsCorrecto)
            {
                JugadoresConRespuestaCorrecta.Add(e.Jugador);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

    }
}
