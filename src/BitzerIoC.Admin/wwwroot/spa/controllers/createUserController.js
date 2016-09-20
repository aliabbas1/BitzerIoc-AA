app.controller('createUserController', function ($scope, $rootScope, $location, BitzerIocAdminService) {

       /*Get roles*/
       var roles = BitzerIocAdminService.GetRoles();
       roles.then(function (result)
       {
           $scope.RolesList = result.data;
       });
   

    $scope.AlreadyExist = function () {
        var UserEmail = $('#Email').val().trim();
        var email = BitzerIocAdminService.ValidateEmail(UserEmail);
        email.then(function (result)
        {
            if (result.data != false)
            {
                $scope.Name = result.data.name;
                $scope.Phone = result.data.phoneNumber == '004500000000' ? '' : result.data.phoneNumber;
                $scope.Role = result.data.roles[0];
                $scope.IsEnable = result.data.isEnable;
                $('#Name').attr('disabled', true);
                $('#Phone').attr('disabled', true);
                $('#IsEnable').attr('disabled', true);
                $('#Roles').attr('disabled', true);
                $('#setup-user-btn').addClass('setup-btn');
                $scope.alreadyEmailMsgs = true;
            }
            else
            {
                $scope.alreadyEmailMsgs = false;
                $('#Name').attr('disabled', false);
                $('#Phone').attr('disabled', false);
                $('#RoleId').attr('disabled', false);
                $('#IsEnable').attr('disabled', false);
                $('#Roles').attr('disabled', false);
                $scope.Role = "";
                $('#setup-user-btn').removeClass('setup-btn');
            }
        },
        function (errors)
        {
            $scope.error = errors;
        });
    }

    $scope.AddUpdateUser = function ()
    {
        var UserId = $scope.UserId == "" ? null : $scope.UserId;
        var UserEmail = $scope.Email;
        var UserName  = $scope.Name;
        var UserPhone = $scope.Phone;

        var RoleId = $('#Roles').val();
        var UserEnable = $scope.IsEnable;
        var OldRoleId = $('#OldRoleId').text() == "" ? null : $('#OldRoleId').text();

        RoleId = RoleId.replace('string:', '');

        if (UserPhone == "")
            UserPhone = "004500000000"

        var Params = UserId + "/" + UserEmail + "/" + UserName + "/" + UserPhone + "/" + RoleId + "/" + OldRoleId + "/" + UserEnable;
        var response = BitzerIocAdminService.SetupUser(Params).then(function (result)
        {
            $location.path("/admin/users");
        });
      
    }

});