using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


    public enum ApiContextSource
    {
        Live,
        Sandbox1,
        Sandbox2,
        Sandbox3,
        Sandbox4
    }

    public enum Clients
    {
        FGX,
        SHD,
        Wellmed,
        Mialisia,
        Purium,
        Visi,
        Ryte,
        ExigoDemo
    }

    public enum CallResult
    {
        Success = 1,
        Failed = 2
    }

    public enum BillingAddressPreferenceType
    {
        HomeAddress = 1, BillingAddress = 0
    }

    public enum ShippingAddressPreferenceType
    {
        HomeAddress = 1, ShippingAddress = 0
    }

    public enum CommissionMethodPreferenceType
    {
        DirectDeposit = 0,
        eWallet = 1,
        Payoneer = 2
    }

    public enum DirectDepositeTypes { Bank, PostalService }

    public enum EnrollmentTypes
    {
        Default = 0,
        WebofficeBinaryTree = 1,
        Xpower = 2
    }
