using CRUD_NwDb.Data;
using CRUD_NwDb.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRUD_NwDb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : Controller
    {
        private readonly ApplicationDbContext _db;
        
        public CustomerController(ApplicationDbContext db)
        {
            _db = db;
        }

        // GET: CustomerController
        [Route("Index")]
        public IActionResult Index()
        {
            return View();  
        }

        // GET: CustomerController/Details/5
        [HttpGet("details/{CustomerId}")]
        public string Details(int CustomerId)
        {
            Console.WriteLine(CustomerId);
            if (_db.Customer.Find(CustomerId) != null)
            {
                var objList = _db.Customer.Find(CustomerId);
                // var objList2 = (IEnumerable<Customer>)_db.Customer.Where(b=>b.CustomerId==CustomerId);
                return objList.City; // Ok - Library for returning Json response
            }
            else
            {
                return "Not Found";
                //return NotFound();
            }
        }

        // GET: CustomerController/Create
        [HttpGet("Create")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: CustomerController/Create
        [HttpPost("Create/post")]
        //[ValidateAntiForgeryToken]
        public string Create(IFormCollection collection)
        {
            try
            {
                //Console.WriteLine(collection);
                Customer obj = new Customer();
                // As SET IDENTITY_INSERT Customer is set OFF, PK will be auto incremented and need not be inserted in the query
                // obj.CustomerId = int.Parse(collection["CustomerId"]);
                obj.Address = collection["Address"];
                obj.City = collection["City"];
                obj.CompanyName = collection["CompanyName"];
                obj.Country = collection["Country"];
                obj.Fax = collection["Fax"];
                obj.Phone = collection["Phone"];
                obj.Region = collection["Region"];
                obj.PostalCode = collection["PostalCode"];

                _db.Customer.Add(obj);
                _db.SaveChanges();
                return "Succesful";
                //return RedirectToAction("Index");
            }
            catch
            {
                return "View()";
            }
        }

        // GET: CustomerController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CustomerController/Edit/5
        [HttpPost("Edit")]
        //[ValidateAntiForgeryToken]
        public string Edit(IFormCollection collection)
        {
            try
            {
                int id = int.Parse(collection["CustomerId"]);
                var obj = _db.Customer.FirstOrDefault(b => b.CustomerId == id);

                if (obj != null)
                {
                    obj.Address = collection["Address"];
                    obj.City = collection["City"];
                    obj.CompanyName = collection["CompanyName"];
                    obj.Country = collection["Country"];
                    obj.Fax = collection["Fax"];
                    obj.Phone = collection["Phone"];
                    obj.Region = collection["Region"];
                    obj.PostalCode = collection["PostalCode"];

                    _db.SaveChanges();
                }
                else
                {
                    return "Record not found";
                }
                return "Success";
            }
            catch
            {
                return "Not working";
            }
        }

        // GET: CustomerController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CustomerController/Delete/5
        [HttpPost("Delete")]
        //[ValidateAntiForgeryToken]
        public string Delete(IFormCollection collection)
        {
            try
            {
                int id = int.Parse(collection["CustomerId"]);
                var obj = _db.Customer.FirstOrDefault(b => b.CustomerId == id);

                if (obj != null)
                {
                    _db.Customer.Remove(obj);
                    _db.SaveChanges();

                    return "Deleted Successfully";
                }
                else
                {
                    return "Record not found";
                }
            }
            catch
            {
                return "Error";
            }
        }
    }
}
