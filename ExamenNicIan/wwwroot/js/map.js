function GetMap() {
    var map = new atlas.Map("myMap", {
        center: [50.10789, 4.4764595],
        zoom: 10,
        view: 'Auto',
        authOptions: {
            authType: 'subscriptionKey',
            subscriptionKey: 'lG1hIRQvycC8mByD5fl2Rx-fJdneXcDeUTCfOa9aKnc'
        }
    });

    map.events.add('ready', function () {
        var datasource = new atlas.source.DataSource();
        map.sources.add(datasource);

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
    });
}