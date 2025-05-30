using UnityEngine;
using System.Net;
using System.Net.Mail;
using System;
public class EmailVerificationSender : AuthManager
{
    public static EmailVerificationSender Instance;
    private string currentOTP;
    private DateTime otpCreatedTime;
    public string GenerateOTP()
    {
        string otp = UnityEngine.Random.Range(0, 1000000).ToString("D6");
        currentOTP = otp.ToString();
        otpCreatedTime = DateTime.Now;
        return currentOTP;
    }
    private void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        else Instance = this;
    }
    public void SendOTPEmal(string toEmail)
    {
        string otp = GenerateOTP();
        currentOTP = otp;
        string subject = "Mã xác minh tài khoản của bạn";
        string htmlMessage = $@"
        <!DOCTYPE html>
<html lang=""vi"">
<head>
  <meta charset=""UTF-8"" />
  <title>Mã Xác Minh Tài Khoản</title>
  <style>
    body {{
      font-family: Arial, sans-serif;
      background-color: #f4f4f4;
      margin: 0;
      padding: 20px;
      color: #333;
    }}
    .container {{
      max-width: 600px;
      margin: auto;
      background-color: #ffffff;
      padding: 30px;
      border-radius: 8px;
      box-shadow: 0 0 10px rgba(0,0,0,0.1);
    }}
    h2 {{
      color: #2a7ae2;
    }}
    .otp-code {{
      font-size: 2rem;
      font-weight: bold;
      color: #000;
      margin: 20px 0;
      padding: 10px 0;
      border: 2px dashed #2a7ae2;
      text-align: center;
      letter-spacing: 4px;
      user-select: all;
    }}
    p {{
      line-height: 1.6;
      font-size: 1rem;
    }}
    .footer {{
      margin-top: 30px;
      font-size: 0.85rem;
      color: #777;
    }}
  </style>
</head>
<body>
  <div class=""container"">
    <p>Kính chào Quý khách,</p>
    <p>Mã xác minh tài khoản của bạn là:</p>
    <div class=""otp-code"">{otp}</div>
    <p>Vui lòng nhập mã này để hoàn tất quá trình xác minh. Mã chỉ có hiệu lực trong vòng 10 phút.</p>
    <p>Để bảo mật thông tin, vui lòng không chia sẻ mã này với bất kỳ ai.</p>
    <p>Nếu bạn không yêu cầu mã này, vui lòng bỏ qua email này hoặc liên hệ với bộ phận hỗ trợ của chúng tôi.</p>
    <div class=""footer"">
      Trân trọng,<br />
      <strong>[Dự Án Tốt Nghiệp Nhóm The Chickens]</strong>
    </div>
  </div>
</body>
</html>
";
        SendEmail(toEmail, subject, htmlMessage);
    }
    public void SendEmail(string toEmail, string subject, string messageEmail)
    {
        string fromEmail = "nguyenquoctuong007@gmail.com";
        string appPassword = "ygku zmqm bcks zrif";

        MailMessage mail = new MailMessage();
        mail.From = new MailAddress(fromEmail);
        mail.To.Add(toEmail);
        mail.Subject = subject;
        mail.IsBodyHtml = true;
        mail.Body = messageEmail;

        SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
        smtpClient.Credentials = new NetworkCredential(fromEmail, appPassword);
        smtpClient.EnableSsl = true;
        try
        {
            smtpClient.Send(mail);
            message.text = "Email đã được gửi thành công.";
        }
        catch (System.Exception ex)
        {
            message.text = "Gửi email thất bại: " + ex.Message;
        }
    }
    public bool VerifyOTP(string inputOTP)
    {
        if ((DateTime.Now - otpCreatedTime).TotalMinutes > 5)
        {
            message.text = "Mã OTP đã hết hạn.";
            return false;
        }
        if (inputOTP == currentOTP)
        {
            message.text = "Xác thực thành công!";
            PlayerPrefs.DeleteKey("Email");
            return true;
        }
        else
        {
            message.text = "Mã xác thực không chính xác.";
            return false;
        }
    }
    public void ResendOTP()
    {
        SendOTPEmal(PlayerPrefs.GetString("Email"));
    }
}
