using AdivinaBinarioCliente.Models.DTOs;
using AdivinaBinarioCliente.Services;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AdivinaBinarioCliente.ViewModels
{
    public class JuegoViewModel
    {
        public RespuestaDTO Respuesta { get; set; } = new RespuestaDTO();
        JuegoService juegoService = new JuegoService();

        public ICommand EnviarRespuestaCommand { get; set; }
        public string Ip { get; set; } = "0.0.0.0";

        public JuegoViewModel()
        {
            EnviarRespuestaCommand = new RelayCommand(EnviarRespuesta);
        }

        private void EnviarRespuesta()
        {
            juegoService.Servidor = Ip;
            juegoService.EnviarRespuesta(Respuesta);
        }
    }
}
