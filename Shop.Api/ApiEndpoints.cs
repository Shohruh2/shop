namespace Shop.Api;

public static class ApiEndpoints
{
    private const string ApiBase = "api";
    
    public static class Product
    {
        private const string Base = $"{ApiBase}/products";

        public const string Create = Base;
        public const string Get = $"{Base}/{{id:guid}}";
        public const string GetAll = Base;
        public const string Update = $"{Base}/{{id:guid}}";
        public const string Delete = $"{Base}/{{id:guid}}";
    }

    public static class Customer
    {
        private const string Base = $"{ApiBase}/customers";

        public const string Create = Base;
        public const string Get = $"{Base}/{{id:guid}}";
        public const string GetAll = Base;
        public const string Update = $"{Base}/{{id:guid}}";
        public const string Delete = $"{Base}/{{id:guid}}"; 
    }
    
    public static class Order
    {
        private const string Base = $"{ApiBase}/orders";

        public const string Create = Base;
        public const string Get = $"{Base}/{{id:guid}}";
        public const string GetAll = Base;
        public const string Update = $"{Base}/{{id:guid}}";
        public const string Delete = $"{Base}/{{id:guid}}"; 
    }
    
    
}