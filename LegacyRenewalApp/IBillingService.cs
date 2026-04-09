namespace LegacyRenewalApp;

public interface IBilingService
{
    void saveInvoice(RenewalInvoice invoice);
    void sendInvoice(Customer customer, RenewalInvoice invoice, string normalizedPlanCode);
}