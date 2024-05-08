using bib.data;
using bib.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;

namespace bib.Controllers
{
    [Authorize(Roles = "admin")]
    public class HomeController : Controller
    {
        private readonly Dbcontext context;
        private readonly IWebHostEnvironment environment;

        public HomeController(Dbcontext logger, IWebHostEnvironment environment)
        {
            this.context = logger;
            this.environment = environment;
        }

        public IActionResult Index()
        {
            IEnumerable<Demande> demandes = context.Demande.OrderByDescending(x => x.Id).ToList();
            return View(demandes);
        }
        //++++++++++++++++++++++++++++++++++++++++++// Start Emprunt //++++++++++++++++++++++++++++++++++++++++++
        
        public IActionResult list_Empteur()
        {
            var empteur = context.Emprunt.OrderByDescending(x => x.Id).ToList();
            return View(empteur);
        }
        public IActionResult add_empteur()
        {
            return View();
        }

        [HttpPost]
        public IActionResult add_empteur(empruntAdd empruntadd)
        {
            if(!ModelState.IsValid)
            {
                return View(empruntadd);
            }
            //Save the Start Emprunt's on ore database
            Emprunt emprunt = new Emprunt()
            {
                Cin = empruntadd.Cin,
                Liver = empruntadd.Liver,
                NameEmprunt = empruntadd.NameEmprunt,
                Telephone = empruntadd.Telephone,
                DateEmprunt = empruntadd.DateEmprunt,
                DateRetoure = empruntadd.DateRetoure,
            };

            context.Emprunt.Add(emprunt);
            context.SaveChanges();

            return RedirectToAction("list_Empteur", "Home");
        }

        public IActionResult edit_Emprunt(int id)
        {
            var emprunts = context.Emprunt.Find(id);

            if (emprunts == null)
            {
                return RedirectToAction("list_Empteur", "Home");
            }

            var empruntAdd = new empruntAdd
            {
                Id = emprunts.Id,
                Cin = emprunts.Cin,
                Liver = emprunts.Liver,
                NameEmprunt = emprunts.NameEmprunt,
                Telephone = emprunts.Telephone,
                DateEmprunt = emprunts.DateEmprunt,
                DateRetoure = emprunts.DateRetoure,
            };

            ViewData["EmpruntId"] = emprunts.Id;

            return View(empruntAdd);

        }

        [HttpPost]
        public IActionResult edit_Emprunt(int id, empruntAdd empruntadd)
        {
            var emprunts = context.Emprunt.Find(id);

            if(emprunts == null)
            {
                return RedirectToAction("list_Empteur", "Home");
            }

            if (!ModelState.IsValid)
            {
                ViewData["EmpruntId"] = emprunts.Id;
                return View(empruntadd);
            }

            //Update emprunt in my Database
            emprunts.Cin = empruntadd.Cin;
            emprunts.Liver = empruntadd.Liver;
            emprunts.NameEmprunt = empruntadd.NameEmprunt;
            emprunts.DateEmprunt = empruntadd.DateEmprunt;
            emprunts.DateRetoure = empruntadd.DateRetoure;
            
            //change my data and save it in my Database
            context.SaveChanges();
            
            //redirect to my list of Epmrunt's
            return RedirectToAction("list_Empteur", "Home");
        }

        //Delete emprunt from my list_emprunt file
        public IActionResult Delete_emprunt(int id)
        {
            var exemprunt = context.Emprunt.Find(id);
            if (exemprunt == null)
            {
                return RedirectToAction("list_Empteur", "Home");
            }
            context.Emprunt.Remove(exemprunt);
            context.SaveChanges();
            TempData["SuccessMessage"] = "Emprunt Supprimer  avec succès.";

            return RedirectToAction("list_Empteur", "Home");
        }
            //++++++++++++++++++++++++++++++++++++++++++// End Emprunt //++++++++++++++++++++++++++++++++++++++++++

            //++++++++++++++++++++++++++++++++++++++++++// Start Livre //++++++++++++++++++++++++++++++++++++++++++
            public IActionResult add_livre(livreAdd livreadd)
        {
            if (livreadd.imageFile == null)
            {
                ModelState.AddModelError("image", "Le fichier image est requis");
                return View("add_livre");
            }

            string newFileName = DateTime.Now.ToString("yyyyMMddmmssfff") + Path.GetExtension(livreadd.imageFile!.FileName);
            string imageDirectory = Path.Combine(environment.WebRootPath, "imgSaved"); // Chemin vers le répertoire d'images
            string imageFullPath = Path.Combine(imageDirectory, newFileName);

            // Vérification si le répertoire existe, sinon le créer
            if (!Directory.Exists(imageDirectory))
            {
                Directory.CreateDirectory(imageDirectory);
            }

            using (var stream = new FileStream(imageFullPath, FileMode.Create))
            {
                livreadd.imageFile.CopyTo(stream);
            }

            Livre nouveauLivre = new Livre()
            {
                Titre = livreadd.Titre,
                Auteur = livreadd.Auteur,
                Discription = livreadd.Discription,
                DatePube = livreadd.DatePube,
                imageName = newFileName,
            };
            context.Livre.Add(nouveauLivre);
            context.SaveChanges();
            TempData["SuccessMessage"] = "Livre ajouté avec succès.";

            // Redirection vers la vue list_livre
            return RedirectToAction("list_livre");

        }

