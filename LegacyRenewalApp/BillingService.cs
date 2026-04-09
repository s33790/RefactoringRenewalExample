namespace LegacyRenewalApp;

public class BillingService : IBilingService
{
    public void saveInvoice(RenewalInvoice invoice)
    {
        LegacyBillingGateway.SaveInvoice(invoice);
    }

    public void sendInvoice(Customer customer, RenewalInvoice invoice, string normalizedPlanCode)
    {
        if (!string.IsNullOrWhiteSpace(customer.Email))
        {
            string subject = "Subscription renewal invoice";
            string body =
                $"Hello {customer.FullName}, your renewal for plan {normalizedPlanCode} " +
                $"has been prepared. Final amount: {invoice.FinalAmount:F2}.";

            LegacyBillingGateway.SendEmail(customer.Email, subject, body);
        }
    }
}