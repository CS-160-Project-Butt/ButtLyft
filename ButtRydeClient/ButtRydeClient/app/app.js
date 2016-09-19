var app = angular.module('app', ['kendo.directives', 'ngRoute', 'ngAnimate', 'ui.bootstrap', 'LocalStorageModule', 'angular-loading-bar', 'angularFileUpload']);

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
        controller: "loginController",
        templateUrl: "/app/views/login.html"
    });

    $routeProvider.when("/signup", {
        controller: "signupController",
        templateUrl: "/app/views/signup.html"
    });
    /*
    $routeProvider.when("/orders", {
        controller: "ordersController",
        templateUrl: "/app/views/orders.html"
    });
    */

    $routeProvider.when("/refresh", {
        controller: "refreshController",
        templateUrl: "/app/views/refresh.html"
    });

    $routeProvider.when("/tokens", {
        controller: "tokensManagerController",
        templateUrl: "/app/views/tokens.html"
    });

    $routeProvider.when("/associate", {
        controller: "associateController",
        templateUrl: "/app/views/associate.html"
    });

    $routeProvider.when("/fileUpload", {
        controller: "fileUploadController",
        templateUrl: "/app/views/fileUpload.html"
    });

    $routeProvider.when("/resources", {
        controller: "resourcesController",
        templateUrl: "/app/views/resources2.html"
    });

    $routeProvider.when("/downloads", {
        controller: "downloadsController",
        templateUrl: "/app/views/resources1.html"
    });

    $routeProvider.when("/changePassword", {
        controller: "usersController",
        templateUrl: "/app/views/changePassword.html"
    });

    $routeProvider.when("/forgotPassword", {
        controller: "usersController",
        templateUrl: "/app/views/forgotPassword.html"
    });

    $routeProvider.when("/forgotPasswordConfirmation", {
        controller: "usersController",
        templateUrl: "/app/views/forgotPasswordConfirmation.html"
    });

    $routeProvider.when("/resetPasswordConfirmation", {
        controller: "usersController",
        templateUrl: "/app/views/resetPasswordConfirmation.html"
    });

    $routeProvider.when("/resetPassword", {
        controller: "usersController",
        templateUrl: "/app/views/resetPassword.html"
    });

    $routeProvider.when("/iotgRoadmap", {
        controller: "iotgRoadmapController",
        templateUrl: "/app/views/iotgRoadmap.html"
    });

    $routeProvider.when("/applicationUsers", {
        controller: "applicationUsersController",
        templateUrl: "/app/views/applicationUsers.html"
    });

    $routeProvider.when("/companies", {
        controller: "companiesController as vm",
        templateUrl: "/app/views/companies.html"
    });

    $routeProvider.when("/departments", {
        controller: "departmentsController",
        templateUrl: "/app/views/departments.html"
    });

    $routeProvider.when("/employees", {
        controller: "employeesController",
        templateUrl: "/app/views/employees.html"
    });

    $routeProvider.when("/claform", {
        controller: "ClaFormCtrl as vm",
        templateUrl: "/app/views/claform/claForm.html"
    });


    $routeProvider.when("/claform/view/:id", {
        controller: "ClaViewCtrl as vm",
        templateUrl: "/app/views/claform/claView.html"
    });

    $routeProvider.when("/claform/view/:id/:edit", {
        controller: "ClaViewCtrl as vm",
        templateUrl: "/app/views/claform/claView.html"
    });


    $routeProvider.when("/claform/admin", {
        controller: "ClaAdminCtrl as vm",
        templateUrl: "/app/views/claform/claAdmin.html"
    });

    $routeProvider.when("/claform/thanks", {
        controller: "ThanksCtrl as vm",
        templateUrl: "/app/views/claform/thanks.html"
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