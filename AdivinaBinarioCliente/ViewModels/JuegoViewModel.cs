﻿using AdivinaBinarioCliente.Models.DTOs;
using AdivinaBinarioCliente.Services;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AdivinaBinarioCliente.ViewModels
{
    public class JuegoViewModel : INotifyPropertyChanged
    {
        JuegoService juegoService = new JuegoService();
        public RespuestaDTO Respuesta { get; set; } = new RespuestaDTO();
        private System.Timers.Timer DesabilitarBotonTimer = new System.Timers.Timer(50000);


        public bool BotonHabilitado { get; set; } = true;
        public string Felicitacion { get; set; } 
        public ICommand EnviarRespuestaCommand { get; set; }
        public string Ip { get; set; } = "0.0.0.0";

        public JuegoViewModel()
        {
            EnviarRespuestaCommand = new RelayCommand(EnviarRespuesta);
            juegoService.OnFelicitacionRecibida += JuegoService_OnFelicitacionRecibida;
            IniciarTimers();

        }

        private void JuegoService_OnFelicitacionRecibida(object sender, string e)
        {
            Felicitacion = e;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Felicitacion"));
        }

        private void IniciarTimers()
        {
            //DesabilitarBotonTimer.Elapsed += (sender, e) =>
            //{
            //    BotonHabilitado = false;
            //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("BotonHabilitado"));
                
            //};

            //DesabilitarBotonTimer.AutoReset = true;
            //DesabilitarBotonTimer.Enabled = true;

        }

        private void EnviarRespuesta()
        {
            juegoService.Servidor = Ip;
            Respuesta.NombreJugador = Environment.UserName;
            Respuesta.IPJugador = Dns.GetHostAddresses(Dns.GetHostName()).Where(x => x.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).FirstOrDefault().ToString();
            juegoService.EnviarRespuesta(Respuesta);
        }
        public event PropertyChangedEventHandler PropertyChanged;

    }
}
