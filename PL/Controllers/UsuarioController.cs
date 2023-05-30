using Microsoft.AspNetCore.Mvc;

namespace PL.Controllers
{
    public class UsuarioController : Controller
    {
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            ML.Result result = BL.Usuario.UsuarioGetbyUserName(username);
            if (result.Correct)
            {
                ML.Usuario usuario = (ML.Usuario)result.Object;
                if (password == usuario.Password)
                {
                    return RedirectToAction("GetAll", "Medicamento");
                }
                else
                {
                    ViewBag.Message = "La contraseña es incorrecta";
                    return PartialView("ModalLogin");
                }
            }
            else
            {
                ViewBag.Message = "El usuario no existe, verifica tus datos nuevamente.";
                return PartialView("ModalLogin");
            }
        }
    }
}
