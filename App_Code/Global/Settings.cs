using Exigo;

using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Xml.Linq;

public static class GlobalSettings
{
    /// <summary>
    /// This property Determines which API Source should be used here.
    /// </summary>
    public static ApiContextSource Source { get { return ApiContextSource.Sandbox4   ; } }

    public static Clients Client { get { return Clients.Purium                      ; } }

    /// <summary>
    /// This property return AuthKeyRequired status
    /// </summary>
    public static string AuthKeyRequired { get { return "AuthKeyRequired"; } }
    public static class ExigoApiCredentials
    {
        public static string LoginName { 
            get {
                string username = "";
                switch(Client)
                {
                    case Clients.FGX :
                        username= "API_WO";
                        break;
                    case Clients.Wellmed :
                username= "jeffk";
                break;
                    case Clients.Mialisia:
                username = "API_MiaLisia";
                break;
                    case Clients.Purium:
                username = "api";
                break;
                    case Clients.SHD:
                username = "API_Khamelion";
                break;
                    case Clients.Visi :
                username = "dataimport";
                break;
                    case Clients.ExigoDemo :
                username = "demo1";
                break;
                    default:
                username = "";
                break;
                 }
                return username;
            }
        }
        public static string Password 
        { 
            get {
                string password = "";
                switch(Client)
                {
                    case Clients.FGX :
                        password = "a5f9EhhjVPRi";
                        break;
                    case Clients.Wellmed :
                        password = "12345";
                break;
                    case Clients.Mialisia:
                password = "M!aL!s!a";
                break;
                    case Clients.Purium:
                password = "api123";
                break;
                    case Clients.SHD:
                password = "$Hd38y";
                break;
                    case Clients.Visi :
                password = "wsijeff";
                break;
                    case Clients.ExigoDemo :
                password = "exigo";
                break;
                    default:
                password = "";
                break;
                 }
                return password;
            }
        }
        public static string CompanyKey
        {
            get
            {
                string companyKey = "";
                switch (Client)
                {
                    case Clients.FGX:
                        companyKey = "fgi";
                        break;
                    case Clients.Wellmed:
                        companyKey = "wellmed";
                        break;
                    case Clients.Mialisia:
                        companyKey = "mialisia";
                        break;
                    case Clients.Purium:
                        companyKey = "php";
                        break;
                    case Clients.SHD:
                        companyKey = "SHD";
                        break;
                    case Clients.Visi :
                        companyKey = "visi";
                        break;
                    case Clients.ExigoDemo :
                        companyKey = "Exigodemo";
                        break;
                    default:
                        companyKey = "";
                        break;
                }
                return companyKey;
            }
        }

        
    }

    public static class ExigoPaymentApiCredentials
    {
        public static string LoginName = "php_gDpGBsM0r";
        public static string Password = "80NZBIfh9NGHYmt9Pk9sb1sQ";
    }

   
   


    public static class ExigoApiSettings
    {
        public static ExigoApiContextSource DefaultContextSource = ExigoApiContextSource.Sandbox4 ;
        public static ExigoODataAPIUrl DefaultODataAPIUrl = ExigoODataAPIUrl.Live;

        public static class WebService
        {
            public static string LiveUrl = "https://api.exigo.com/3.0/ExigoApi.asmx";
            public static string Sandbox1Url = "https://sandboxapi1.exigo.com/3.0/ExigoApi.asmx";
            public static string Sandbox2Url = "https://sandboxapi2.exigo.com/3.0/ExigoApi.asmx";
            public static string Sandbox3Url = "https://sandboxapi3.exigo.com/3.0/ExigoApi.asmx";
            public static string Sandbox4Url = "http://sandboxapi4.exigo.com/3.0/ExigoApi.asmx";
        }

