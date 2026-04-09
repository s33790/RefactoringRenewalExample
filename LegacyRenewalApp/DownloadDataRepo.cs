using System;

namespace LegacyRenewalApp;

public class DownloadDataRepo : IDownloadDataRepo
{
    public SubscriptionPlan getPlan(string normalizedPlanCode)
    {
        var planRepository = new SubscriptionPlanRepository();

        var plan = planRepository.GetByCode(normalizedPlanCode);
     
        return plan;
    }
    
    public Customer getCustomer(int customerId)
    {
        var customerRepository = new CustomerRepository();

        var customer = customerRepository.GetById(customerId);

        if (!customer.IsActive)
        {
            throw new InvalidOperationException("Inactive customers cannot renew subscriptions");
        }
        return customer;
    }
}