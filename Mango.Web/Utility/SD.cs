namespace Mango.Web.Utility
{
    public class SD
    {
        public static string CouponApiBase { get; set; } = default!;
        public static string AuthApiBase { get; set; } = default!;
        public static string ProductApiBase { get; set; } = default!;
        public static string ShoppingCartApiBase { get; set; } = default!;
        public static string OrdeApiBase { get; set; } = default!;

        public const string RoleAdmin = "ADMIN";
        public const string RoleCustomer = "CUSTOMER";
        public const string TokenCookie = "JWTToken"; 
        public enum ApiType
        {
            GET,
            POST,
            PUT,
            DELETE
        }

        // se requiere de suscripcion en stripe
        public const string Status_Pending = "Pending";
        public const string Status_Approved = "Approved";
        public const string Status_ReadyForPickup = "ReadyForPickup";
        public const string Status_Completed = "Completed";
        public const string Status_Refunded = "Refunded";
        public const string Status_Cancelled = "Cancelled";

        public enum ContentType
        {
            Json,
            MultipartFormData,
        }

    }
}
