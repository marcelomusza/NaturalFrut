using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using NaturalFrut.Models;

namespace NaturalFrut.App_BLL.ViewModels
{
    public class ClienteViewModel
    {

        public int? ID { get; set; }

        [Required]
        public string Nombre { get; set; }

        public ClienteViewModel()
        {
            ID = 0;
        }

        public ClienteViewModel(Cliente cliente)
        {
            ID = cliente.ID;
            Nombre = cliente.Nombre;
        }
        
    }
}