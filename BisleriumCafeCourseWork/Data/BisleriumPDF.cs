using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BisleriumCafeCourseWork.Data
{
	// Represents a PDF report for Bislerium Cafe
	public class BisleriumPDF
	{
		// Frequency of the report (e.g., Daily, Monthly)
		public string Frequency { get; set; }

		// Title of the PDF report
		public string Title { get; set; }

		// Total revenue covered in the report
		public double TotalRevenue { get; set; }

		// Name of the PDF file
		public string FileName { get; set; }

		// Name of the user generating the report
		public string UserName { get; set; }

		// Role of the user generating the report
		public string Role { get; set; }

		// Collection of coffee-related records in the report
		public IEnumerable<ReportModel> CoffeeRecords { get; set; }

		// Collection of add-in-related records in the report
		public IEnumerable<ReportModel> AddInRecords { get; set; }
	}
}
