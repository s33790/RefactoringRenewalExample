namespace LegacyRenewalApp;

public interface IDownloadDataRepo
{
    SubscriptionPlan getPlan(string normalizedPlanCode);

    Customer getCustomer(int customerId);
}