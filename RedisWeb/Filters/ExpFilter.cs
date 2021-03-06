﻿using Newtonsoft.Json;
using RedisWeb.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RedisWeb.Filters
{
    public class ExpFilter : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            Exception exp = filterContext.Exception;

            //获取ex的第一级内部异常
            Exception innerEx = exp.InnerException == null ? exp : exp.InnerException;
            //循环获取内部异常直到获取详细异常信息为止
            while (innerEx.InnerException != null)
            {
                innerEx = innerEx.InnerException;
            }
            NLogLogger nlog = new NLogLogger();
            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                nlog.Error(innerEx.Message);
                JsonConvert.SerializeObject(new { status = 1, msg = "请求发生错误，请联系管理员" });
            }
            else
            {
                nlog.Error("Error", exp);
                ViewResult vireResult = new ViewResult();
                vireResult.ViewName = "/Views/Shared/Error.cshtml";
                filterContext.Result = vireResult;
            }
            //告诉MVC框架异常被处理
            filterContext.ExceptionHandled = true;
            base.OnException(filterContext);
        }
    }
}