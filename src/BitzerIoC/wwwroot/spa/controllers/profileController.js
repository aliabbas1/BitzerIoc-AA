
app.controller('profileController', function ($scope, BitzerIocUserService) {
    var Email = $('#hiddenEmail').val();
    var profile = BitzerIocUserService.GetProfileByEmail(Email);

    profile.then(function (result) {
        if (result.data != "" || result.data != null) {
            $scope.txtName = result.data.name;
            $scope.txtEmail = result.data.email;
            $scope.txtPassword = result.data.password;
            $scope.master = result.data.name;
        }
    });


    $scope.ValidatePassword = function () {
        $scope.differentPassword = false;
        var CurrentPassword = $scope.txtCurrentPassword;
        $('.error-msg').removeClass('hidden');
        if (CurrentPassword != undefined) {
            var validUser = BitzerIocUserService.ValidateCredential(Email, CurrentPassword);
            validUser.then(function (result) {
                if (result.data == true) {
                    $('#txtNewPassword').removeAttr("disabled");
                    $('#txtConfirmPassword').removeAttr("disabled");
                    $('#txtNewPassword').focus();
                    $scope.WrongPassword = false;
                }
                else {
                    $scope.WrongPassword = true;
                    $('#txtNewPassword').prop("disabled", true);
                    $('#txtConfirmPassword').prop("disabled", true);
                    $('#btnChangePassword').prop("disabled", true);
                }
            });
        }
    }

    /*Function to update password*/
    $scope.UpdatePassword = function () {
        var NewPassword = $scope.txtNewPassword;
        if ($scope.txtCurrentPassword == $scope.txtNewPassword) {
            $scope.differentPassword = true;
            return;
        }
        else {
            $scope.differentPassword = false;
        }

        if (NewPassword != "") {
            var SavePassword = BitzerIocUserService.UpdatePassword(Email, NewPassword);
            SavePassword.then(function (result) {
                if (result.data == true) {
                    $('#modalChangePassword').modal('hide');
                }
                else {
                    console.error("unable to update user password");
                }
            });
        }
    }
    /* /Function to update password*/

    $scope.UpdateName = function () {
        var Name = $scope.txtName;
        var SaveName = BitzerIocUserService.UpdateName(Email, Name);
        SaveName.then(function (result) {
            if (result.data == true) {
                $('.btnProfile').prop("disabled", true);
                $scope.master = Name;
                $scope.successMsgs = true;
            }
        });
    }

    $scope.reset = function () {
        $scope.txtName = angular.copy($scope.master);
        $('.btnProfile').prop("disabled", true);
        $scope.successMsgs = false;
    };

    $scope.namechanged = function () {
        $('.btnProfile').prop("disabled", false);
        $scope.successMsgs = false;

        if ($scope.txtName == $scope.master)
            $('.btnProfile').prop("disabled", true);

        if ($scope.txtName == undefined)
            $('#btnSaveProfile').prop("disabled", true);
    }
});