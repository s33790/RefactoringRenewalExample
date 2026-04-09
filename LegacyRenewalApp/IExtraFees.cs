namespace LegacyRenewalApp;

public interface IExtraFees
{
    (decimal supportFee, string notes) calculateSupportFee(string planCode, bool inlcudePremiumSupport);

    public (decimal paymentFee, string notes) calculatePaymentFee(
        decimal subtotalAfterDiscount,
        decimal supportFee,
        string normalizedPaymentMethod);
}