﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using CustomerAPI.Models;

namespace CustomerAPI.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class CustomersController : ApiController
    {
        private Database1Entities db = new Database1Entities();

        // GET: api/Customers
        public IQueryable<Customer> GetCustomers()
        {
            return db.Customers;
        }

        // GET: api/Customers/5
        [ResponseType(typeof(Customer))]
        public IHttpActionResult GetCustomer(int id)
        {
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return NotFound();
            }

            return Ok(customer);
        }

        // PUT: api/Customers/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCustomer(int id, Customer customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != customer.CustomerId)
            {
                return BadRequest();
            }

            db.Entry(customer).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Customers
        [Route("api/Customers/PostTest")]
        [ResponseType(typeof(string))]
        [HttpPost]
        public IHttpActionResult PostTest([FromBody]string customer)
        {
            return Ok(customer);
        }
        [Route("api/Customers/PostCustomer")]
        [ResponseType(typeof(Customer))]
        public IHttpActionResult PostCustomer(Customer customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if(customer.CustomerId!=0)
            {
                var cust = db.Customers.Where(x => x.CustomerId == customer.CustomerId).FirstOrDefault();
                if(cust != null)
                {
                    cust.CustomerName = customer.CustomerName;
                    cust.Address = customer.Address;
                    cust.Zip = customer.Zip;
                    cust.City = customer.City;
                    cust.Telephone = customer.Telephone;
                    cust.ContactFirst = customer.ContactFirst;
                    cust.ContactLast = customer.ContactLast;
                    cust.UpdatedDate = DateTime.Now;
                    db.Customers.AddOrUpdate(cust);
                    db.SaveChanges();
                    return Ok(cust);
                }
            }
            customer.CreatedDate = DateTime.Now;
            db.Customers.Add(customer);
            db.SaveChanges();
            return Ok(customer);
        }
        
        // DELETE: api/Customers/5
        [ResponseType(typeof(Customer))]
        public IHttpActionResult DeleteCustomer(int id)
        {
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return NotFound();
            }

            db.Customers.Remove(customer);
            db.SaveChanges();

            return Ok(customer);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CustomerExists(int id)
        {
            return db.Customers.Count(e => e.CustomerId == id) > 0;
        }
    }
}