        //Affichage dans Liste
        public IActionResult list_livre()
        {
            var livers = context.Livre.OrderByDescending(p => p.Id).ToList();
            return View(livers);
        }

        //Edite Livre => L'affichage dans  fromulaire
        public IActionResult Edite_livre(int id)
        {
            var livre = context.Livre.Find(id);
            if (livre == null)
            {
                return RedirectToAction("list_livre", "Home");
            }

            var dblivre = new livreAdd()
            {
                Titre = livre.Titre,
                Auteur = livre.Auteur,
                Discription = livre.Discription,
                DatePube = livre.DatePube,
            };

            ViewData["Id"] = livre.Id;
            ViewData["ImgFile"] = livre.imageName;

            return View("editeLivre", dblivre);
        }

        // Save La Modification
        [HttpPost]
        public IActionResult Edite_livre(int id, livreAdd livre)
        {
            var exLivre = context.Livre.Find(id);
            if (exLivre == null)
            {
                return RedirectToAction("list_livre", "Home");
            }

            //Modifictaion l'image
            if (livre.imageFile != null)
            {
                string newFileName = DateTime.Now.ToString("yyyyMMddmmssfff") + Path.GetExtension(livre.imageFile.FileName);
                string imageDirectory = Path.Combine(environment.WebRootPath, "imgSaved"); // Chemin  D'image
                string imageFullPath = Path.Combine(imageDirectory, newFileName);

                // Vérification si le répertoire existe
                if (!Directory.Exists(imageDirectory))
                {
                    Directory.CreateDirectory(imageDirectory);
                }

                using (var stream = new FileStream(imageFullPath, FileMode.Create))
                {
                    livre.imageFile.CopyTo(stream);
                }

                // Supprimer l'ancienne image
                string oldImageFullPath = Path.Combine(environment.WebRootPath, "imgSaved", exLivre.imageName);
                if (System.IO.File.Exists(oldImageFullPath))
                {
                    System.IO.File.Delete(oldImageFullPath);
                }

                exLivre.imageName = newFileName;
            }
            // Mettre a jour
            exLivre.Titre = livre.Titre;
            exLivre.Auteur = livre.Auteur;
            exLivre.Discription = livre.Discription;
            exLivre.DatePube = livre.DatePube;

            context.SaveChanges();
            TempData["SuccessMessage"] = "Livre Modifier avec succès.";
            return RedirectToAction("list_livre", "Home");

        }

        //Delete Liver
        public IActionResult Delete_livre(int id)
        {
            var exLivre = context.Livre.Find(id);
            if (exLivre == null)
            {
                return RedirectToAction("list_livre", "Home");
            }
            string imageFullPath = Path.Combine(environment.WebRootPath, "imgSaved", exLivre.imageName);

            if (System.IO.File.Exists(imageFullPath))
            {
                System.IO.File.Delete(imageFullPath);
            }

            // Suppression du livre de la BDD
            context.Livre.Remove(exLivre);
            context.SaveChanges();
            TempData["SuccessMessage"] = "Livre Supprimer  avec succès.";

            return RedirectToAction("list_livre", "Home");
        }
        //++++++++++++++++++++++++++++++++++++++++++// End Livre //++++++++++++++++++++++++++++++++++++++++++

        //++++++++++++++++++++++++++++++++++++++++++// Start Utilisateur //++++++++++++++++++++++++++++++++++++++++++
        
        public IActionResult list_utilisateur()
        {
            var Utilisateur = context.Utilisateur.OrderByDescending(p => p.Id).ToList();
            return View(Utilisateur);
        }

        

        //Delete emprunt from my list_emprunt file
        public IActionResult Delete_utilisateur(int id)
        {
            var exutilisateur = context.Utilisateur.Find(id);
            if (exutilisateur == null)
            {
                return RedirectToAction("list_utilisateur", "Home");
            }
            context.Utilisateur.Remove(exutilisateur);
            context.SaveChanges();
            TempData["SuccessMUtilisateur"] = "Utilisateur Supprimer  avec succès.";

            return RedirectToAction("list_utilisateur", "Home");
        }

