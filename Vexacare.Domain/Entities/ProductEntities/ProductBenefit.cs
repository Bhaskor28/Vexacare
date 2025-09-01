namespace Vexacare.Domain.Entities.ProductEntities
{
    //map table of product and benefit
    public class ProductBenefit
    {
        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int BenefitId { get; set; }
        public Benefit Benefit { get; set; }

    }
}
