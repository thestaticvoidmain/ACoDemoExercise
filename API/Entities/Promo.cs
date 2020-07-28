using System;

namespace API.Entities
{
    public class Promo : BaseEntity
    {
        public Package Package { get; set; }
        public int PackageId { get; set; }
        public Discount Discount {get;set;}
        public int DiscountId {get;set;}
        public DateTime StartDate {get;set;}
        public DateTime EndDate {get; set; }
    }

    public class ActivePromo
    {
        public int PackageId { get; set; }
        public int MinQty { get; set; }
        public int DiscountValue { get; set; }
        public int DiscountTypeId { get; set; }
    }
}