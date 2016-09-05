﻿using Exigo;


using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;


//namespace WebOffice
//{
public static class GlobalUtilities
{
    /// <summary>
    /// Sets the value of the string to be the first non-nullable parameter found for the strings provided.
    /// </summary>
    /// <param name="strings"></param>
    /// <returns>The first non-null, non-empty string found.</returns>
    public static string Coalesce(params string[] strings)
    {
        return strings.Where(s => !string.IsNullOrEmpty(s)).FirstOrDefault();
    }

    /// <summary>
    /// Condenses the provided string to the provided max length of characters. If the content is longer than the max length, "..." will be appended to the end.
    /// </summary>
    /// <param name="content">The content to be condensed.</param>
    /// <param name="maxLength">The maximum number of allowable characters.</param>
    /// <returns>The content equal or less than the max length.</returns>
    public static string Condense(string content, int maxLength)
    {

        string contentText = Regex.Replace(content, @"<(.|\n)*?>", string.Empty);
        int length = contentText.Length;
        content = Regex.Match(contentText, @"^.{1," + (maxLength - 1) + @"}\b(?<!\s)").Value;
        if (length > maxLength) content += "...";

        return content;
    }

    /// <summary>
    /// Perform a deep Copy of the object.
    /// </summary>
    /// <typeparam name="T">The type of object being copied.</typeparam>
    /// <param name="source">The object instance to copy.</param>
    /// <returns>The copied object.</returns>
    public static T Clone<T>(T source)
    {
        if (!typeof(T).IsSerializable)
        {
            throw new ArgumentException("The type must be serializable.", "source");
        }

        // Don't serialize a null object, simply return the default for that object
        if (Object.ReferenceEquals(source, null))
        {
            return default(T);
        }

        IFormatter formatter = new BinaryFormatter();
        Stream stream = new MemoryStream();
        using (stream)
        {
            formatter.Serialize(stream, source);
            stream.Seek(0, SeekOrigin.Begin);
            return (T)formatter.Deserialize(stream);
        }
    }

    /// <summary>
    /// Gets the start date for an autoship with the provided frequency type.
    /// </summary>
    /// <param name="frequency">How often the autoship will run</param>
    /// <returns>The start date for an autoship</returns>
    public static DateTime GetNewAutoOrderStartDate(FrequencyType frequency)
    {
        DateTime autoshipstartDate = new DateTime();
        var now = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

        switch (frequency)
        {
            case FrequencyType.Weekly: autoshipstartDate = now.AddDays(7); break;
            case FrequencyType.BiWeekly: autoshipstartDate = now.AddDays(14); break;
            case FrequencyType.EveryFourWeeks: autoshipstartDate = now.AddDays(28); break;
            case FrequencyType.Monthly: autoshipstartDate = now.AddMonths(1); break;
            case FrequencyType.BiMonthly: autoshipstartDate = now.AddMonths(2); break;
            case FrequencyType.Quarterly: autoshipstartDate = now.AddMonths(3); break;
            case FrequencyType.SemiYearly: autoshipstartDate = now.AddMonths(6); break;
            case FrequencyType.Yearly: autoshipstartDate = now.AddYears(1); break;
            default: autoshipstartDate = now; break;
        }

        // Ensure we are not returning a day of 29, 30 or 31.
        autoshipstartDate = GetNextAvailableAutoOrderStartDate(autoshipstartDate);

        return autoshipstartDate;
    }

    /// <summary>
    /// Gets the next available date for an autoship starting with the provided date.
    /// </summary>
    /// <param name="date">The original start date</param>
    /// <returns>The nearest available start date for an autoship</returns>
    public static DateTime GetNextAvailableAutoOrderStartDate(DateTime date)
    {
        DayOfWeek dayOfWeek = DayOfWeek.Tuesday;

        DateTime AutoshipDate = date.AddDays((dayOfWeek <= date.DayOfWeek ? 7 : 0) + dayOfWeek - date.DayOfWeek);

        // Ensure we are not returning a day of 29, 30 or 31.
        if (AutoshipDate.Day > 28)
        {
            DateTime newdate = new DateTime(date.AddMonths(1).Year, date.AddMonths(1).Month, 1).Date;

            // Get New Date (Tuesday) of Next month

            date = newdate.AddDays((dayOfWeek <= newdate.DayOfWeek ? 7 : 0) + dayOfWeek - newdate.DayOfWeek);
        }
        else
        {
            date = AutoshipDate;
        }

        return date;
    }

