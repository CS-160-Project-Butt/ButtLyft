var app = angular.module('app', ['ngRoute', 'ngAnimate', 'ui.bootstrap', 'LocalStorageModule', 'angular-loading-bar', 'angularFileUpload']);

app.config(function ($routeProvider, $locationProvider) {

    $routeProvider.when("/home", {
        controller: "homeController",
        templateUrl: "/app/views/home.html"
    });

    $routeProvider.when("/contact", {
        controller: "contactController",
        templateUrl: "/app/views/contact.html"
    });

    $routeProvider.when("/login", {
        controller: "loginCtrl as vm",
        templateUrl: "/app/views/login.html"
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
//production
//var serviceBase = 'http://partners.advantech.com:81/';
//qa
//var serviceBase = 'http://partners.advantech.com:8081/';
//var serviceBase = 'http://ngauthenticationapi.azurewebsites.net/';
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