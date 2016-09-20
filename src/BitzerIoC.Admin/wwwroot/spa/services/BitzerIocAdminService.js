app.service('BitzerIocAdminService', function ($http, $route, configurations)
{
    /// Architect, configurations created and managed by khurram shehzad.
    /// Service methods created by Ali Abbas.
    /// These Service method call different controllers(like identity,UserOperations,Gateway) of WebApi.
    /// Version 1.0
    /// To do: Remove hardcoded boundaryId 

    var baseApiUrl = null;
    if (configurations.environment == 'Development') {
        baseApiUrl = configurations.baseApiUrlDevelopment;
    }
    if (configurations.environment == 'Production') {
        baseApiUrl = configurations.baseApiUrlProduction;
    }

    this.GetUsers = function () {
        return $http.get(baseApiUrl+'/identity/GetUsersWithRoles?boundaryId=1');
    };

    this.GetProfileByEmail = function (Email) {

        return $http.get(baseApiUrl+'/identity/GetUserProfileByUsername?email=' + Email + '&boundaryId=1');

    };

    this.GetRoles = function () {
        return $http.get(baseApiUrl+'/identity');
    };

    this.ValidateEmail = function (UserEmail) {
        return $http.get(baseApiUrl + '/identity/AlreadyExistEmail?UserEmail=' + UserEmail + '&boundaryId=1')
    };

    //ToDo: Rename to SaveUser
    this.SetupUser = function (Params)
    {
        return $http.post(baseApiUrl + '/UserOperations/' + Params);
    }

    this.GetGateways = function ()
    {
       return $http.get(baseApiUrl+'/Gateway/1');
    }

    this.ValidateCredential = function (UserName, Password) {
        return $http.get(baseApiUrl+'/identity/ValidateCredentials?username=' + encodeURIComponent(UserName) + '&password=' + encodeURIComponent(Password));
    }

    this.UpdatePassword = function (UserName, Password) {
        return $http.post(baseApiUrl + '/identity/UpdatePassword?username=' + UserName + '&password=' + Password);
    }

    this.UpdateName = function (UserName, Name) {
        return $http.post(baseApiUrl+'/identity/UpdateName?username=' + UserName + '&name=' + Name);
    }

    this.GetUserBoundaries = function (UserId) {
        return $http.get(baseApiUrl+'/UserOperations/GetUserBoundaries/' + UserId);
    }

    this.DeleteUserBoundary = function (UserId) {
        return $http.delete(baseApiUrl+'/UserOperations/' + UserId + '/1');
    }

    this.DeleteUser = function (UserId) {
        return $http.delete(baseApiUrl+'/UserOperations/' + UserId);
    }

    this.ValidateGateway = function (GatewayMac) {
        return $http.get(baseApiUrl + '/Gateway/' + GatewayMac);
    }
    this.SetupGateway = function (params) {
        return $http.post(baseApiUrl + '/Gateway/' + params);
    }

    this.GetGatewayById = function (params) {
        return $http.get(baseApiUrl + '/Gateway/'+params);
    }
    this.UpdateGateway = function (params) {
        return $http.post(baseApiUrl + '/Gateway/' + params);
    }
    this.DeleteGateway = function (params) {
        return $http.delete(baseApiUrl + '/Gateway/' + params);
    }

    console.warn('Application:' + configurations.appName);
    console.log('Base API Url is:', baseApiUrl);
    console.log('Environment is:', configurations.environment);
});








