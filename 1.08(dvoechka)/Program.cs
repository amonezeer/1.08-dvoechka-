using System;
using System.Collections.Generic;

enum TaxType
{
    VAT18,
    VAT10,
    NoVAT
}
enum DiscountType
{
    None,
    Seasonal,
    Promotional
}
struct Product
{
    public string Name { get; }
    public int Quantity { get; }
    public decimal PricePerUnit { get; }
    public DiscountType Discount { get; }

    public Product(string name, int quantity, decimal pricePerUnit, DiscountType discount)
    {
        Name = name;
        Quantity = quantity;
        PricePerUnit = pricePerUnit;
        Discount = discount;
    }

    public decimal GetDiscountedPrice()
    {
        decimal discountAmount = 0;
        switch (Discount)
        {
            case DiscountType.Seasonal:
                discountAmount = 0.10m; 
                break;
            case DiscountType.Promotional:
                discountAmount = 0.05m; 
                break;
        }
        return PricePerUnit * (1 - discountAmount) * Quantity;
    }

    public override string ToString()
    {
        return $"{Name} x{Quantity} @ {PricePerUnit:C2} = {GetDiscountedPrice():C2} (Скидка: {Discount})";
    }
}
struct Receipt
{
    private List<Product> _products;
    public TaxType Tax { get; }

    public Receipt(TaxType tax)
    {
        _products = new List<Product>();
        Tax = tax;
    }
    public void AddProduct(Product product)
    {
        _products.Add(product);
    }
    public decimal CalculateTotal()
    {
        decimal subtotal = 0;
        foreach (var product in _products)
        {
            subtotal += product.GetDiscountedPrice();
        }

        decimal taxAmount = 0;
        switch (Tax)
        {
            case TaxType.VAT18:
                taxAmount = subtotal * 0.18m;
                break;
            case TaxType.VAT10:
                taxAmount = subtotal * 0.10m;
                break;
        }

        return subtotal + taxAmount;
    }
    public void PrintReceipt()
    {
        Console.BackgroundColor = ConsoleColor.Green;
        Console.WriteLine("кассовый чек : ");
        Console.BackgroundColor = ConsoleColor.DarkRed;
        Console.WriteLine("|---------------------------|");

        foreach (var product in _products)
        {
            Console.WriteLine(product.ToString());
        }

        Console.WriteLine("|---------------------------|");
        Console.BackgroundColor = ConsoleColor.Blue;
        Console.WriteLine($"итоговая сумма: {CalculateTotal():C2} (налог: {Tax})");
    }
}

class Program
{
    static void Main()
    {
        Receipt receipt = new Receipt(TaxType.VAT18);

        receipt.AddProduct(new Product("чипсеки", 2, 50m, DiscountType.Seasonal));
        receipt.AddProduct(new Product("пепса", 1, 30m, DiscountType.None));
        receipt.AddProduct(new Product("морожени", 3, 100m, DiscountType.Promotional));

        receipt.PrintReceipt();
        Console.BackgroundColor= ConsoleColor.DarkGray;
    }
}
