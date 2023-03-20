// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
// Get the slider and output element
var slider = document.getElementById("Year");
var output = document.getElementById("year");

// Update the output when the slider is moved
slider.oninput = function() {
  output.innerHTML = this.value;
}