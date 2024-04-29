
$(document).ready(function () {
    $('#togglePassword').click(function () {
        let passwordInput = $('input[name="Password"]');
        let passwordType = passwordInput.attr('type');
        let showText = $('#togglePassword').data('show-text');

        if (passwordType === 'password') {
            passwordInput.attr('type', 'text');
            $('#togglePassword i').removeClass('fa-eye').addClass('fa-eye-slash');
            $('#togglePassword').text(showText);
        } else {
            passwordInput.attr('type', 'password');
            $('#togglePassword i').removeClass('fa-eye-slash').addClass('fa-eye');
            $('#togglePassword').text(showText);
        }
    });
});