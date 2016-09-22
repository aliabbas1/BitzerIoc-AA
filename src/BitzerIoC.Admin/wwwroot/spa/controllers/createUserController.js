app.controller('createUserController', function ($scope, $rootScope, $location, BitzerIocAdminService) {

    /*------------  Functions ------------------*/
    /*Get roles*/
    var roles = BitzerIocAdminService.GetRoles();
    roles.then(function (result) {
        $scope.RolesList = result.data;
    });


    /// AlreadyExist method verify that email aleardy exist or not,
    /// If already exist then return true else return false
    /// PR, version 1.1.0

    $scope.AlreadyExist = function () {
        var UserEmail = $('#txtEmail').val().trim();
        var email = BitzerIocAdminService.ValidateEmail(UserEmail);
        email.then(function (result) {
            if (result.data != false) {
                $scope.Name = result.data.name;
                $scope.Phone = result.data.phoneNumber == '004500000000' ? '' : result.data.phoneNumber;
                $scope.Role = result.data.roles[0];
                $scope.IsEnable = result.data.isEnable;
                $('#txtName').attr('disabled', true);
                $('#txtPhone').attr('disabled', true);
                $('#chkIsEnable').attr('disabled', true);
                $('#ddlRoles').attr('disabled', true);
                $('#btnSaveUser').addClass('setup-btn');
                $scope.alreadyEmailMsgs = true;
            }
            else {
                $scope.alreadyEmailMsgs = false;
                $('#txtName').attr('disabled', false);
                $('#txtPhone').attr('disabled', false);
                //$('#RoleId').attr('disabled', false);
                $('#chkIsEnable').attr('disabled', false);
                $('#ddlRoles').attr('disabled', false);
                $scope.Role = "";
                $('#btnSaveUser').removeClass('setup-btn');
            }
        },
        function (errors) {
            $scope.error = errors;
        });
    }

    /// AddUser method is used for insertion of gateway
    /// parameter =UserEmail, UserName,UserPhone,RoleId,UserEnable,boundaryId
    /// after successfully insertion of user, redirect to user page. 
    /// version 2.0 
    /// ToDo: remove hard cord boundary=1


    $scope.AddUser = function () {
        var UserEmail = $scope.Email;
        var UserName = $scope.Name;
        var UserPhone = $scope.Phone;
        var RoleId = $('#ddlRoles').val();
        var UserEnable = $scope.IsEnable;

        RoleId = RoleId.replace('string:', '');

        if (UserPhone == "" || UserPhone == undefined)
            UserPhone = "004500000000"

        var Params = UserEmail + "/" + UserName + "/" + UserPhone + "/" + RoleId + "/" + UserEnable + "/" + 1;
        var response = BitzerIocAdminService.SaveUser(Params).then(function (result) {
            $location.path("/admin/users");
        });
    }
    /*------------ / Functions ------------------*/
});