﻿using Application.Utils;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Infrastructure.RazorUtils
{
    public class BaseRazorFilter<T> : BaseRazor where T : BaseFilterParam, new()
    {
        public BaseRazorFilter()
        {
            FilterParams = new T();
        }
        [BindProperty(SupportsGet = true)]
        public T FilterParams { get; set; }
    }
}