    /// <summary>
    /// Determines if the supplied year is a leap year.
    /// </summary>
    /// <param name="year">The year to check</param>
    /// <returns>True if the provided year is a leap year.</returns>
    public static bool IsLeapYear(int year)
    {
        if (year % 4 != 0)
        {
            return false;
        }
        if (year % 100 == 0)
        {
            return (year % 400 == 0);
        }
        return true;
    }

    /// <summary>
    /// Validates the provided credit card number using a Luhn algorithm.
    /// </summary>
    /// <param name="creditCardNumber">The credit card number to validate.</param>
    /// <returns>The validity of the credit card number. True = valid card, False = invalid card.</returns>
    public static bool ValidateCreditCard(string creditCardNumber)
    {
        const string allowed = "0123456789";
        int i;

        var cleanNumber = new StringBuilder();
        for (i = 0; i < creditCardNumber.Length; i++)
        {
            if (allowed.IndexOf(creditCardNumber.Substring(i, 1)) >= 0)
                cleanNumber.Append(creditCardNumber.Substring(i, 1));
        }
        if (cleanNumber.Length < 13 || cleanNumber.Length > 16)
            return false;

        for (i = cleanNumber.Length + 1; i <= 16; i++)
            cleanNumber.Insert(0, "0");

        int multiplier, digit, sum, total = 0;
        string number = cleanNumber.ToString();

        for (i = 1; i <= 16; i++)
        {
            multiplier = 1 + (i % 2);
            digit = int.Parse(number.Substring(i - 1, 1));
            sum = digit * multiplier;
            if (sum > 9)
                sum -= 9;
            total += sum;
        }

        return (total % 10 == 0);
    }


    /// <summary>
    /// Attempts to parse the provided object as the provided type. If the parsing is unsuccessful, it will reutrn the provided default value.
    /// </summary>
    /// <typeparam name="T">The type to parse your string to.</typeparam>
    /// <param name="s">The object to parse.</param>
    /// <param name="defaultValue">The value that will be returned if parsing is unsuccessful.</param>
    /// <returns></returns>
    public static T TryParse<T>(object s, object defaultValue)
    {
        try
        {
            return (T)Convert.ChangeType(s, typeof(T));
        }
        catch
        {
            return (T)defaultValue;
        }
    }

    /// <summary>
    /// Returns the client's IP address, or (localhost) if there isn't one.
    /// </summary>
    /// <returns>The cleint's IP address</returns>
    public static string GetClientIP()
    {
        var ip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
        if (ip.Equals("::1", StringComparison.InvariantCultureIgnoreCase)) ip = "127.0.0.1";

        return ip;
    }

    /// <summary>
    /// Formats an error message (usually from an Exception) for use in Javascript by removing all line breaks and converting double-quotes to single-quotes.
    /// </summary>
    /// <param name="errormessage">The error message (usually from an Exception).</param>
    /// <returns>A formatted error message for use in Javascript.</returns>
    public static string FormatErrorMessageForJavascript(string errormessage)
    {
        errormessage = errormessage.Replace("'", "");
        errormessage = errormessage.Replace(System.Environment.NewLine, " ");
        errormessage = errormessage.Replace("\r\n", " ").Replace("\n", " ").Replace("\r", " ");

        return errormessage;
    }


    /// <summary>
    /// Gets a formatted display name based on the company's rules. The default setting is that if a company name is found display the company name; otherwise, display the first and last name.
    /// </summary>
    /// <returns>The formatted display name.</returns>
    public static string GetDisplayName(string companyName, string firstName, string lastName)
    {
        return GlobalUtilities.Coalesce(companyName, firstName + " " + lastName);
    }

