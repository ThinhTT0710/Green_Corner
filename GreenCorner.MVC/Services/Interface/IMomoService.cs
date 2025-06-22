using GreenCorner.MVC.Models;
using GreenCorner.MVC.Models.Momo;

namespace GreenCorner.MVC.Services.Interface
{
	public interface IMomoService
	{
		Task<MomoCreatePaymentResponseModel> CreatePaymentAsync(OrderInfoModel model);
		MomoExecuteResponseModel PaymentExecuteAsync(IQueryCollection collection);
	}
}
