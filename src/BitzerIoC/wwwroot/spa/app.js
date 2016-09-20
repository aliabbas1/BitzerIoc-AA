//Application Module

var app = angular.module("BitzerUserModule", ["ngRoute"]);

app.run(['$location', '$rootScope', function ($location, $rootScope)
{
    $rootScope.$on('$routeChangeSuccess', function (event, current, previous) {
        if (current.hasOwnProperty('$$route')) {
            $rootScope.title = current.$$route.title;
        }
    });
}]);

/*Constants use throughout the application*/
app.constant('configurations',
{
    appName: 'BitzerIoC.Web',
    appVersion: 1.0,
    environment: 'Development',
    baseApiUrlDevelopment: 'http://localhost:5003/api',
    baseApiUrlProduction:  'http://local.api.bitzerioc.com/api',

    // environment: 'Production','Development'
});


//Showing Routing
app.config(['$routeProvider', '$locationProvider', '$httpProvider', function ($routeProvider, $locationProvider, $httpProvider)
{
    $httpProvider.defaults.useXDomain = true;

    $routeProvider.when('/main/dashboard',
                        {
                            templateUrl: '/Dashboard/UserDashboard',
                            controller: 'userDashboardController',
                            //title is custom property set here and get in main page
                            title: 'BitzerIoC :: User Dashboard'
                        });

  
    $routeProvider.when('/main/profile',
            {
                templateUrl: '/Dashboard/Profile',
                controller: 'profileController',
                //title is custom property set here and get in main page
                title: 'BitzerIoC :: User Profile'
            });

    $routeProvider.otherwise(
                        {
                            redirectTo: '/main/profile'

                        });

    $locationProvider.html5Mode(true).hashPrefix('!')
}]);
