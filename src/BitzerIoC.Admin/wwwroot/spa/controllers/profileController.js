
app.controller('profileController', function ($scope, $rootScope, $location, BitzerIocAdminService) {

    /*------------  Functions ------------------*/

    var Email = $('#hiddenEmail').val();

    /// Get Profile by email
    /// Version 1.0
    var profile = BitzerIocAdminService.GetProfileByEmail(Email);
    profile.then(function (result) {
        if (result.data != "" || result.data != null) {
            $scope.txtName = result.data.name;
            $scope.txtEmail = result.data.email;
            $scope.txtPassword = result.data.password;
            $scope.master = result.data.name;
        }
    },
        function (errors) {
            $scope.error = errors;
        });

    /// Function to Validate Password, if password is valid then enable new password and confirm password fileds
    /// if password is not valid it display error message.
    /// version 1.0
    $scope.ValidatePassword = function () {
        $scope.differentPassword = false;
        var CurrentPassword = $scope.txtCurrentPassword;
        $('.error-msg').removeClass('hidden');
        if (CurrentPassword != undefined) {
            var validUser = BitzerIocAdminService.ValidateCredential(Email, CurrentPassword);
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

    /// Function to update password
    /// version 1.0
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
            var SavePassword = BitzerIocAdminService.UpdatePassword(Email, NewPassword);
            SavePassword.then(function (result) {
                if (result.data == true) {
                    $('#modalChangePassword').modal('hide');
                }
            });
        }
    }


    /// Function to update name of user
    /// version 1.0
    $scope.UpdateName = function () {
        var Name = $scope.txtName;
        var SaveName = BitzerIocAdminService.UpdateName(Email, Name);
        SaveName.then(function (result) {
            if (result.data == true) {
                $('.btnProfile').prop("disabled", true);
                $scope.master = Name;
                $scope.successMsgs = true;
            }
        });
    }

    /// Function reset profile name of user
    /// version 1.0
    $scope.reset = function () {
        $scope.txtName = angular.copy($scope.master);
        $('.btnProfile').prop("disabled", true);
        $scope.successMsgs = false;
    };

    /// Function display success message and disable save button 
    /// version 1.0
    $scope.namechanged = function () {
        $('.btnProfile').prop("disabled", false);
        $scope.successMsgs = false;

        if ($scope.txtName == $scope.master)
            $('.btnProfile').prop("disabled", true);

        if ($scope.txtName == undefined)
            $('#btnSaveProfile').prop("disabled", true);
    }

    /*------------ / Functions ------------------*/
});