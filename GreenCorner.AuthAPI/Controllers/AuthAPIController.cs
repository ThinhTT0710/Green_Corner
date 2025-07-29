using GreenCorner.AuthAPI.Models;
using GreenCorner.AuthAPI.Models.DTO;
using GreenCorner.AuthAPI.Services.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace GreenCorner.AuthAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthAPIController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly UserManager<User> _userManager;
        private readonly IEmailService _emailService;
        protected ResponseDTO _response;
        public AuthAPIController(IAuthService authService, UserManager<User> userManager, IEmailService emailService)
        {
            _authService = authService;
            this._response = new ResponseDTO();
            _userManager = userManager;
            _emailService = emailService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterationRequestDTO registerRequest) 
        {
            var response = await _authService.Register(registerRequest);
            if (!string.IsNullOrEmpty(response))
            {
                _response.Message = response;
                _response.IsSuccess = false;
                _response.Result = null;
                return BadRequest(_response);
            }
            var userEntity = await _userManager.FindByEmailAsync(registerRequest.Email);
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(userEntity);
            var encodeToken = Base64UrlEncoder.Encode(token);
            var confirmationLink = $"https://localhost:7000/Auth/ConfirmEmail?userId={userEntity.Id}&token={encodeToken}";

            await _emailService.SendEmailAsync(registerRequest.Email, "Xác minh Email của bạn", $"<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">\r\n<html dir=\"ltr\" xmlns=\"http://www.w3.org/1999/xhtml\" xmlns:o=\"urn:schemas-microsoft-com:office:office\" lang=\"vi\">\r\n <head>\r\n  <meta charset=\"UTF-8\">\r\n  <meta content=\"width=device-width, initial-scale=1\" name=\"viewport\">\r\n  <meta name=\"x-apple-disable-message-reformatting\">\r\n  <meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\">\r\n  <meta content=\"telephone=no\" name=\"format-detection\">\r\n  <title>New Template</title><!--[if (mso 16)]>\r\n    <style type=\"text/css\">\r\n    a {{text-decoration: none;}}\r\n    </style>\r\n    <![endif]--><!--[if gte mso 9]><style>sup {{ font-size: 100% !important; }}</style><![endif]--><!--[if gte mso 9]>\r\n<noscript>\r\n         <xml>\r\n           <o:OfficeDocumentSettings>\r\n           <o:AllowPNG></o:AllowPNG>\r\n           <o:PixelsPerInch>96</o:PixelsPerInch>\r\n           </o:OfficeDocumentSettings>\r\n         </xml>\r\n      </noscript>\r\n<![endif]-->\r\n  <style type=\"text/css\">.rollover:hover .rollover-first {{\r\n  max-height:0px!important;\r\n  display:none!important;\r\n}}\r\n.rollover:hover .rollover-second {{\r\n  max-height:none!important;\r\n  display:block!important;\r\n}}\r\n.rollover span {{\r\n  font-size:0px;\r\n}}\r\nu + .body img ~ div div {{\r\n  display:none;\r\n}}\r\n#outlook a {{\r\n  padding:0;\r\n}}\r\nspan.MsoHyperlink,\r\nspan.MsoHyperlinkFollowed {{\r\n  color:inherit;\r\n  mso-style-priority:99;\r\n}}\r\na.n {{\r\n  mso-style-priority:100!important;\r\n  text-decoration:none!important;\r\n}}\r\na[x-apple-data-detectors],\r\n#MessageViewBody a {{\r\n  color:inherit!important;\r\n  text-decoration:none!important;\r\n  font-size:inherit!important;\r\n  font-family:inherit!important;\r\n  font-weight:inherit!important;\r\n  line-height:inherit!important;\r\n}}\r\n.d {{\r\n  display:none;\r\n  float:left;\r\n  overflow:hidden;\r\n  width:0;\r\n  max-height:0;\r\n  line-height:0;\r\n  mso-hide:all;\r\n}}\r\n@media only screen and (max-width:600px) {{.bd {{ padding-right:0px!important }} .bc {{ padding-left:0px!important }}  *[class=\"gmail-fix\"] {{ display:none!important }} p, a {{ line-height:150%!important }} h1, h1 a {{ line-height:120%!important }} h2, h2 a {{ line-height:120%!important }} h3, h3 a {{ line-height:120%!important }} h4, h4 a {{ line-height:120%!important }} h5, h5 a {{ line-height:120%!important }} h6, h6 a {{ line-height:120%!important }}  .z p {{ }}   h1 {{ font-size:36px!important; text-align:left }} h2 {{ font-size:26px!important; text-align:left }} h3 {{ font-size:20px!important; text-align:left }} h4 {{ font-size:24px!important; text-align:left }} h5 {{ font-size:20px!important; text-align:left }} h6 {{ font-size:16px!important; text-align:left }}         .z p, .z a {{ font-size:16px!important }}   .u, .u h1, .u h2, .u h3, .u h4, .u h5, .u h6 {{ text-align:center!important }}     .t .rollover:hover .rollover-second, .u .rollover:hover .rollover-second, .v .rollover:hover .rollover-second {{ display:inline!important }}   a.n, button.n {{ font-size:20px!important; padding:10px 20px 10px 20px!important; line-height:120%!important }} a.n, button.n, .r {{ display:inline-block!important }}    .g table, .h table, .i table, .g, .i, .h {{ width:100%!important; max-width:600px!important }} .adapt-img {{ width:100%!important; height:auto!important }}        .h-auto {{ height:auto!important }} }}\r\n@media screen and (max-width:384px) {{.mail-message-content {{ width:414px!important }} }}</style>\r\n </head>\r\n <body class=\"body\" style=\"width:100%;height:100%;-webkit-text-size-adjust:100%;-ms-text-size-adjust:100%;padding:0;Margin:0\">\r\n  <div dir=\"ltr\" class=\"es-wrapper-color\" lang=\"vi\" style=\"background-color:#FAFAFA\"><!--[if gte mso 9]>\r\n\t\t\t<v:background xmlns:v=\"urn:schemas-microsoft-com:vml\" fill=\"t\">\r\n\t\t\t\t<v:fill type=\"tile\" color=\"#fafafa\"></v:fill>\r\n\t\t\t</v:background>\r\n\t\t<![endif]-->\r\n   <table width=\"100%\" cellspacing=\"0\" cellpadding=\"0\" class=\"es-wrapper\" role=\"none\" style=\"mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;padding:0;Margin:0;width:100%;height:100%;background-repeat:repeat;background-position:center top;background-color:#FAFAFA\">\r\n     <tr>\r\n      <td valign=\"top\" style=\"padding:0;Margin:0\">\r\n       <table cellpadding=\"0\" cellspacing=\"0\" align=\"center\" class=\"h\" role=\"none\" style=\"mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;width:100%;table-layout:fixed !important;background-color:transparent;background-repeat:repeat;background-position:center top\">\r\n         <tr>\r\n          <td align=\"center\" style=\"padding:0;Margin:0\">\r\n           <table bgcolor=\"#ffffff\" align=\"center\" cellpadding=\"0\" cellspacing=\"0\" class=\"ba\" role=\"none\" style=\"mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;background-color:transparent;width:600px\">\r\n             <tr>\r\n              <td align=\"left\" style=\"Margin:0;padding-top:10px;padding-right:20px;padding-bottom:10px;padding-left:20px\">\r\n               <table cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" role=\"none\" style=\"mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px\">\r\n                 <tr>\r\n                  <td valign=\"top\" align=\"center\" class=\"bd\" style=\"padding:0;Margin:0;width:560px\">\r\n                   <table cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" role=\"presentation\" style=\"mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px\">\r\n                     <tr>\r\n                      <td align=\"center\" style=\"padding:0;Margin:0;padding-bottom:20px;font-size:0px\"><img src=\"https://elhkujz.stripocdn.email/content/guids/CABINET_10d1d06e75aab920225af45c402856882294e7b952539d26af39746942ad0676/images/image.png\" alt=\"\" width=\"450\" title=\"Logo\" class=\"adapt-img\" style=\"display:block;font-size:12px;border:0;outline:none;text-decoration:none\"></td>\r\n                     </tr>\r\n                   </table></td>\r\n                 </tr>\r\n               </table></td>\r\n             </tr>\r\n           </table></td>\r\n         </tr>\r\n       </table>\r\n       <table cellpadding=\"0\" cellspacing=\"0\" align=\"center\" class=\"g\" role=\"none\" style=\"mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;width:100%;table-layout:fixed !important\">\r\n         <tr>\r\n          <td align=\"center\" style=\"padding:0;Margin:0\">\r\n           <table bgcolor=\"#ffffff\" align=\"center\" cellpadding=\"0\" cellspacing=\"0\" class=\"z\" role=\"none\" style=\"mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;background-color:#FFFFFF;width:600px\">\r\n             <tr>\r\n              <td align=\"left\" style=\"Margin:0;padding-right:20px;padding-left:20px;padding-top:30px;padding-bottom:30px\">\r\n               <table cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" role=\"none\" style=\"mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px\">\r\n                 <tr>\r\n                  <td align=\"center\" valign=\"top\" style=\"padding:0;Margin:0;width:560px\">\r\n                   <table cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" role=\"presentation\" style=\"mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px\">\r\n                     <tr>\r\n                      <td align=\"center\" style=\"padding:0;Margin:0;padding-top:10px;padding-bottom:10px;font-size:0px\"><img src=\"https://elhkujz.stripocdn.email/content/guids/CABINET_67e080d830d87c17802bd9b4fe1c0912/images/55191618237638326.png\" alt=\"\" width=\"100\" style=\"display:block;font-size:14px;border:0;outline:none;text-decoration:none\"></td>\r\n                     </tr>\r\n                     <tr>\r\n                      <td align=\"center\" style=\"padding:0;Margin:0;padding-bottom:10px\"><h1 class=\"u\" style=\"Margin:0;font-family:arial, 'helvetica neue', helvetica, sans-serif;mso-line-height-rule:exactly;letter-spacing:0;font-size:46px;font-style:normal;font-weight:bold;line-height:46px;color:#333333\">Xác nhận email của bạn</h1></td>\r\n                     </tr>\r\n                     <tr>\r\n                      <td align=\"center\" class=\"bd bc\" style=\"Margin:0;padding-top:5px;padding-right:40px;padding-bottom:5px;padding-left:40px\"><p style=\"Margin:0;mso-line-height-rule:exactly;font-family:arial, 'helvetica neue', helvetica, sans-serif;line-height:21px;letter-spacing:0;color:#333333;font-size:14px\">Bạn nhận được thông báo này vì địa chỉ email của bạn đã được đăng ký với trang web của chúng tôi. Vui lòng nhấp vào nút bên dưới để xác minh địa chỉ email của bạn và xác nhận bạn là chủ sở hữu của tài khoản này.</p></td>\r\n                     </tr>\r\n                     <tr>\r\n                      <td align=\"center\" style=\"padding:0;Margin:0;padding-top:10px;padding-bottom:5px\"><p style=\"Margin:0;mso-line-height-rule:exactly;font-family:arial, 'helvetica neue', helvetica, sans-serif;line-height:21px;letter-spacing:0;color:#333333;font-size:14px\">Nếu bạn chưa đăng ký với chúng tôi, vui lòng bỏ qua email này.</p></td>\r\n                     </tr>\r\n                     <tr>\r\n                      <td align=\"center\" style=\"padding:0;Margin:0;padding-top:10px;padding-bottom:10px\"><span class=\"r\" style=\"border-style:solid;border-color:#2CB543;background:#5C68E2;border-width:0px;display:inline-block;border-radius:6px;width:auto\"><a href=\"{confirmationLink}\" target=\"_blank\" class=\"n\" style=\"mso-style-priority:100 !important;text-decoration:none !important;mso-line-height-rule:exactly;color:#FFFFFF;font-size:20px;padding:10px 30px 10px 30px;display:inline-block;background:#5C68E2;border-radius:6px;font-family:arial, 'helvetica neue', helvetica, sans-serif;font-weight:normal;font-style:normal;line-height:24px;width:auto;text-align:center;letter-spacing:0;mso-padding-alt:0;mso-border-alt:10px solid #5C68E2;padding-left:30px;padding-right:30px\">XÁC MINH EMAIL</a></span></td>\r\n                     </tr>\r\n                     <tr>\r\n                      <td align=\"center\" class=\"bd bc\" style=\"Margin:0;padding-top:5px;padding-right:40px;padding-bottom:5px;padding-left:40px\"><p style=\"Margin:0;mso-line-height-rule:exactly;font-family:arial, 'helvetica neue', helvetica, sans-serif;line-height:21px;letter-spacing:0;color:#333333;font-size:14px\">Sau khi được xác nhận, email này sẽ được liên kết duy nhất với tài khoản của bạn.</p></td>\r\n                     </tr>\r\n                   </table></td>\r\n                 </tr>\r\n               </table></td>\r\n             </tr>\r\n           </table></td>\r\n         </tr>\r\n       </table>\r\n       <table cellpadding=\"0\" cellspacing=\"0\" align=\"center\" class=\"i\" role=\"none\" style=\"mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;width:100%;table-layout:fixed !important;background-color:transparent;background-repeat:repeat;background-position:center top\">\r\n         <tr>\r\n          <td align=\"center\" style=\"padding:0;Margin:0\">\r\n           <table align=\"center\" cellpadding=\"0\" cellspacing=\"0\" class=\"y\" style=\"mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;background-color:transparent;width:640px\" role=\"none\">\r\n             <tr>\r\n              <td align=\"left\" style=\"Margin:0;padding-right:20px;padding-left:20px;padding-bottom:20px;padding-top:20px\">\r\n               <table cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" role=\"none\" style=\"mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px\">\r\n                 <tr>\r\n                  <td align=\"left\" style=\"padding:0;Margin:0;width:600px\">\r\n                   <table cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" role=\"presentation\" style=\"mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px\">\r\n                     <tr>\r\n                      <td align=\"center\" style=\"padding:0;Margin:0;padding-bottom:35px\"><h6 style=\"Margin:0;font-family:arial, 'helvetica neue', helvetica, sans-serif;mso-line-height-rule:exactly;letter-spacing:0;font-size:16px;font-style:normal;font-weight:normal;line-height:20.8px;color:#333333\">GREEN CORNER 2025</h6></td>\r\n                     </tr>\r\n                   </table></td>\r\n                 </tr>\r\n               </table></td>\r\n             </tr>\r\n           </table></td>\r\n         </tr>\r\n       </table></td>\r\n     </tr>\r\n   </table>\r\n  </div>\r\n </body>\r\n</html>");
            _response.Message = "Tạo tài khoản thành công, vui lòng kiểm tra email.";
            return Ok(_response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginRequest)
        {
            try
            {
                var loginResponse = await _authService.Login(loginRequest);
                if (loginResponse.User == null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Email hoặc mật khẩu không đúng";
                    return BadRequest(_response);
                }
                var user = await _userManager.FindByEmailAsync(loginRequest.Email);

                bool isEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user);
                if (!isEmailConfirmed)
                {
                    _response.Message = "Vui lòng xác nhận email của bạn trước khi đăng nhập.";
                    _response.IsSuccess = false;
                    return BadRequest(_response);
                }
                _response.Result = loginResponse;
                return Ok(_response);
			}
			catch (Exception ex)
			{
				_response.IsSuccess = false;
				_response.Message = ex.Message;
				return BadRequest(_response);
			}
		}

        [HttpPost("google-login")]
        public async Task<IActionResult> GoogleLogin([FromBody] GoogleLoginRequestDTO googleLoginRequest)
        {
            try
            {
                var loginResponse = await _authService.LoginWithGoogle(googleLoginRequest);
                if (loginResponse.User == null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Đăng nhập với Google không thành công. Vui lòng thử lại";
                    return BadRequest(_response);
                }
                _response.Result = loginResponse;
                return Ok(_response);
			}
			catch (Exception ex)
			{
				_response.IsSuccess = false;
				_response.Message = ex.Message;
				return BadRequest(_response);
			}

		}

        [HttpPost("facebook-login")]
        public async Task<IActionResult> FacebookLogin([FromBody] FacebookLoginRequestDTO facebookLoginRequest)
        {
            try
            {
                var loginResponse = await _authService.LoginWithFacebook(facebookLoginRequest);
                if (loginResponse.User == null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Đăng nhập với Facebook không thành công. Vui lòng thử lại";
                    return BadRequest(_response);
                }

                _response.Result = loginResponse;
                return Ok(_response);
            }
			catch (Exception ex)
			{
				_response.IsSuccess = false;
				_response.Message = ex.Message;
				return BadRequest(_response);
			}
		}

        [HttpPost("assign-role")]
        public async Task<IActionResult> AssignRole([FromBody] RegisterationRequestDTO model)
        {
            var assignRoleSuccess = await _authService.AssignRole(model.Email, model.RoleName.ToUpper());
            if (!assignRoleSuccess)
            {
                _response.IsSuccess = false;
                _response.Message = "Không thể chỉ định vai trò";
                return BadRequest(_response);
            }
            return Ok(_response);
        }

        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId == null || token == null)
            {
                _response.IsSuccess = false;
                _response.Message = "Yêu cầu xác nhận không hợp lệ.";
                return BadRequest(_response);
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                _response.IsSuccess = false;
                _response.Message = "Không tìm thấy người dùng.";
                return NotFound(_response);
            }
            var decodedToken = Base64UrlEncoder.Decode(token);
            var result = await _userManager.ConfirmEmailAsync(user, decodedToken);
            if (result.Succeeded)
            {
                _response.IsSuccess = true;
                _response.Message = "Email đã được xác nhận thành công.";
                return Ok(_response);
            }
            else
            {
                _response.IsSuccess = false;
                _response.Message = "Yêu cầu xác nhận không hợp lệ.";
                return BadRequest(_response);
            }
        }

        [HttpGet("resend-confirm-email")]
        public async Task<IActionResult> ResendConfirmEmail(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                _response.IsSuccess = false;
                _response.Message = "Không tìm thấy người dùng.";
                return NotFound(_response);
            }
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var encodeToken = Base64UrlEncoder.Encode(token);
            var confirmationLink = $"https://localhost:7000/Auth/ConfirmEmail?userId={user.Id}&token={encodeToken}";

            await _emailService.SendEmailAsync(email, "Confirm your email", $"<h1>Welcome to GreenCorner</h1><p>Please confirm your email by <a href='{confirmationLink}'>clicking here</a></p>");
            _response.Message = "Email đã được gửi thành công, vui lòng kiểm tra email.";
            return Ok(_response);
        }

        [HttpGet("email-forgot-password")]
        public async Task<IActionResult> EmailForgotPassword(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                _response.IsSuccess = false;
                _response.Message = "Không tìm thấy người dùng.";
                return NotFound(_response);
            }
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var encodeToken = Base64UrlEncoder.Encode(token);
            var forgotPasswordLink = $"https://localhost:7000/Auth/ForgotPassword?userId={user.Id}&token={encodeToken}";

            await _emailService.SendEmailAsync(email, "Đặt lại mật khẩu của bạn", $"<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">\r\n<html dir=\"ltr\" xmlns=\"http://www.w3.org/1999/xhtml\" xmlns:o=\"urn:schemas-microsoft-com:office:office\" lang=\"vi\">\r\n <head>\r\n  <meta charset=\"UTF-8\">\r\n  <meta content=\"width=device-width, initial-scale=1\" name=\"viewport\">\r\n  <meta name=\"x-apple-disable-message-reformatting\">\r\n  <meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\">\r\n  <meta content=\"telephone=no\" name=\"format-detection\">\r\n  <title>New Template</title><!--[if (mso 16)]>\r\n    <style type=\"text/css\">\r\n    a {{text-decoration: none;}}\r\n    </style>\r\n    <![endif]--><!--[if gte mso 9]><style>sup {{ font-size: 100% !important; }}</style><![endif]--><!--[if gte mso 9]>\r\n<noscript>\r\n         <xml>\r\n           <o:OfficeDocumentSettings>\r\n           <o:AllowPNG></o:AllowPNG>\r\n           <o:PixelsPerInch>96</o:PixelsPerInch>\r\n           </o:OfficeDocumentSettings>\r\n         </xml>\r\n      </noscript>\r\n<![endif]-->\r\n  <style type=\"text/css\">.rollover:hover .rollover-first {{\r\n  max-height:0px!important;\r\n  display:none!important;\r\n}}\r\n.rollover:hover .rollover-second {{\r\n  max-height:none!important;\r\n  display:block!important;\r\n}}\r\n.rollover span {{\r\n  font-size:0px;\r\n}}\r\nu + .body img ~ div div {{\r\n  display:none;\r\n}}\r\n#outlook a {{\r\n  padding:0;\r\n}}\r\nspan.MsoHyperlink,\r\nspan.MsoHyperlinkFollowed {{\r\n  color:inherit;\r\n  mso-style-priority:99;\r\n}}\r\na.n {{\r\n  mso-style-priority:100!important;\r\n  text-decoration:none!important;\r\n}}\r\na[x-apple-data-detectors],\r\n#MessageViewBody a {{\r\n  color:inherit!important;\r\n  text-decoration:none!important;\r\n  font-size:inherit!important;\r\n  font-family:inherit!important;\r\n  font-weight:inherit!important;\r\n  line-height:inherit!important;\r\n}}\r\n.d {{\r\n  display:none;\r\n  float:left;\r\n  overflow:hidden;\r\n  width:0;\r\n  max-height:0;\r\n  line-height:0;\r\n  mso-hide:all;\r\n}}\r\n@media only screen and (max-width:600px) {{.bd {{ padding-right:0px!important }} .bc {{ padding-left:0px!important }}  *[class=\"gmail-fix\"] {{ display:none!important }} p, a {{ line-height:150%!important }} h1, h1 a {{ line-height:120%!important }} h2, h2 a {{ line-height:120%!important }} h3, h3 a {{ line-height:120%!important }} h4, h4 a {{ line-height:120%!important }} h5, h5 a {{ line-height:120%!important }} h6, h6 a {{ line-height:120%!important }}  .z p {{ }}   h1 {{ font-size:36px!important; text-align:left }} h2 {{ font-size:26px!important; text-align:left }} h3 {{ font-size:20px!important; text-align:left }} h4 {{ font-size:24px!important; text-align:left }} h5 {{ font-size:20px!important; text-align:left }} h6 {{ font-size:16px!important; text-align:left }}         .z p, .z a {{ font-size:16px!important }}   .u, .u h1, .u h2, .u h3, .u h4, .u h5, .u h6 {{ text-align:center!important }}     .t .rollover:hover .rollover-second, .u .rollover:hover .rollover-second, .v .rollover:hover .rollover-second {{ display:inline!important }}   a.n, button.n {{ font-size:20px!important; padding:10px 20px 10px 20px!important; line-height:120%!important }} a.n, button.n, .r {{ display:inline-block!important }}    .g table, .h table, .i table, .g, .i, .h {{ width:100%!important; max-width:600px!important }} .adapt-img {{ width:100%!important; height:auto!important }}        .h-auto {{ height:auto!important }} }}\r\n@media screen and (max-width:384px) {{.mail-message-content {{ width:414px!important }} }}</style>\r\n </head>\r\n <body class=\"body\" style=\"width:100%;height:100%;-webkit-text-size-adjust:100%;-ms-text-size-adjust:100%;padding:0;Margin:0\">\r\n  <div dir=\"ltr\" class=\"es-wrapper-color\" lang=\"vi\" style=\"background-color:#FAFAFA\"><!--[if gte mso 9]>\r\n\t\t\t<v:background xmlns:v=\"urn:schemas-microsoft-com:vml\" fill=\"t\">\r\n\t\t\t\t<v:fill type=\"tile\" color=\"#fafafa\"></v:fill>\r\n\t\t\t</v:background>\r\n\t\t<![endif]-->\r\n   <table width=\"100%\" cellspacing=\"0\" cellpadding=\"0\" class=\"es-wrapper\" role=\"none\" style=\"mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;padding:0;Margin:0;width:100%;height:100%;background-repeat:repeat;background-position:center top;background-color:#FAFAFA\">\r\n     <tr>\r\n      <td valign=\"top\" style=\"padding:0;Margin:0\">\r\n       <table cellpadding=\"0\" cellspacing=\"0\" align=\"center\" class=\"h\" role=\"none\" style=\"mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;width:100%;table-layout:fixed !important;background-color:transparent;background-repeat:repeat;background-position:center top\">\r\n         <tr>\r\n          <td align=\"center\" style=\"padding:0;Margin:0\">\r\n           <table bgcolor=\"#ffffff\" align=\"center\" cellpadding=\"0\" cellspacing=\"0\" class=\"ba\" role=\"none\" style=\"mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;background-color:transparent;width:600px\">\r\n             <tr>\r\n              <td align=\"left\" style=\"Margin:0;padding-top:10px;padding-right:20px;padding-bottom:10px;padding-left:20px\">\r\n               <table cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" role=\"none\" style=\"mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px\">\r\n                 <tr>\r\n                  <td valign=\"top\" align=\"center\" class=\"bd\" style=\"padding:0;Margin:0;width:560px\">\r\n                   <table cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" role=\"presentation\" style=\"mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px\">\r\n                     <tr>\r\n                      <td align=\"center\" style=\"padding:0;Margin:0;padding-bottom:20px;font-size:0px\"><img src=\"https://elhkujz.stripocdn.email/content/guids/CABINET_10d1d06e75aab920225af45c402856882294e7b952539d26af39746942ad0676/images/image.png\" alt=\"\" width=\"450\" title=\"Logo\" class=\"adapt-img\" style=\"display:block;font-size:12px;border:0;outline:none;text-decoration:none\"></td>\r\n                     </tr>\r\n                   </table></td>\r\n                 </tr>\r\n               </table></td>\r\n             </tr>\r\n           </table></td>\r\n         </tr>\r\n       </table>\r\n       <table cellpadding=\"0\" cellspacing=\"0\" align=\"center\" class=\"g\" role=\"none\" style=\"mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;width:100%;table-layout:fixed !important\">\r\n         <tr>\r\n          <td align=\"center\" style=\"padding:0;Margin:0\">\r\n           <table bgcolor=\"#ffffff\" align=\"center\" cellpadding=\"0\" cellspacing=\"0\" class=\"z\" role=\"none\" style=\"mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;background-color:#FFFFFF;width:600px\">\r\n             <tr>\r\n              <td align=\"left\" style=\"Margin:0;padding-right:20px;padding-left:20px;padding-top:30px;padding-bottom:30px\">\r\n               <table cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" role=\"none\" style=\"mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px\">\r\n                 <tr>\r\n                  <td align=\"center\" valign=\"top\" style=\"padding:0;Margin:0;width:560px\">\r\n                   <table cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" role=\"presentation\" style=\"mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px\">\r\n                     <tr>\r\n                      <td align=\"center\" style=\"padding:0;Margin:0;padding-top:10px;padding-bottom:10px;font-size:0px\"><img src=\"https://elhkujz.stripocdn.email/content/guids/CABINET_67e080d830d87c17802bd9b4fe1c0912/images/55191618237638326.png\" alt=\"\" width=\"100\" style=\"display:block;font-size:14px;border:0;outline:none;text-decoration:none\"></td>\r\n                     </tr>\r\n                     <tr>\r\n                      <td align=\"center\" style=\"padding:0;Margin:0;padding-bottom:10px\"><h1 class=\"u\" style=\"Margin:0;font-family:arial, 'helvetica neue', helvetica, sans-serif;mso-line-height-rule:exactly;letter-spacing:0;font-size:46px;font-style:normal;font-weight:bold;line-height:46px;color:#333333\">Đặt lại mật khẩu của bạn</h1></td>\r\n                     </tr>\r\n                     <tr>\r\n                      <td align=\"center\" class=\"bd bc\" style=\"Margin:0;padding-top:5px;padding-right:40px;padding-bottom:5px;padding-left:40px\"><p style=\"Margin:0;mso-line-height-rule:exactly;font-family:arial, 'helvetica neue', helvetica, sans-serif;line-height:21px;letter-spacing:0;color:#333333;font-size:14px\">Bạn nhận được thông báo này vì bạn đã yêu cầu đặt lại mật khẩu cho tài khoản của mình trên trang web của chúng tôi.</p></td>\r\n                     </tr>\r\n                     <tr>\r\n                      <td align=\"center\" style=\"padding:0;Margin:0;padding-top:10px;padding-bottom:5px\"><p style=\"Margin:0;mso-line-height-rule:exactly;font-family:arial, 'helvetica neue', helvetica, sans-serif;line-height:21px;letter-spacing:0;color:#333333;font-size:14px\">Vui lòng nhấp vào nút bên dưới để <strong>thiết lập mật khẩu mới</strong> cho tài khoản của bạn:</p></td>\r\n                     </tr>\r\n                     <tr>\r\n                      <td align=\"center\" style=\"padding:0;Margin:0;padding-top:10px;padding-bottom:10px\"><span class=\"r\" style=\"border-style:solid;border-color:#2CB543;background:#5C68E2;border-width:0px;display:inline-block;border-radius:6px;width:auto\"><a href=\"{forgotPasswordLink}\" target=\"_blank\" class=\"n\" style=\"mso-style-priority:100 !important;text-decoration:none !important;mso-line-height-rule:exactly;color:#FFFFFF;font-size:20px;padding:10px 30px 10px 30px;display:inline-block;background:#5C68E2;border-radius:6px;font-family:arial, 'helvetica neue', helvetica, sans-serif;font-weight:normal;font-style:normal;line-height:24px;width:auto;text-align:center;letter-spacing:0;mso-padding-alt:0;mso-border-alt:10px solid #5C68E2;padding-left:30px;padding-right:30px\">Đặt lại mật khẩu</a></span></td>\r\n                     </tr>\r\n                     <tr>\r\n                      <td align=\"center\" class=\"bd bc\" style=\"Margin:0;padding-top:5px;padding-right:40px;padding-bottom:5px;padding-left:40px\"><p style=\"Margin:0;mso-line-height-rule:exactly;font-family:arial, 'helvetica neue', helvetica, sans-serif;line-height:21px;letter-spacing:0;color:#333333;font-size:14px\">Nếu bạn <strong>không yêu cầu</strong> đổi mật khẩu, bạn có thể <strong>bỏ qua email này một cách an toàn</strong> – sẽ không có thay đổi nào được thực hiện.</p></td>\r\n                     </tr>\r\n                   </table></td>\r\n                 </tr>\r\n               </table></td>\r\n             </tr>\r\n           </table></td>\r\n         </tr>\r\n       </table>\r\n       <table cellpadding=\"0\" cellspacing=\"0\" align=\"center\" class=\"i\" role=\"none\" style=\"mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;width:100%;table-layout:fixed !important;background-color:transparent;background-repeat:repeat;background-position:center top\">\r\n         <tr>\r\n          <td align=\"center\" style=\"padding:0;Margin:0\">\r\n           <table align=\"center\" cellpadding=\"0\" cellspacing=\"0\" class=\"y\" style=\"mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;background-color:transparent;width:640px\" role=\"none\">\r\n             <tr>\r\n              <td align=\"left\" style=\"Margin:0;padding-right:20px;padding-left:20px;padding-bottom:20px;padding-top:20px\">\r\n               <table cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" role=\"none\" style=\"mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px\">\r\n                 <tr>\r\n                  <td align=\"left\" style=\"padding:0;Margin:0;width:600px\">\r\n                   <table cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" role=\"presentation\" style=\"mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px\">\r\n                     <tr>\r\n                      <td align=\"center\" style=\"padding:0;Margin:0;padding-bottom:35px\"><h6 style=\"Margin:0;font-family:arial, 'helvetica neue', helvetica, sans-serif;mso-line-height-rule:exactly;letter-spacing:0;font-size:16px;font-style:normal;font-weight:normal;line-height:20.8px;color:#333333\">GREEN CORNER 2025</h6></td>\r\n                     </tr>\r\n                   </table></td>\r\n                 </tr>\r\n               </table></td>\r\n             </tr>\r\n           </table></td>\r\n         </tr>\r\n       </table></td>\r\n     </tr>\r\n   </table>\r\n  </div>\r\n </body>\r\n</html>");
            _response.Message = "Email đã được gửi thành công, vui lòng kiểm tra email.";
            return Ok(_response);
        }

        [HttpPost("fotgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordRequestDTO forgotPasswordRequest)
        {
            var user = await _userManager.FindByIdAsync(forgotPasswordRequest.UserId);
            if (user == null)
            {
                _response.IsSuccess = false;
                _response.Message = "Không tìm thấy người dùng.";
                return NotFound(_response);
            }
            var decodedToken = Base64UrlEncoder.Decode(forgotPasswordRequest.Token);
            var result = await _userManager.ResetPasswordAsync(user, decodedToken, forgotPasswordRequest.Password);
            if (result.Succeeded)
            {
                _response.IsSuccess = true;
                _response.Message = "Đã đặt lại mật khẩu thành công.";
                return Ok(_response);
            }
            else
            {
                _response.IsSuccess = false;
                _response.Message = "Không thể đặt lại mật khẩu.";
                return BadRequest(_response);
            }
        }

        [HttpGet("GetSecurityStamp/{userId}")]
        public async Task<IActionResult> GetSecurityStamp(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                _response.IsSuccess = false;
                _response.Message = "Không tìm thấy người dùng.";
                return NotFound(_response);
            }
            else
            {
                _response.IsSuccess = true;
                _response.Result = user.SecurityStamp;
                _response.Message = "Đã đặt lại mật khẩu thành công.";
                return Ok(_response);
            }
        }
    }
}
