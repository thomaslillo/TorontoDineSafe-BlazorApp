using Microsoft.JSInterop;
using System.Threading.Tasks;
using System.Collections.Generic;
using TorontoDineSafeApp.Models;

namespace TorontoDineSafeApp.Services
{
    public class MapJsInterop
    {
        private readonly IJSRuntime _jsRuntime;

        public MapJsInterop(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async Task<bool> InitializeMapAsync()
        {
            return await _jsRuntime.InvokeAsync<bool>("initializeMap");
        }

        public async Task<bool> AddMarkersAsync(IEnumerable<DineSafeRecord> locations)
        {
            return await _jsRuntime.InvokeAsync<bool>("addMarkers", new object[] { locations });
        }

        public async Task<bool> ResizeMapAsync()
        {
            return await _jsRuntime.InvokeAsync<bool>("resizeMap");
        }
    }
}
