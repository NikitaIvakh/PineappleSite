$(document).ready(function () {
    $('#togglePassword').click(function () {
        var passwordInput = $('input[name="Password"]');
        var passwordType = passwordInput.attr('type');

        if (passwordType === 'password') {
            passwordInput.attr('type', 'text');
            $('#togglePassword i').removeClass('fa-eye').addClass('fa-eye-slash');
            $('#togglePassword').text('Скрыть');
        } else {
            passwordInput.attr('type', 'password');
            $('#togglePassword i').removeClass('fa-eye-slash').addClass('fa-eye');
            $('#togglePassword').text('Показать');
        }
    });
});