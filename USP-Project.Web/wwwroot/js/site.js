// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
// Get the slider and output element
const slider = document.getElementById("YearOfProduction");
const output = document.getElementById("year");

// Update the output when the slider is moved
slider.oninput = function () {
    output.innerHTML = this.value;
}

const brandSelect = document.getElementById("Brand");
console.log(brandSelect)

brandSelect.addEventListener("change", async function (e) {
    console.log(this, e.target.value)
    const brandId = e.target.value;
    
    const response = await fetch(`/Cars/Models?brandId=${brandId}`);
    const models = await response.json();
    
    // Dynamically change Model Select options:
    const modelSelectElement = document.getElementById("Model");
    
    // Remove all old option elements:
    while (modelSelectElement.children.length !== 1) {
        modelSelectElement.removeChild(modelSelectElement.lastChild);
    }

    // And replace them with the newly fetched ones:
    models.forEach((model, i) => {
        const optionElement = document.createElement("option");
        optionElement.value = model.id;
        optionElement.text = model.name;
        
        modelSelectElement.appendChild(optionElement);
    })
})