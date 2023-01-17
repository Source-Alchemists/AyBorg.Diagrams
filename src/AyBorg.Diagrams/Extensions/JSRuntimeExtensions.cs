using System.Threading.Tasks;
using AyBorg.Diagrams.Core.Geometry;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace AyBorg.Diagrams.Extensions
{
    public static class JSRuntimeExtensions
    {
        public static async Task<Rectangle> GetBoundingClientRect(this IJSRuntime jsRuntime, ElementReference element)
        {
            return await jsRuntime.InvokeAsync<Rectangle>("AyBorgDiagrams.getBoundingClientRect", element);
        }

        public static async Task ObserveResizes<T>(this IJSRuntime jsRuntime, ElementReference element,
            DotNetObjectReference<T> reference) where T : class
        {
            await jsRuntime.InvokeVoidAsync("AyBorgDiagrams.observe", element, reference, element.Id);
        }

        public static async Task UnobserveResizes(this IJSRuntime jsRuntime, ElementReference element)
        {
            await jsRuntime.InvokeVoidAsync("AyBorgDiagrams.unobserve", element, element.Id);
        }
    }
}
