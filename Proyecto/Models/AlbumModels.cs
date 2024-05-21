using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto.Models
{
   public class AlbumModels
    {
        public string Nombre { get; set; } = "";
        public string NombreArtista { get; set; } = "";
        public string URLImagen { get; set; } = "";
        public int AñoLanzamiento { get; set; }
        public int TiotalCanciones {  get; set; }
        public string CancionPopular { get; set; } = "";
        public string URLVideo { get; set; } = "";
    }
}
