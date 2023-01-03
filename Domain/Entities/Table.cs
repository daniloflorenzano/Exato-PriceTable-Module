using Domain.Entities.Enums;
using Domain.Primitives;

namespace Domain.Entities
{
    public class Table : Entity
    {
        public readonly DateTime CreationDate = DateTime.Now;
        public Guid ExternalId = Guid.NewGuid();

        public string Name { get; set; }
        public string? Description { get; set; }

        /// <summary>
        /// Select Cumulative Price: 1, Non Cumulative Price: 2, or Fixed Price: default
        /// </summary>
        public TableType Type { get; set; }
        public bool Active { get; set; }
        public DateTime? ExpirationDate { get; set;}

        public Table(string name, string? description, DateTime? expirationDate, TableType type, bool active = true)
        {
            Name = name;
            Description = description;
            ExpirationDate = expirationDate;
            Type = type;
            Active = active;
        }

        public Table(string name, TableType type)
        {
            Name = name;
            Type = type;
        }
    }
}
