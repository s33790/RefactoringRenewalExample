
namespace LegacyRenewalApp
{
    public class SubscriptionRenewalService
    {
        private readonly IInputValidator _inputValidator;
        private readonly IDownloadDataRepo _downloadDataRepo;
        private readonly IDiscountRules _discountRules;
        private readonly ITaxPolicy _taxPolicy;
        private readonly IExtraFees _extraFees;
        private readonly IInvoiceFactory _invoiceFactory;
        private readonly IBilingService _legacyBilingService;

        public SubscriptionRenewalService(IInputValidator inputValidator, IDownloadDataRepo downloadDataRepo, 
            IDiscountRules discountRules, ITaxPolicy taxPolicy,  IExtraFees extraFees,
            IInvoiceFactory invoiceFactory,  IBilingService legacyBilingService)
        {
            _inputValidator = inputValidator;
            _downloadDataRepo = downloadDataRepo;
            _discountRules = discountRules;
            _taxPolicy = taxPolicy;
            _extraFees = extraFees;
            _invoiceFactory = invoiceFactory;
            _legacyBilingService = legacyBilingService;
        }

        public SubscriptionRenewalService() : this(
            new InputValidator(),
            new DownloadDataRepo(),
            new DiscountRules(),
            new TaxPolicy(),
            new ExtraFees(),
            new InvoiceBuild(),
            new BillingService()
        ){}
        public RenewalInvoice CreateRenewalInvoice(
            int customerId,
            string planCode,
            int seatCount,
            string paymentMethod,
            bool includePremiumSupport,
            bool useLoyaltyPoints)
        {
            //========= VALIDATE 
            _inputValidator.validaInput(customerId, planCode, seatCount, paymentMethod);

            string notes = string.Empty;
            string normalizedPlanCode = planCode.Trim().ToUpperInvariant();
            string normalizedPaymentMethod = paymentMethod.Trim().ToUpperInvariant();
            
            //========= CUSTOMER REPO
            var customer = _downloadDataRepo.getCustomer(customerId);
            var plan = _downloadDataRepo.getPlan(normalizedPlanCode);
            
            //========= DISCOUNTS
            decimal baseAmount = (plan.MonthlyPricePerSeat * seatCount * 12m) + plan.SetupFee;
            var (discountAmount, discountNotes) = _discountRules.CalculateDiscount(
                customer, 
                plan, 
                seatCount, 
                useLoyaltyPoints);

            notes += discountNotes;
            decimal subtotalAfterDiscount = baseAmount - discountAmount;
            
            //===========EXTRA FEES
            var (supportFee, supportFeeNotes) = _extraFees.calculateSupportFee(planCode, includePremiumSupport);
            notes += supportFeeNotes;
            
            var (paymentFee, paymentFeeNotes) =
                _extraFees.calculatePaymentFee(subtotalAfterDiscount, supportFee, normalizedPaymentMethod);
            notes += paymentFeeNotes;
            
            //============TAX FEEES
            var (finalAmount, taxAmount, taxNotes) = _taxPolicy.calculateTax(
                customer, subtotalAfterDiscount, supportFee, paymentFee
                );
            notes += taxNotes;

            //=========INVOICE BUILDUP
            var invoice = _invoiceFactory.CreateInvoice(
                customer,
                customerId,
                normalizedPlanCode, 
                normalizedPaymentMethod,
                seatCount, 
                baseAmount, 
                discountAmount,
                supportFee, 
                paymentFee, 
                taxAmount, 
                finalAmount, 
                notes);
            //===========SEND EMAIL
            _legacyBilingService.saveInvoice(invoice);
            _legacyBilingService.sendInvoice(customer, invoice, normalizedPlanCode);

            return invoice;
        }
    }
}