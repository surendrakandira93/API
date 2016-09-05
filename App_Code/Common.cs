using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Net.Mail;
using System.Data;
using System.Configuration;

/// <summary>
/// Summary description for Common
/// </summary>
public static class Common
{
    #region GetClientIp
    public static string GetClientIP
    {
        get
        {
            String ClientIP;
            try
            {
                ClientIP = HttpContext.Current.Request.UserHostAddress;
            }
            catch (Exception ex)
            {
                ClientIP = "Error";
            }
            return ClientIP;
        }
    }
    #endregion
  
   

    public static string GetUserFriendlyMessage(string message)
    {
        string UFmessage = string.Empty;
        if (message.Contains("cvv") || message.Contains("credit card") || message.Contains("credit card"))
        {
            UFmessage= "Enrollment failed due to failing Credit Card processing. please try with different card";
        }
        return UFmessage;
    }
	 public static byte[] ReadFully(Stream input)
    {
        MemoryStream ms = new MemoryStream();
        input.CopyTo(ms);
        return ms.ToArray();
    }
    public static byte[] StreamToByteArray(Stream stream)
    {
        if (stream is MemoryStream)
        {
            return ((MemoryStream)stream).ToArray();
        }
        else
        {
            return ReadFully(stream);
        }
    }
    public static string Coalesce(params string[] strings)
    {
        return strings.Where(s => !string.IsNullOrEmpty(s)).FirstOrDefault();
    }

    public static string DisplayNameQuery
    {
        get
        {

            return @"(CASE WHEN ISNULL(c.Field3,'') != '' THEN c.Field3
						 	WHEN ISNULL(cs.WebAlias,'') != '' THEN cs.WebAlias
						 	WHEN ISNULL(c.Company,'') != '' THEN c.Company
							WHEN ISNULL(c.FirstName,'') != '' THEN c.FirstName
							WHEN ISNULL(c.LastName,'') != '' THEN c.LastName
							ELSE '' END)";
        }
    }
    public static string GetUserFriendlyMessageOrder(string message)
    {
        string UFmessage = string.Empty;
        if (message.ToLower().Contains("cvv"))
        {
            UFmessage = "Order Process failed due to failing CVV processing. please try with valid CVV number";
        }
        else if (message.ToLower().Contains("invalid credit card") || message.ToLower().Contains("invalid card"))
        {
            UFmessage = "Order Process failed due to  Invalid Credit Card Number. please try again with different card";
        }
        else if (message.ToLower().Contains("activity limit"))
        {
            UFmessage = "Order Process failed due to Activity limit exceeded. please try again later or with different card";
        }
        else if (message.ToLower().Contains("credit limit"))
        {
            UFmessage = "Order Process failed due to Credit limit exceeded. please try with different card";
        }
        else if (message.ToLower().Contains("issuer decline") || message.ToLower().Contains("transaction not permitted by issuer"))
        {
            UFmessage = "Order Process failed due to transaction not permitted by issuer. please try with different card";
        }
        else if (message.ToLower().Contains("general error"))
        {
            UFmessage = "Order Process failed due to general error. please try with different card";
        }
        else if (message.ToLower().Contains("the cc payment type [jcb] and/or currency [usd] is not accepted"))
        {
            UFmessage = "Order Process failed due to the cc payment type [jcb] and/or currency [usd] is not acceptedr. please try with different card";
        }
        else if (message.ToLower().Contains("object reference") || message.ToLower().Contains("valid value"))
        {
            UFmessage = "Order Process failed due to invalid request parameter. Please try again";
        }
        return UFmessage;
    }
    #region Encryption/Decryption methods
    public static string Decrypt(string coded, string key, string ivKey)
    {
        string plaintext = null;

        using (var aesAlg = new AesManaged())
        using (var hasher = new SHA256Managed())
        {
            aesAlg.Key = hasher.ComputeHash(Encoding.UTF8.GetBytes(key));
            aesAlg.IV = Encoding.UTF8.GetBytes(ivKey);

            var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

            using (var msDecrypt = new MemoryStream(Convert.FromBase64String(coded)))
            {
                using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                {
                    using (var srDecrypt = new StreamReader(csDecrypt))
                    {
                        plaintext = srDecrypt.ReadToEnd();
                    }
                }
            }
        }
        return plaintext;
    }

    // NOTE: We don't use the Encrypt method within the WebOffice, but we've supplied the encryption method here as an 
    //       example to your vendors and others of how to write a simple Rijndael enryption method in C#.
    //       
    public static string Encrypt(string uncoded, string key, string ivKey)
    {
        byte[] encrypted;

        using (var aesAlg = new AesManaged())
        using (var hasher = new SHA256Managed())
        {
            aesAlg.Key = hasher.ComputeHash(Encoding.UTF8.GetBytes(key));
            aesAlg.IV = Encoding.UTF8.GetBytes(ivKey);

            var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
            using (var msEncrypt = new MemoryStream())
            {
                using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (var swEncrypt = new StreamWriter(csEncrypt))
                    {
                        swEncrypt.Write(uncoded);
                    }
                    encrypted = msEncrypt.ToArray();
                }
            }
        }
        return Convert.ToBase64String(encrypted);
    }

    #endregion

    //public static void SendMail(string RecipientEmail, string ReplyToEmail, string subject, string text, string AuthKey, string FromEmail = "ryte@mail.wsicloud.com")
    //{
    //    // Configure our message's details        
    //    MailAddress recipientEmail = new MailAddress(RecipientEmail);
    //    MailAddress fromEmail = new MailAddress(FromEmail);
    //    MailAddress replyToEmail = new MailAddress(ReplyToEmail);

    //    // Credentials used to send mail through SMTP
    //    SmtpClient smtp = new SmtpClient(Settings.Mail.SMTPServerUrl, Settings.Mail.SMTPServerPort);
    //    smtp.Credentials = new System.Net.NetworkCredential(Settings.Mail.SMTPServerLoginName, Settings.Mail.SMTPServerPassword);
    //    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
    //    smtp.EnableSsl = Settings.Mail.SMTPSecureConnectionRequired;

    //    // Create the MailMessage object
    //    MailMessage m = new MailMessage(fromEmail, recipientEmail);

    //    // Email Priority
    //    m.Priority = System.Net.Mail.MailPriority.Normal;


    //    // Reply To
    //    m.ReplyToList.Add(replyToEmail);

    //    // From email
    //    m.Sender = fromEmail;

    //    // Is the body using HTML?
    //    m.IsBodyHtml = true;

    //    // Email Subject
    //    m.Subject = subject;

    //    // Email Body
    //    m.Body = text;

    //    // Send the email
    //    smtp.Send(m);
    //}

    public static class EnrollmentPacks
    {
        public const  string membershipKitCode = "0066EN99";
    }
}