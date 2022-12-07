using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Item
{
    internal class Item : Entity
    {
        public readonly Guid ExternalId = Guid.NewGuid();

        public string Description { get; set; }
        public ItemType Type { get; private set; }
        public double Price { get; set; }
        public int InitialAmount { get; set; }
        public int LimitAmount { get; set; }


        public Item(string description, double price, int initialAmount, int limitAmount)
        {
            Description = description;
            Type = 0;
            Price = price;
            InitialAmount = initialAmount;
            LimitAmount = limitAmount;
        }
    }
}
