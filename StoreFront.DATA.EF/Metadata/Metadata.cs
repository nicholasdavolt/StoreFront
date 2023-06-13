using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreFront.DATA.EF.Models//Metadata
{
	//internal class Metadata
	//{
	//}

	#region BuilderMetadata
	public class BuilderMetadata
	{
		//public int Id { get; set; }

		[Required(ErrorMessage = "*Builder is required")]
		[StringLength(50, ErrorMessage = "*Max 50 characters")]
		[Display(Name = "Builder")]
		public string Builder1 { get; set; } = null!;
	}
	#endregion

	#region OrderMetadata

	public class OrderMetadata
	{
		//public int Id { get; set; }
		//public string UserId { get; set; } = null!;

		[Required(ErrorMessage = "*Order Date is required")]
		[Display(Name = "Order Date")]
		[DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]

		public DateTime OrderDate { get; set; }

		[Required(ErrorMessage = "*Ship To is required")]
		[StringLength(100, ErrorMessage = "*Max 100 characters")]
		[Display(Name = "Ship To")]
		public string ShipToName { get; set; } = null!;


		[Required(ErrorMessage = "*City is required")]
		[StringLength(50, ErrorMessage = "*Max 50 characters")]
		[Display(Name = "City")]
		public string ShipCity { get; set; } = null!;

		[StringLength(2, ErrorMessage = "*Max 2 characters")]
		[Display(Name = "State")]
		public string? ShipState { get; set; }

		[Required(ErrorMessage = "*Zip is required")]
		[StringLength(5, ErrorMessage = "*Max 5 characters")]
		[Display(Name = "Zip")]
		[DataType(DataType.PostalCode)]
		public string ShipZip { get; set; } = null!;

	}
	#endregion

	#region ProductMetadata

	public class ProductMetadata
	{
		//public int Id { get; set; }

		[Required(ErrorMessage = "*Product is required")]
		[StringLength(50, ErrorMessage = "*Max 50 characters")]
		[Display(Name = "Product")]
		public string ProductName { get; set; } = null!;

		[StringLength(1500, ErrorMessage = "*Max 1500 characters")]
		[Display(Name = "Description")]
		public string? ProductDesc { get; set; }

		[Display(Name = "Units in Stock" )]
		public short? UnitsInStock { get; set; }

		[Display(Name ="Sell Price")]
		[DataType(DataType.Currency)]
		[Range(0, (double)decimal.MaxValue)]
		public decimal? SellPrice { get; set; }

		[Display(Name = "Purchase Price")]
		[DataType(DataType.Currency)]
		[Range(0, (double)decimal.MaxValue)]
		public decimal? PurchasePrice { get; set; }
		//public int TypeId { get; set; }
		//public int StatusId { get; set; }
		//public int? BuilderId { get; set; }

		[StringLength(75)]
		[Display(Name = "Image")]
		public string? ProductImage { get; set; }
	}
	#endregion

	#region StatusMetadata
	public class StatusMetadata
	{
		//public int Id { get; set; }

		[StringLength(50, ErrorMessage = "*Max 50 characters")]
		[Display(Name = "Status")]
		public string? Status1 { get; set; }
	}
	#endregion

	#region TypeMetadata
	public class TypeMetadata
	{

		//public int Id { get; set; }

		[Required(ErrorMessage = "Type is required")]
		[StringLength(50, ErrorMessage = "*Max 50 characters")]
		public string Type1 { get; set; } = null!;
	}

	#endregion

	#region UserDetailMetadata
	public class UserDetailsMetadata
	{
		//public string UserId { get; set; } = null!;

		[Required(ErrorMessage = "*First Name is required")]
		[StringLength(50, ErrorMessage = "*Max 50 characters")]
		[Display(Name = "First Name")]
		public string FirstName { get; set; } = null!;

		[Required(ErrorMessage = "*Last Name is required")]
		[StringLength(50, ErrorMessage = "*Max 50 characters")]
		[Display(Name = "Last Name")]
		public string LastName { get; set; } = null!;

		[StringLength(150, ErrorMessage = "*Max 150 characters")]
		public string? Address { get; set; }

		[StringLength(50, ErrorMessage = "*Max 50 characters")]
		public string? City { get; set; }

		[StringLength(2, ErrorMessage = "*Max 2 characters")]
		public string? State { get; set; }

		[StringLength(5, ErrorMessage = "*Max 5 characters")]
		[DataType(DataType.PostalCode)]
		public string? Zip { get; set; }

		[StringLength(24, ErrorMessage = "*Max 24 Characters")]
		[DataType(DataType.PhoneNumber)]
		public string? Phone { get; set; }
	}

	#endregion
}
