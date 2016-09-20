//Application Module

var app = angular.module("BitzerAdminModule", ["ngRoute", 'ui.grid']);

app.constant('configurations',
{
    appName: 'BitzerIoC.Admin',
    appVersion: 1.0,
    environment: 'Development',
    baseApiUrlDevelopment: 'http://localhost:5003/api',
    baseApiUrlProduction: 'http://local.api.bitzerioc.com/api'
    // environment: 'Production'
});



//Mange event on app level
//Whenever route change successfully this code executed
//Validate route exisit and then set page title
//Title property get from route config
app.run(['$location', '$rootScope', function ($location, $rootScope) {
    $rootScope.$on('$routeChangeSuccess', function (event, current, previous) {
        if (current.hasOwnProperty('$$route')) {
            $rootScope.title = current.$$route.title;
        }
    });
}]);


//Showing Routing
app.config(['$routeProvider', '$locationProvider', function ($routeProvider, $locationProvider) {
    $routeProvider.when('/admin/dashboard',
                        {
                            templateUrl: '/Dashboard/AdminDashboard',
                            controller: 'adminDashboardController',
                            //title is custom property set here and get in main page
                            title: 'BitzerIoC :: Admin Dashboard'
                        });

    $routeProvider.when('/admin/users',
                     {
                         templateUrl: '/Dashboard/Users',
                         controller: 'usersController',
                         //title is custom property set here and get in main page
                         title: 'BitzerIoC :: Users'
                     });

    $routeProvider.when('/admin/devices',
                 {
                     templateUrl: '/Dashboard/Devices',
                     controller: 'devicesController',
                     //title is custom property set here and get in main page
                     title: 'BitzerIoC :: Devices'
                 });

    $routeProvider.when('/admin/gateways',
             {
                 templateUrl: '/Dashboard/Gateways',
                 controller: 'gatewaysController',
                 //title is custom property set here and get in main page
                 title: 'BitzerIoC :: Gateways'
             });

    $routeProvider.when('/admin/creategateway',
             {
                 templateUrl: '/Dashboard/CreateGateway',
                 controller: 'createGatewayController',
                 //title is custom property set here and get in main page
                 title: 'BitzerIoC ::Create Gateway'
             });

    $routeProvider.when('/admin/updategateway/:gatewayId?',
         {
             templateUrl: function (urlattr) {
                 var param = "";
                 if (urlattr.gatewayId != null) {
                     param = "gatewayId=" + urlattr.gatewayId;
                 }
                 return '/Dashboard/UpdateGateway?' + param;
             },
             controller: 'updateGatewayController',
             //title is custom property set here and get in main page
             title: 'BitzerIoC ::Update Gateway'
         });

   $routeProvider.when('/admin/createuser',
            {
                templateUrl: '/Dashboard/CreateUser',
                controller: 'createUserController',
                //title is custom property set here and get in main page
                title: 'BitzerIoC :: Create User'
            });
    $routeProvider.when('/admin/profile',
            {
                templateUrl: '/Dashboard/Profile',
                controller: 'profileController',
                //title is custom property set here and get in main page
                title: 'BitzerIoC :: User Setting'
            });

      
    $routeProvider.otherwise(
                        {
                            redirectTo: '/admin/users'
                        });

    $locationProvider.html5Mode(true).hashPrefix('!')
}]);


//Shared Factory
//app.factory('SharedData', function ()
//{
//    return {
//              routeData:{
//                    Values;
//                  },
//              update: function (values)
//              {
//                  this.routeData.Values = values;

//              }
//    };
//});