    /// <summary>
    /// Returns the supplied DateTime, or the earliest possible DateTime that SQL allows.
    /// </summary>
    /// <param name="dateTime"></param>
    /// <returns></returns>
    public static DateTime GetSqlDateTime(DateTime? dateTime)
    {
        return dateTime ?? new DateTime(1753, 1, 1);
    }

    /// <summary>
    /// Formats the time difference into text similar to what you would see in a social network.
    /// </summary>
    /// <param name="startDateTime">The DateTime of the event. This date is usually in the past.</param>
    /// <param name="endDateTime">The DateTime you want to compare your event DateTime to. Usually DateTime.Now.</param>
    /// <returns>A formatted string description explaining the time difference in simple terms.</returns>
    public static string GetFormattedTimeDifference(DateTime startDateTime, DateTime endDateTime)
    {
        var startDate = startDateTime;
        var endDate = endDateTime;
        var diff = endDate.Subtract(startDate);

        // Start with the smallest increments, and go up from there.
        var description = string.Empty;
        if (Math.Round(diff.TotalSeconds, 0) == 0) description = string.Format("Just now");
        else if (Math.Round(diff.TotalSeconds, 0) == 1) description = string.Format("{0:0} second ago", diff.TotalSeconds);
        else if (Math.Round(diff.TotalSeconds, 0) < 60) description = string.Format("{0:0} seconds ago", diff.TotalSeconds);
        else if (Math.Round(diff.TotalMinutes, 0) == 1) description = string.Format("{0:0} minute ago", diff.TotalMinutes);
        else if (Math.Round(diff.TotalMinutes, 0) < 60) description = string.Format("{0:0} minutes ago", diff.TotalMinutes);
        else if (Math.Round(diff.TotalHours, 0) == 1) description = string.Format("{0:0} hour ago", diff.TotalHours);
        else if (Math.Round(diff.TotalHours, 0) < 24) description = string.Format("{0:0} hours ago", diff.TotalHours);
        else if (Math.Round(diff.TotalDays, 0) == 1) description = string.Format("{0:0} day ago", diff.TotalDays);
        else if (Math.Round(diff.TotalDays, 0) < 7) description = string.Format("{0:0} days ago", diff.TotalDays);
        else description = string.Format("{0:MMMM d, yyyy}", startDate);

        return description;
    }

   
   

    public static bool IsTestCreditCard(string creditCardNumber)
    {
        // Filter out any undesired characters using the same utility used by the rest of the application.
        creditCardNumber = creditCardNumber.FormatForExigo(ExigoDataFormatType.CreditCardNumber);

        // If the credit card number is 9696 (Exigo's default test credit card number), return true.
        return (creditCardNumber.Equals("9696", StringComparison.InvariantCultureIgnoreCase) || creditCardNumber.Equals("5467", StringComparison.InvariantCultureIgnoreCase));
    }
    public static string FormatForExigo(this string value, ExigoDataFormatType type)
    {
        string s = value.TrimStart(' ').TrimEnd(' ');
        s = FormatByType(s, type);
        return s;
    }
    public enum ExigoDataFormatType
    {
        Phone,
        TaxID,
        Email,
        Username,
        WebAlias,
        CreditCardNumber,
        BankAccountNumber,
        BankRoutingNumber
    }
    private static string FormatByType(string value, ExigoDataFormatType type)
    {
        string s = value;
        switch (type)
        {
            case ExigoDataFormatType.Phone:
                s = s.Replace("-", "").Replace(" ", "").Replace(".", "").Replace("(", "").Replace(")", "");
                break;
            case ExigoDataFormatType.TaxID:
                s = s.Replace("-", "").Replace(" ", "").Replace(".", "");
                break;
            case ExigoDataFormatType.Email:
                s = s.Replace(" ", "").ToLower();
                break;
            case ExigoDataFormatType.Username:
                s = s.Replace(" ", "").ToLower();
                break;
            case ExigoDataFormatType.WebAlias:
                s = s.Replace(" ", "").ToLower();
                break;
            case ExigoDataFormatType.CreditCardNumber:
                s = s.Replace("-", "").Replace(" ", "").Replace(".", "");
                break;
            case ExigoDataFormatType.BankAccountNumber:
                s = s.Replace("-", "").Replace(" ", "").Replace(".", "");
                break;
            case ExigoDataFormatType.BankRoutingNumber:
                s = s.Replace("-", "").Replace(" ", "").Replace(".", "");
                break;
            default:
                throw new Exception(string.Format("'{0}' cannot be formatted for Exigo using Exigo.WebService;DataFormatType.{0}", value, type.ToString()));
        }
        return s;
    }

