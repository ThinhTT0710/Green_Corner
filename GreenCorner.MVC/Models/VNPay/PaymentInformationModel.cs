namespace GreenCorner.MVC.Models.VNPay
{
    public class PaymentInformationModel
    {
		public string OrderType { get; set; }
        public double Amount { get; set; }
        public string OrderDescription { get; set; }
        public string UserId { get; set; }
	}
}
