using Domain.Entities.Enums;
using Domain.Primitives;
using Domain.ValueObjects;

namespace Domain.Entities
{
    public class Item : Entity
    {
        public readonly Guid ExternalId = Guid.NewGuid();
        public string Description { get; set; }
        public Price Price { get; set; }
        public DateTime PurchaseDate { get; } = DateTime.Now;

        public Item()
        {
        }
        
        /// <summary>
        /// Cria entidade Item para ser usado em tabela de preço fixo.
        /// </summary>
        /// <param name="description">Descrição do item</param>
        /// <param name="initialPrice">Preço inicial</param>
        public Item(string description, decimal initialPrice)
        {
            Description = description;
            Price = new Price() { InitialValue = initialPrice };
        }

        /// <summary>
        /// Cria entidade Item para ser usado em tabelas de desconto cumulativo e não cumulativo.
        /// </summary>
        /// <param name="description">Descrição/Nome do item</param>
        /// <param name="initialPrice">Preço inicial</param>
        /// <param name="priceSequence">Sequência de preços que serão aplicados conforme os descontos</param>
        /// <param name="amountLimitsToApplyDiscount">Lista dos limites de quantidade para aplicação dos descontos</param>
        /// <param name="type">Tipo do desconto, cumulativo ou nao cumulativo</param>
        /// <exception cref="ArgumentException"></exception>
        public Item(string description, decimal initialPrice, List<decimal> priceSequence, List<int> amountLimitsToApplyDiscount)
        {
            if (priceSequence.Count != amountLimitsToApplyDiscount.Count)
                throw new ArgumentException("priceSequence list must have the same amount of elements as amountLimitsToApplyDiscount");
            
            Description = description;
            Price = new Price()
            {
                InitialValue = initialPrice, 
                PriceSequence = priceSequence,
                AmountLimitsToApplyDiscount = amountLimitsToApplyDiscount
            };
        }
    }
}
