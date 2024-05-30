document.addEventListener('DOMContentLoaded', function () {
    const petForm = document.getElementById('pet-form');
    const walkerForm = document.getElementById('walker-form');
    const petsList = document.getElementById('pets-list');
    const walkersList = document.getElementById('walkers-list');

    petForm.addEventListener('submit', async function (event) {
        event.preventDefault();
        const petName = document.getElementById('pet-name').value;
        const petBreed = document.getElementById('pet-breed').value;

        const response = await fetch('/api/registerPet', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ petName, petBreed })
        });

        if (response.ok) {
            alert('Pet registered successfully!');
            fetchPets();
        } else {
            alert('Failed to register pet.');
        }
    });

    walkerForm.addEventListener('submit', async function (event) {
        event.preventDefault();
        const walkerName = document.getElementById('walker-name').value;
        const walkerLocation = document.getElementById('walker-location').value;

        const response = await fetch('/api/registerWalker', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ walkerName, walkerLocation })
        });

        if (response.ok) {
            alert('Walker registered successfully!');
            fetchWalkers();
        } else {
            alert('Failed to register walker.');
        }
    });

    async function fetchPets() {
        const response = await fetch('/api/getPets');
        const pets = await response.json();

        petsList.innerHTML = '';
        pets.forEach(pet => {
            const li = document.createElement('li');
            li.textContent = `${pet.name} (${pet.breed})`;
            petsList.appendChild(li);
        });
    }

    async function fetchWalkers() {
        const response = await fetch('/api/getWalkers');
        const walkers = await response.json();

        walkersList.innerHTML = '';
        walkers.forEach(walker => {
            const li = document.createElement('li');
            li.textContent = `${walker.name} (${walker.location})`;
            walkersList.appendChild(li);
        });
    }

    // Initial fetch to populate lists
    fetchPets();
    fetchWalkers();
});
