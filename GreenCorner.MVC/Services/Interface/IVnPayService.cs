using GreenCorner.MVC.Models.VNPay;

namespace GreenCorner.MVC.Services.Interface
{
    public interface IVnPayService
    {
        string CreatePaymentUrl(PaymentInformationModel model, HttpContext context);
        PaymentResponseModel PaymentExecute(IQueryCollection collections);
    }
}
