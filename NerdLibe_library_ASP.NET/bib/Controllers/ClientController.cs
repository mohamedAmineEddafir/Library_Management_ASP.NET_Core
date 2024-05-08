using bib.data;
using bib.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Security.Claims;

namespace bib.Controllers
{
    public class ClientController : Controller
    {
        private readonly Dbcontext context;

        public ClientController(Dbcontext logger)
        {
            this.context = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult about() 
        { 
            return View();
        }

        //++++++++++++++++++++++++++++// Start - Books & SaveDemande //+++++++++++++++++++++++++++++
        public IActionResult books()
        {
            var livres = context.Livre.OrderByDescending(p => p.Id).ToList();
            var demandeadd = new demandeAdd();
            var viewModel = new BooksViewModel
            {
                Livres = livres,
                demandeAdd = demandeadd
            };
            return View(viewModel);
        }
        [HttpPost]
        public IActionResult SaveDemande([FromBody] demandeAdd demandeadd)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(demandeadd);
                }
                //Save the new user's on ore database
                Demande demands = new Demande()
                {
                    Name = demandeadd.Name,
                    Prenome = demandeadd.Prenome,
                    Liver = demandeadd.Liver,
                    ReservationDate = demandeadd.ReservationDate,
                };
                // Ajouter la demande à votre contexte de base de données
                context.Demande.Add(demands);
                context.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Une erreur s'est produite : {ex.Message}");
            }
        }


        public IActionResult events()
        {
            var eventes = context.Evente.OrderByDescending(p => p.Id).ToList();
            return View(eventes);
        }

        public IActionResult trending()
        {
            return View();
        }

        //++++++++++++++++++++++++++++++// Start - Sign in //++++++++++++++++++++++++++++++++
        public IActionResult sign_in()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> sign_in(string email, string password)
        {
            // Vérifiez si l'utilisateur existe avec l'email fourni
            var user = context.Utilisateur.SingleOrDefault(u => u.Email == email);

            if (user != null && user.Password == password)
            {
                // Créez les revendications de l'utilisateur
                var claims = new[]
                {
                    new Claim(ClaimTypes.Email, email),
                    new Claim(ClaimTypes.Role, user.Role)
                };

                // Créez l'identité de l'utilisateur
                ClaimsIdentity identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                // j'ai pas fait ici la Créez les propriétés de l'authentification
                AuthenticationProperties authProperties = new AuthenticationProperties();

                // Signez l'utilisateur
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                                              new ClaimsPrincipal(identity),
                                              authProperties);

                return RedirectToAction("Index", "Client");
            }
            else
            {
                TempData["ErrorMessage"] = "Invalid email or password.";
                return RedirectToAction("sign_in", "Client");
            }
        }

        //++++++++++++++++++++++++++++++// End - Sign in //++++++++++++++++++++++++++++++++

        //++++++++++++++++++++++++++++++// Start - Contact //++++++++++++++++++++++++++++++
        public IActionResult contact()
        {
            return View();
        }

        //++++++++++++++++++++++++++++++// End - Contact //++++++++++++++++++++++++++++++++ 

        //++++++++++++++++++++++++++++++// Start - Sign Up //++++++++++++++++++++++++++++++++ 
        public IActionResult sign_up()
        {
            return View();
        }

        [HttpPost]
        public IActionResult sign_up(UserAdd useradd)
        {
            if (!ModelState.IsValid)
            {
                return View(useradd);
            }
            //Save the new user's on ore database
            Utilisateur utilisateur = new Utilisateur()
            {
                Name = useradd.Name,
                Prenome = useradd.Prenome,
                Email = useradd.Email,
                Password = useradd.Password,
                ConfirmePassword = useradd.ConfirmePassword,
            };

            Console.WriteLine(utilisateur);
            context.Utilisateur.Add(utilisateur);
            context.SaveChanges();

            TempData["SuccessMessage"] = "Utilisateur ajouté avec succès !";

            return RedirectToAction("sign_in", "Client");
        }
        //++++++++++++++++++++++++++++++// End - Sign Up //++++++++++++++++++++++++++++++++

        //++++++++++++++++++++++++++++++// Start - logout //++++++++++++++++++++++++++++++++
        public async Task<IActionResult> Logout()
        {
            // Effacez l'authentification de l'utilisateur
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("sign_in", "Client");
        }
    }
}
