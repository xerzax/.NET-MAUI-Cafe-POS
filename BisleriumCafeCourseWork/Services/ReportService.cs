﻿using BisleriumCafeCourseWork.Data;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Element = iTextSharp.text.Element;



namespace BisleriumCafeCourseWork.Services
{

	public class ReportService
	{
		// Method to generate and handle the PDF report
		public static void GeneratePdfReport(IJSRuntime jsRuntime, BisleriumPDF pdfExport)
		{
			// Convert the generated PDF to Base64 string and save it using JS Interop
			jsRuntime.InvokeAsync<object>("saveAsFile", pdfExport.FileName, Convert.ToBase64String(PdfReport(pdfExport)));
			// Optionally, open the PDF file after generation
			jsRuntime.InvokeAsync<object>("openPdfFile", pdfExport.FileName);
		}

		// Method to create the PDF report content
		private static byte[] PdfReport(BisleriumPDF pdfExport)
		{
			var document = new Document(PageSize.A4, 20, 20, 40, 40);

			var output = new MemoryStream();

			var writer = PdfWriter.GetInstance(document, output);

			writer.CloseStream = false;

			writer.PageEvent = new PdfPageEventHelper();

			document.Open();

			var titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 18);

			var title = new Paragraph("Bislerium Revenue Report", titleFont)
			{
				Alignment = Element.ALIGN_CENTER
			};

			document.Add(title);

			var headingFont = FontFactory.GetFont(FontFactory.HELVETICA, 12);

			var headingParagraph = new Paragraph(pdfExport.Title, headingFont)
			{
				Alignment = Element.ALIGN_CENTER,
				SpacingAfter = 20
			};

			document.Add(headingParagraph);

			var dateFont = FontFactory.GetFont(FontFactory.HELVETICA, 10);

			var currentDate = DateTime.Now.ToString("dddd, dd MMMM yyyy", CultureInfo.InvariantCulture);

			var infoParagraph = new Paragraph();

			var frequencyChunk = new Chunk($"Frequency: {pdfExport.Frequency}", dateFont);

			var dateChunk = new Chunk($"{currentDate}", dateFont);

			infoParagraph.Add(frequencyChunk);

			infoParagraph.Add(Chunk.TABBING);

			infoParagraph.Add(Chunk.TABBING);

			infoParagraph.Add(dateChunk);

			infoParagraph.TabSettings = new TabSettings(200f);

			infoParagraph.SpacingAfter = 20;

			document.Add(infoParagraph);

			var coffeeTable = new PdfPTable(5)
			{
				WidthPercentage = 100
			};

			coffeeTable.SetWidths(new[] { 2, 1, 1, 2,2 });

			coffeeTable.AddCell(CreateCell("Coffee Name", true));
			coffeeTable.AddCell(CreateCell("Price", true));
			coffeeTable.AddCell(CreateCell("Qty Sold", true));

			coffeeTable.AddCell(CreateCell("Total Sales", true));
			coffeeTable.AddCell(CreateCell("Last Ordered Date", true));

			if (!pdfExport.CoffeeRecords.Any())
			{
				var noRecordsCell = new PdfPCell(new Phrase("No records to display.", FontFactory.GetFont(FontFactory.HELVETICA)))
				{
					Colspan = 5,
					Padding = 8,
					HorizontalAlignment = Element.ALIGN_CENTER
				};

				coffeeTable.AddCell(noRecordsCell);
			}
			else
			{
				foreach (var item in pdfExport.CoffeeRecords)
				{
					coffeeTable.AddCell(CreateCell(item.Name));
					coffeeTable.AddCell(CreateCell($"NPR {item.Price.ToString("N2", CultureInfo.CreateSpecificCulture("ne-NP"))}"));
					coffeeTable.AddCell(CreateCell(item.Qty.ToString()));
					coffeeTable.AddCell(CreateCell($"NPR {item.TotalSales.ToString("N2", CultureInfo.CreateSpecificCulture("ne-NP"))}"));

					coffeeTable.AddCell(CreateCell(item.LastOrderedDate));
				}
			}

			document.Add(coffeeTable);

			var addInTable = new PdfPTable(5)
			{
				WidthPercentage = 100
			};

			addInTable.SetWidths(new[] { 2, 1, 1, 2,2 });

			addInTable.AddCell(CreateCell("Add In Title", true));
			addInTable.AddCell(CreateCell("Price", true));
			addInTable.AddCell(CreateCell("Qty Sold", true));
			addInTable.AddCell(CreateCell("Total Sales", true));
			addInTable.AddCell(CreateCell("Last Ordered Date", true));

			if (!pdfExport.AddInRecords.Any())
			{
				var noRecordsCell = new PdfPCell(new Phrase("No records to display.", FontFactory.GetFont(FontFactory.HELVETICA)))
				{
					Colspan = 5,
					Padding = 8,
					HorizontalAlignment = Element.ALIGN_CENTER
				};

				addInTable.AddCell(noRecordsCell);
			}
			else
			{
				foreach (var item in pdfExport.AddInRecords)
				{
					addInTable.AddCell(CreateCell(item.Name));
					addInTable.AddCell(CreateCell($"NPR {item.Price.ToString("N2", CultureInfo.CreateSpecificCulture("ne-NP"))}"));
					addInTable.AddCell(CreateCell(item.Qty.ToString()));

					addInTable.AddCell(CreateCell($"NPR {item.TotalSales.ToString("N2", CultureInfo.CreateSpecificCulture("ne-NP"))}"));

					//addInTable.AddCell(CreateCell(item.TotalSales.ToString()));
					addInTable.AddCell(CreateCell(item.LastOrderedDate));
				}
			}

			document.Add(addInTable);

			document.Add(new Paragraph("\n"));

			var revenueFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 14);

			var revenueParagraph = new Paragraph($"Total Revenue: Rs {pdfExport.TotalRevenue.ToString("N2", CultureInfo.CreateSpecificCulture("ne-NP"))}", revenueFont)
			{
				Alignment = Element.ALIGN_CENTER,
				SpacingAfter = 10
			};

			document.Add(revenueParagraph);

			document.Add(new Paragraph("\n"));

			var reportSignatureFont = FontFactory.GetFont(FontFactory.HELVETICA_OBLIQUE, 12);

			var reportFooterSignature = new Paragraph("_____________________________________", reportSignatureFont)
			{
				Alignment = Element.ALIGN_RIGHT,
				SpacingAfter = 10
			};

			document.Add(reportFooterSignature);

			var reportFooterFont = FontFactory.GetFont(FontFactory.HELVETICA_OBLIQUE, 12);

			var reportFooterParagraph = new Paragraph($"{pdfExport.UserName} ({pdfExport.Role})", reportFooterFont)
			{
				Alignment = Element.ALIGN_RIGHT,
				SpacingAfter = 10
			};

			document.Add(reportFooterParagraph);

			document.Close();

			return output.ToArray();
		}

		private static PdfPCell CreateCell(string content, bool isHeader = false)
		{
			var cell = new PdfPCell(new Phrase(content, isHeader ? FontFactory.GetFont(FontFactory.HELVETICA_BOLD) : FontFactory.GetFont(FontFactory.HELVETICA)))
			{
				Padding = 8,
				BackgroundColor = isHeader ? new BaseColor(200, 200, 200) : BaseColor.WHITE
			};

			return cell;
		}
	}
}
