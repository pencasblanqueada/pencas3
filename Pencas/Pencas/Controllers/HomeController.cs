using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pencas.Models;

namespace Pencas.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            
            //PencasDB db = new PencasDB();
            //DAOUsuario usuarioDAO = new DAOUsuario();
            //List<Usuario> usuarios = usuarioDAO.GetUsuarios();
            //ViewBag.Valor = usuarios[0].Email;
            return View();
        }

        public PartialViewResult EnviarPronostico(string email, string token, string resultados, string resultadosEspeciales)
        {
            DAOUsuario usuarioDAO = new DAOUsuario();
            DAOPronostico pronosticoDAO = new DAOPronostico();
            try
            {
                if (DateTime.Now > new DateTime(2013, 6, 14))
                {
                    ViewBag.Mensaje = "El plazo para enviar pronósticos ha finalizado";
                    ViewBag.ColorTexto = "Red";
                    return PartialView("_Mensaje");
                }
                usuarioDAO.AsociarMailToken(email, token);
                Usuario u = usuarioDAO.GetUsuarioPorToken(token);
                pronosticoDAO.GuardarPronostico(resultados, u);
                pronosticoDAO.GuardarPronosticosEspeciales(resultadosEspeciales, u);

                ViewBag.Mensaje = "Su pronóstico ha sido enviado. Para consultarlo presione el botón \"Ver Pronóstico\" arriba de la tabla";
                ViewBag.ColorTexto = "Green";
                return PartialView("_Mensaje");
            }
            catch (YaEstaAsociadoElTokenConOtroUsuarioException)
            {
                ViewBag.Mensaje = "Código inválido";
                ViewBag.ColorTexto = "Red";
                return PartialView("_Mensaje");
            }
            catch (NoExisteElTokenException)
            {
                ViewBag.Mensaje = "Código inválido";
                ViewBag.ColorTexto = "Red";
                return PartialView("_Mensaje");
            }
            catch (YaEstaAsociadoElUsuarioConElTokenException)
            {
                ViewBag.Mensaje = "Ya ha realizado su pronóstico. Click aquí para verlo";
                ViewBag.ColorTexto = "Red";
                return PartialView("_Mensaje");
            }
            catch (Exception)
            {
                ViewBag.Mensaje = "Ocurrió un error inesperado. Inténtelo más tarde o contáctenos";
                ViewBag.ColorTexto = "Red";
                return PartialView("_Mensaje");
            }
            //catch (YaExisteUsuarioException)
            //{
            //    Usuario u = usuarioDAO.GetUsuarioPorEmail(email);
            //    if (!token.Equals(usuarioDAO.GetTokenPorEmail(email)))
            //    {
            //        ViewBag.Mensaje = "Código inválido";
            //        ViewBag.ColorTexto = "Red";
            //        return PartialView("_Mensaje");
            //    }
            //    if (u.EnvioPronostico)
            //    {
            //        ViewBag.Mensaje = "Ya ha realizado su pronóstico. Click aquí para verlo";
            //        ViewBag.ColorTexto = "Red";
            //        return PartialView("_Mensaje");
            //    }
            //    else
            //    {
            //        ViewBag.ColorTexto = "Red";
            //        return PartialView("_Mensaje");
            //    }
            //}
        }

        public PartialViewResult VerPronostico(string token)
        {
            DAOUsuario usuarioDAO = new DAOUsuario();
            DAOPronostico pronosticoDAO = new DAOPronostico();
            Usuario usuario;
            try
            {
                usuario = usuarioDAO.GetUsuarioPorToken(token);
            }
            catch (NoExisteUsuarioException)
            {
                ViewBag.Mensaje = "Código inválido";
                ViewBag.ColorTexto = "Red";
                return PartialView("_Mensaje");
            }
            string pronosticos = pronosticoDAO.RecuperarPronostico(usuario);
            ViewBag.Pronosticos = pronosticos.Split(';');
            return PartialView("_VerPronosticos");
        }

        public ActionResult About()
        {
            return View();
        }
    }
}