    #region Photo Manipulation
    public static byte[] ResizeImage(byte[] original, int maxWidth, int maxHeight)
    {
        using (var ms = new MemoryStream(original))
        using (var bmp = new Bitmap(ms))
        {
            ImageFormat format = bmp.RawFormat;
            decimal ratio;
            int newWidth = 0;
            int newHeight = 0;

            if (bmp.Width > maxWidth || bmp.Height > maxHeight)
            {
                if (bmp.Width > bmp.Height)
                {
                    ratio = (decimal)maxWidth / bmp.Width;
                    newWidth = maxWidth;
                    decimal lnTemp = bmp.Height * ratio;
                    newHeight = (int)lnTemp;
                }
                else
                {
                    ratio = (decimal)maxHeight / bmp.Height;
                    newHeight = maxHeight;
                    decimal lnTemp = bmp.Width * ratio;
                    newWidth = (int)lnTemp;
                }
            }

            if (newWidth == 0) newWidth = bmp.Width;
            if (newHeight == 0) newHeight = bmp.Height;

            using (var bmpOut = new Bitmap(newWidth, newHeight))
            using (var msOut = new MemoryStream())
            {
                Graphics g = Graphics.FromImage(bmpOut);

                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.FillRectangle(Brushes.White, 0, 0, newWidth, newHeight);
                g.DrawImage(bmp, 0, 0, newWidth, newHeight);

                bmpOut.Save(msOut, ImageFormat.Jpeg);

                return (msOut.ToArray());
            }
        }
    }
    public static byte[] Crop(string imageUrl, int width, int height, int X, int Y)
    {
        // Download the image
        var webClient = new WebClient();
        byte[] imageBytes = webClient.DownloadData(imageUrl);

        return Crop(imageBytes, width, height, X, Y);
    }
    public static byte[] Crop(byte[] imageBytes, int width, int height, int X, int Y)
    {
        // Convert the bytes into an Image
        MemoryStream ms = new MemoryStream(imageBytes);
        System.Drawing.Image returnImage = System.Drawing.Image.FromStream(ms);

        using (System.Drawing.Image OriginalImage = returnImage)
        {
            using (System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(width, height))
            {
                bmp.SetResolution(OriginalImage.HorizontalResolution, OriginalImage.VerticalResolution);
                using (System.Drawing.Graphics Graphic = System.Drawing.Graphics.FromImage(bmp))
                {
                    Graphic.SmoothingMode = SmoothingMode.AntiAlias;
                    Graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    Graphic.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    Graphic.DrawImage(OriginalImage, new System.Drawing.Rectangle(0, 0, width, height), X, Y, width, height, System.Drawing.GraphicsUnit.Pixel);
                    MemoryStream nms = new MemoryStream();
                    bmp.Save(nms, OriginalImage.RawFormat);
                    return nms.GetBuffer();
                }
            }
        }
    }
    #endregion

    /// <summary>
    /// Fetches the default period type ID. Used in other static classes.
    /// </summary>
    /// <returns>The current period ID</returns>
    public static int GetDefaultPeriodTypeID()
    {
        return PeriodTypes.Default;
    }
}
//}