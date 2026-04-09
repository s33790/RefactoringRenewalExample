using System;

namespace LegacyRenewalApp;

public class TaxPolicy : ITaxPolicy
{
    public (decimal finalamount, decimal taxAmount, string notes) calculateTax(
        Customer customer,
        decimal subtotalAfterDiscount, 
        decimal supportFee, 
        decimal paymentFee)
    {
        string notes = string.Empty;
        decimal taxRate = customer.Country switch
        {
            "Poland" => 0.23m,
            "Germany" => 0.19m,
            "Czech Republic" => 0.21m,
            "Norway" => 0.25m,
            _ => 0.20m
        };

        decimal taxBase = subtotalAfterDiscount + supportFee + paymentFee;
        decimal taxAmount = taxBase * taxRate;
        decimal finalAmount = taxBase + taxAmount;

        if (finalAmount < 500m)
        {
            finalAmount = 500m;
            notes += "minimum invoice amount applied; ";
        }
        return (finalAmount, taxAmount, notes);
    }
}