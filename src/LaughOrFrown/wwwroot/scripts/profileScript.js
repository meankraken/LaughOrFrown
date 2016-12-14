$(document).ready(function () {
    $('#logoutForm a').on('click', function () {
        $('#logoutForm').submit();
    });
    
    $('#cancelBtn').on('click', function () {
        window.location = "/profile";
    }) 
});