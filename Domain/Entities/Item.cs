using Domain.Enums;

namespace Domain
{
    public class Item : Entity
    {
        public readonly Guid ExternalId = Guid.NewGuid();

        public string Description { get; set; }
        public double InitalPrice { get; set; }
        public List<double>? PriceSequence { get; set; }
        public List<int>? AmountLimitsToApplyDiscount { get; set; }
        public readonly ItemType Type;


        /// <summary>
        /// Construtor de Item para ser usado em tabela de preço fixo.
        /// </summary>
        /// <param name="description">Descrição/Nome do item</param>
        /// <param name="initialPrice">Preço inicial</param>
        public Item(string description, double initialPrice)
        {
            Description = description;
            InitalPrice = initialPrice;
            Type = 0;
        }

        /// <summary>
        /// Construtor de Item para ser usado em tabelas de desconto cumulativo e não cumulativo.
        /// </summary>
        /// <param name="description">Descrição/Nome do item</param>
        /// <param name="initialPrice">Preço inicial</param>
        /// <param name="priceSequence">Sequência de preços que serão aplicados conforme os descontos</param>
        /// <param name="amountLimitsToApplyDiscount">Lista dos limites de quantidade para aplicação dos descontos</param>
        /// <exception cref="ArgumentException"></exception>
        public Item(string description, double initialPrice, List<double> priceSequence, List<int> amountLimitsToApplyDiscount)
        {
            if (priceSequence.Count != amountLimitsToApplyDiscount.Count - 1)
                throw new ArgumentException("priceSequence list must have one more element than changePriceLimits list");
            
            Description = description;
            InitalPrice = initialPrice;
            PriceSequence = priceSequence;
            AmountLimitsToApplyDiscount = amountLimitsToApplyDiscount;
        }
    }
}
