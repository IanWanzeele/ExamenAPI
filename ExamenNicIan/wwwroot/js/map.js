function GetMap() {
    navigator.geolocation.getCurrentPosition(function (position) {
        var latitude = position.coords.latitude;
        var longitude = position.coords.longitude;
        console.log("Latitude: " + latitude + ", Longitude: " + longitude);

        // Initialize the map centered on the user's location
        initializeMap(latitude, longitude);

        // Send AJAX request with user's location
        sendLocationAjax(latitude, longitude);
    }, function (error) {
        console.error('Error getting user location:', error);

        // If user denies access to their location or if geolocation is not supported, center the map on Belgium
        var belgiumLatitude = 50.5503;
        var belgiumLongitude = 4.3517;

        // Initialize the map centered on Belgium
        initializeMap(belgiumLatitude, belgiumLongitude);

        // Send AJAX request with Belgium's location
        sendLocationAjax(belgiumLatitude, belgiumLongitude);
    });
}

function initializeMap(latitude, longitude) {
    var zoomLevel = latitude === 50.5503 && longitude === 4.3517 ? 7 : 13.5;
    var map = new atlas.Map("myMap", {
        center: [longitude, latitude], // Center the map on the specified location
        zoom: zoomLevel,
        view: 'Auto',
        authOptions: {
            authType: 'subscriptionKey',
            subscriptionKey: 'lG1hIRQvycC8mByD5fl2Rx-fJdneXcDeUTCfOa9aKnc'
        }
    });

    // Wait for the map to be ready
    map.events.add('ready', function () {
        // Remove hidden elements
        var hiddenElements = document.querySelectorAll('.hidden-accessible-element');
        hiddenElements.forEach(function (element) {
            element.parentNode.removeChild(element);
        });

        // Remove atlas-control-container element
        var controlContainer = document.querySelector('.atlas-control-container');
        if (controlContainer) {
            controlContainer.parentNode.removeChild(controlContainer);
        }

        // Add a marker at the specified location
        var marker = new atlas.HtmlMarker({
            position: [longitude, latitude],
            htmlContent: '<div style="color: red;">You are here</div>'
        });

        map.markers.add(marker);
    });
}

function sendLocationAjax(latitude, longitude) {
    console.log('Latitude:', latitude);
    console.log('Longitude:', longitude);
    // Send AJAX request to HomeController action with latitude and longitude parameters
    $.ajax({
        url: '/Restaurants/Index',
        method: 'POST',
        data: { latitude: latitude, longitude: longitude },
        success: function (response) {
            console.log('Location processed successfully:', response);
        },
        error: function (xhr, status, error) {
            console.error('Error processing location:', error);
        }
    });
}

$(document).ready(function () {
    GetMap();
});
