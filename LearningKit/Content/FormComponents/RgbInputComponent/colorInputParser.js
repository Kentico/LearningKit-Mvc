// Modifies the partial intensity values of the RGB input fields whenever a different color is selected
var parseColorSelector = function (target) {
    document.getElementById(target.getAttribute('data-red-id')).value = target.value.substring(1, 3);
    document.getElementById(target.getAttribute('data-green-id')).value = target.value.substring(3, 5);
    document.getElementById(target.getAttribute('data-blue-id')).value = target.value.substring(5, 7);
};