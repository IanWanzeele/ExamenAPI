var map;

function GetMap() {
    navigator.geolocation.getCurrentPosition(function (position) {
        var latitude = position.coords.latitude;
        var longitude = position.coords.longitude;

        initializeMap(latitude, longitude);

    }, function (error) {
        console.error('Error getting user location:', error);

        var belgiumLatitude = 50.5503;
        var belgiumLongitude = 4.3517;

        // Centreer de kaart op Belgie als location niet aanstaat
        initializeMap(belgiumLatitude, belgiumLongitude);
        
    });
}

function initializeMap(latitude, longitude) {
    var zoomLevel = latitude === 50.5503 && longitude === 4.3517 ? 7 : 11;
    map = new atlas.Map("myMap", {
        center: [longitude, latitude],
        zoom: zoomLevel, 
        view: 'Auto',
        authOptions: {
            authType: 'subscriptionKey',
            subscriptionKey: 'lG1hIRQvycC8mByD5fl2Rx-fJdneXcDeUTCfOa9aKnc'
        }
        
    });

    
    map.events.add('ready', function () {
        // Verwijder hidden data
        var hiddenElements = document.querySelectorAll('.hidden-accessible-element');
        hiddenElements.forEach(function (element) {
            element.parentNode.removeChild(element);
        });

        // Verwijder atlas-control-container element
        var controlContainer = document.querySelector('.atlas-control-container');
        if (controlContainer) {
            controlContainer.parentNode.removeChild(controlContainer);
        }
        fetchAndAddPOIs(latitude, longitude);
    });
}

function fetchAndAddPOIs(latitude, longitude) {
    if (latitude === 50.5503 && longitude === 4.3517) {
        return; 
    }
    var radius = 10000; // In meters
    var apiUrl = `https://overpass-api.de/api/interpreter?data=[out:json];node["amenity"="restaurant"](around:${radius},${latitude},${longitude});out;`;

    fetch(apiUrl)
        .then(response => response.json())
        .then(data => {
            var dataSource = new atlas.source.DataSource();
            map.sources.add(dataSource);

            data.elements.forEach(element => {
                if (element.tags && element.tags.name) {
                    var point = new atlas.data.Point([element.lon, element.lat]);
                    var feature = new atlas.data.Feature(point, {
                        name: element.tags.name,
                        addrstreet: element.tags['addr:street'] || '',
                        addrhousenumber: element.tags['addr:housenumber'] || '',
                        cuisine: element.tags.cuisine || '',
                        phone: element.tags.phone || '',
                        website: element.tags.website || '',
                        addrcity: element.tags['addr:city'] || '',
                        addrpostcode: element.tags['addr:postcode'] || '',
                        stars: element.tags.stars || '',
                        opening_hours: element.tags.opening_hours || '',
                        description: element.tags.description || ''
                    });
                    dataSource.add(feature);
                }
            });

            //Toevoegen van POI's aan de kaart
            map.layers.add(new atlas.layer.SymbolLayer(dataSource, null, {
                textOptions: {
                    textField: ['get', 'name'], 
                    offset: [0, 1.2], 
                    color: 'red' 
                }
            }));
            var symbols = document.querySelectorAll('.atlas-map svg image');
            symbols.forEach(function (symbol) {
                symbol.style.cursor = 'pointer';
            });


            // Klikevent voor de map
            map.events.add('click', function (e) {
               
                if (e.shapes && e.shapes.length > 0 && e.shapes[0] instanceof atlas.Shape) {
                    var properties = e.shapes[0].getProperties();
                    displayRestaurantData(properties.name,
                        properties.addrstreet,
                        properties.addrhousenumber,
                        properties.cuisine,
                        properties.phone,
                        properties.website,
                        properties.addrcity,
                        properties.addrpostcode,
                        properties.stars,
                        properties.opening_hours,
                        properties.description);
                }
            });
        })
        .catch(error => {
            console.error('Error fetching POIs:', error);
        });
}

function displayRestaurantData(name, addrstreet, addrhousenumber, cuisine, phone, website, addrcity, addrpostcode, stars, opening_hours, description) {
    var restaurantDataElement = document.getElementById('restaurantData');
    // Verwijder restaurant data
    restaurantDataElement.innerHTML = '';
    // Voeg restaurant data opnieuw toe
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
