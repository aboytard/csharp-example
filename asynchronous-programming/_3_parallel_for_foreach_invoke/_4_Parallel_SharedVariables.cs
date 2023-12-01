using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace _3_parallel_for_foreach_invoke
{
    public class Customer
    {
        public string Name { get; set; }
        public List<double> Accounts { get; set; }
    }

    public class _4_Parallel_SharedVariables
    {

        static object synchronizer = new object();
        public void DoWork()
        {
            double totalAccount = 0;
            var customerlist = GetCustomers();
            Parallel.ForEach(customerlist, customer =>
            {
                double total = 0;
                foreach (var account in customer.Accounts)
                {
                    total += account;
                }

                //works in lock must be atomic. expensive operations increase the thread wait time
                //and operation will be slow maybe than synchronous operations. 
                lock (synchronizer)
                {
                    totalAccount += total;
                }
            });

            Console.WriteLine("total amount is {0}", totalAccount);
        }



        private List<Customer> GetCustomers()
        {
            List<Customer> customers = new List<Customer>()
            {
                new Customer(){ Name="A", Accounts=new List<double>(){123234,345345,54546756,56867867,897897 } },
                new Customer(){ Name="B", Accounts=new List<double>(){ 6575464,7686,4534534,76867867}  },
                new Customer(){ Name="C", Accounts=new List<double>(){234,546,6758,87978,98087 }  },
                new Customer(){ Name="D", Accounts=new List<double>(){ 436,567,78978,8908}  }
            };

            return customers;
        }
    }
}
