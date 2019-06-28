function openColorModalDialog(dialogData) {
    // Gets the form component's input element
    var inputElement = window.document.querySelector('#' + dialogData.colorInputId);

    // Opens the modal dialog
    kentico.modalDialog.open({
        url: dialogData.modalDialogUrl,
        applyCallback: function(dialogWindow) {
            // Retrieves the selected value from the modal dialog
            var selectedValue = dialogWindow.document.querySelector('input[name="color"]:checked').value;
            // Updates the value of the input element in the property configuration dialog
            inputElement.value = selectedValue;
        },
        applyButtonText: 'Confirm color selection',
        title: 'Select a color',
        // Passes the current color to the modal dialog data
        data: { value: inputElement.value }
    });
}