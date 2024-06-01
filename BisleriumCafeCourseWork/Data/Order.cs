using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BisleriumCafeCourseWork.Data
{
	//Represents the orders in the cafe
	public class Order
	{
        public Guid Id { get; set; }

		public Guid CoffeeId { get; set; }

		public List<Guid> AddInId { get; set; }

		public Guid UserId { get; set; }

		public double TotalPrice { get; set; }
        public double CoffeeTotal { get; set; }
        public double AddInTotal { get; set; }


        public Guid CreatedBy { get; set; }

		public DateTime OrderDate { get; set; }



	}
}
