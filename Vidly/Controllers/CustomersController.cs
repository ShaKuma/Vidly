using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vidly.Models;
using System.Data.Entity;
using Vidly.ViewModel;

namespace Vidly.Controllers
{
    public class CustomersController : Controller
    {
        public ApplicationDbContext _context;
        public CustomersController()
        {
            _context = new ApplicationDbContext();
        }
        // GET: Customers
        public ActionResult Index()
        {
            var customer = _context.Customer.Include(m => m.MembershipType).ToList();
            return View(customer);
        }
        public ActionResult CustomerForm()
        {
            CustomerFormViewModel viewModel = new CustomerFormViewModel
            {
                Customer = new Customer(),
                MembershipTypes = _context.MembershipType.ToList()
            };
            return View(viewModel);
        }

        public ActionResult Edit(int id)
        {
            var customer = _context.Customer.SingleOrDefault(c => c.Id == id);
            if(customer is null)
            {
                return HttpNotFound();
            }
            else
            {
                CustomerFormViewModel viewModel = new CustomerFormViewModel
                {
                    Customer = customer,
                    MembershipTypes = _context.MembershipType.ToList()
                };
                return View("CustomerForm", viewModel);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(Customer customer)
        {
            if (!ModelState.IsValid)
            {
                CustomerFormViewModel viewModel = new CustomerFormViewModel
                {
                    Customer = customer,
                    MembershipTypes = _context.MembershipType.ToList()
                };
                return View("CustomerForm", viewModel);
            }
            if(customer.Id == 0)
            {
                _context.Customer.Add(customer);
            }
            else
            {
                var customerInDb = _context.Customer.SingleOrDefault(c => c.Id == customer.Id);
                if(customerInDb is null)
                {
                    return HttpNotFound();
                }
                customerInDb.IsSubscribedToNewsLetter = customer.IsSubscribedToNewsLetter;
                customerInDb.MembershipTypeId = customer.MembershipTypeId;
                customerInDb.Name = customer.Name;
                customerInDb.BirthDate = customer.BirthDate;
            }
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}