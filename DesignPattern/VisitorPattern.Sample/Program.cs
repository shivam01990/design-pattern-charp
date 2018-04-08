using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisitorPattern.Sample
{
    class Program
    {
        /// <summary>
        /// Entry point into console application.
        /// </summary>
        static void Main()
        {
            // Setup food collection
            var food = new FastFood();
            food.Attach(new Pizza());
            food.Attach(new Pasta());
            food.Attach(new Burger());

            // food are 'visited'
            food.Accept(new PriceVisitor());
            food.Accept(new QuantityVisitor());

            // Wait for user
            Console.ReadKey();
        }
    }

    /// <summary>
    /// The 'Visitor' abstract class
    /// </summary>
    public abstract class Visitor
    {
        // Use reflection to see if the Visitor has a method 
        // named Visit with the appropriate parameter type 
        // (i.e. a specific Food). If so, invoke it.
        public void ReflectiveVisit(IElement element)
        {
            var types = new Type[] { element.GetType() };
            var mi = this.GetType().GetMethod("Visit", types);

            if (mi != null)
            {
                mi.Invoke(this, new object[] { element });
            }
        }
    }

    /// <summary>
    /// A 'ConcreteVisitor' class
    /// </summary>
    class PriceVisitor : Visitor
    {
        // Visit clerk
        public void Visit(Pizza pizza)
        {
            DoVisit(pizza);
        }

        // Visit director
        public void Visit(Pasta pasta)
        {
            DoVisit(pasta);
        }

        private void DoVisit(IElement element)
        {
            var food = element as Food;

            // Provide 10% pay raise
            food.Price *= 1.10;
            Console.WriteLine("{0} {1}'s new price: {2:C}",
                food.GetType().Name, food.Name,
                food.Price);
        }
    }

    /// <summary>
    /// A 'ConcreteVisitor' class
    /// </summary>
    class QuantityVisitor : Visitor
    {
        // Visit director
        public void Visit(Pasta director)
        {
            DoVisit(director);
        }

        private void DoVisit(IElement element)
        {
            var food = element as Food;

            // Provide 3 extra quantity days
            food.Quantity += 3;
            Console.WriteLine("{0} {1}'s new quantity: {2}",
                food.GetType().Name, food.Name,
                food.Quantity);
        }
    }

    /// <summary>
    /// The 'Element' interface
    /// </summary>
    public interface IElement
    {
        void Accept(Visitor visitor);
    }

    /// <summary>
    /// The 'ConcreteElement' class
    /// </summary>
    class Food : IElement
    {
        // Constructor
        public Food(string name, double price,
            int quantity)
        {
            this.Name = name;
            this.Price = price;
            this.Quantity = quantity;
        }

        // Gets or sets name
        public string Name { get; set; }

        // Gets or set price
        public double Price { get; set; }

        // Gets or sets Quantity days
        public int Quantity { get; set; }

        public virtual void Accept(Visitor visitor)
        {
            visitor.ReflectiveVisit(this);
        }
    }

    /// <summary>
    /// The 'ObjectStructure' class
    /// </summary>
    class FastFood : List<Food>
    {
        public void Attach(Food food)
        {
            Add(food);
        }

        public void Detach(Food food)
        {
            Remove(food);
        }

        public void Accept(Visitor food)
        {
            // Iterate over all fastfood and accept visitor
            this.ForEach(item => item.Accept(food));

            Console.WriteLine();
        }
    }

    // Three Food types

    class Pizza : Food
    {
        // Constructor
        public Pizza()
            : base("Veggie Pizza", 100.0, 10)
        {
        }
    }

    class Pasta : Food
    {
        // Constructor
        public Pasta()
            : base("Corn Pasta", 50.0, 5)
        {
        }
    }

    class Burger : Food
    {
        // Constructor
        public Burger()
            : base("Veg Burger", 20.0, 25)
        {
        }
    }
}
