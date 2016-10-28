var app = angular.module('app', ['ngRoute', 'ngAnimate', 'ngMap', 'ui.bootstrap', 'LocalStorageModule']);

app.config(function ($routeProvider, $locationProvider) {

    $routeProvider.when("/home", {
        controller: "homeCtrl as vm",
        templateUrl: "/app/views/home.html"
    });

    $routeProvider.when("/driver", {
        controller: "driverCtrl as vm",
        templateUrl: "/app/views/driver.html"
    });

    $routeProvider.when("/login", {
        controller: "loginCtrl as vm",
        templateUrl: "/app/views/login.html"
    });

    $routeProvider.when("/userList", {
        controller: "userListCtrl as vm",
        templateUrl: "/app/views/userList.html"
    });

    $routeProvider.when("/userDetails", {
        controller: "userDetailsCtrl as vm",
        templateUrl: "/app/views/userDetails.html"
    });



    $routeProvider.otherwise({ redirectTo: "/home" });

    if (window.history && window.history.pushState) {
        $locationProvider.html5Mode(true);
    }

});

var serviceBase = 'http://localhost:1272/';
//var serviceBase = 'http://rydeserver.azurewebsites.net/';

app.constant('ngAuthSettings', {
    apiServiceBaseUri: serviceBase,
    clientId: 'ngAuthApp'
});

app.value('$', $);

app.config(function ($httpProvider) {
    $httpProvider.interceptors.push('authInterceptorService');
});

app.run(['authService', function (authService) {
    authService.fillAuthData();
}]);