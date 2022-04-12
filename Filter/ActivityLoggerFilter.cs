using CarDealer.DataAcces.Abstract;
using CarDealer.Entities;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UAParser;

namespace CarDealer.Filter
{
    public class ActivityLoggerFilter : IActionFilter
    {
        private IActivityLogDal activityLogDal;
        public ActivityLoggerFilter(IActivityLogDal activityLogDal)
        {
            this.activityLogDal = activityLogDal;
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {

        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var controller = context.RouteData.Values["controller"];
            var action = context.RouteData.Values["action"];
            var url = $"{controller}/{action}";
            var data = "";
            var userAgent = context.HttpContext.Request.Headers["User-Agent"];
            var uaParser = Parser.GetDefault();
            ClientInfo client = uaParser.Parse(userAgent);
            var browser = client.ToString();
            if (!string.IsNullOrEmpty(context.HttpContext.Request.QueryString.Value))
            {
                data = context.HttpContext.Request.QueryString.Value;
            }
            else
            {
                var userdata = context.ActionArguments.FirstOrDefault();
                var stringUserData = JsonConvert.SerializeObject(userdata);
                data = stringUserData;
            }
            var username = context.HttpContext.User.Identity.Name ?? "Anonymous";
            var ipAddress = context.HttpContext.Connection.RemoteIpAddress.ToString();

            SaveActivityLogs(url, data, username, ipAddress,browser);

        }
        private void SaveActivityLogs(string url, string data, string username, string ipAddress,string browser)
        {
            var context = new ActivityLog
            {
                ActivityTime = DateTime.Now,
                Data = data,
                IpAdress = ipAddress,
                Url = url,
                Username = username,
                Browser=browser

            };
            activityLogDal.Add(context);
        }
    }
}
