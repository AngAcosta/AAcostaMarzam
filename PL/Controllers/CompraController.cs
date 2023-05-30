using Microsoft.AspNetCore.Mvc;

namespace PL.Controllers
{
    public class CompraController : Controller
    {
        public ActionResult GetAll(ML.Medicamento medicamento)
        {
            ML.Result result = new ML.Result();
            result = BL.Medicamento.GetAll();
            medicamento.Medicamentos = result.Objects;
            HttpContext.Session.GetString("Productos");
            return View(medicamento);
        }

        public ActionResult AddProducto(ML.Medicamento med)
        {
            med.Medicamentos = new List<object>();
            decimal suma = 0;
            if (HttpContext.Session.GetString("Productos") == null)
            {
                if (med.IdMedicamento == 0)
                {
                    return View(med);
                }
                else
                {
                    ML.Result result = BL.Medicamento.GetById(med.IdMedicamento);

                    ML.Medicamento medicamento = (ML.Medicamento)result.Object;
                    medicamento.Cantidad = 1;
                    medicamento.Subtotal = int.Parse(medicamento.Precio) * medicamento.Cantidad;
                    med.Total = medicamento.Subtotal;
                    med.TotalProductos = medicamento.Cantidad;
                    med.Medicamentos.Add(medicamento);

                    HttpContext.Session.SetString("Productos", Newtonsoft.Json.JsonConvert.SerializeObject(med.Medicamentos));

                    return View(med);
                }

            }
            else
            {
                int prod = 0;
                ML.Result result = BL.Medicamento.GetById(med.IdMedicamento);
                ML.Medicamento producto = (ML.Medicamento)result.Object;
                producto.Cantidad = 1;
                producto.Subtotal = int.Parse(producto.Precio) * producto.Cantidad;

                decimal sum = producto.Subtotal;

                bool existe = false;
                bool uno = false;

                List<object> medc = Newtonsoft.Json.JsonConvert.DeserializeObject<List<object>>(HttpContext.Session.GetString("Productos"));

                foreach (var obj in medc)
                {
                    ML.Medicamento objProducto = Newtonsoft.Json.JsonConvert.DeserializeObject<ML.Medicamento>(obj.ToString());
                    if (producto.IdMedicamento == objProducto.IdMedicamento)
                    {
                        objProducto.Cantidad++;
                        objProducto.Subtotal = int.Parse(objProducto.Precio) * objProducto.Cantidad;
                        suma += objProducto.Subtotal;
                        existe = true;
                    }
                    if (uno == false)
                    {
                        if (existe == true)
                        {
                            med.Medicamentos.Add(objProducto);
                            break;
                        }
                        if (existe == false)
                        {
                            med.Medicamentos.Add(producto);
                            uno = true;
                        }
                    }

                    med.Medicamentos.Add(objProducto);
                    prod += objProducto.Cantidad;
                }
                suma = suma + sum;
                med.Total = suma;
                med.TotalProductos = prod + producto.Cantidad;

                HttpContext.Session.SetString("Productos", Newtonsoft.Json.JsonConvert.SerializeObject(med.Medicamentos));
                HttpContext.Session.SetInt32("TotalProductos", med.TotalProductos);
                return View(med);
            }
        }

        public  ActionResult RestarProducto(ML.Medicamento medicamento)
        {
            medicamento.Medicamentos = new List<object>();

            int prod = 0;

            List<object> medc = Newtonsoft.Json.JsonConvert.DeserializeObject<List<object>>(HttpContext.Session.GetString("Productos"));

            foreach(var obj in medc)
            {
                ML.Medicamento objProducto = Newtonsoft.Json.JsonConvert.DeserializeObject<ML.Medicamento>(obj.ToString());

                if (medicamento.IdMedicamento == objProducto.IdMedicamento) 
                {
                    objProducto.Cantidad--;
                    objProducto.Subtotal = int.Parse(objProducto.Precio) * objProducto.Cantidad;
                    prod = 1;
                }
                medicamento.Medicamentos.Add(objProducto);
            }
            medicamento.TotalProductos -= prod;

            HttpContext.Session.SetString("Productos", Newtonsoft.Json.JsonConvert.SerializeObject(medicamento.Medicamentos));
            HttpContext.Session.SetInt32("TotalProductos", medicamento.TotalProductos);
            return View("AgregarProducto", medicamento);
        }

        public ActionResult SumarProducto(ML.Medicamento medicamento)
        {
            medicamento.Medicamentos = new List<object>();
            int prod = 0;

            List<object> medc = Newtonsoft.Json.JsonConvert.DeserializeObject<List<object>>(HttpContext.Session.GetString("Productos"));

            foreach (var obj in medc)
            {
                ML.Medicamento objProducto = Newtonsoft.Json.JsonConvert.DeserializeObject<ML.Medicamento>(obj.ToString());
                if (medicamento.IdMedicamento == objProducto.IdMedicamento)
                {
                    objProducto.Cantidad++;
                    objProducto.Subtotal = int.Parse(objProducto.Precio) * objProducto.Cantidad;
                    prod = 1;
                }
                medicamento.Medicamentos.Add(objProducto);
            }
            medicamento.TotalProductos += prod;

            HttpContext.Session.SetString("Productos", Newtonsoft.Json.JsonConvert.SerializeObject(medicamento.Medicamentos));
            HttpContext.Session.SetInt32("TotalProductos", medicamento.TotalProductos);
            return View("AgregarProducto", medicamento);
        }

        public ActionResult Delete(int IdProducto)
        {
            ML.Medicamento medicamento = new ML.Medicamento();
            medicamento.Medicamentos = new List<object>();

            List<object> medc = Newtonsoft.Json.JsonConvert.DeserializeObject<List<object>>(HttpContext.Session.GetString("Productos"));

            foreach (var obj in medc)
            {
                ML.Medicamento objProducto = Newtonsoft.Json.JsonConvert.DeserializeObject<ML.Medicamento>(obj.ToString());
                medicamento.Medicamentos.Add(objProducto);
            }

            medicamento.Medicamentos.RemoveAll(c => c.Equals(IdProducto));

            HttpContext.Session.SetString("Productos", Newtonsoft.Json.JsonConvert.SerializeObject(medicamento.Medicamentos));

            ViewBag.Message = "Se ha eliminado el producto.";

            return PartialView("Modal");
        }

        public JsonResult Comprar()
        {
            HttpContext.Session.Remove("Productos");
            bool correct = true;
            return Json(correct);
        }
    }
}