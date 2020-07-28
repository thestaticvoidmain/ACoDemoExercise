namespace API.Entities
{
  public class Package : BaseEntity
  {
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public string ImgURL { get; set; }
  }

  public class BasketItem
  {
    public int PackageId { get; set; }
    public int Price { get; set; }
    public int Quantity { get; set; }
    public string PackageName { get; set; }
    public string ImgUrl { get; set; }
  }
}