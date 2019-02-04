var lang = new Lang();

lang.dynamic('en', 'js/langpack/en.json');

lang.init({
    defaultLang: 'pt'
});

window.onload = () => {

    let form = document.getElementById("form");

    form && form.addEventListener('submit', (event) => {
        if (form.checkValidity() === false) {
            event.preventDefault();
            event.stopPropagation();
        }
        form.classList.add("was-validated");
    });

};
