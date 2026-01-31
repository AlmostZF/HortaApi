using System.ComponentModel.DataAnnotations.Schema;
using DDDPractice.DDDPractice.Domain.Enums;

namespace HortaGestao.Domain.Entities;

public class ProductEntity
{
    public Guid Id { get; private set; }
    public string Name { get; private set;}
    public ProductType ProductType { get; private set; }
    public decimal UnitPrice { get; private set; }
    public Guid SellerId { get; private set; }
    public virtual SellerEntity Seller { get; private set; }
    public string ConservationDays { get; private set; }

    public string Image { get; private set; }

    public string ShortDescription { get; private set; }
    
    public string LargeDescription { get; private set; }
    
    public string Weight { get; private set; }
    
    public bool IsActive { get; private set; }

    public ProductEntity( string name, ProductType productType, decimal unitPrice, Guid sellerId,
        string conservationDays, string image, string shortDescription, string largeDescription,
        string weight)
    {
        if (unitPrice <= 0) throw new ArgumentException("Unit price must be greater than zero.");
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Product name is required.");
        
        Id = Guid.NewGuid();
        Name = name;
        ProductType = productType;
        UnitPrice = unitPrice;
        SellerId = sellerId;
        ConservationDays = conservationDays;
        Image = image;
        ShortDescription = shortDescription;
        LargeDescription = largeDescription;
        Weight = weight;
        IsActive = false;
    }


    public void UpdateProductEntity(string name, string productType, decimal unitPrice, Guid sellerId,
        string conservationDays, string image, string shortDescription, string largeDescription,
        string weight, bool isActive)
    {
        Name = name;
        ProductType = StringToProductType(productType);
        UnitPrice = unitPrice;
        SellerId = sellerId;
        ConservationDays = conservationDays;
        Image = image;
        ShortDescription = shortDescription;
        LargeDescription = largeDescription;
        Weight = weight;
        IsActive = isActive;
    }

    public static ProductType StringToProductType(string productType)
    {
        if (Enum.TryParse<ProductType>(productType, ignoreCase: true, out var categoryEnum))
        {
           return categoryEnum;
        }

        return categoryEnum;
    }
    
    

    public void UpdatePrice(decimal newPrice)
    {
        if (newPrice <= 0)
            throw new ArgumentException("The new price must be greater than zero.");
        
        UnitPrice = newPrice;
    }

    public void Deactivate() => IsActive = false;
    public void Activate() => IsActive = true;
    
    
}