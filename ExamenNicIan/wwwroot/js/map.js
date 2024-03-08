function GetMap() {
    // Get user's location using Azure Maps Geolocation service
    navigator.geolocation.getCurrentPosition(function (position) {
        var latitude = position.coords.latitude;
        var longitude = position.coords.longitude;
        console.log("Latitude: " + latitude + ", Longitude: " + longitude);

        var map = new atlas.Map("myMap", {
            center: [longitude, latitude], // Center the map on the user's location
            zoom: 13.5,
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

            // Add a marker at the user's location
            var marker = new atlas.HtmlMarker({
                position: [longitude, latitude],
                htmlContent: '<div style="color: red;">You are here</div>'
            });

            map.markers.add(marker);
        });
    }, function (error) {
        // If user denies access to their location or if geolocation is not supported, center the map on Belgium
        var map = new atlas.Map("myMap", {
            center: [4.3517,50.5503], // Center on Belgium
            zoom: 7,
            view: 'Auto',
            authOptions: {
                authType: 'subscriptionKey',
                subscriptionKey: 'lG1hIRQvycC8mByD5fl2Rx-fJdneXcDeUTCfOa9aKnc'
            }
        });
    });
}

window.onload = GetMap;
