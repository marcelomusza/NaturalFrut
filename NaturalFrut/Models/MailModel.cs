using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NaturalFrut.Models
{
    public class MailModel
    {
        [Display(Name = "Destinatario")]
        public string To { get; set; }

        [Display(Name = "Título")]
        public string Subject { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Detalles")]
        public string Body { get; set; }

    }
}