using Example.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Example.Business
{
    public class BizManager
        :IBizManager<Customer>
    {
        private static readonly IList<Customer> _repository = new List<Customer>();

        public IList<Customer> GetAll()
        {
            return new ReadOnlyCollection<Customer>(_repository);
        }

        public Customer GetByID(string id)
        {
            return _repository.Where(cust => cust.Id == id).FirstOrDefault();
        }


        public void Add (Customer customer)
        {
            Random rnd = new Random();
            customer.Id = rnd.Next(10000000, 99999999).ToString();
            _repository.Add(customer);
        }


        public bool DeleteById(string id)
        {
            var customer = _repository.Where(cust => cust.Id == id).FirstOrDefault();
            if (customer != null)
            {
                _repository.Remove(customer);
                return true;
            }
            return false;
        }

        public void UpdateById(string id, Customer customer)
        {
            var targetCustomer = _repository.FirstOrDefault(cust => cust.Id == id);

            if (targetCustomer != null)
            {
                targetCustomer.LastName = customer.LastName;
                targetCustomer.FirstName = customer.FirstName;
                targetCustomer.DOB = customer.DOB;
                targetCustomer.SSN = customer.SSN;
            }
        }
    }
}
