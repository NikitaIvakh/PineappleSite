$(document).ready(function () {
        $('input[name="selectedProducts"]').change(function () {
            var selectedCount = $('input[name="selectedProducts"]:checked').length;
            if (selectedCount >= 2) {
                $('#deleteAllButton').show();
            } else {
                $('#deleteAllButton').hide();
            }
        });
    });