/*This controller specifies the url text and url link*/
app.controller('nevigationController', function ($scope, $location, $window, configurations)
{

    $scope.UsersLinkText = "Users";
    $scope.UsersLinkUrl = "/admin/users";

    $scope.GatewayLinkText = "Gateway";
    $scope.GatewayLinkUrl = "/admin/gateways";

    $scope.ProfileLinkText = "My account";
    $scope.ProfileLinkUrl = "/admin/profile";

    $scope.LogoutLinkText = "Logout";
    $scope.LogoutLinkUrl =  "/Account/Logout";

});

///*Url settings*/
//var baseBitzerIoCAdminUrl = null;
//if (configurations.environment == 'Development') {
//    baseBitzerIoCAdminUrl = configurations.baseBitzerIoCAdminUrlDevelopment;
//}
//if (configurations.environment == 'Production') {
//    baseBitzerIoCAdminUrl = configurations.baseBitzerIoCAdminUrlProduction;
//}
///* /Url settings*/