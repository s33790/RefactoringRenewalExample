namespace LegacyRenewalApp;

public interface IInputValidator
{
    void validaInput(int customerId, string planCode, int seatCount, string paymentMethod);
}