
document.getElementById("clearSearch").addEventListener("click", function () {
    document.getElementById("searchProduct").value = "";
    window.location.href = "/Home/GetProducts";
});