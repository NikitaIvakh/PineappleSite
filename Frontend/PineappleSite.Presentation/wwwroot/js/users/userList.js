
document.getElementById("clearSearch").addEventListener("click", function () {
    document.getElementById("searchUser").value = "";
    window.location.href = "/User/Index";
});

$(document).ready(function () {
    $('input[name="selectedUserIds"]').change(function () {
        var selectedCount = $('input[name="selectedUserIds"]:checked').length;
        if (selectedCount >= 2) {
            $('#deleteAllButton').show();
        } else {
            $('#deleteAllButton').hide();
        }
    });
});