
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

		private readonly ILogger<FridgyClient> logger;
		private ServiceClientCredentials credentials;
		private f.Fridgy client;


		public FridgyClient()
		{
			this.logger = AppLoggerFactory.GetLogger<FridgyClient>();
			client = new f.Fridgy(new TokenCredentials("empty"));
		}

		public FridgyClient(string token)
		{
			this.logger = AppLoggerFactory.GetLogger<FridgyClient>();
			credentials = new TokenCredentials(token);
			client = new f.Fridgy(credentials);
		}

		public string PutItemInFridge(DTOs.Product product)
		{
			Item newitem = new Item();
			Fridge fridge = this.GetFridges().FirstOrDefault();
			if (fridge != null)
			{
				BaseItem item = new BaseItem();
				item.Barcode = product.Barcode;
				item.Qty = 1;
				newitem = client.Add.FridgeMethod(item, fridge.Id.ToString());
			}
			return newitem.Id.ToString();
		}
		public DTOs.Inventory GetFridgeInventory()
		{
			DTOs.Inventory inventory = new DTOs.Inventory();

			Fridge fridge = this.GetFridges().FirstOrDefault();
			if (fridge != null)
			{
				IList<Item> items = this.GetItems(fridge.Id.ToString());

				foreach (Item item in items)
				{
					DTOs.Product product = this.GetProductByBarcode(item.Barcode);

					DTOs.InventoryItem inventoryItem = inventory.Items.SingleOrDefault(w => w.Id == product.Id);
					if (inventoryItem == null)
					{
						inventoryItem = new DTOs.InventoryItem(product);
						inventory.Items.Add(inventoryItem);
					}

					inventoryItem.Quantity += item.Qty;
					inventoryItem.Qty += item.Qty;
				}
			}

			return inventory;
		}

		public IList<DTOs.Product> GetProductsByName(string name)
		{
			IList<DTOs.Product> products = new List<DTOs.Product>();

			IList<Product> productlist = client.Get.Products("name.asc", name);
			foreach (var p in productlist)
			{
				var product = this.MapProduct(p);
				products.Add(product);
			}

			return products;
		}

		public DTOs.Product GetProductByBarcode(string barcode)
		{
			// this function returns the CIU-conform Product object
			Product product = client.Get.Barcode(barcode);
			return MapProduct(product);
		}



		public void SetToken(string value)
		{
			credentials = new TokenCredentials(value);
			client = new f.Fridgy(credentials);
		}

		public void SetBasicAuth(string username, string password)
		{
			BasicAuthenticationCredentials credentials = new BasicAuthenticationCredentials();
			credentials.Password = password;
			credentials.UserName = username;
			client = new f.Fridgy(credentials);
		}

		//public IList<Product> GetProducts()
		//{
		//	IList<Product> productlist = client.Get.Products();
		//	return productlist;
		//}

		// DEPRECIATED
		//public Product GetProductsByBarcode(string barcode)
		//{
		//	Product product = client.Get.Barcode(barcode);
		//	return product;
		//}

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

		public User RegisterUser(string username, string password, string displayname, string email)
		{
			PostUser user = new PostUser();
			user.Displayname = displayname;
			user.Username = username;
			user.Password = password;
			user.Email = email;
			User createdUser = client.Create.UserMethod(user);
			return createdUser;
		}

		public Fridge CreateNewFridge(string name)
		{
			BaseFridge fridge = new BaseFridge();
			fridge.Name = name;
			return client.Create.Fridges(fridge);
		}

		public Fridge AddUserToFridge(string userUUID, string fridgeUUID)
		{
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

		public string RetrieveToken()
		{
			TokensResponse resp = client.Get.Jwttoken();
			if (!(resp is null)) return resp.Token;
			return null;
		}


		private DTOs.Product MapProduct(Product p)
		{
			DTOs.Product product = new DTOs.Product
			{
				Id = p.Id.ToString(),
				Name = p.Name.Trim(),
				Barcode = p.Barcode,
				QtyType = (DTOs.Product.QtyTypes)Enum.Parse(typeof(DTOs.Product.QtyTypes), p.QtyType),
				Qty = (double)p.Qty
			};

			if (p.Nutrient != null)
			{
				// making things shorter...
				var n = p.Nutrient;

				//product.NutritionValues.Add(new DTOs.NutritionValue() { Name = "Energy in kJ", Value = "" }); // TODO: what the value here?

				if (n.EnergyKcal != null)
				{
					product.NutritionValues.Add(new DTOs.NutritionValue() { Name = "energy in kcal", Value = $"{n.EnergyKcal}g" });
				}

				if (n.Fat != null)
				{
					product.NutritionValues.Add(new DTOs.NutritionValue() { Name = "fat", Value = $"{n.Fat}g" });
				}
				if (n.FatSaturated != null)
				{
					product.NutritionValues.Add(new DTOs.NutritionValue() { Name = "saturated fatty acids", Value = $"{n.FatSaturated}g" });
				}
				if (n.Carbs != null)
				{
					product.NutritionValues.Add(new DTOs.NutritionValue() { Name = "carbohydrate", Value = $"{n.Carbs}g" });
					if (n.CarbsSugar != null)
					{
						product.NutritionValues.Add(new DTOs.NutritionValue() { Name = "of which sugar", Value = $"{n.CarbsSugar}g" });
					}
				}
				if (n.Fiber != null)
				{
					product.NutritionValues.Add(new DTOs.NutritionValue() { Name = "dietary fiber", Value = $"{n.Fiber}g" });
				}
				if (n.Protein != null)
				{
					product.NutritionValues.Add(new DTOs.NutritionValue() { Name = "protein", Value = $"{n.Protein}g" });
				}
				if (n.Salt != null)
				{
					product.NutritionValues.Add(new DTOs.NutritionValue() { Name = "salt", Value = $"{n.Salt}g" });
				}
			}

			return product;
		}







	}
}
