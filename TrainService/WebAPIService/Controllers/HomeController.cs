﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebAPIService.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "选择车站";
            ViewBag.StationList = StationsController._Get();
            return View();
        }

        public ActionResult DishBook(string station)
        {
            ViewBag.Tags = TagsController._Get();
            ViewBag.Station = station;
            return View();
        }

        public ActionResult MyOrders()
        {
            return View();
        }


        public ActionResult OrderDetail(uint orderId)
        {
            var model = DAL.DalFactory.Orders.GetOrderByOrderId(orderId);
            ViewBag.SubOrders = DAL.DalFactory.Orders.GetSubOrders(Convert.ToInt32(orderId));
            return View(model);
        }
    }
}
