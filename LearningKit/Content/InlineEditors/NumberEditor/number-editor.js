(function () {
    // Registers the 'number-editor' inline property editor within the page builder scripts
    window.kentico.pageBuilder.registerInlineEditor("number-editor", {
        init: function (options) {
            var editor = options.editor;
            
            // Click action for the 'Plus' button
            editor.querySelector("#plus-btn").addEventListener("click", function () {
                // Creates a custom event that notifies the widget about a change in the value of a property
                var event = new CustomEvent("updateProperty", {
                    detail: {
                        value: options.propertyValue + 1,
                        name: options.propertyName
                    }
                });
                editor.dispatchEvent(event);
            });

            // Click action for the 'Minus' button
            editor.querySelector("#minus-btn").addEventListener("click", function () {
                var event = new CustomEvent("updateProperty", {
                    detail: {
                        value: options.propertyValue - 1,
                        name: options.propertyName
                    }
                });
                editor.dispatchEvent(event);
            });
        }
    });
})();