using System;
using System.Collections.Generic;

namespace ConsoleApplication16
{
    public class DontHaveIdFieldException : Exception{}
    public class NoDataException : Exception{}
    public class EntityItem { }

    public class Customer : EntityItem, IHasId
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class Transaction : EntityItem, IHasId
    {
        public int Id { get; set; }
        public int Amount { get; set; }
    }

    public class Entity<T> where T : EntityItem
    {
        protected List<T> Data { get; set; }

        public T GetById(int id) 
        {
            foreach (var entity in Data)
            {
                var withId = entity as IHasId;
                if (withId == null)
                {
                    throw new DontHaveIdFieldException();
                }

                if (withId.Id == id)
                {
                    return entity;
                }
            }

            throw new NoDataException();
        }
    }

    public class CustomerRepository : Entity<Customer>
    {
        public void Initialize()
        {
            Data = new List<Customer>
            {
                new Customer {Id = 1, Name = "Test1"},
                new Customer {Id = 2, Name = "Test2"},
                new Customer {Id = 3, Name = "Test3"},
            };
        }
    }

    public class TransactionRepository : Entity<Transaction>
    {
        public void Initialize()
        {
            Data = new List<Transaction>
            {
                new Transaction {Id = 1, Amount = 10},
                new Transaction {Id = 2, Amount = 20},
                new Transaction {Id = 3, Amount = 30},
            };
        }
    }

    interface IHasId
    {
        int Id { get; set; }
    }

    static class Program
    {
        public static void Main()
        {
            try
            {
                var transactionRepository = new TransactionRepository();
                transactionRepository.Initialize();

                var customerRepository = new CustomerRepository();
                customerRepository.Initialize();



                var tr = transactionRepository.GetById(10);
                var cust = customerRepository.GetById(1);

                Console.WriteLine(tr.Amount);
                Console.WriteLine(cust.Name);
            }
            catch (DontHaveIdFieldException)
            {
                Console.WriteLine("Cant call GetById()");
            }
            catch (NoDataException)
            {
                Console.WriteLine("No Data");
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex);
            }
           
            Console.ReadKey();
        }
    }
}
