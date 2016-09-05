using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Xml.Linq;
using System.Net;
using System.IO;
using Exigo;
using Exigo.OData;
using System.Data.Services.Client;
using System.Data.SqlClient;
using Exigo.OData.ApiLog;



public static class ExigoApiContext
{
    #region Properties
    private static string LoginName = GlobalSettings.ExigoApiCredentials.LoginName;
    private static string Password = GlobalSettings.ExigoApiCredentials.Password;
    private static string Company = GlobalSettings.ExigoApiCredentials.CompanyKey;

    private static string APIUrl
    {
        get
        {
            string apiUrl = "";
            switch (GlobalSettings.ExigoApiSettings.DefaultODataAPIUrl)
            {
                case ExigoODataAPIUrl.Live:
                    apiUrl = GlobalSettings.ExigoApiSettings.OData.LiveUrl;
                    break;
                case ExigoODataAPIUrl.Sandbox1:
                    apiUrl = GlobalSettings.ExigoApiSettings.OData.Sandbox1Url;
                    break;
                case ExigoODataAPIUrl.Sandbox2:
                    apiUrl = GlobalSettings.ExigoApiSettings.OData.Sandbox2Url;
                    break;
                case ExigoODataAPIUrl.Sandbox3:
                    apiUrl = GlobalSettings.ExigoApiSettings.OData.Sandbox3Url;
                    break;
            }
            return apiUrl;
        }
    }
    #endregion Properties

    #region Contexts

    // not in use just for saving error from unsed files 
    public static ExigoApi CreateWebServiceContext()
    {
        return GetNewWebServiceContext(GlobalSettings.Source, "");
    }

    public static ExigoApi CreateWebServiceContext(string APIKey)
    {
        return GetNewWebServiceContext(GlobalSettings.Source, APIKey);
    }

    public static ExigoApi GetNewWebServiceContext()
    {
        return new ExigoApi
        {
            ApiAuthenticationValue = new ApiAuthentication
            {
                LoginName = LoginName,
                Password = Password,
                Company = Company
            }
        };
    }

    public static ExigoApi GetNewWebServiceContext(ApiContextSource source, string APIKey)
    {
        try
        {
            var res = new ExigoApi
            {
                ApiAuthenticationValue = new ApiAuthentication
                {
                    LoginName = GlobalSettings.ExigoApiCredentials.LoginName,
                    Password = GlobalSettings.ExigoApiCredentials.Password,
                    Company = GlobalSettings.ExigoApiCredentials.CompanyKey
                },
                Url = GlobalSettings.ExigoAPISourceUrl.CurrentWebServiceUrl
            };
            return res;
        }
        catch { throw; }
    }

    public static ExigoPaymentApi CreatePaymentContext()
    {
        return new ExigoPaymentApi();
    }


 


    public static Exigo.OData.ApiLog.ApiLog CreateApiLogContext()
    {
        var context = new ApiLog(new Uri("http://api.exigo.com/4.0/" + Company + "/db/ApiLog"));
        context.IgnoreMissingProperties = true;
        var credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes(LoginName + ":" + Password));
        context.SendingRequest +=
            (object s, SendingRequestEventArgs e) =>
                e.RequestHeaders.Add("Authorization", "Basic " + credentials);
        return context;
    }



    #region OData Contexts
    public static Exigo.OData.ExigoContext CreateODataContext()
    {
        return GetNewODataContext(GlobalSettings.ExigoApiSettings.DefaultContextSource);
    }
    public static ExigoContext CreateODataContext(ExigoApiContextSource source)
    {
        return GetNewODataContext(source);
    }
    private static ExigoContext GetNewODataContext(ExigoApiContextSource source)
    {
        var sourceUrl = "";
        switch (source)
        {
            case ExigoApiContextSource.Live:
                sourceUrl = GlobalSettings.ExigoApiSettings.OData.LiveUrl;
                break;
            case ExigoApiContextSource.Sandbox1:
                sourceUrl = GlobalSettings.ExigoApiSettings.OData.Sandbox1Url;
                break;
            case ExigoApiContextSource.Sandbox2:
                sourceUrl = GlobalSettings.ExigoApiSettings.OData.Sandbox2Url;
                break;
            case ExigoApiContextSource.Sandbox3:
                sourceUrl = GlobalSettings.ExigoApiSettings.OData.Sandbox3Url;
                break;
        }

        var context = new ExigoContext(new Uri(sourceUrl + "/model"));
        context.IgnoreMissingProperties = true;
        context.IgnoreResourceNotFoundException = true;
        var credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes(LoginName + ":" + Password));
        context.SendingRequest +=
            (object s, SendingRequestEventArgs e) =>
                e.RequestHeaders.Add("Authorization", "Basic " + credentials);
        return context;
    }


    public static Exigo.OData.Extended.extendeddatacontext CreateExtendedODataContext()
    {
        
        var context = new Exigo.OData.Extended.extendeddatacontext(new Uri(APIUrl + "/db/extendeddatacontext"));
        context.IgnoreMissingProperties = true;
        context.IgnoreResourceNotFoundException = true;
        var credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes(LoginName + ":" + Password));
        context.SendingRequest +=
            (object s, SendingRequestEventArgs e) =>
                e.RequestHeaders.Add("Authorization", "Basic " + credentials);
        return context;
    }

    public static Exigo.OData.GenericResource.genericresourcedatacontext CreateGenericResourceODataContext()
    {
        var context = new Exigo.OData.GenericResource.genericresourcedatacontext(new Uri(APIUrl + "/db/genericresourcedatacontext"));
        context.IgnoreMissingProperties = true;
        context.IgnoreResourceNotFoundException = true;
        var credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes(LoginName + ":" + Password));
        context.SendingRequest +=
            (object s, SendingRequestEventArgs e) =>
                e.RequestHeaders.Add("Authorization", "Basic " + credentials);
        return context;
    }

    public static Exigo.OData.CommissionReport.commissionreport CreatecommissionreportODataContext()
    {
        var context = new Exigo.OData.CommissionReport.commissionreport(new Uri(APIUrl + "/db/commissionreport"));
        context.IgnoreMissingProperties = true;
        context.IgnoreResourceNotFoundException = true;
        var credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes(LoginName + ":" + Password));
        context.SendingRequest +=
            (object s, SendingRequestEventArgs e) =>
                e.RequestHeaders.Add("Authorization", "Basic " + credentials);
        return context;
    }

 
    public static Exigo.OData.Authorization.authorization CreateAuthorizationODataContext()
    {
        var context = new Exigo.OData.Authorization.authorization(new Uri(APIUrl + "/db/authorization"));
        context.IgnoreMissingProperties = true;
        context.IgnoreResourceNotFoundException = true;
        var credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes(LoginName + ":" + Password));
        context.SendingRequest +=
            (object s, SendingRequestEventArgs e) =>
                e.RequestHeaders.Add("Authorization", "Basic " + credentials);
        return context;
    }

    public static Exigo.OData.dynamicCart.dynamiccarts DynamiccartContext()
    {
        var context = new Exigo.OData.dynamicCart.dynamiccarts(new Uri(APIUrl + "/db/dynamiccarts"));
        context.IgnoreMissingProperties = true;
        context.IgnoreResourceNotFoundException = true;
        var credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes(LoginName + ":" + Password));
        context.SendingRequest +=
            (object s, SendingRequestEventArgs e) =>
                e.RequestHeaders.Add("Authorization", "Basic " + credentials);
        return context;
    }
   

    #endregion

    #endregion Contexts
}

