const rutas = [
    {
        from: "Bogotá",
        to: "Cali",
        empresa: "Expreso Brasilia",
        tipo: "Bus intermunicipal de larga distancia",
        duracion: "10 horas",
        precio: 95000,
        rating: 5.0,
        img: "https://picsum.photos/300/180?1",
        coords: [4.71, -74.07, 3.42, -76.52]
    },
    {
        from: "Santa Marta",
        to: "Cartagena",
        empresa: "Berlinas",
        tipo: "Servicio directo",
        duracion: "4-5 horas",
        precio: 58000,
        rating: 4.8,
        img: "https://picsum.photos/300/180?2",
        coords: [11.2408, -74.1990, 10.3910, -75.4794]
    },
    {
        from: "Bogotá",
        to: "Medellín",
        empresa: "Rápido Ochoa",
        tipo: "Premium",
        duracion: "8 horas",
        precio: 140000,
        rating: 4.9,
        img: "https://picsum.photos/300/180?3",
        coords: [4.71, -74.07, 6.217, -75.567]
    }
];

const rutasContainer = document.getElementById("rutasContainer");

function crearTarjeta(ruta) {
    const card = document.createElement("div");
    card.className = "flex gap-6 p-4 mb-4 rounded-xl border hover:bg-blue-50 cursor-pointer transition ruta";
    card.dataset.coords = ruta.coords.join(",");

    const img = document.createElement("img");
    img.src = ruta.img;
    img.className = "w-48 h-32 rounded-lg object-cover";

    const info = document.createElement("div");
    info.className = "flex flex-col justify-between w-full";

    info.innerHTML = `
        <h4 class="font-bold text-lg">${ruta.from} → ${ruta.to}</h4>
        <p class="text-sm text-gray-700"><strong>Empresa:</strong> ${ruta.empresa}</p>
        <p class="text-sm text-gray-700"><strong>Tipo:</strong> ${ruta.tipo}</p>
        <p class="text-sm text-gray-700"><strong>Duración:</strong> ${ruta.duracion}</p>
        <div class="flex justify-between items-center mt-2">
            <span>⭐ ${ruta.rating}</span>
            <span class="text-lg font-bold text-[#00335c]">$${ruta.precio.toLocaleString()} COP</span>
        </div>
    `;

    card.appendChild(img);
    card.appendChild(info);

    card.addEventListener("mouseenter", () => {
        let c = card.dataset.coords.split(",").map(Number);
        mostrarRuta(c[0], c[1], c[2], c[3]);
    });

    rutasContainer.appendChild(card);
}

function cargarRutas() {
    rutasContainer.innerHTML = "";
    rutas.forEach(r => crearTarjeta(r));
}

document.addEventListener("DOMContentLoaded", cargarRutas);
