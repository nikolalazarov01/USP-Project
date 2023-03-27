using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace USP_Project.Web.Models.Cars;

public class CreateCarInputModel
{
	[Required]
	[Display(Name = "Brand")]
	public Guid BrandId { get; set; }

	[Required]
	[Display(Name = "Model")]
	public Guid ModelId { get; set; }

	public ICollection<string> Extras { get; set; } = new List<string>();

	public IEnumerable<SelectListItem> Brands { get; set; }
	public IEnumerable<SelectListItem> Models { get; set; }
}