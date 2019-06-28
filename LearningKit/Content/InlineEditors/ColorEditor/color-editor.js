(function () {
    // Registers the 'number-editor' inline property editor within the page builder scripts
    window.kentico.pageBuilder.registerInlineEditor("color-editor", {
        //DocSection:InitFunction
        init: function (options) {
            var editor = options.editor;

            // Adds a click action for the 'Modal dialog' button
            editor.querySelector("#modal-btn").addEventListener("click", function () {
                // Opens the modal dialog window
                kentico.modalDialog.open({
                    // Gets the modal dialog URL from the 'data-url' attribute of the inline editor wrapping element
                    url: editor.getAttribute("data-url"),
                    applyCallback: function (dialogWindow) {
                        // Creates and dispatches an event that notifies the widget about changes of properties
                        var event = new CustomEvent("updateProperty", {
                            detail: {
                                // Retrieves the color value from radio buttons within the modal dialog window
                                value: dialogWindow.document.querySelector('input[name="color"]:checked').value,
                                name: options.propertyName
                            }
                        });
                        editor.dispatchEvent(event);
                    },
                    applyButtonText: "Confirm color selection",
                    title: "Select a color",
                    // Passes the current color value to the modal dialog data
                    data: { value: options.propertyValue }
                });
            });
        }
        //EndDocSection:InitFunction
    });
})();