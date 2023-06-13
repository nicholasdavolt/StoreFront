using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreFront.DATA.EF.Models//Metadata
{
	//internal class Partials
	//{
	//}

	#region Builder
	[ModelMetadataType(typeof(BuilderMetadata))]
	public partial class Builder { }
	#endregion

	#region Order
	[ModelMetadataType(typeof(OrderMetadata))]
	public partial class Order { }
	#endregion

	#region Product
	[ModelMetadataType(typeof(ProductMetadata))]
	public partial class Product { }
	#endregion

	#region Status
	[ModelMetadataType(typeof(StatusMetadata))]
	public partial class Status { }
	#endregion

	#region Type
	[ModelMetadataType(typeof(TypeMetadata))]
	public partial class Type { }
	#endregion

	#region UserDetail
	[ModelMetadataType(typeof(UserDetailsMetadata))]
	public partial class UserDetail { }
	#endregion
}
