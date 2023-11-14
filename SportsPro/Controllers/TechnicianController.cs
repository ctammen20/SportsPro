using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Mvc;
using SportsPro.Models;

namespace SportsPro.Controllers
{
    public class TechnicianController : Controller
    {
        private SportsProContext context { get; set; }

        public TechnicianController(SportsProContext ctx)
        {
            context = ctx;
        }

        [Route("[controller]s")]
        public IActionResult List()
        {
            List<Technician> techs = context.Technicians.OrderBy(t => t.Name).ToList();
            return View(techs);
        }

        [HttpGet]
        public IActionResult Add()
        {
            ViewBag.Action = "Add";
            return View("AddEdit", new Technician());
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            ViewBag.Action = "Edit";
            var tech = context.Technicians.Find(id);
            return View("AddEdit", tech);
        }

        [HttpPost]
        public IActionResult Save(Technician tech)
        {
            string successMessage;

            if (ModelState.IsValid)
            {
                if (tech.TechnicianID == 0)
                {
                    context.Technicians.Add(tech);
                    successMessage = tech.Name + " " + " was added.";
                }
                else
                {
                    context.Technicians.Update(tech);
                    successMessage = tech.Name + " " + " was updated.";
                }
                context.SaveChanges();
                TempData["message"] = successMessage;

                return RedirectToAction("List");
            }
            else
            {
                if (tech.TechnicianID == 0)
                {
                    ViewBag.Action = "Add";
                }
                else
                {
                    ViewBag.Action = "Edit";
                }                    
                return View(tech);
            }
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var tech = context.Technicians.Find(id);
            return View(tech);
        }

        [HttpPost]
        public IActionResult Delete(Technician tech)
        {
            context.Technicians.Remove(tech);
            context.SaveChanges();
            TempData["message"] = tech.Name + " was deleted.";

            return RedirectToAction("List");
        }
    }
}