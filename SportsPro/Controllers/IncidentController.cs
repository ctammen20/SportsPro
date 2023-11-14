using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportsPro.Models;

namespace SportsPro.Controllers
{
    public class IncidentController : Controller
    {
        private SportsProContext context { get; set; }

        public IncidentController(SportsProContext ctx)
        {
            context = ctx;
        }

        [Route("[controller]s")]
        public IActionResult List(string filter)
        {
            if (filter == null)
                filter = "all";

            List<Incident> incidents = context.Incidents
                .Include(i => i.Customer)
                .Include(i => i.Product)
                .OrderBy(i => i.DateOpened)
                .ToList();

            IncidentListViewModel model = new IncidentListViewModel();

            model.Filter = filter;
            model.Incidents = incidents;

            return View("List", model);
        }

        private IncidentViewModel GetIncidentViewModel(string action)
        {
            IncidentViewModel model = new IncidentViewModel()
            {
                Customers = context.Customers
                    .OrderBy(c => c.FirstName)
                    .ToList(),

                Products = context.Products
                    .OrderBy(p => p.Name)
                    .ToList(),

                Technicians = context.Technicians
                    .OrderBy(t => t.Name)
                    .ToList(),
            };

            if (!String.IsNullOrEmpty(action))
                model.action = action;

            return model;
        }

        [HttpGet]
        public IActionResult Add()
        {
            // ViewBag.Action = "Add";

            // StoreListsInViewBag();

            IncidentViewModel model = GetIncidentViewModel("Add");
                
            model.Incident = new Incident();

            return View("AddEdit", model);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            // ViewBag.Action = "Edit";

            // StoreListsInViewBag();

            IncidentViewModel model = GetIncidentViewModel("Edit");
            model.Incident = context.Incidents.Find(id);

            return View("AddEdit", model);
        }

        [HttpPost]
        public IActionResult Save(Incident incident)
        {
            string successMessage;

            if (ModelState.IsValid)
            {
                if (incident.IncidentID == 0)
                {
                    context.Incidents.Add(incident);
                    successMessage = incident.Title + " " + "was added.";
                }
                else
                {
                    context.Incidents.Update(incident);
                    successMessage = incident.Title + " " + "was updated.";
                }
                context.SaveChanges();
                TempData["message"] = successMessage;
                return RedirectToAction("List");
            }
            else
            {
                // StoreListsInViewBag();

                IncidentViewModel model = GetIncidentViewModel("");

                model.Incident = incident;

                if (incident.IncidentID == 0)
                {
                    model.action = "Add";
                    // ViewBag.Action = "Add";
                }
                else
                {
                    model.action = "Edit";
                    // ViewBag.Action = "Edit";
                }

                return View("AddEdit", model);
            }
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var product = context.Incidents.Find(id);
            return View(product);
        }

        [HttpPost]
        public IActionResult Delete(Incident incident)
        {
            context.Incidents.Remove(incident);
            context.SaveChanges();

            TempData["message"] = incident.Title + " was deleted.";

            return RedirectToAction("List");
        }
    }
}