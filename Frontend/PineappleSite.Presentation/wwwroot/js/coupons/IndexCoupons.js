
document.getElementById("clearSearch").addEventListener("click", function () {
    document.getElementById("searchCode").value = "";
    window.location.href = "/Coupon/Index";
});

$(document).ready(function () {
    $('input[name="selectedCoupons"]').change(function () {
        var selectedCount = $('input[name="selectedCoupons"]:checked').length;
        if (selectedCount >= 2) {
            $('#deleteAllButton').show();
        } else {
            $('#deleteAllButton').hide();
        }
    });
});