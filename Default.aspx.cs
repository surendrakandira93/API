using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using Exigo;
using System.Data.OleDb;
using System.Data;
using System.Net.Mail;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        var res = ExigoApiContext.CreateWebServiceContext().GetItems(new GetItemsRequest() { CurrencyCode = "usd", LanguageID = 0, PriceType = 2, WarehouseID = 6, WebID = 1, WebCategoryID = 362 });
        var orderapi1 = ExigoApiContext.CreateWebServiceContext().GetOrders(new GetOrdersRequest() { OrderIDs = new int[]{ 5202177, 5202178 } });
        List<OrderDetailRequest> detail = new List<OrderDetailRequest>();
        detail.Add(new OrderDetailRequest() {ItemCode= "1121x", Quantity=1});
        var res1 = ExigoApiContext.CreateWebServiceContext().CalculateOrder( new CalculateOrderRequest() { CurrencyCode="eur", PriceType=8, WarehouseID=1, ShipMethodID = 8,  State ="AT" , Country="AT", Details= detail.ToArray() });

        //var res = client.CreateCustomerLead("dwBhAGsAYQB5AGEAfAB3AGEAawBhAHkAYQB8AHcAYQBrAGEAeQBhAA==", new WSIAPI.CreateCustomerLeadRequest{ FirstName="test", LastName="lead", Email="lead@test.com", Company="test company", Phone="1234567890", Phone2="0987654321", Notes="test wakaya lead from replicated site", CustomerID=3 });
        // <add name="sqlreporting" connectionString="Data Source=bi.exigo.com;Initial Catalog=SouthHillReporting;Persist Security Info=True;User ID=SHD_User1;Password=V7UH6ejN;Pooling=False;Connect Timeout=500000" providerName="System.Data.SqlClient"/>

        // var res = ExigoApiContext.CreateWebServiceContext().GetCustomers(new Exigo.GetCustomersRequest() { Email  = "ankfdsenbdy@test.com" });
        //   var res = ExigoApiContext.CreateWebServiceContext().SetAccountDirectDeposit(new Exigo.SetAccountDirectDepositRequest() { CustomerID = 1266992, BankAccountNumber = "0987654321", BankAccountType = Exigo.BankAccountType.CheckingPersonal, BankCountry = "KR", BankName = "test bank name", NameOnAccount = "test name on account", DepositAccountType = Exigo.DepositAccountType.Checking });
        //var res = ExigoApiContext.CreateWebServiceContext().GetAccountDirectDeposit(new Exigo.GetAccountDirectDepositRequest() { CustomerID=1266992 });
        //string ivkey = "V2r@$z0(!^1ivkey", key = "18q2hth0746gw19gf4942xap028zqoin", text="test|me|5";
        //var res = Encrypt(text, key, ivkey);
        //var res1 = Decrypt(res, key, ivkey);
        // var res = ExigoApiContext.CreateWebServiceContext().GetCustomers(new Exigo.GetCustomersRequest() { CustomerID = 1 });

        //var getorders = ExigoApiContext.CreateWebServiceContext().GetOrders(new GetOrdersRequest() { OrderID = 5192480 });
        //  var res = ExigoApiContext.CreatePaymentContext().FetchCreditCardToken("4111111111111111", 9,2018);
        //  var res = ExigoApiContext.CreateWebServiceContext().GetItems(new GetItemsRequest() { ItemCodes = new string[] { "USYG239001" },WarehouseID=5,CurrencyCode="usd",PriceType=2 });
        // var res1 = ExigoApiContext.CreateWebServiceContext().GetItems(new GetItemsRequest() { WarehouseID=5, LanguageID=0, CurrencyCode="usd", PriceType=2,RestrictToWarehouse=true  , WebCategoryID=220, WebID=1  });
        //var res = ExigoApiContext.CreateWebServiceContext().SetItemImage(new SetItemImageRequest() { ItemCode = "USAN200001", SmallImageName = "https://buyygy.com/90forLifeStore/content/images/thumbs/0005036_anthology-13-x-8-cutting-mat.jpeg" });
        //   gettotalitemcount();
        //var res = ExigoApiContext.CreateWebServiceContext().GetCustomerSite(new GetCustomerSiteRequest() { CustomerID=71280});   //List<string> lst = new List<string>();
        //lst.Add("USML9021");
        //lst.Add("USML0021");
        //lst.Add("USML0022");
        //var res = ExigoApiContext.CreateWebServiceContext().GetItems(new GetItemsRequest() { LanguageID=0, CurrencyCode="usd", PriceType=2, WarehouseID=5, ItemCodes= lst.ToArray(),RestrictToWarehouse=true,ReturnLongDetail=true ,WebCategoryID=183,WebID=1 });
        //var res = ExigoApiContext.CreateWebServiceContext().ChangeOrderStatus(new ChangeOrderStatusRequest() { OrderID= 67200, OrderStatus= OrderStatusType.Printed  });
        // var res = ExigoApiContext.CreateWebServiceContext().UpdateCustomer(new UpdateCustomerRequest() { CustomerID = 71204, CustomerType = 7 });
    }

  

    public static void ImportImages()
    {
        var ds = new DataSet("CSV File");
        var filename = @"c:\Customer.csv";
        var connString = string.Format(
            @"Provider=Microsoft.Jet.OleDb.4.0; Data Source={0};Extended Properties=""Text;HDR=YES;FMT=Delimited""",
            Path.GetDirectoryName(filename)
        );
        using (var conn = new OleDbConnection(connString))
        {
            conn.Open();
            var query = "SELECT * FROM [" + Path.GetFileName(filename) + "]";
            using (var adapter = new OleDbDataAdapter(query, conn))
            {                
                adapter.Fill(ds);
            }
        }
        string erroritemcodes = "";

        var res = ds.Tables[0];
        foreach(DataRow dr in res.Rows)
        {
            var res1 = new UpdateCustomerResponse();
            try
            {
                res1 = ExigoApiContext.CreateWebServiceContext().UpdateCustomer(new UpdateCustomerRequest() { CustomerID = dr["CustomerID"].ToString().ToInt(),  CustomerStatus=dr["CustomerStatus"].ToString().ToInt() });
            }
            catch(Exception ex)
            {

            }
        }
    }

    public static void AddItemsinCategory()
    {
        List<int> lst = new List<int>();

        lst.Add(186);
        lst.Add(183);
        lst.Add(187);
        lst.Add(188);
        lst.Add(189);
        lst.Add(216);

        foreach(int i in lst)
        {
            var categoryitems = ExigoApiContext.CreateWebServiceContext().GetItems(new GetItemsRequest() { CurrencyCode="usd", LanguageID=0, PriceType=2, WarehouseID=5, WebCategoryID=i, WebID=1 });
            List<string> items = new List<string>();           
            foreach (ItemResponse res in categoryitems.Items)
            {
                items.Add(res.ItemCode);               
            }
           var resus= ExigoApiContext.CreateWebServiceContext().AddProductsToCategory(new AddProductsToCategoryRequest() { CategoryID = 220, ItemCodes =items.ToArray(), WebID = 1 });
           var resca= ExigoApiContext.CreateWebServiceContext().AddProductsToCategory(new AddProductsToCategoryRequest() { CategoryID = 221, ItemCodes = items.ToArray(), WebID = 1 });
        }
    }

    public static void AddItemsIntoCategoryAndSetPrice()
    {
        Dictionary<int, int> dict = new Dictionary<int, int>();
        /*  dict.Add(186, 193);
        dict.Add(213, 214);
         dict.Add(187, 194);
         dict.Add(185, 192);
         dict.Add(188, 195);
         dict.Add(183, 190);
         dict.Add(184, 191);
         dict.Add(189, 196);
         //dict.Add(220, 221);
         //dict.Add(216, 217);*/

        dict.Add(220, 221);
        string catfaileditems="", costfaileditem="";
        foreach(var  d in dict)
        {
            var items = ExigoApiContext.CreateWebServiceContext().GetItems(new GetItemsRequest() { CurrencyCode="usd", LanguageID=0, PriceType=2, RestrictToWarehouse=true, WarehouseID=5, WebCategoryID=d.Key,WebID=1 });
            List<string> itemcodes = new List<string>() ;

            foreach(ItemResponse r in items.Items)
            {
                itemcodes.Add(r.ItemCode);
                try
                {
                    var res1 = ExigoApiContext.CreateWebServiceContext().SetItemPrice(new SetItemPriceRequest() { BusinessVolume = r.BusinessVolume, CommissionableVolume = r.CommissionableVolume, CurrencyCode = "cad", ItemCode = r.ItemCode, Price = (r.Price * 1.3M), PriceType = 2, ShippingPrice = r.ShippingPrice * 1.3M, TaxablePrice = r.TaxablePrice * 1.3M });
                }
                catch(Exception ex)
                {
                    costfaileditem = costfaileditem + " , " + r.ItemCode;
                }
                
            }

            try
            {
               // var res = ExigoApiContext.CreateWebServiceContext().AddProductsToCategory(new AddProductsToCategoryRequest() { CategoryID = d.Value, WebID = 1, ItemCodes = itemcodes.ToArray() });
            }
            catch(Exception ex)
            {
                catfaileditems = catfaileditems + " , " + d.Key;
            }
                   
        }

        HttpContext.Current.Response.Write("Failed items in Seeting price are " + costfaileditem);
        HttpContext.Current.Response.Write("Failed Categories in Seeting Category are " + catfaileditems);

    }   

    #region Encryption/Decryption methods
    private string Decrypt(string coded, string key, string ivKey)
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
    private string Encrypt(string uncoded, string key, string ivKey)
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

    private ChargeCreditCardTokenRequest Request_ChargeCreditCard()
    {
        // DEVELOPER NOTE: This method charges the credit card using tokenization, 
        // which is PCI-Compliant. This is the default method to use when charging credit cards.


        // Fetch our credit card token.
        var token = ExigoApiContext.CreatePaymentContext().FetchCreditCardToken("4111111111111111",
            9, 2018);

        var request = new ChargeCreditCardTokenRequest();
        
        request.OrderID = 5188994;
        request.CreditCardToken = token;
        request.BillingName = "test";
        request.CvcCode = "123";
        request.BillingAddress = "707 e main st";
        request.BillingCity = "LEHI";
        request.BillingState = "Utah";
        request.BillingZip = "84043";
        request.BillingCountry = "US";
        return request;
    }

    private SetAccountCreditCardTokenRequest SetAccountCreditCard()
    {
        // DEVELOPER NOTE: This method charges the credit card using tokenization, 
        // which is PCI-Compliant. This is the default method to use when charging credit cards.


        // Fetch our credit card token.
        var token = ExigoApiContext.CreatePaymentContext().FetchCreditCardToken("4111111111111111",
            9, 2018);

        var request = new SetAccountCreditCardTokenRequest();
        request.CreditCardType = (int) AccountCreditCardType.Primary ;
        request.CreditCardToken = token;
        request.BillingName = "andrewm";
        request.CustomerID =68342 ;
        //request.BillingAddress = "707 e main st";
        //request.BillingCity = "LEHI";
        request.BillingState = "Ut";
        //request.BillingZip = "84043";
        request.BillingCountry = "US";
        request.ExpirationMonth = 5;
        request.ExpirationYear = 2019;
    
        return request;

        
    }

    #endregion

    
}