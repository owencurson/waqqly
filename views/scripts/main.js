document.addEventListener("DOMContentLoaded", function() {
    const petForm = document.getElementById('pet-form');
    const walkerForm = document.getElementById('walker-form');
    const petsList = document.getElementById('pets-list');
    const walkersList = document.getElementById('walkers-list');

    petForm.addEventListener('submit', async function(event) {
        event.preventDefault();
        const petName = document.getElementById('pet-name').value;
        const petBreed = document.getElementById('pet-breed').value;

        if (!petName || !petBreed) {
            alert('Please fill in both pet name and pet breed.');
            return;
        }

        const submitButton = petForm.querySelector('button[type="submit"]');
        submitButton.disabled = true;

        try {
            const response = await fetch('/api/registerPet', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({ petName, petBreed })
            });

            if (response.ok) {
                alert('Pet registered successfully!');
                loadPets();
            } else {
                const error = await response.json();
                alert(`Failed to register pet: ${error.message || response.statusText}`);
            }
        } catch (error) {
            console.error('Error registering pet:', error);
            alert('Failed to register pet.');
        } finally {
            submitButton.disabled = false;
        }
    });

    walkerForm.addEventListener('submit', async function(event) {
        event.preventDefault();
        const walkerName = document.getElementById('walker-name').value;
        const walkerLocation = document.getElementById('walker-location').value;

        if (!walkerName || !walkerLocation) {
            alert('Please fill in both walker name and walker location.');
            return;
        }

        const submitButton = walkerForm.querySelector('button[type="submit"]');
        submitButton.disabled = true;

        try {
            const response = await fetch('/api/registerWalker', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({ walkerName, walkerLocation })
            });

            if (response.ok) {
                alert('Walker registered successfully!');
                loadWalkers();
            } else {
                const error = await response.json();
                alert(`Failed to register walker: ${error.message || response.statusText}`);
            }
        } catch (error) {
            console.error('Error registering walker:', error);
            alert('Failed to register walker.');
        } finally {
            submitButton.disabled = false;
        }
    });

    async function loadPets() {
        try {
            const response = await fetch('/api/getPets');
            const pets = await response.json();
            petsList.innerHTML = '';
            pets.forEach(pet => {
                const li = document.createElement('li');
                li.textContent = `${pet.name} (${pet.breed})`;
                petsList.appendChild(li);
            });
        } catch (error) {
            console.error('Error loading pets:', error);
            alert('Failed to load pets.');
        }
    }

    async function loadWalkers() {
        try {
            const response = await fetch('/api/getWalkers');
            const walkers = await response.json();
            walkersList.innerHTML = '';
            walkers.forEach(walker => {
                const li = document.createElement('li');
                li.textContent = `${walker.name} (${walker.location})`;
                walkersList.appendChild(li);
            });
        } catch (error) {
            console.error('Error loading walkers:', error);
            alert('Failed to load walkers.');
        }
    }

    loadPets();
    loadWalkers();
});
