/*This controller specifies the url text and url link*/
app.controller('nevigationController', function ($scope, $location, $window, configurations)
{
    $scope.ProfileLinkText = "My account";
    $scope.ProfileLinkUrl = "/main/profile";
    
    $scope.LogoutLinkText = "Logout";
    $scope.LogoutLinkUrl =  "/Account/Logout";

});


///*Url settings*/
//var baseBitzerIoCUrl = null;
//if (configurations.environment == 'Development') {
//    baseBitzerIoCUrl = configurations.baseBitzerIoCUrlDevelopment;
//}
//if (configurations.environment == 'Production') {
//    baseBitzerIoCUrl = configurations.baseBitzerIoCUrlProduction;
//}
///* /Url settings*/