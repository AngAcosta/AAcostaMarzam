using Microsoft.AspNetCore.Mvc;

namespace PL.Controllers
{
    public class MedicamentoController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment;

        public MedicamentoController(IConfiguration configuration, Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment)
        {
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
        }

        public ActionResult GetAll()
        {
            ML.Medicamento medicamento = new ML.Medicamento();
            ML.Result result = new ML.Result();
            result.Objects = new List<object>();

            try
            {
                using (var client = new HttpClient())
                {
                    string urlApi = _configuration["urlApi"];
                    client.BaseAddress = new Uri(urlApi);

                    var responseTask = client.GetAsync("Medicamento/GetAll");
                    responseTask.Wait();

                    var resultServicio = responseTask.Result;

                    if (resultServicio.IsSuccessStatusCode)
                    {
                        var readTask = resultServicio.Content.ReadAsAsync<ML.Result>();
                        readTask.Wait();

                        foreach (var resultItem in readTask.Result.Objects)
                        {
                            ML.Medicamento resultItemList = Newtonsoft.Json.JsonConvert.DeserializeObject<ML.Medicamento>(resultItem.ToString());
                            result.Objects.Add(resultItemList);
                        }
                    }
                    medicamento.Medicamentos = result.Objects;
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Error al mostrar los registros";
                return PartialView("Modal");
            }
            return View(medicamento);
        }
        //    ML.Result result = BL.Medicamento.GetAll();
        //    if (result.Correct)
        //    {
        //        medicamento.Medicamentos = result.Objects;
        //        return View(medicamento);
        //    }
        //    else
        //    {
        //        return View(medicamento);
        //    }
        //}

        public ActionResult Form(int? IdMedicamento)
        {
            ML.Medicamento medicamento = new ML.Medicamento();

            ML.Result result = new ML.Result();
            result.Object = new object();

            if (IdMedicamento != null)
            {
                using (var client = new HttpClient())
                {
                    string urlApi = _configuration["urlApi"];
                    client.BaseAddress = new Uri(urlApi);

                    var responseTask = client.GetAsync("Medicamento/GetById" + IdMedicamento);
                    responseTask.Wait();

                    var resultServicio = responseTask.Result;

                    if (resultServicio.IsSuccessStatusCode)
                    {
                        var readTask = resultServicio.Content.ReadAsAsync<ML.Result>();
                        readTask.Wait();
                        var resultItem = readTask.Result.Object;

                        ML.Medicamento resultItemList = Newtonsoft.Json.JsonConvert.DeserializeObject<ML.Medicamento>(resultItem.ToString());
                        result.Object = resultItemList;

                        result.Correct = true;
                    }
                    medicamento = (ML.Medicamento)result.Object;

                    return View(medicamento);
                }
            }
            else
            {
                return View(medicamento);
            }
        }
        //ML.Medicamento medicamento = new ML.Medicamento();

        //if (IdMedicamento == null)
        //{
        //    return View(medicamento);
        //}
        //else
        //{
        //    ML.Result result = BL.Medicamento.GetById(IdMedicamento.Value);

        //    if (result.Correct)
        //    {
        //        medicamento = (ML.Medicamento)result.Object;
        //        return View(medicamento);
        //    }
        //    else
        //    {
        //        ViewBag.Message = "Ocurrio un error al consultar la informacion";
        //        return View("Modal");
        //    }
        //}


        [HttpPost]
        public ActionResult Form(ML.Medicamento medicamento)
        {
            //ML.Result result = new ML.Result();

            IFormFile file = Request.Form.Files["fuImage"];

            if (file != null)
            {
                byte[] imagen = ConvertToBytes(file);

                medicamento.Imagen = Convert.ToBase64String(imagen);
            }

            if (medicamento.IdMedicamento == 0)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_configuration["urlApi"]);

                    var postTask = client.PostAsJsonAsync<ML.Medicamento>("Medicamento/Add", medicamento);
                    postTask.Wait();

                    var result = postTask.Result;

                    if (result.IsSuccessStatusCode)
                    {
                        ViewBag.Message = "Se ha agregado el registro Correctamente";
                    }
                    else
                    {
                        ViewBag.Message = "Se ha agregado el registro Correctamente";
                    }
                    return PartialView("Modal");
                }
            }
            else
            {
                using (var client = new HttpClient())
                {

                    client.BaseAddress = new Uri(_configuration["urlApi"]);

                    var postTask = client.PostAsJsonAsync<ML.Medicamento>("Medicamento/Update", medicamento);
                    postTask.Wait();

                    var result = postTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        ViewBag.Message = "Se ha actualizado el registro.";
                        return PartialView("Modal");
                    }
                    else
                    {
                        ViewBag.Message = "No se ha podido actualizar el registro.";
                        return PartialView("Modal");
                    }
                }
            }
        }

        //if (medicamento.IdMedicamento == 0)
        //{
        //    result = BL.Medicamento.Add(medicamento);

        //    if (result.Correct)
        //    {
        //        ViewBag.Mesagge = "Se Agrego el Medicamento";
        //    }
        //    else
        //    {
        //        ViewBag.Message = "No se Agrego el Medicamento";
        //    }
        //    return View("Modal");
        //}
        //else
        //{
        //    result = BL.Medicamento.Update(medicamento);

        //    if (result.Correct)
        //    {
        //        ViewBag.Message = "Se Actualizo el Medicamento";
        //        return View("Modal");
        //    }
        //    else
        //    {
        //        ViewBag.Message = "No se Actualizo el Medicamento";
        //        return View("Modal");
        //    }
        //    return View("Modal");
        //}
        //}

        [HttpGet]
        public ActionResult Delete(int IdMedicamento)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_configuration["urlApi"]);

                var postTask = client.PostAsJsonAsync<int>("Medicamento/Delete", IdMedicamento);
                postTask.Wait();

                var result = postTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    ViewBag.Message = "Se ha Eliminado el Medicamento";
                    return PartialView("Modal");
                }
                else
                {
                    ViewBag.Message = "No se ha Eliminado el Medicamento";
                    return PartialView("Modal");
                }
            }
        }
        //    ML.Result result = BL.Medicamento.Delete(IdMedicamento);

        //    if (result.Correct)
        //    {
        //        ViewBag.Message = "Se Elimino el Medicamento";
        //        return View("Modal");
        //    }
        //    else
        //    {
        //        ViewBag.Message = "No se Elimino el Medicamento";
        //    }
        //    return View();
        //}

        public byte[] ConvertToBytes(IFormFile imagen)
        {
            using var fileStream = imagen.OpenReadStream();

            byte[] bytes = new byte[fileStream.Length];
            fileStream.Read(bytes, 0, (int)fileStream.Length);
            return bytes;
        }
    }
}