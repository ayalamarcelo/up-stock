/* 
  Este código configura un servidor Express que responda a las
  peticiones GET en la ruta raíz con el mensaje "Hola, mundo" 
*/

const express = require('express');
const app = express();
const PORT = 4040;

app.use(express.json()); // Middleware para parsear json


const users =  [
    { id: 1, name: 'Sol Miñones'},
    { id: 2, name: 'María Gonzalez Ayarza'}
];

app.get('/', (req, res) => {
    res.send('¡Hola, mundo!');
});

// endpoint 'GET' para obtener usuarios
app.get('/users', (req, res) => {
    res.json(users);
});

// endpoint para agregar un usuario con 'POST'
app.post('/users', (req, res) => {
    const newUser = req.body;
    newUser.id = users.length + 1;
    users.push(newUser);
    res.status(201).json(newUser);
});

// endpoint para eliminar un usuario por su id

app.delete('/users', (req, res) => {
    const userId = parseInt(req.params.id);
    const userIndex = users.findIndex(user => user.id === userId);

    if(userIndex >= 0) {
        users.splice(userIndex, 1);
        res.status(204).send();
    } else {
        res.status(404).json({ message: 'Usuario no encontrado'});
    }
});

app.listen(PORT, () => {
    console.log(`Servidor corriendo en http://localhost:${PORT}`);
});

