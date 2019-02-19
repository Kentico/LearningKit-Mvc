(function () {
    'use strict';
    
    // Fills address form fields with the selected address whenever 
    // an address is selected from the address selector drop-down element
    $('.js-address-selector-div').change(function () {
        var $selectorDiv = $(this),
            $addressDiv = $selectorDiv.parent(),
            $selector = $selectorDiv.find('.js-address-selector'),
            url = $selectorDiv.data('statelistaction'),
            postData = {
                addressId: $selector.val()
            };

        // If the new address option is selected, clears all address form fields
        if (!postData.addressId) {
            eraseFields($addressDiv);
            return;
        }

        // Sends a POST request to the 'CustomerAddress' endpoint of the 'CheckoutController' 
        $.post(url, postData, function (data) {
            fillFields($addressDiv, data);
        });
    });

    function fillFields($addressDiv, data) {
        fillBasicFields($addressDiv, data);
        fillCountryStateFields($addressDiv, data);
    }

    // Fills the 'Address line 1' and '2', 'City', and 'Postal code'
    // address form fields from the selected address
    function fillBasicFields($addressDiv, data) {
        var basicFields = $addressDiv.data('fields'),
            addressType = $addressDiv.data('addresstype');

        $.each(basicFields, function (i, val) {
            var fieldId = '#' + addressType + '_' + val,
                fieldVal = data[val];

            $(fieldId).val(fieldVal);
        });
    }

    // Fills the Country and State drop-down selectors from selected address data
    function fillCountryStateFields($addressDiv, data) {
        var $countryStateSelector = $addressDiv.find('.js-country-state-selector'),
            countryField = $countryStateSelector.data('countryfield'),
            stateField = $countryStateSelector.data('statefield'),
            $countrySelector = $countryStateSelector.find('.js-country-selector');

        // Sets id of the country's state for the 'countryStateSelector' script
        $countryStateSelector.data('stateselectedid', data[stateField]);

        // Raises the change event on the country selector drop-down element, 
        // This invokes the 'coutryStateSelector' script that shows the
        // states drop-down selector with the selected state
        $countrySelector.val(data[countryField]).change();
    }

    // Clears all address form fields
    function eraseFields($addressDiv) {
        var data = {};
        fillFields($addressDiv, data);
    }
}());
