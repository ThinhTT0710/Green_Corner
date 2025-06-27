using Microsoft.AspNetCore.Mvc;

namespace GreenCorner.RewardAPI.Models.DTO
{
    public class ResponseDTO
    {
        public bool IsSuccess { get; set; } = true;
        public string Message { get; set; } = "Success";
        public object Result { get; set; } = null!;
    }
}
