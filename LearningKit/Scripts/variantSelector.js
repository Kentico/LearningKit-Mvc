(function () {
    'use strict';

    // Initializes contextual variables
    var url = $('.cart-item-selector').data('variant-action'),
        stockMessage = $("#stockMessage"),
        totalPrice = $('#totalPrice'),
        selectedSKUID = $('#selectedVariantID');

    // Updates the displayed product details when a different variant
    // is selected from the variants drop-down selector
    $('.js-variant-selector').change(function () {
        var id = $(this).val();
        updateVariantSelection(id);
    });

    // Updates the product information from data retrieved for the selected variant
    function updateVariantSelection(variantId) {
        $.post(url, { variantID: variantId }, function (data) {
            stockMessage.text(data.stockMessage);
            totalPrice.text(data.totalPrice);
            selectedSKUID.val(variantId);
        });
    }

}());