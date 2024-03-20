var map; // Define map variable in the outer scope

function GetMap() {
    navigator.geolocation.getCurrentPosition(function (position) {
        var latitude = position.coords.latitude;
        var longitude = position.coords.longitude;

        initializeMap(latitude, longitude);
        fetchAndAddPOIs(latitude, longitude);
    }, function (error) {
        console.error('Error getting user location:', error);

        var belgiumLatitude = 50.5503;
        var belgiumLongitude = 4.3517;

        // Initialize the map centered on Belgium
        initializeMap(belgiumLatitude, belgiumLongitude);
        fetchAndAddPOIs(latitude, longitude);
    });
}

function initializeMap(latitude, longitude) {
    var zoomLevel = latitude === 50.5503 && longitude === 4.3517 ? 7 : 13.5;
    map = new atlas.Map("myMap", {
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
    });
}

function fetchAndAddPOIs(latitude, longitude) {
    var radius = 10000; // Radius in meters
    var apiUrl = `https://overpass-api.de/api/interpreter?data=[out:json];node["amenity"="restaurant"](around:${radius},${latitude},${longitude});out;`;

    fetch(apiUrl)
        .then(response => response.json())
        .then(data => {
            data.elements.forEach(element => {
                if (element.tags && element.tags.name) {
                    var marker = new atlas.HtmlMarker({
                        position: [element.lon, element.lat], // Ensure the coordinates are in [longitude, latitude] format
                        htmlContent: `...`,
                        anchor: 'center'
                    });
                    map.markers.add(marker);
                    console.log(`Name: ${element.tags.name}, Street: ${element.tags.addrstreet}, House Number: ${element.tags.addrhousenumber}`);
                }
            });
        })
        .catch(error => {
            console.error('Error fetching POIs:', error);
        });
}
function displayRestaurantData(name, addrstreet, addrhousenumber, cuisine, phone, website, addrcity, addrpostcode, stars, opening_hours, description) {
    var restaurantDataElement = document.getElementById('restaurantData');
    // Clear the existing restaurant data
    restaurantDataElement.innerHTML = '';
    // Add the new restaurant data
    restaurantDataElement.innerHTML = `
        <h2>${name}</h2>
        <p>${addrstreet} ${addrhousenumber}</p>
        <p>${addrcity}, ${addrpostcode}</p>  
        <p>${cuisine}</p>
        <p>${phone}</p>
        <p><a href="${website}">${website}</a></p>            
        <p>Opening hours: ${opening_hours}</p>
        <p>${description}</p>
    `;
}


$(document).ready(function () {
    GetMap();
});
