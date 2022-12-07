using Domain.Enums;

namespace Domain
{
    public class Table : Entity
    {
        public readonly Guid ExternalId = Guid.NewGuid();
        public readonly DateTime CreationDate = DateTime.Now;

        public string Name { get; set; }
        public string? Description { get; set; }

        /// <summary>
        /// Select Cumulative Price: 1, Non Cumulative Price: 2, or Fixed Price: default
        /// </summary>
        public TableType Type { get; set; }
        public bool Active { get; set; }
        public DateTime? ExpirationDate { get; set;}

        public Table(string name, string? description, DateTime? expirationDate, TableType type = 0, bool active = true)
        {
            Name = name;
            Description = description;
            ExpirationDate = expirationDate;
            Type = type;
            Active = active;
        }
    }
}
