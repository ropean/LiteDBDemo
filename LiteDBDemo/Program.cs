using Faker;
using Jeffrey;
using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LidtDBDemo
{

  // Create your POCO class
  public class Customer
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public string[] Phones { get; set; }
    public bool IsActive { get; set; }
  }


  class Program
  {

    static void Write(LiteDatabase db)
    {

      // Get customer collection
      var collections = db.GetCollection<Customer>("customers");

      // Create your new customer instance
      var customer = new Customer
      {
        //Name = "John Doe",
        //Phones = new string[] { "8000-0000", "9000-0000" },
        IsActive = true
      };

      customer.Name = Name.FullName();
      customer.Phones = new string[] { Phone.Number(), Phone.CellNumber(), Phone.Extension(), Phone.SubscriberNumber() };
      customer.IsActive = false;

      // Insert new customer document (Id will be auto-incremented)
      collections.Insert(customer);

      // Update a document inside a collection
      //customer.Name = "Joana Doe";

      //collections.Update(customer);

      // Index document using a document property
      //collections.EnsureIndex(x => x.Name);

      // Use Linq to query documents
      //var results = collections.Find(x => x.Name.StartsWith("Jo"));

    }

    static int times = 1000;

    static void WriteWithOpenEverytime()
    {

      for (int i = 0; i < times; i++)
      {

        // Open database (or create if doesn't exist)
        using (var db = new LiteDatabase(@"MyData.db"))
        {
          Write(db);
        }

      }

    }

    static void WriteWithOpenOnce()
    {

      // Open database (or create if doesn't exist)
      using (var db = new LiteDatabase(@"MyData.db"))
      {

        for (int i = 0; i < times; i++)
        {
          Write(db);
        }

      }

    }

    static void Main(string[] args)
    {

      //Task.Factory.StartNew(WriteWithOpenEverytime);
      CodeTimer.Time("WriteWithOpenEverytime", 1, WriteWithOpenEverytime);
      CodeTimer.Time("WriteWithOpenOnce", 1, WriteWithOpenOnce);

      Console.ReadLine();

    }

  }
}
