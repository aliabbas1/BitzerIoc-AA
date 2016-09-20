/*Function shows modal popup*/
function forgotModal()
{
    $(".email-error").addClass("hidden");
    $('#modalTitle').html("Forgot password?");
    $('#forgotModal').modal();
    $("#forgotemail").val('');
}

/*
  Function make request to webapi and pass username as parameter
  Web api send email to particular user along with forgot password link 
*/
function ForgotPassword(baseUrl,returnUrl)
{
    var username = $("#forgotemail").val();
    if (username != "") {

        $.ajax({
            url: baseUrl+'/identity',
            type: 'POST',
            dataType: 'json',
            crossDomain: true,
            data: { username: username, returnUrl: returnUrl },
            //data:"",
            success: function (data)
            {
                if (data == true)
                {
                    $('#forgotModal').modal('hide');
                    $(".email-error").addClass("hidden");
                    $('#EmailSentModal').modal();
                    setTimeout(function () { $('#EmailSentModal').modal('hide'); }, 2500);
                }
                else
                {
                    $(".email-error").removeClass("hidden");
                    $(".email-error").html("Email you entered does not belong to any account!");
                }
            },
            compelte: function ()
            {
                console.info("Email sent to user :: " + username);
            },
            error: function ()
            {
                alert('Error: Invalid url provided or unable to send email');
            },

        });
    }
}

