﻿using System.Web.Mvc;

namespace DotNetRevolution.EventSourcing.Web.Areas.Domain
{
    public class DomainAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Domain";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Domain_default",
                "Domain/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new string[] { "DotNetRevolution.EventSourcing.Web.Areas.Domain.Controllers" }
            );
        }
    }
}