using Microsoft.AspNetCore.Mvc;
using Microsoft.FSharp.Core;
using System;
using System.Threading.Tasks;
using FSharpPlusCSharp;

namespace SomeBasicEFApp.Web.Controllers
{
    public static class ControllerExtensions
    {
        public static IActionResult NotFoundWhenMissing<T>(this ControllerBase c, FSharpOption<T> option, Func<T, IActionResult> onValue) =>
            option.Match<T, IActionResult>(value => onValue(value), () => c.NotFound());
        public static async Task<IActionResult> NotFoundWhenMissingAsync<T>(this ControllerBase c, FSharpOption<T> option, Func<T, Task<IActionResult>> onValue) =>
            await option.Match<T, Task<IActionResult>>(async value => await onValue(value), async () => await Task.FromResult(c.NotFound()));
        public static IActionResult BadRequestWhenMissing<T>(this ControllerBase c, FSharpOption<T> option, Func<T, IActionResult> onValue) =>
            option.Match<T, IActionResult>(value => onValue(value), () => c.BadRequest());
        public static async Task<IActionResult> BadRequestWhenMissingAsync<T>(this ControllerBase c, FSharpOption<T> option, Func<T, Task<IActionResult>> onValue) =>
            await option.Match<T, Task<IActionResult>>(async value => await onValue(value), async () => await Task.FromResult(c.BadRequest()));
    }
}
