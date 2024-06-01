using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace BisleriumCafeCourseWork.Data
{
	// Represents add-ins for coffee
	public class CoffeeAddIns
	{
		//represents the unique identifier for coffee add-ins
        public Guid Id { get; set; }
		[Required(ErrorMessage = "Name is required")]
		[StringLength(100, ErrorMessage = "Name must be less than 100 characters")]
		public string Name { get; set; }

		public Guid CreatedBy { get; set; }

		[Required(ErrorMessage = "Price is required")]
		[Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero")]
		public double Price { get; set; }
	}
}
