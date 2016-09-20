/*
HTML 5 validation like required attribute take care of form in latest version
but in case of old browser this form prevent to send emoty data of form.
Used in Homecontroller @script section
*/
function ForgetPasswordValidateForm()
{
    var password = $("#Password").val();
    var confirmPassword = $("#ConfirmPassword").val();
    var retrunUrl = $("#ReturnUrl").val();

    if (password == '' || confirmPassword == '' || retrunUrl == '')
    {
        console.error("Unable to validate forget password form");
        return false;
    }
    else
    {
        return true;
    }
}

