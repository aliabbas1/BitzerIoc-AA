function modalChangePassword() {
    $('#modalTitle').html("Change password");
    $('.error-msg').addClass('hidden');

    $('#txtNewPassword').prop("disabled", true);
    $('#txtConfirmPassword').prop("disabled", true);
    $('#btnChangePassword').prop("disabled", true);

    $('#txtCurrentPassword').val('');
    $('#txtNewPassword').val('');
    $('#txtConfirmPassword').val('');

    $('#modalChangePassword').modal();
}
