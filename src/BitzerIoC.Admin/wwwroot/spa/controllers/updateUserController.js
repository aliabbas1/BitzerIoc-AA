app.controller('updateUserController', function ($scope, $routeParams, $location, BitzerIocAdminService) {

    
    /* Function get the user on the basis of userId*/
    /* version 2.0*/

    var userId = $routeParams.userId;
    $scope.UserId = userId;
    GetUser(userId);
    GetRoles();

    /*------------  Functions ------------------*/
    /*Get roles*/
    function GetRoles() {
        var roles = BitzerIocAdminService.GetRoles();
        roles.then(function (result) {
            $scope.RolesList = result.data;
        });
    }

    /// Get User Profile information of particular user  
    /// Version 1.0
    /// ToDo: remove hard cord boundary=1

    function GetUser(userId) {
        var params = userId + "/" + 1;
        var user = BitzerIocAdminService.GetUserProfileById(params);
        user.then(function (result) {
            if (result.data != "" || result.data != null) {
                $scope.UserId = result.data.userId;
                $scope.Email = result.data.email;
                $scope.Name = result.data.name;
                $scope.RoleId = result.data.roleId[0];
                $scope.HiddenRoleId = result.data.roleId[0];
                $scope.Phone = result.data.phoneNumber == "004500000000" ? "" : result.data.phoneNumber;
                $scope.IsEnable = result.data.isEnable;
                $('#txtEmail').attr('disabled', true);
            }
        });
    }

    /// Update the  User, return true if successfully updated    
    /// Version 1.0
    /// ToDo: remove hard cord boundary=1
    $scope.UpdateUser = function () {
        //var UserId = $scope.UserId == "" ? null : $scope.UserId;
        var UserId = $scope.UserId;
        var UserEmail = $scope.Email;
        var UserName = $scope.Name;
        var UserPhone = $scope.Phone;

        var RoleId = $('#ddlRoles').val();
        var UserEnable = $scope.IsEnable;
        var OldRoleId = $('#txtHdnOldRoleId').text();

        RoleId = RoleId.replace('string:', '');

        if (UserPhone == "")
            UserPhone = "004500000000"
        var Params = UserId + "/" + UserEmail + "/" + UserName + "/" + UserPhone + "/" + RoleId + "/" + OldRoleId + "/" + UserEnable + "/" + 1;
        var response = BitzerIocAdminService.UpdateUser(Params).then(function (result) {
            $location.path("/admin/users");
        });
    }

    /*------------ / Functions ------------------*/
});