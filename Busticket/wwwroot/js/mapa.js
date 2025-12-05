var map = L.map('map').setView([4.5, -74], 6);

L.tileLayer('https://{s}.tile.openstreetmap.fr/hot/{z}/{x}/{y}.png', {
    maxZoom: 18
}).addTo(map);

let rutaActiva = null;

function mostrarRuta(lat1, lon1, lat2, lon2) {
    if (rutaActiva) map.removeControl(rutaActiva);

    rutaActiva = L.Routing.control({
        waypoints: [L.latLng(lat1, lon1), L.latLng(lat2, lon2)],
        lineOptions: { styles: [{ color: '#005dff', weight: 6 }] },
        addWaypoints: false,
        draggableWaypoints: false,
        show: false
    }).addTo(map);

    map.setView([lat1, lon1], 7);
}
