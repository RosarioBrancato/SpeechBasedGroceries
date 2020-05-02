
using f = SpeechBasedGroceries.Parties.Fridgy.Client;
using SpeechBasedGroceries.Parties.Fridgy.Client.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using RestSharp;
using SpeechBasedGroceries.AppServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SpeechBasedGroceries.Parties.Fridgy.Client;
using Microsoft.Rest;
using Microsoft.VisualBasic.CompilerServices;

namespace SpeechBasedGroceries.Parties.Fridgy
{
	public class FridgyClient
	{

		private readonly ILogger<FridgyClient> _logger;
		private Microsoft.Rest.ServiceClientCredentials credentials;
		private f.Fridgy client;

		public FridgyClient()
		{
			_logger = AppLoggerFactory.GetLogger<FridgyClient>();
			client = new f.Fridgy(new TokenCredentials("empty"));
		}

		public FridgyClient(string token)
		{
			_logger = AppLoggerFactory.GetLogger<FridgyClient>();
			credentials = new TokenCredentials(token);
			client = new f.Fridgy(credentials);
		}

		public void setToken(string value)
		{
			credentials = new TokenCredentials(value);
			client = new f.Fridgy(credentials);

		}
		public void setBasicAuth(string username, string password) {
			BasicAuthenticationCredentials credentials = new BasicAuthenticationCredentials();
			credentials.Password = password;
			credentials.UserName = username;
			client = new f.Fridgy(credentials);
		}

		public IList<Product> GetProducts()
		{
			IList<Product> productlist = client.Get.Products();
			return productlist;
		}

		public IList<Product> GetProductsByName(string name)
		{
			IList<Product> productlist = client.Get.Products("name.asc", name);
			return productlist;
		}

        // DEPRECIATED
        public Product GetProductsByBarcode(string barcode)
		{
			Product product = client.Get.Barcode(barcode);
			return product;
		}

		public DTOs.Product GetProductByBarcode(string barcode)
		{
            // this function returns the CIU-conform Product object
            Product product = client.Get.Barcode(barcode);
			return MappingProduct(product);
		}

		public IList<Fridge> GetFridges()
		{
			IList<Fridge> fridges = client.Get.Fridges();
			return fridges;
		}
		public IList<Item> GetItems(string fridgeUUID)
		{
			IList<Item> items = client.Get.Items(fridgeUUID);
			return items;
		}

		public User RegisterUser(string username, string password, string displayname, string email) {
			PostUser user = new PostUser();
			user.Displayname = displayname;
			user.Username = username;
			user.Password = password;
			user.Email = email;
			User createdUser = client.Create.UserMethod(user);
			return createdUser;
		}

		public Fridge CreateNewFridge(string name) {
			BaseFridge fridge = new BaseFridge();
			fridge.Name = name;
			return client.Create.Fridges(fridge);
		}

		public Fridge AddUserToFridge(string userUUID, string fridgeUUID) {
			Owner owner = new Owner();
			owner.Uuid = Guid.Parse(userUUID);
			return client.Add.Owners(owner, fridgeUUID);
		}

		public void RemoveUserFromFridge(string userUUID, string fridgeUUID)
		{
			client.Delete.Owners(fridgeUUID, userUUID);
		}

		public void DeleteUser(string uuid)
		{
			client.Delete.Users(uuid);
		}

		public string RetrieveToken() {
			TokensResponse resp = client.Get.Jwttoken();
			if (!(resp is null)) return resp.Token;
			return null;
		}






        private DTOs.Product MappingProduct(Product p)
        {
			DTOs.Product product = new DTOs.Product
			{
				Id = p.Id.ToString(),
				Name = p.Name,
				Barcode = p.Barcode
			};

            // making things shorter...
			var n = p.Nutrient;

			product.NutritionValues.Add(new DTOs.NutritionValue() { Name = "Energy in kJ", Value = "" }); // TODO: what the value here?
			product.NutritionValues.Add(new DTOs.NutritionValue() { Name = "Energy in kcal", Value = $"{n.EnergyKcal}g" });
			product.NutritionValues.Add(new DTOs.NutritionValue() { Name = "Fat", Value = $"{n.Fat}g" });
			product.NutritionValues.Add(new DTOs.NutritionValue() { Name = "saturated fatty acids", Value = $"{n.FatSaturated}g" });
			product.NutritionValues.Add(new DTOs.NutritionValue() { Name = "Carbohydrate", Value = $"{n.Carbs}g" });
			product.NutritionValues.Add(new DTOs.NutritionValue() { Name = "of which sugar", Value = $"{n.CarbsSugar}g" });
			product.NutritionValues.Add(new DTOs.NutritionValue() { Name = "Dietary fiber", Value = $"{n.Fiber}g" });
			product.NutritionValues.Add(new DTOs.NutritionValue() { Name = "Protein", Value = $"{n.Protein}g" });
			product.NutritionValues.Add(new DTOs.NutritionValue() { Name = "Salt", Value = $"{n.Salt}g" });

			return product;
		}







	}
}
