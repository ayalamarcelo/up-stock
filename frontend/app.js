const API_URL = 'https://localhost:5432/api';

document.addEventListener("DOMContentLoaded", async () => {
    await cargarClientes();
    await cargarActivos();
});

async function cargarClientes() {
    try {
        const response = await fetch(`${API_URL}/clients`);
        const clientes = await response.json();
        const select = document.getElementById('select-clientes');
        
        select.innerHTML = '<option value="">-- Selecciona un cliente --</option>';
        
        clientes.forEach(cliente => {
            if (cliente.isActive) {
                select.innerHTML += `<option value="${cliente.clientID}">${cliente.name} (${cliente.dniCuit})</option>`;
            }
        });
    } catch (error) {
        console.error("Error al cargar clientes:", error);
        document.getElementById('select-clientes').innerHTML = '<option value="">Error al conectar con la API</option>';
    }
}

async function cargarActivos() {
    try {
        const response = await fetch(`${API_URL}/assets`);
        const activos = await response.json();
        const contenedor = document.getElementById('contenedor-activos');
        
        contenedor.innerHTML = ''; 

        activos.forEach(activo => {
            if (!activo.isDeleted) {

                contenedor.innerHTML += `
                    <label class="flex items-center space-x-3 bg-white p-2.5 rounded-md border border-gray-100 shadow-sm cursor-pointer hover:bg-gray-100 transition">
                        <input type="checkbox" name="activos_seleccionados" value="${activo.assetID}" class="w-4 h-4 text-blue-600 border-gray-300 rounded focus:ring-blue-500">
                        <span class="text-gray-900 font-medium text-sm">${activo.name}</span>
                        <span class="text-xs text-gray-400 font-mono bg-gray-100 px-2 py-0.5 rounded ml-auto">${activo.codeID}</span>
                    </label>
                `;
            }
        });
        
        if (contenedor.children.length === 0) {
            contenedor.innerHTML = '<p class="text-gray-500 text-sm">No hay activos disponibles.</p>';
        }
    } catch (error) {
        console.error("Error al cargar activos:", error);
        document.getElementById('contenedor-activos').innerHTML = '<p class="text-red-500 text-sm">Error al cargar el inventario.</p>';
    }
}

document.getElementById('form-alquiler').addEventListener('submit', async (e) => {
    e.preventDefault(); 

    const clientID = document.getElementById('select-clientes').value;
    const rentalDateExpected = document.getElementById('fecha-esperada').value;
   
    const checkboxes = document.querySelectorAll('input[name="activos_seleccionados"]:checked');
    const assetIDs = Array.from(checkboxes).map(cb => cb.value);

    if (assetIDs.length === 0) {
        alert("Por favor, selecciona al menos un activo para alquilar.");
        return;
    }

    const payload = {
        clientID: clientID,
        rentalDateExpected: new Date(rentalDateExpected).toISOString(),
        userID: "00000000-0000-0000-0000-000000000000",
        assets: assetIDs 
    };

    try {
        const response = await fetch(`${API_URL}/rental`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(payload)
        });

        if (response.ok) {
            alert("🎉 ¡Alquiler y combinación de activos registrados con éxito!");
            document.getElementById('form-alquiler').reset();
            await cargarActivos();
        } else {
            const errorData = await response.text();
            alert("Error en el servidor: " + errorData);
        }
    } catch (error) {
        console.error("Error al procesar el alquiler:", error);
        alert("Hubo un error al intentar guardar el alquiler.");
    }
});