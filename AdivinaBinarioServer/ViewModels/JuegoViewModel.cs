using AdivinaBinarioCliente.Models.DTOs;
using AdivinaBinarioServer.Models.DTOs;
using AdivinaBinarioServer.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;

namespace AdivinaBinarioServer.ViewModels 
{ 
    public class JuegoViewModel : INotifyPropertyChanged
    {
        JuegoServer servidor = new JuegoServer();
        public ObservableCollection<JugadorDTO> JugadoresConRespuestaCorrecta { get; set; } = new ObservableCollection<JugadorDTO>();
        public string NumeroBinario { get; set; }
        public string Ip { get; set; } = "0.0.0.0";

        //private System.Timers.Timer OcultarNumeroTimer = new System.Timers.Timer(10000);
        //private System.Timers.Timer ReiniciarJuegoTimer = new System.Timers.Timer(35000);
        private System.Timers.Timer EnviarFelicitacionesTimer = new System.Timers.Timer(25000);





        private void IniciarTimers()
        {
            //OcultarNumeroTimer.Elapsed += (sender, e) =>
            //{
            //    OcultarNumero();
            //};

            //ReiniciarJuegoTimer.Elapsed += (sender, e) =>
            //{
            //    ReiniciarJuego();
            //    ReiniciarJuegoTimer.Stop();

            //};

            EnviarFelicitacionesTimer.Elapsed += (sender, e) =>
            {
                EnviarFelicitaciones();
                EnviarFelicitacionesTimer.Stop();
            };




            EnviarFelicitacionesTimer.AutoReset = true;
            EnviarFelicitacionesTimer.Enabled = true;

            //OcultarNumeroTimer.AutoReset = true;
            //OcultarNumeroTimer.Enabled = true;
            //ReiniciarJuegoTimer.AutoReset = true;
            //ReiniciarJuegoTimer.Enabled = true;

        }




        public JuegoViewModel()
        {
            NumeroBinario = GenerarNumeroBinarioRandom();
            var ips = Dns.GetHostAddresses(Dns.GetHostName());
            Ip = ips.FirstOrDefault(ip => ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).ToString();
            servidor.OnRespuestaRecibida += Servidor_OnRespuestaRecibida;
            IniciarTimers();
        }


        private void ReiniciarJuego()
        {
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                JugadoresConRespuestaCorrecta.Clear();
                NumeroBinario = GenerarNumeroBinarioRandom();
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("NumeroBinario"));
            }));
        }

        void OcultarNumero()
        {
            NumeroBinario = "";
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("NumeroBinario"));
        }

        void EnviarFelicitaciones()
        {
            foreach (var jugador in JugadoresConRespuestaCorrecta)
            {
                servidor.MandarFelicitacion(jugador.IP);
            }

            ReiniciarJuego();
            

        }

        

        private bool ValidarNumero(int numeroDecimal, string numeroBinario)
        {
            // Convierte el número decimal a binario y luego compara con el número binario dado
            //string binario = Convert.ToString(numeroDecimal, 2);
            int valorDecimal = 0;
            int expo = 0;

            for (int i = numeroBinario.Length - 1; i >= 0; i--)
            {
                int digit = numeroBinario[i] - '0'; // Convertir el carácter en un valor entero

                valorDecimal += digit * (int)Math.Pow(2, expo);
                expo++;
            }
            return valorDecimal == numeroDecimal;
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
                JugadoresConRespuestaCorrecta.Add(new JugadorDTO() { 
                    Nombre = e.NombreJugador,
                    IP = e.IPJugador
                });
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

    }
}
