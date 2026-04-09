namespace LegacyRenewalApp;

public interface ITaxPolicy
{
    public (decimal finalamount, decimal taxAmount, string notes) calculateTax(
        Customer customer,
        decimal subtotalAfterDiscount,
        decimal supportFee, 
        decimal paymentFee);
}