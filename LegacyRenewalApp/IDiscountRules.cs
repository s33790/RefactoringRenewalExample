namespace LegacyRenewalApp;

public interface IDiscountRules
{
    (decimal discountAmmount, string notes) CalculateDiscount(
        Customer customer,
        SubscriptionPlan plan,
        int seatCount,
        bool useLoyaltyPoints);
}