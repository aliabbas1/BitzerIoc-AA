app.service('BitzerIocUserService', function ($http, $route, configurations)
{
    /// Architect, configurations created and managed by khurram shehzad.
    /// Service methods created by Ali Abbas.
    /// These Service method call different controllers(like identity controller) of WebApi.
    /// Version 1.0
    /// To do: Remove hardcoded boundaryId 

    var baseApiUrl = null;
    if (configurations.environment == 'Development')
    {
        baseApiUrl = configurations.baseApiUrlDevelopment;
    }
    if (configurations.environment == 'Production')
    {
        baseApiUrl = configurations.baseApiUrlProduction;
    }

    this.GetProfileByEmail = function (Email)
    {
         return $http.get(baseApiUrl+'/identity/GetUserProfileByUsername?email=' + Email + '&boundaryId=1');
    };
    
    this.ValidateCredential = function (UserName, Password)
    {
        return $http.get(baseApiUrl+'/identity/ValidateCredentials?username=' + encodeURIComponent(UserName) + '&password=' + encodeURIComponent(Password));
    }
    
    this.UpdatePassword = function (UserName, Password)
    {
        return $http.post(baseApiUrl + '/identity/UpdatePassword?username=' + UserName + '&password=' + Password);
    }

    this.UpdateName = function (UserName, Name)
    {
        return $http.post(baseApiUrl + '/identity/UpdateName?username=' + UserName + '&name=' + Name);
    }

    console.warn('Application:' + configurations.appName);
    console.log('Base API Url is:', baseApiUrl);
    console.log('Environment is:', configurations.environment);
});








