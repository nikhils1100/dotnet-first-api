using CRUD_NwDb.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using CsvHelper;
using System.Globalization;
using System.Text;
using CsvHelper.Configuration;
using CRUD.DataAccess;

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
        public IActionResult Details(int CustomerId)
        {
            try{
                var objList = _db.Customer.Find(CustomerId);
                if (objList!= null)
                {
                    string obj_json = JsonConvert.SerializeObject(objList);

                    return Ok(obj_json); 
                }
                else
                {
                    return NotFound("Not Found");
                    //return NotFound();
                }
            }
            catch(Exception e){
                Console.WriteLine(e.Message);
                return BadRequest(e.Message);
            }
        }


        // POST: CustomerController/Create
        [HttpPost("Create/post")]
        [Consumes("multipart/form-data")] // Required when testing in Swagger
        //[ValidateAntiForgeryToken]
        public IActionResult Create(IFormCollection collection)
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
                return Ok(obj_json);
                //return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return BadRequest(e.Message);
            }
        }


        // POST: CustomerController/Edit/5
        [HttpPost("Edit")]
        [Consumes("multipart/form-data")]
        //[ValidateAntiForgeryToken]
        public IActionResult Edit(IFormCollection collection)
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
                    return NoContent();
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return BadRequest(e.Message);
            }
        }


        // POST: CustomerController/Delete/5
        [HttpPost("Delete")]
        [Consumes("multipart/form-data")]
        //[ValidateAntiForgeryToken]
        public IActionResult Delete(IFormCollection collection)
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
                    return Ok(obj_json);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return BadRequest(e.Message);
            }
        }

        // GET: CustomerController/Csv/get
        [HttpGet("Csv/get")]
        public IActionResult GetCsvData(){
            try{
                var path = "C:\\Users\\Nikhil\\Downloads\\Northwind_database_csv\\customer_custom.csv";

                using (StreamReader reader = new StreamReader(path))
                using (CsvReader csv_reader = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    var records = csv_reader.GetRecords<Customer>().ToList();

                    return Ok(records);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return NotFound(e.Message);
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
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return BadRequest(e.Message);
            }
        }

        // UPDATE: Conterollers/Csv/UpdateToDatabase
        [HttpPost("Csv/CsvToDatabase")]
        public IActionResult CsvToDatabase(string path){
            try
            {
                using (StreamReader reader = new StreamReader(path))
                using (CsvReader csv_reader = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    var record = new Customer();
                    var records = csv_reader.GetRecords<Customer>().ToList();

                    foreach(var r in records)
                    {
                        Customer obj = new Customer();
                        obj.Address = r.Address;
                        obj.City = r.City;
                        obj.CompanyName = r.CompanyName;
                        obj.Country = r.Country;
                        obj.Fax = r.Fax;
                        obj.Phone = r.Phone;
                        obj.PostalCode = r.PostalCode;
                        obj.Region = r.Region;

                        _db.Customer.Add(obj);
                    }
                    _db.SaveChanges();

                    records = csv_reader.GetRecords<Customer>().ToList();
                    return Ok(records);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return StatusCode(500);
            }
        }
    }
}