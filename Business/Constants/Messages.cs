using Core.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Business.Constants
{
    public static class Messages
    {
        public static string ProductAddedMessage = "Product added";
        public static string ProductNameInvalid = "Product Name invalid";
        public static string ProductListedMessage = "Products listed";
        public static string MaintenanceTime = "Maintenance Time";

        public static string ProductCountOfCategoryError = "10 dan fazla olamaz";

        public static string ProductAlreadyExists = "Aynı isimde ürün eklenemez";
        public static string CategoryListedMessage= "Categories listed";
        public static string CategoryLimitExceded = "Category limit exceeded";

        public static string AuthorizationDenied = "Yetkisiz erişim";
        public static string UserRegistered="User saved";
        public static string UserNotFound="Kullanıcı bulunamadı";
        public static string PasswordError="Parola hatalı";
        public static string SuccessfulLogin="Başarılı giriş";

        public static string UserAlreadyExists = "Kullanıcı zaten var";
        public static string AccessTokenCreated = "Tokena erişildi";
    }
}
