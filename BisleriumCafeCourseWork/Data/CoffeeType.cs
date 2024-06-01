using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BisleriumCafeCourseWork.Data
{
	// Represents different types of coffee available
	public class CoffeeType
	{
		// Unique identifier for a coffee type
		public Guid Id{ get; set; }
        public Guid CreatedBy { get; set; }

        public string Type { get; set; }

		public double Price { get; set; }
	}
}
