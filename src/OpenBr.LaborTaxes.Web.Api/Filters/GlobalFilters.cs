﻿using Microsoft.AspNetCore.Mvc;

namespace OpenBr.LaborTaxes.Web.Api.Filters
{

    /// <summary>
    /// Filter global da aplicação
    /// </summary>
    public static class GlobalFilters
    {

        /// <summary>
        /// Configure filters
        /// </summary>
        /// <param name="opt">Mvc options object instance</param>
        public static void Configure(MvcOptions opt)
        {
            opt.Filters.Add<ValidateModelFilter>();
        }
    }

}