#region Supporting Classes
public class ExigoPaymentApi
{
    private string PaymentLoginName = GlobalSettings.ExigoPaymentApiCredentials.LoginName;
    private string PaymentPassword = GlobalSettings.ExigoPaymentApiCredentials.Password;

    /// <summary>
    /// Generate and return a new token to be used in an Exigo credit card transaction.
    /// </summary>
    /// <param name="creditCardNumber">The credit card number you wish to use for this transaction</param>
    /// <param name="expirationMonth">The expiration month of the credit card you wish to use for this transaction</param>
    /// <param name="expirationYear">The expiration year of the credit card you wish to use for this transaction</param>
    /// <returns></returns>
    public string FetchCreditCardToken(string creditCardNumber, int expirationMonth, int expirationYear)
    {
        XNamespace ns = "http://payments.exigo.com";
        var xRequest = new XDocument(
            new XElement(ns + "CreditCard",
                new XElement(ns + "CreditCardNumber", creditCardNumber),
                new XElement(ns + "ExpirationMonth", expirationMonth),
                new XElement(ns + "ExpirationYear", expirationYear)
                ));
        var xResponse = PostRest("https://payments.exigo.com/2.0/token/rest/CreateCreditCardToken", PaymentLoginName, PaymentPassword, xRequest);

        return xResponse.Root.Element(ns + "CreditCardToken").Value;
    }
    private XDocument PostRest(string url, string username, string password, XDocument element)
    {
        string postData = element.ToString();

        var request = (HttpWebRequest)WebRequest.Create(url);
        request.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes(username + ":" + password)));
        request.Method = "POST";
        request.ContentLength = postData.Length;
        request.ContentType = "text/xml";

        var writer = new StreamWriter(request.GetRequestStream());
        writer.Write(postData);
        writer.Close();

        try
        {
            var response = (HttpWebResponse)request.GetResponse();
            using (var responseStream = new StreamReader(response.GetResponseStream()))
            {
                return XDocument.Parse(responseStream.ReadToEnd());
            }
        }
        catch (WebException ex)
        {
            var response = (HttpWebResponse)ex.Response;
            if (response.StatusCode == HttpStatusCode.Unauthorized) throw new Exception("Invalid Credentials");
            using (var responseStream = new StreamReader(ex.Response.GetResponseStream()))
            {
                XNamespace ns = "http://schemas.microsoft.com/ws/2005/05/envelope/none";
                XDocument doc = XDocument.Parse(responseStream.ReadToEnd());
                throw new Exception(doc.Root.Element(ns + "Reason").Element(ns + "Text").Value);
            }
        }
    }
}

public class ExigoImageApi
{
    public void SaveImage(string path, string filename, byte[] contents)
    {
        var request = (HttpWebRequest)WebRequest.Create(GlobalSettings.ODataAPISourceUrl.CurrentOdataUrl + "/images" + (path.StartsWith("/") ? "" : "/") + path + "/" + filename);
        request.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes(GlobalSettings.ExigoApiCredentials.LoginName + ":" + GlobalSettings.ExigoApiCredentials.Password)));
        request.Method = "POST";
        request.ContentLength = contents.Length;
        var writer = request.GetRequestStream();
        writer.Write(contents, 0, contents.Length);
        writer.Close();
        var response = (HttpWebResponse)request.GetResponse();
    }
}
#endregion Supporting Classes

public enum ExigoApiContextSource
{
    Live,
    Sandbox1,
    Sandbox2,
    Sandbox3,
    Sandbox4
}

public enum ExigoODataAPIUrl
{
    Live,
    Sandbox1,
    Sandbox2,
    Sandbox3
}