        public static class OData
        {
            public static string LiveUrl = "https://api.exigo.com/4.0/" + GlobalSettings.ExigoApiCredentials.CompanyKey;
            public static string Sandbox1Url = "https://sandboxapi1.exigo.com/4.0/" + GlobalSettings.ExigoApiCredentials.CompanyKey;
            public static string Sandbox2Url = "https://sandboxapi2.exigo.com/4.0/" + GlobalSettings.ExigoApiCredentials.CompanyKey;
            public static string Sandbox3Url = "https://sandboxapi3.exigo.com/4.0/" + GlobalSettings.ExigoApiCredentials.CompanyKey;
        }
    }

    public static class ODataAPISourceUrl
    {
        public static string LiveUrl = "https://api.exigo.com/4.0/" + GlobalSettings.ExigoApiCredentials.CompanyKey;
        public static string Sandbox1Url = "https://sandboxapi1.exigo.com/4.0/" + GlobalSettings.ExigoApiCredentials.CompanyKey;
        public static string Sandbox2Url = "https://sandboxapi2.exigo.com/4.0/" + GlobalSettings.ExigoApiCredentials.CompanyKey;
        public static string CurrentOdataUrl
        {
            get
            {
                string url = LiveUrl;
                switch (GlobalSettings.Source)
                {
                    case ApiContextSource.Sandbox1:
                        url = Sandbox1Url;
                        break;
                    case ApiContextSource.Sandbox2:
                        url = Sandbox2Url;
                        break;
                    case ApiContextSource.Live:
                        url = LiveUrl;
                        break;
                    default:
                        url = LiveUrl;
                        break;

                }
                return url;
            }
        }
    }

    public static class ExigoAPISourceUrl
    {
        public static string Live { get { return "https://api.exigo.com/3.0/ExigoApi.asmx"; } }
        public static string SandBox1 { get { return "https://sandboxapi1.exigo.com/3.0/ExigoApi.asmx"; } }
        public static string SandBox2 { get { return "https://sandboxapi2.exigo.com/3.0/ExigoApi.asmx"; } }

        public static string SandBox3 { get { return "https://sandboxapi3.exigo.com/3.0/ExigoApi.asmx"; } }

        public static string SandBox4 { get { return "http://sandboxapi4.exigo.com/3.0/ExigoApi.asmx"; } }

        public static string CurrentWebServiceUrl
        {
            get
            {
                string url = Live;
                switch (GlobalSettings.Source)
                {
                    case ApiContextSource.Sandbox1:
                        url = SandBox1;
                        break;
                    case ApiContextSource.Sandbox2:
                        url = SandBox2;
                        break;
                    case ApiContextSource.Sandbox3:
                        url = SandBox3;
                        break;
                    case ApiContextSource.Sandbox4:
                        url = SandBox4;
                        break;
                    case ApiContextSource.Live:
                        url = Live;
                        break;
                    default:
                        url = Live;
                        break;

                }
                return url;
            }
        }
    }

    public static class ConnectionStrings
    {
        public static string Sql = ConfigurationManager.ConnectionStrings["sqlreporting"].ConnectionString;
        public static string ReplicatedSqlConnectionStringName = "sqlreporting";
        public static string ReplicatedSql = ConfigurationManager.ConnectionStrings[ReplicatedSqlConnectionStringName].ConnectionString;

        public static string AuthenticationSql = ConfigurationManager.ConnectionStrings["AuthManager"].ConnectionString;
    }

    
    


    public static class DomainUrl
    {
        public const string LocalWeboffice = "http://localhost:8137/";
        public const string LocalPublic = "http://localhost:1610/";
        public const string BetaWSIWeboffice = "http://fgxexigooffice.wsidevteam.com/";
        public const string BetaWSIPublic = "http://fgxexigo.wsidevteam.com/";
        public const string BetaDevWeboffice = "http://wodev.fvrg.net/";
        public const string BetaDevPublic = "http://enrolldev.fvrg.net/";
        public const string LiveWeboffice = "https://fgxpress.org/";
        public const string LivePublic = "https://fgxpress.info/";
    }

}



public static class PeriodTypes
{
    public const int Default = 1;
    public const int Monthly = 1;
    public const int Weekly = 1;
}
