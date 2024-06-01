using BisleriumCafeCourseWork.Data;
using BisleriumCafeCourseWork.Data.Enum;
using Microsoft.AspNetCore.Components;
using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BisleriumCafeCourseWork.Services
{
    public class OrderService:GenericService<Order>
    {
		public static string appDataDirectoryPath = UtilityService.GetAppDirectoryPath();
		public static string appProductsFilePath = UtilityService.GetAppCoffeeOrdersFilePath();

	
		// getting order by id
        public static Order GetById(Guid Id)
		{
			var order = GetAll(appProductsFilePath).FirstOrDefault(x => x.Id == Id);

			if (order == null)
			{
				throw new Exception("Product not found.");
			}

			return order;
		}

		//deleting the order by id
		public static List<Order> Delete(Guid id)
		{
			var orderList = GetAll(appProductsFilePath);
			var orderToDelete = orderList.FirstOrDefault(x => x.Id == id);

			if (orderToDelete == null)
			{
				throw new Exception("order type not found.");
			}

			orderList.Remove(orderToDelete);

			SaveAll(orderList, appDataDirectoryPath, appProductsFilePath);

			return orderList;
		}



		//checking the users privelege
		public static OrderScheme  CheckPrivelege(Guid userid)
		{
			var orderList = GetAll(appProductsFilePath);
			var orderByUser = orderList.Where(x => x.UserId == userid).ToList();
			if (orderByUser.Count >= 10)
			{
				return OrderScheme.Free;
			}
			if (orderByUser != null)
			{
				// Calculate the first and last day of the previous month
				var today = DateTime.Today;
				var firstDayOfLastMonth = new DateTime(today.Year, today.Month, 1).AddMonths(-1);
				var lastDayOfLastMonth = firstDayOfLastMonth.AddMonths(1).AddDays(-1);

				// Count the number of weekdays in the previous month
				int totalWeekdays = Enumerable.Range(0, (lastDayOfLastMonth - firstDayOfLastMonth).Days + 1)
					.Count(day => firstDayOfLastMonth.AddDays(day).DayOfWeek != DayOfWeek.Saturday && firstDayOfLastMonth.AddDays(day).DayOfWeek != DayOfWeek.Sunday);

				// Filter orders to last month and exclude weekends, then count distinct weekdays with orders
				int distinctOrderWeekdays = orderByUser
					.Where(x => x.OrderDate >= firstDayOfLastMonth && x.OrderDate <= lastDayOfLastMonth && x.OrderDate.DayOfWeek != DayOfWeek.Saturday && x.OrderDate.DayOfWeek != DayOfWeek.Sunday)
					.Select(x => x.OrderDate.Date)
					.Distinct()
					.Count();

				return distinctOrderWeekdays == totalWeekdays ? OrderScheme.Discount : OrderScheme.None;

			}
				return OrderScheme.None;
		}

		//creating the new order
		public static  List<Order> Create(Guid userId,Guid createdBy, Guid coffeeId, List<Guid> addInId,Double coffeTotal, double addInTotal, double totalPrice, DateTime orderDate)
		{
			var orderList = GetAll(appProductsFilePath);

			if(userId == Guid.Empty || coffeeId == Guid.Empty)
			{
				throw new Exception("Select Coffee and User.");

			}

			var order = new Order()
			{
				Id = Guid.NewGuid(),
				UserId = userId,
				CoffeeId = coffeeId,
				AddInId = addInId,
				CoffeeTotal = coffeTotal,
				AddInTotal = addInTotal,
				TotalPrice = totalPrice,
				CreatedBy = createdBy,
				OrderDate = orderDate
			};

			orderList.Add(order);
			SaveAll(orderList, appDataDirectoryPath, appProductsFilePath);
			return orderList;
		}

		//editing the order
		public static List<Order> Edit(Guid id, Guid coffeeId, List<Guid> addInId, double totalPrice)
		{
			var orderList = GetAll(appProductsFilePath);
			var orderToUpdate = orderList.FirstOrDefault(x => x.Id == id);

			if (orderToUpdate == null)
			{
				throw new Exception("Order not found.");
			}

			orderToUpdate.CoffeeId = coffeeId;
			orderToUpdate.AddInId = addInId;
			orderToUpdate.TotalPrice = totalPrice;

			SaveAll(orderList, appDataDirectoryPath, appProductsFilePath);
			return orderList;
		}

		//accessing the addins 
		public static List<Guid> GetAddInList(Guid id)
		{
			var orderList = GetAll(appProductsFilePath);

			var orderToUpdate = orderList.FirstOrDefault(x => x.Id == id);
			return orderToUpdate.AddInId;
		}

	}
}
