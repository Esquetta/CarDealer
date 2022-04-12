using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace CarDealer.Filter
{
    public class ExceptionHandler:ExceptionFilterAttribute
    {
        public string ViewName { get; set; }

        public Type ExceptionType { get; set; }
        public override void OnException(ExceptionContext context)
        {
            if (context.Exception.GetType()==ExceptionType)
            {
                var result = new ViewResult {ViewName = ViewName};
                var modeDataProvider = new EmptyModelMetadataProvider();
                result.ViewData = new ViewDataDictionary(modeDataProvider,context.ModelState);
                result.ViewData.Add("PasswordOrUsernameInvlaid", "Username or password is invlaid");
                context.Result = result;
                context.ExceptionHandled = true;
                base.OnException(context);
            }
        }
    }
}
