using Domain.Entities.Enums;
using Domain.Primitives;

namespace Domain.Entities
{
    public class Table : Entity
    {
        public Guid ExternalId = Guid.NewGuid();
        public string Name { get; set; }
        public string? Description { get; set; }
        public DiscountType Type { get; set; }
        public bool Active { get; set; }
        public DateTime? ExpirationDate { get; set;}
        public DateTime CreationDate { get; } = DateTime.Now;
        
        /// <summary>
        /// Cria uma nova entidade Table
        /// </summary>
        /// <param name="name">Nome da tabela</param>
        /// <param name="description">Descrição da tabela</param>
        /// <param name="expirationDate">Data de expiração. Uma vez expirada, não será possível adicionar itens</param>
        /// <param name="type">Desconto Cumulativo: 1, Desconto Não Cumulativo: 2, Preços Fixos: padrão</param>
        /// <param name="active">Ativa: True (padrão), Inativa: False</param>
        public Table(string name, string? description, DateTime? expirationDate, DiscountType type, bool active = true)
        {
            Name = name;
            Description = description;
            ExpirationDate = expirationDate;
            Type = type;
            Active = active;
        }

        public Table(string name, DiscountType type)
        {
            Name = name;
            Type = type;
        }
    }
}
