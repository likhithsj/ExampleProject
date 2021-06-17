using Example.Business;
using Example.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Example.Controllers
{
    [Authorize]
    [Route ("api/[controller]")]
    [ApiController]

    public class CustomerController : ControllerBase
    {
        private readonly IBizManager<Customer> _ibizManager;

        public CustomerController(IBizManager<Customer> ibizManager)
        {
            _ibizManager = ibizManager;
        }

        //GET: api/customer
        [HttpGet]
        public IActionResult GetAllCustomers()
        {
            var response = _ibizManager.GetAll();
            if(response == null)
            {
                return NotFound();
            }
            return Ok(response);
        }

        //GET: api/Customer/12
        [HttpGet ("{id}", Name = "GetCustomerByID")]
        public IActionResult GetCustomerByID (string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest("Invalid customer ID");
            }
            var response = _ibizManager.GetByID(id);
            if (response == null)
            {
                return NotFound();
            }
            return Ok(response);
        }

        //POST: api/customer
        [Authorize(Policy = Policies.ADMIN_ROLE)]
        [HttpPost]
        public IActionResult AddCustomer([FromBody] Customer customer)
        {
            if (customer == null)
            {
                return BadRequest();
            }
            _ibizManager.Add(customer);
            return CreatedAtRoute("GetCustomerbyID", new { id = customer.Id }, customer);
        }

        //PUT: api/customer/123-11-1234
        [HttpPut("{id}")]
        public IActionResult UpdateCustomerByID(string id, [FromBody] Customer customer)
        {
            if (string.IsNullOrWhiteSpace(id) || id!=customer.Id || customer==null)
            {
                return BadRequest();
            }
            var customerById = _ibizManager.GetByID(id);
            if(customerById == null)
            {
                return NotFound();
            }
            _ibizManager.UpdateById(id, customer);
            return new NoContentResult();
        }

        //DELETE: api/customers/1234
        [Authorize(Policy = Policies.ADMIN_ROLE)]                   
        [HttpDelete("{id}")]
        public IActionResult DeleteCustomerByID(string id)
        {
            if(string.IsNullOrWhiteSpace(id))
            {
                return BadRequest();
            }
            var wasDeleted = _ibizManager.DeleteById(id);

            if(!wasDeleted)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
