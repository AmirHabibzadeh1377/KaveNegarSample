using Kavenegar;

using Microsoft.AspNetCore.Mvc;

namespace SendSms.Controllers
{
    public class KaveNegarController : Controller
    {
        [HttpPost]
        public IActionResult SendSms(string phoneNumber)
        {
            string userPhoneNumber = "0";  // شماره تلفن کاربر
            string otpCode = GenerateOtp();          // تولید OTP
            Console.WriteLine($"Generated OTP: {otpCode}");  // برای تست

            SendOtp(phoneNumber, otpCode);  // ارسال OTP

            // در این مرحله باید از کاربر بخواهیم که OTP را وارد کند
            Console.Write("کد تایید شما: ");
            string userInputOtp = Console.ReadLine();

            var verified =  VerifyOtp(userInputOtp, otpCode);

            if (verified)
            {
                Console.WriteLine("باموفقیت وارد سیستم شدید");
            }
            else
            {
                Console.WriteLine("کیرم دهنت");
            }

            return View();
        }

        static string GenerateOtp()
        {
            Random random = new Random();
            string otp = string.Join("", Enumerable.Range(0, 6).Select(n => random.Next(0, 10).ToString()));
            return otp;
        }

        static void SendOtp(string phoneNumber, string otp)
        {
            var api = new KavenegarApi("6932705A31324159307A55415571714F4D65693179357A372B4950722B54597A716A654B4C51356D722F633D");

            string message = $"کد تایید شما: {otp}";  // پیام برای ارسال

            try
            {
                var result = api.Send("",phoneNumber, message);
                //foreach (var r in result.)
                //{
                //    Console.WriteLine($"MessageId: {r.Messageid}, Status: {r.StatusText}");
                //}

                var messageId = result.Messageid;
                var statusText = result.StatusText;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending OTP: {ex.Message}");
            }
        }

        // تابع برای بررسی صحت OTP وارد شده
        static bool VerifyOtp(string userInputOtp, string correctOtp)
        {
            return userInputOtp == correctOtp;
        }
    }
}
