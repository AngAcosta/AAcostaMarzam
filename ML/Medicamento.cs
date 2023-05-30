using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML
{
    public class Medicamento
    {
        public int IdMedicamento { get; set; }

        public string Nombre { get; set; }

        public string Descripcion { get; set; }

        public string Precio { get; set; }

        public string Imagen { get; set; }

        public int Cantidad { get; set; }   

        public decimal Subtotal { get; set; }

        public decimal Total { get; set; }

        public int TotalProductos { get; set; }

        public List<object> Medicamentos { get; set; }

        public List<object> Productos { get; set; }
    }
}