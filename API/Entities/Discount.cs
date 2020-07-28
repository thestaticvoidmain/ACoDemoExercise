namespace API.Entities
{
  public class Discount : BaseEntity
  {
    public string Description { get; set; }
    public int MinQuantity { get; set; }
    public int DiscountValue { get; set; }
    public DiscountType DiscountType { get; set; }
    public int DiscountTypeId { get; set; }

  }

  public class DiscountApplied
  {
    public DiscountApplied(int packageId)
    {
      PackageId = packageId;
    }

    public int PackageId { get; set; }
    public int AddedQuantity { get; set; }
    public int DiscountedPrice { get; set; }
  }
}