using Domain.Enums;
using Domain.ValueObjects;

namespace Domain
{
    public class Item : Entity
    {
        public readonly Guid ExternalId = Guid.NewGuid();
        public string Description { get; set; }
        public Price Price { get; set; }
        public List<decimal>? PriceSequence { get; set; }
        public List<int>? AmountLimitsToApplyDiscount { get; set; }


        /// <summary>
        /// Construtor de Item para ser usado em tabela de preço fixo.
        /// </summary>
        /// <param name="description">Descrição/Nome do item</param>
        /// <param name="initialPrice">Preço inicial</param>
        /// <param name="costPrice">Preço de custo. Por padrão inicia em 0</param>
        public Item(string description, decimal initialPrice, decimal costPrice = 0)
        {
            Description = description;
            Price = new Price() { InitialValue = initialPrice, CostValue = costPrice };
        }

        /// <summary>
        /// Construtor de Item para ser usado em tabelas de desconto cumulativo e não cumulativo.
        /// </summary>
        /// <param name="description">Descrição/Nome do item</param>
        /// <param name="initialPrice">Preço inicial</param>
        /// <param name="priceSequence">Sequência de preços que serão aplicados conforme os descontos</param>
        /// <param name="amountLimitsToApplyDiscount">Lista dos limites de quantidade para aplicação dos descontos</param>
        /// <param name="costPrice">Preço de custo. Por padrão inicia em 0</param>
        /// <exception cref="ArgumentException"></exception>
        public Item(string description, decimal initialPrice, List<decimal> priceSequence, List<int> amountLimitsToApplyDiscount, decimal costPrice = 0)
        {
            if (priceSequence.Count != amountLimitsToApplyDiscount.Count)
                throw new ArgumentException("priceSequence list must have the same amount of elements as amountLimitsToApplyDiscount");
            
            Description = description;
            Price = new Price() { InitialValue = initialPrice, CostValue = costPrice };
            PriceSequence = priceSequence;
            AmountLimitsToApplyDiscount = amountLimitsToApplyDiscount;
        }
    }
}
