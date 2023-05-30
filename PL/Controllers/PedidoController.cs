using Microsoft.AspNetCore.Mvc;

namespace PL.Controllers
{
    public class PedidoController : Controller
    {
        public ActionResult Comprar()
        {
            HttpContext.Session.Remove("Productos");

            ViewBag.Message = "Compra Exitosa!";

            return View("ModalCompra");
        }

        public ActionResult ConfirmarPedido()
        {
            ML.Medicamento medicamento = new ML.Medicamento();
            return View(medicamento);
        }
    }
}