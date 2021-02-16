using CRUD_NwDb.Data;
using CRUD_NwDb.Models;
using CsvHelper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.AccessControl;
using System.Linq;
using System.Threading.Tasks;
using CRUD_NwDb;
using System.Globalization;
using System.Text;
using CsvHelper.Configuration;

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
        protected IActionResult Index()
        {
            return View();  
        }

        // GET: CustomerController/Details/5
        [HttpGet("details/{CustomerId}")]
        public string Details(int CustomerId)
        {
            try{
                if (_db.Customer.Find(CustomerId) != null)
                {
                    var objList = _db.Customer.Find(CustomerId);
                    // var objList2 = (IEnumerable<Customer>)_db.Customer.Where(b=>b.CustomerId==CustomerId);

                    string obj_json = JsonConvert.SerializeObject(objList);

                    return obj_json; // Ok - Library for returning Json response
                }
                else
                {
                    return "Not Found";
                    //return NotFound();
                }
            }
            catch(Exception e){
                throw;
            }
        }


        // POST: CustomerController/Create
        [HttpPost("Create/post")]
        [Consumes("multipart/form-data")] // Required when testing in Swagger
        //[ValidateAntiForgeryToken]
        public string Create(IFormCollection collection)
        {
            try
            {
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

                string obj_json = JsonConvert.SerializeObject(obj);
                return obj_json;
                //return RedirectToAction("Index");
            }
            catch(Exception e)
            {
                throw;
            }
        }


        // POST: CustomerController/Edit/5
        [HttpPost("Edit")]
        [Consumes("multipart/form-data")]
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

                    string obj_json = JsonConvert.SerializeObject(obj);
                    return obj_json;
                }
                else
                {
                    return "Record not found";
                }
}
            catch(Exception e)
            {
                throw;
            }
        }


        // POST: CustomerController/Delete/5
        [HttpPost("Delete")]
        [Consumes("multipart/form-data")]
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

                    string obj_json = JsonConvert.SerializeObject(obj);
                    return obj_json;
                }
                else
                {
                    return "Record not found";
                }
            }
            catch(Exception e)
            {
                throw;
            }
        }

        // GET: CustomerController/Csv/get
        [HttpGet("Csv/get")]
        public IActionResult GetCsvData(){
            try{
                var path = "C:\\Users\\Nikhil\\Downloads\\Northwind_database_csv\\customer_custom.csv";
   
                StreamReader reader = new StreamReader(path);
                CsvReader csv_reader = new CsvReader(reader, CultureInfo.InvariantCulture);
                var records = csv_reader.GetRecords<Customer>().ToList();
                
                reader.Dispose();
                csv_reader.Dispose();
                return Ok(records);
            }
            catch(Exception e){
                throw;
            }
        }

        // POST: CustomerController/Csv/post
        [HttpPost("Csv/post")]
        public IActionResult WriteCsvData(Customer customer){
            try{
                var path = "C:\\Users\\Nikhil\\Downloads\\Northwind_database_csv\\customer_custom.csv";

                // Append to the file.
                var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    // Don't write the header again.
                    HasHeaderRecord = false,
                };
                using(Stream stream = System.IO.File.Open(path, FileMode.Append))
                using(StreamWriter sw = new StreamWriter(stream))
                using(CsvWriter csv_writer = new CsvWriter(sw, config)){
                    csv_writer.WriteRecord<Customer>(customer);

                    return Ok(customer);
                }
            }
            catch(Exception e){
                throw;
            }
        }

    }
}
