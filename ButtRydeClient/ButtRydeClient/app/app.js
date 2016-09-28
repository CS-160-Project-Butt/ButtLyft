var app = angular.module('app', ['ngRoute', 'ngAnimate', 'ui.bootstrap', 'LocalStorageModule']);

app.config(function ($routeProvider, $locationProvider) {

    $routeProvider.when("/home", {
        controller: "homeCtrl as vm",
        templateUrl: "/app/views/home.html"
    });

    $routeProvider.when("/login", {
        controller: "loginCtrl as vm",
        templateUrl: "/app/views/login.html"
    });

    $routeProvider.when("/userList", {
        controller: "userListCtrl as vm",
        templateUrl: "/app/views/userList.html"
    });


    $routeProvider.otherwise({ redirectTo: "/home" });

    if (window.history && window.history.pushState) {
        $locationProvider.html5Mode(true);
    }

});

//var serviceBase = 'http://localhost:51423/';
//dev
//var serviceBase = 'http://localhost:50918/';
var serviceBase = 'http://localhost:1272/';

app.constant('ngAuthSettings', {
    apiServiceBaseUri: serviceBase,
    clientId: 'ngAuthApp'
});

app.config(function ($httpProvider) {
    $httpProvider.interceptors.push('authInterceptorService');
});

app.run(['authService', function (authService) {
    authService.fillAuthData();
}]);