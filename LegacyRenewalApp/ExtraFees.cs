using System;

namespace LegacyRenewalApp;

public class ExtraFees : IExtraFees
{
    public (decimal supportFee, string notes) calculateSupportFee(string planCode, bool inlcudePremiumSupport)
    {
        decimal supportFee = 0m;
        string notes = string.Empty;
        if (inlcudePremiumSupport)
        {
            if (planCode == "START")
            {
                supportFee = 250m;
            }
            else if (planCode == "PRO")
            {
                supportFee = 400m;
            }
            else if (planCode == "ENTERPRISE")
            {
                supportFee = 700m;
            }

            notes += "premium support included; ";
        }
        
        return (supportFee, notes);
    }

    public (decimal paymentFee, string notes) calculatePaymentFee(
        decimal subtotalAfterDiscount,
        decimal supportFee, 
        string normalizedPaymentMethod)
    {
        decimal paymentFee = 0m;
        string notes = string.Empty;
        if (normalizedPaymentMethod == "CARD")
        {
            paymentFee = (subtotalAfterDiscount + supportFee) * 0.02m;
            notes += "card payment fee; ";
        }
        else if (normalizedPaymentMethod == "BANK_TRANSFER")
        {
            paymentFee = (subtotalAfterDiscount + supportFee) * 0.01m;
            notes += "bank transfer fee; ";
        }
        else if (normalizedPaymentMethod == "PAYPAL")
        {
            paymentFee = (subtotalAfterDiscount + supportFee) * 0.035m;
            notes += "paypal fee; ";
        }
        else if (normalizedPaymentMethod == "INVOICE")
        {
            paymentFee = 0m;
            notes += "invoice payment; ";
        }
        else
        {
            throw new ArgumentException("Unsupported payment method");
        }

        return (paymentFee, notes);
    }
}