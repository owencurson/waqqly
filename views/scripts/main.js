document.getElementById('pet-form').addEventListener('submit', async (event) => {
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
    const result = await response.json();
    alert(result.message);
});

document.getElementById('walker-form').addEventListener('submit', async (event) => {
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
    const result = await response.json();
    alert(result.message);
});
