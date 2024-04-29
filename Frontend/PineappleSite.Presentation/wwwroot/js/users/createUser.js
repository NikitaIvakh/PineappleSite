
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

$(document).ready(function () {
    $('#togglePasswordConfirm').click(function () {
        let passwordInput = $('input[name="PasswordConfirm"]');
        let passwordType = passwordInput.attr('type');
        let showText = $('#togglePasswordConfirm').data('show-text');

        if (passwordType === 'password') {
            passwordInput.attr('type', 'text');
            $('#togglePasswordConfirm i').removeClass('fa-eye').addClass('fa-eye-slash');
            $('#togglePasswordConfirm').text(showText);
        } else {
            passwordInput.attr('type', 'password');
            $('#togglePasswordConfirm i').removeClass('fa-eye-slash').addClass('fa-eye');
            $('#togglePasswordConfirm').text(showText);
        }
    });
})