using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BisleriumCafeCourseWork.Data
{
	// Model representing a report item for Bislerium Cafe
	public class ReportModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public double Price { get; set; }
		public double Qty { get; set; }


		public double TotalSales { get; set; }

        public string LastOrderedDate { get; set; }
    }
}
