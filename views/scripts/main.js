document.getElementById('pet-form').addEventListener('submit', async (event) => {
    event.preventDefault();
    const petName = document.getElementById('pet-name').value;
    const petBreed = document.getElementById('pet-breed').value;

    const response = await fetch('https://waqqlyapp.azurewebsites.net/api/registerPet', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({ petName, petBreed })
    });
    const result = await response.json();
    alert(result.message);
    fetchPets(); // Refresh the pet list after registration
});

document.getElementById('walker-form').addEventListener('submit', async (event) => {
    event.preventDefault();
    const walkerName = document.getElementById('walker-name').value;
    const walkerLocation = document.getElementById('walker-location').value;

    const response = await fetch('https://waqqlyapp.azurewebsites.net/api/registerWalker', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({ walkerName, walkerLocation })
    });
    const result = await response.json();
    alert(result.message);
    fetchWalkers(); // Refresh the walker list after registration
});

async function fetchPets() {
    const response = await fetch('https://waqqlyapp.azurewebsites.net/api/getPets');
    const pets = await response.json();
    const petsList = document.getElementById('pets-list');
    petsList.innerHTML = ''; // Clear existing list
    pets.forEach(pet => {
        const petItem = document.createElement('div');
        petItem.textContent = `Name: ${pet.name}, Breed: ${pet.breed}`;
        petsList.appendChild(petItem);
    });
}

async function fetchWalkers() {
    const response = await fetch('https://waqqlyapp.azurewebsites.net/api/getWalkers');
    const walkers = await response.json();
    const walkersList = document.getElementById('walkers-list');
    walkersList.innerHTML = ''; // Clear existing list
    walkers.forEach(walker => {
        const walkerItem = document.createElement('div');
        walkerItem.textContent = `Name: ${walker.name}, Location: ${walker.location}`;
        walkersList.appendChild(walkerItem);
    });
}

document.addEventListener('DOMContentLoaded', () => {
    fetchPets();
    fetchWalkers();
});
