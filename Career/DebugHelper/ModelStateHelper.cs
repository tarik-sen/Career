using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
namespace Career.DebugHelper;

public static class ModelStateHelper{    public static void LogModelValidationDrawbacks(ModelStateDictionary modelState)    {        foreach (var key in modelState.Keys)        {            var errors = modelState[key].Errors;
            foreach (var error in errors)            {                Console.WriteLine($"Validation error for {key}: {error.ErrorMessage}");            }        }    }}