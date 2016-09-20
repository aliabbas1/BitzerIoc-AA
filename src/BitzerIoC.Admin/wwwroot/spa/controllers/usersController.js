app.controller('usersController', function ($scope, $filter, $rootScope, $location, BitzerIocAdminService)
{

        $rootScope.UserId = "";
        $rootScope.Email = "";
        $rootScope.Name = "";
        $rootScope.Role = "";
        $rootScope.RoleId = "";
        $rootScope.Phone = "";
        $rootScope.IsEnable = "";
        $rootScope.HiddenRoleId = "";
        var user = "";

        /*Bind User Gridview*/
        BindUserGrid();


     /*Edit user button event*/
        $scope.EditUser = function (Email)
        {
            $location.path("/admin/createuser");
            var profile = BitzerIocAdminService.GetProfileByEmail(Email);

            profile.then(function (result)
            {
                if (result.data != "" || result.data != null)
                {
                    $rootScope.UserId = result.data.userId;
                    $rootScope.Email = result.data.email;
                    $rootScope.Name = result.data.name;
                    $rootScope.RoleId = result.data.roleId[0];
                    $rootScope.HiddenRoleId = result.data.roleId[0];
                    $rootScope.Phone = result.data.phoneNumber == "004500000000" ? "" : result.data.phoneNumber;
                    $rootScope.IsEnable = result.data.isEnable;
                    $('#Email').attr('disabled', true);
                }
            });

        }

    /* /Edit user button event*/



    $scope.DeleteUserModal = function (Email) 
    {
        $('#modalTitle').html("Delete user?");
        $("#HiddenId").val(Email);
        $('#DeleteUserModal').modal();
    }

    $scope.DeleteUser = function ()
    {
        var UserId = $("#HiddenId").val();
        var boundary = BitzerIocAdminService.GetUserBoundaries(UserId);
        var deleteStatus = "";

        boundary.then(function (result)
        {
            if (result.data.length == 1)
            {
                deleteStatus = BitzerIocAdminService.DeleteUser(UserId);
                deleteStatus.then(function (result)
                {
                    if (result.data == true)
                    {
                        $('#DeleteUserModal').modal('hide');
                        BindUserGrid();
                    }
                        
                });

            }
            if (result.data.length > 1)
            {
                deleteStatus = BitzerIocAdminService.DeleteUserBoundary(UserId);
                deleteStatus.then(function (result)
                {
                    if (result.data == true)
                    {
                        $('#DeleteUserModal').modal('hide');
                        BindUserGrid();
                    }
                    
                });
            }
        });
    }


    /*------------  Functions ------------------*/

    /*To Do: Should be ready fro UI-Grid*/
    /* Function Bid the Users Grid*/
    function BindUserGrid()
    {
        /*Get the users*/
        var users = BitzerIocAdminService.GetUsers();
        users.then(function (result)
        {
            $scope.userGridOpts.data = result.data;
            $scope.searchUser = result.data;
        });
    }

    /*------------ / Functions ------------------*/

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
                    '<a class="glyphicon glyphicon-pencil edit-btn" ng-click="grid.appScope.EditUser(row.entity.email)"></a>'
            },
            {
                field: 'Delete', displayName: 'Delete', enableColumnMenu: false, width: '8%', cellTemplate:
                       '<a ng-hide="u.sp" class="glyphicon glyphicon-remove edit-btn" data-target="#DeleteUserModal" ng-click="grid.appScope.DeleteUserModal(row.entity.userId)"></a>'
            }
        ]
    }
    /*  end binding of users ui-grid*/


    /*------------  Functions ------------------*/
    /* Search user from Grid*/
    /* this function is temporary and will be remove when we enable filter in $scope.userGridOpts. */
    /*Author = Ali Abbas, version 1.0*/
    $scope.searchUsers = function () {
        $scope.userGridOpts.data = $filter('filter')($scope.searchUser, $scope.filterUser, undefined);
    };
    /*------------ / Functions ------------------*/
});





