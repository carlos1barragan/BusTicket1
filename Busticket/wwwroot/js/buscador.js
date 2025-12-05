document.getElementById("btnBuscar").addEventListener("click", filtrar);

function filtrar() {
    const desde = document.getElementById("desdeInput").value.toLowerCase();
    const hasta = document.getElementById("hastaInput").value.toLowerCase();

    document.querySelectorAll(".ruta").forEach(card => {
        const texto = card.innerText.toLowerCase();

        card.style.display =
            texto.includes(desde) && texto.includes(hasta)
                ? "flex"
                : "none";
    });
}
