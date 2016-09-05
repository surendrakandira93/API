using Exigo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// Extension Class Version : 1.0.0.0.0.1
/// </summary>

public static class Extension
{
    /// <summary>
    /// Converts string value into Integer. 
    /// </summary> 
    /// <param name="value"></param>
    /// <remarks>Extended By : Anirudh Krishan Vaishnav </remarks>
    /// <returns>Integer Equilent of string value or Zero if invalid integer string</returns>
    public static int ToInt(this string value)
    {
        int result = 0;
        if (value != null)
            int.TryParse(value, out result);
        return result;

    }

    ///<summary>Gets the first week day following a date.</summary> 
    ///<param name="date">The date.</param> 
    ///<param name="dayOfWeek">The day of week to return.</param> 
    ///<returns>The first dayOfWeek day following date, or date if it is on dayOfWeek.</returns> 
    public static DateTime GetNextWeekDay(this DateTime date, DayOfWeek dayOfWeek)
    {
        return date.AddDays((dayOfWeek < date.DayOfWeek ? 7 : 0) + dayOfWeek - date.DayOfWeek);
    }

    /// <summary>
    /// Converts Integer Equailent.
    /// </summary>
    /// <param name="value">string value that need to be converted</param>
    /// <param name="default">Integer Default value that will be returned if Invalid string provided into parameter.</param>
    /// <remarks>Extended By : Anirudh Krishan Vaishnav</remarks>
    /// <returns>Integer Equilent of string value or Default if invalid integer string</returns>
    public static int? ToIntOrDefault(this string value, int? @default)
    {
        int result = 0;
        if (int.TryParse(value, out result))
            return result;
        else
            return @default;

    }


    /// <summary>
    /// Converts string value into 64 Integer. 
    /// </summary> 
    /// <param name="value"></param>
    /// <remarks>Extended By : Anirudh Krishan Vaishnav </remarks>
    /// <returns>64 Integer Equilent of string value or Zero if invalid integer string</returns>
    public static Int64 ToInt64(this string value)
    {
        Int64 result = 0;
        if (value != null)
            Int64.TryParse(value, out result);
        return result;

    }

    /// <summary>
    /// Converts 64 Integer Equailent.
    /// </summary>
    /// <param name="value">string value that need to be converted</param>
    /// <param name="default">Integer Default value that will be returned if Invalid string provided into parameter.</param>
    /// <remarks>Extended By : Anirudh Krishan Vaishnav</remarks>
    /// <returns>64 Integer Equilent of string value or Default if invalid integer string</returns>
    public static Int64? ToInt64OrDefault(this string value, Int64? @default)
    {
        Int64 result = 0;
        if (Int64.TryParse(value, out result))
            return result;
        else
            return @default;

    }

    /// <summary>
    /// Converts string value into Long. 
    /// </summary> 
    /// <param name="value"></param>
    /// <remarks>Extended By : Anirudh Krishan Vaishnav</remarks>
    /// <returns>Long Equilent of string value or Zero if invalid Long string</returns>
    public static long ToLong(this string value)
    {
        long result = 0;
        long.TryParse(value, out result);
        return result;

    }

    /// <summary>
    /// Converts string value into Float. 
    /// </summary> 
    /// <param name="value"></param>
    /// <remarks>Extended By : Anirudh Krishan Vaishnav</remarks>
    /// <returns>Float Equilent of string value or Zero if invalid float string</returns>
    public static float ToFloat(this string value)
    {
        float result = 0;
        float.TryParse(value, out result);
        return result;

    }

    /// <summary>
    /// Converts string value into Double. 
    /// </summary> 
    /// <param name="value"></param>
    /// <remarks>Extended By : Anirudh Krishan Vaishnav</remarks>
    /// <returns>Double Equilent of string value or Zero if invalid Double string</returns>
    public static double ToDouble(this string value)
    {
        double result = 0;
        double.TryParse(value, out result);
        return result;

    }

