using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NaturalFrut.DTOs
{
    public class ProductoDTO
    {

        public int? ID { get; set; }

        [Required]
        public string Nombre { get; set; }

        [Required]
        public string Descripcion { get; set; }

        [Required]
        public int Cantidad { get; set; }

        public CategoriaDTO Categoria { get; set; }

        [Required]
        public int CategoriaId { get; set; }


        public MarcaDTO Marca { get; set; }

        [Required]
        public int MarcaId { get; set; }

        public bool EsBlister { get; set; }

        public bool EsMix { get; set; }

    }
}