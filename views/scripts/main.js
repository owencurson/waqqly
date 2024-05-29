document.addEventListener('DOMContentLoaded', () => {
    console.log('DOM fully loaded and parsed');
    const button = document.querySelector('button');
    button.addEventListener('click', () => {
        alert('Button clicked!');
    });
});