    /// <summary>
    /// Converts string value into Boolean. 
    /// </summary> 
    /// <param name="value"></param>
    /// <remarks>Extended By : Anirudh Krishan Vaishnav</remarks>
    /// <returns>Boolean Equilent of string value or Zero if invalid Boolean string</returns>
    public static bool ToBool(this string value)
    {
        bool result = false;
        if (!string.IsNullOrEmpty(value))
        {
            if (value.Trim() == "1")
                result = true;
            else if (value.Trim() == "0")
                result = false;
            else
                bool.TryParse(value, out result);
        }

        return result;
    }

    /// <summary>
    /// Converts string value into Boolean. 
    /// </summary> 
    /// <param name="value"></param>
    /// <remarks>Extended By : Anirudh Krishan Vaishnav</remarks>
    /// <returns>Boolean Equilent of string value or Default if invalid Boolean string</returns>
    public static bool ToBool(this string value, bool @default)
    {
        bool result = false;
        if (value.Trim() == "1")
            result = true;
        else if (value.Trim() == "0")
            result = false;
        else
        {
            if (bool.TryParse(value, out result))
                return result;
            else
                return @default;

        }

        return result;
    }

    /// <summary>
    /// Upper case First later of any string
    /// </summary>
    /// <param name="str"></param>
    /// <remarks>Extended By : Dileep Kumar</remarks>
    /// <returns>Upper case First later of any string</returns>
    public static string UppercaseFirst(this string value)
    {
        // Check for empty string.
        if (string.IsNullOrEmpty(value))
        {
            return string.Empty;
        }
        // Return char and concat substring.
        if (value.Length == 1)
        {
            return char.ToUpper(value[0]).ToString();
        }
        else
        {
            return char.ToUpper(value[0]) + value.Substring(1);
        }
    }

    /// <summary>
    ///  Determines if the provided object can be parsed as the provided type. Essentially a condensed TryParse.
    /// </summary>
    /// <typeparam name="T">The type of object to attempt to parse the provided object as.</typeparam>
    /// <param name="objectToBeParsed">The object to attempt to parse.</param>
    /// <returns>Whether or not the object can be parsed as the provided type.</returns>
    public static bool CanBeParsedAs<T>(this object objectToBeParsed)
    {
        try
        {
            var castedObject = Convert.ChangeType(objectToBeParsed, typeof(T));
            return castedObject != null;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Gets the percentage complete for the attached qualification response from a GetRankQualification web service call.
    /// </summary>
    /// <param name="response">A response from the web service.</param>
    /// <returns>The percentage complete represented by a decimal between 0 and 100.</returns>
    public static decimal GetPercentComplete(this QualificationResponse[] response)
    {
        var numerator = 0M;
        var denominator = 0M;
        var highestdenominator = 0M;

        // Get the highest denominator
        foreach (var qual in response)
        {
            var valueAsDecimal = 0M;
            if (qual.Required.CanBeParsedAs<bool>() || !qual.Required.CanBeParsedAs<decimal>()) valueAsDecimal = Convert.ToDecimal(qual.Qualifies);
            else valueAsDecimal = Convert.ToDecimal(qual.Required);

            if (valueAsDecimal > highestdenominator) highestdenominator = valueAsDecimal;
        }


        // Calculate the qualification ratios
        foreach (var qual in response)
        {
            denominator += highestdenominator;

            if (qual.Required.CanBeParsedAs<bool>() || !qual.Required.CanBeParsedAs<decimal>()) numerator += (Convert.ToDecimal(qual.Qualifies) * highestdenominator);
            else
            {
                var actual = Convert.ToDecimal(qual.Actual);
                var required = Convert.ToDecimal(qual.Required);

                if (actual > required) actual = required;
                numerator += Math.Floor(actual / required) * highestdenominator;
            }
        }

        var percentComplete = (numerator / denominator) * 100;
        if (percentComplete > 100M) percentComplete = 100M;

        return percentComplete;
    }
}

