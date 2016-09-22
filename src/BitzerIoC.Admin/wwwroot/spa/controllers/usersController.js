app.controller('usersController', function ($scope, $filter, $location, BitzerIocAdminService) {

    /*Bind User Gridview*/
    BindUserGrid();

    /*------------  Functions ------------------*/

    /// funtion redirect to upateuser page
    /// version 1.0 
    $scope.EditUser = function (userId) {
        $location.path("/admin/updateuser/" + userId);
    }

    /// funtion open delete user popup
    /// version 1.0 
    $scope.DeleteUserModal = function (Email) {
        $('#modalTitle').html("Delete user?");
        $("#txtHdnUserId").val(Email);
        $('#DeleteUserModal').modal();
    }

    /// funtion delete the user record if it has only one boundary access.
    /// if user have multiple boundaries then delete user in that specific boundary. 
    /// version 1.0 
    $scope.DeleteUser = function () {
        var UserId = $("#txtHdnUserId").val();
        var boundary = BitzerIocAdminService.GetUserBoundaries(UserId);
        var deleteStatus = "";

        boundary.then(function (result) {
            if (result.data.length == 1) {
                deleteStatus = BitzerIocAdminService.DeleteUser(UserId);
                deleteStatus.then(function (result) {
                    if (result.data == true) {
                        $('#DeleteUserModal').modal('hide');
                        BindUserGrid();
                    }
                });
            }
            if (result.data.length > 1) {
                deleteStatus = BitzerIocAdminService.DeleteUserBoundary(UserId);
                deleteStatus.then(function (result) {
                    if (result.data == true) {
                        $('#DeleteUserModal').modal('hide');
                        BindUserGrid();
                    }
                });
            }
        });
    }

    /* Function Bid the Users Grid*/
    function BindUserGrid() {
        /*Get the users*/
        var users = BitzerIocAdminService.GetUsers();
        users.then(function (result) {
            $scope.userGridOpts.data = result.data;
            $scope.searchUser = result.data;
        });
    }

    /* bind user ui-Grid*/
    /*Angular-Ui-Grid version 3.1.1*/
    /*Author = Ali Abbas, version 1.0*/
    $scope.userGridOpts = {
        enableHorizontalScrollbar: 0,
        enableVerticalScrollbar: 0,
        columnDefs: [
            { field: 'name', displayName: 'Name' },
            { field: 'email', displayName: 'Email' },
            {
                field: 'roles[0]', displayName: 'Profile'
            },
            {
                field: 'isEnable', displayName: 'Enabled', enableColumnMenu: false, cellTemplate:
                       '<div ng-if="row.entity.isEnable == true"><img src="/images/GreenDotIcon.svg"></div>' +
                       '<div ng-if="row.entity.isEnable == false"><img src="/images/GrayDotIcon.svg"></div>'
            },
            {
                field: 'Edit', displayName: 'Edit', enableColumnMenu: false, width: '8%', cellTemplate:
                    '<a class="glyphicon glyphicon-pencil edit-btn" ng-click="grid.appScope.EditUser(row.entity.userId)"></a>'
            },
            {
                field: 'Delete', displayName: 'Delete', enableColumnMenu: false, width: '8%', cellTemplate:
                       '<a ng-hide="u.sp" class="glyphicon glyphicon-remove edit-btn" data-target="#DeleteUserModal" ng-click="grid.appScope.DeleteUserModal(row.entity.userId)"></a>'
            }
        ]
    }
    /*  end binding of users ui-grid*/


    /* Search user from Grid*/
    /* this function is temporary and will be remove when we enable filter in $scope.userGridOpts. */
    /*Author = Ali Abbas, version 1.0*/
    $scope.searchUsers = function () {
        $scope.userGridOpts.data = $filter('filter')($scope.searchUser, $scope.filterUser, undefined);
    };

    /*------------ / Functions ------------------*/
});





