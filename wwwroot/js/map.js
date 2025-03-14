// Map initialization and management
let map;
let markers = [];
let markerClusterGroup;

// Initialize the map
window.initializeMap = function () {
    // Create map centered on Toronto
    map = L.map('map').setView([43.6532, -79.3832], 11);

    // Add OpenStreetMap tile layer
    L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
        attribution: '&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
    }).addTo(map);

    return true;
};

// Add markers to the map
window.addMarkers = function (locations) {
    // Clear existing markers
    clearMarkers();
    
    // Add new markers
    for (let i = 0; i < locations.length; i++) {
        const location = locations[i];
        
        // Create marker
        const marker = L.marker([location.latitude, location.longitude]);
        
        // Create popup content
        const popupContent = `
            <div>
                <h5>${location.establishmentName}</h5>
                <p><strong>Address:</strong> ${location.establishmentAddress}</p>
                <p><strong>Type:</strong> ${location.establishmentType}</p>
                <p><strong>Status:</strong> ${location.establishmentStatus}</p>
                ${location.infractionDetails ? `<p><strong>Infraction:</strong> ${location.infractionDetails}</p>` : ''}
                ${location.severity ? `<p><strong>Severity:</strong> ${location.severity}</p>` : ''}
                <p><strong>Inspection Date:</strong> ${location.inspectionDate}</p>
            </div>
        `;
        
        // Bind popup to marker
        marker.bindPopup(popupContent);
        
        // Add marker to map
        marker.addTo(map);
        
        // Store marker reference
        markers.push(marker);
    }
    
    // Adjust map view to fit all markers if there are any
    if (markers.length > 0) {
        const group = new L.featureGroup(markers);
        map.fitBounds(group.getBounds());
    }
    
    return true;
};

// Clear all markers from the map
function clearMarkers() {
    for (let i = 0; i < markers.length; i++) {
        map.removeLayer(markers[i]);
    }
    markers = [];
}

// Resize map when container size changes
window.resizeMap = function () {
    if (map) {
        map.invalidateSize();
    }
    return true;
};