        //edit Utilisateur
        public IActionResult editUtilisateur(int id)
        {
            var utilisateur = context.Utilisateur.Find(id);

            if (utilisateur == null)
            {
                return RedirectToAction("list_utilisateur", "Home");
            }

            var UserAdd = new UserAdd
            {
                Name = utilisateur.Name,
                Prenome = utilisateur.Prenome,
                Email = utilisateur.Email,
                Password = utilisateur.Password,
                ConfirmePassword = utilisateur.ConfirmePassword,
                Role = utilisateur.Role,
            };

            ViewData["UtilisateurId"] = utilisateur.Id;

            return View("editUtilisateur", UserAdd);

        }

        [HttpPost]
        public IActionResult editUtilisateur(int id, UserAdd UserAdd)
        {
            var exutilisateur = context.Utilisateur.Find(id);

            if (exutilisateur == null)
            {
                return RedirectToAction("list_utilisateur", "Home");
            }

            if (!ModelState.IsValid)
            {
                ViewData["UtilisateurId"] = exutilisateur.Id;
                return View(UserAdd);
            }

            //Update emprunt in my Database
            exutilisateur.Name = UserAdd.Name;
            exutilisateur.Prenome = UserAdd.Prenome;
            exutilisateur.Email = UserAdd.Email;
            exutilisateur.Password = UserAdd.Password;
            exutilisateur.ConfirmePassword = UserAdd.ConfirmePassword;
            exutilisateur.Role = UserAdd.Role;


            context.SaveChanges();
            TempData["SuccessMUtilisateur"] = "Modification Utilisateur  avec succès.";

            return RedirectToAction("list_utilisateur", "Home");
        }

        //++++++++++++++++++++++++++++++++++++++++++// End Utilisateur //++++++++++++++++++++++++++++++++++++++++++

        //++++++++++++++++++++++++++++++++++++++++++// Start Eventes //++++++++++++++++++++++++++++++++++++++++++++++


        public IActionResult list_Evenets()
        {
            var eventes = context.Evente.OrderByDescending(p => p.Id).ToList();
            return View(eventes);

        }

        public IActionResult add_Evente(eventeAdd eventeAdd)
        {
            if (eventeAdd.imageFile == null)
            {
                ModelState.AddModelError("image", "Le fichier image est requis");
                return View("add_Evente");
            }

            string newFileName = DateTime.Now.ToString("yyyyMMddmmssfff") + Path.GetExtension(eventeAdd.imageFile!.FileName);
            string imageDirectory = Path.Combine(environment.WebRootPath, "imgEvente"); // Chemin vers le répertoire d'images
            string imageFullPath = Path.Combine(imageDirectory, newFileName);

            // Vérification si le répertoire existe, sinon le créer
            if (!Directory.Exists(imageDirectory))
            {
                Directory.CreateDirectory(imageDirectory);
            }

            using (var stream = new FileStream(imageFullPath, FileMode.Create))
            {
                eventeAdd.imageFile.CopyTo(stream);
            }

            Evente nouveauEventee = new Evente()
            {
                Titre = eventeAdd.Titre,
                localisation = eventeAdd.localisation,
                Discription = eventeAdd.Discription,
                date = eventeAdd.date,
                imageName = newFileName,
            };
            context.Evente.Add(nouveauEventee);
            context.SaveChanges();
            TempData["SuccessMessage"] = "Evente ajouté avec succès.";

            // Redirection vers la vue list_livre
            return RedirectToAction("list_Evenets", "Home"); 
        }
        //++++++++++++++++++++++++++++++++++++++++++// End Eventes //++++++++++++++++++++++++++++++++++++++++++++++

        //++++++++++++++++++++++++++++++++++++++++++// Start Demande //++++++++++++++++++++++++++++++++++++++++++++++
        public IActionResult liste_demands()
        {
            IEnumerable<Demande> demandes = context.Demande.OrderByDescending(x => x.Id).ToList();
            return View(demandes);
        }
        //Delete emprunt from my list_emprunt file
        public IActionResult delete_Demande(int id)
        {
            var exdemande = context.Demande.Find(id);
            if (exdemande == null)
            {
                return RedirectToAction("liste_demands", "Home");
            }
            context.Demande.Remove(exdemande);
            context.SaveChanges();
            TempData["SuccessMessage"] = "Emprunt Supprimer  avec succès.";

            return RedirectToAction("liste_demands", "Home");
        }
        //++++++++++++++++++++++++++++++++++++++++++// End Demande //++++++++++++++++++++++++++++++++++++++++++++++

        //++++++++++++++++++++++++++++++++++++++++++// Start Logout //+++++++++++++++++++++++++++++++++++++++++++++
        public IActionResult Logout()
        { 
            return RedirectToAction("Index", "Client");
        }
    }
}