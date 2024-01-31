﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class Articulo
    {

        public int CodigoArticulo { get; set; }

        public string Nombre { get; set; }

        public string Descripcion { get; set; }

        public string UrlImagen { get; set; }

        public int Precio { get; set; }

        public Categoria Categoria { get; set; }

        public Marca Marca { get; set; }
    }
}
