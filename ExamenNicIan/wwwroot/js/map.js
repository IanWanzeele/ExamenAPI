// map.js
var searchParams = {};

function search() {
    // Retrieve search parameters
    var query = document.getElementById("searchInput").value.trim();
    searchParams = {
        query: query
    };
    // Perform search or further actions based on the search parameters
    // For example:
    console.log("Search Query:", searchParams.query);
}

function GetMap() {
    // Add Map Control JavaScript code here.
    // Instantiate a map object
    var map = new atlas.Map("myMap", {
        view: 'Auto',

        // Add your Azure Maps subscription key. https://aka.ms/am-primaryKey
        authOptions: {
            authType: 'subscriptionKey',
            subscriptionKey: 'lG1hIRQvycC8mByD5fl2Rx-fJdneXcDeUTCfOa9aKnc'
        }
    });
    // Wait until the map resources are ready.
    map.events.add('ready', function () {
        // Create a data source and add it to the map.
        var datasource = new atlas.source.DataSource();
        map.sources.add(datasource);

        // Add a layer for rendering point data.
        var resultLayer = new atlas.layer.SymbolLayer(datasource, null, {
            iconOptions: {
                image: 'pin-round-darkblue',
                anchor: 'center',
                allowOverlap: true
            },
            textOptions: {
                anchor: "top"
            }
        });

        map.layers.add(resultLayer);

        // Use MapControlCredential to share authentication between a map control and the service module.
        var pipeline = atlas.service.MapsURL.newPipeline(new atlas.service.MapControlCredential(map));
        // Construct the SearchURL object
        var searchURL = new atlas.service.SearchURL(pipeline);

        // Perform initial search (e.g., for restaurants)
        var query = 'restaurants';
        var radius = 9000;
        var lat = 47.64452336193245;
        var lon = -122.13687658309935;

        searchURL.searchPOI(atlas.service.Aborter.timeout(10000), query, {
            limit: 10,
            lat: lat,
            lon: lon,
            radius: radius,
            view: 'Auto'
        }).then((results) => {
            // Extract GeoJSON feature collection from the response and add it to the datasource
            var data = results.geojson.getFeatures();
            datasource.add(data);
            // Set camera to bounds to show the results
            map.setCamera({
                bounds: data.bbox,
                zoom: 10,
                padding: 15
            });

        });

        // Add a click event listener to the map to retrieve the selected location
        map.events.add('click', function (e) {
            var lat = e.position[0];
            var lon = e.position[1];
            // Perform further actions with latitude and longitude (e.g., setting a filter radius)
            console.log("Latitude:", lat, "Longitude:", lon);
        });
    });
}