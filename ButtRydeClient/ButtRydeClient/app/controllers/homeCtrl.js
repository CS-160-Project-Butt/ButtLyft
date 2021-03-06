﻿'use strict';
app.controller('homeCtrl', ['$route', '$q', '$scope', '$interval', '$timeout', '$location', 'authService', 'signalService', 'NgMap', 'accountService',
    function ($route, $q, $scope, $interval, $timeout, $location, authService, signalService, NgMap, accountService) {
        var vm = this;

        signalService.initialize(); //inits the signalservice factory
        // accountService.deposit(30);  // Deposit $30 for the rider to spend
        vm.startAddress = [0, 0]; //this is used to store the location of the rider's pickup point
        vm.endAddress = [0, 0]; // this is the location of the rider's destination
        vm.inputAddress = null; //user types in the textbox and queries where to go, then hits enter or presses search
        //vm.mapCenter = [0, 0]; //position of the center of the map.
        vm.centerMarker = null;
        vm.dragging = false; //bool: if user is done dragging the map, expand the marker
        vm.drivers = {};
        vm.fare = 0;

        vm.pickupSelected = false;

        vm.showConfirmButton = false;
        vm.hidePanel = false;

        vm.driverLocation = [0, 0]
        vm.drivers = signalService.getDrivers();
        vm.driverInfo = signalService.getDriverInfo();
        vm.showAllDrivers = true;
        


        vm.init = function () {
            navigator.geolocation.getCurrentPosition(function (position) {

                //vm.mapCenter[0] = position.coords.latitude;
                //vm.mapCenter[1] = position.coords.longitude;

                NgMap.getMap('rider').then(function (map) {
                    vm.map = map; //we get the map
                    vm.initBool = false;
                    //console.log(vm.map); //checking out the map
                    //console.log('markers', map.markers);
                    //console.log('shapes', map.shapes);

                    vm.onCenterChanged();//initialize the position of the center marker
                    vm.onDragEnd();
                    if (accountService.getBalance() == 0) {
                        accountService.deposit(30)
  //                      console.log(accountService.getBalance())
                        
                    }
                });
            }, function (err) { });
        }
        vm.init();

        vm.placeChanged = function () {
            vm.place = this.getPlace();
//            console.log('location', vm.place.geometry.location);
            vm.map.setCenter(vm.place.geometry.location);
        }

        vm.calculateFare = function () {
            if (vm.map.directionsRenderers[0].directions != undefined) {
                var distanceValue = vm.map.directionsRenderers[0].directions.routes[0].legs[0].distance.value;
                var durationValue = vm.map.directionsRenderers[0].directions.routes[0].legs[0].duration.value;
                var fare = ((distanceValue / 1000.0) * .3) + ((durationValue / 60) * 1.5) //fare per km + fare per min driven
                //            console.log(fare)
                if (fare > 5)
                { vm.fare = fare }
                else vm.fare = 5;

                vm.showConfirmButton = true;
                vm.onCenterChanged();
            }
        }
        ///**
        // * Used by the html map to alter/update the map location
        // * when we start program, map center initialized to geolocation
        // * after that its wherever the user drags to(number array pair), or the typed in address(string)
        // * after the center is changed we have to update the position of the center marker
        // * parameter: location - usually is null, if not null then force map position to be &location, update center marker
        // */
        //vm.processLocation = function (location) {
        //    console.log('processing location')
        //    if (vm.mapCenter[0] == 0 && vm.mapCenter[1] == 0) {
        //        navigator.geolocation.getCurrentPosition(function (position) {
        //            vm.mapCenter[0] = position.coords.latitude;
        //            vm.mapCenter[1] = position.coords.longitude;

        //            vm.onCenterChanged();
        //        }, function (err) { });
        //    }
        //    if (location != null) {
        //        vm.mapCenter = vm.inputAddress;
        //        vm.inputAddress = null;

        //        vm.onCenterChanged(vm.mapCenter);
        //    }
        //    return vm.mapCenter;
        //}

        /**
         * when we are done dragging the map or if idle we have to expand the little box thing
         */
        vm.onDragEnd = function () {
            vm.dragging = false;
        }

        /**
         * this function is usually called when the user drags on the map
         * it will sync the location of the center marker to the map screen center
         * parameter: center, usually null, if not null then we will set the pos of marker to the argument(&center) instead
         */
        vm.onCenterChanged = function (center) {
            if (vm.map != undefined) {
                vm.dragging = true;
                var temp = [0, 0];
                temp[0] = vm.map.getCenter().lat();
                temp[1] = vm.map.getCenter().lng();
                if (center != null) temp = center
                if (temp[0] != null && temp[1] != null) vm.centerMarker = temp;

                //vm.map.setCenter(vm.centerMarker)
            }
        }



        /**
         * once we figure out where we want to get picked up from we click the button and the button calls this and does things
         */
        vm.setStartAddress = function () {
//            console.log(vm.centerMarker)
            vm.startAddress = angular.copy(vm.centerMarker);
            vm.pickupSelected = true;

            if (vm.endAddress != [0, 0]) {
                $timeout(function () {
                    vm.calculateFare();
                }, 1000);
            }
        }
        vm.setEndAddress = function () {
            vm.endAddress = angular.copy(vm.centerMarker);
            if (vm.startAddress != [0, 0]) {
                $timeout(function () {
                    vm.calculateFare();
                    
                }, 1000);
            }
        }

        vm.confirmFare = function () {
            if (vm.map.directionsRenderers[0].directions != undefined) {
                vm.showConfirmButton = false;
                vm.hidePanel = true;
                signalService.setDestinationCoords(vm.endAddress);
                signalService.boardCastConfirmSignal(vm.startAddress);
            }
        }

        vm.triggerCounter = function () {
            signalService.hit();
        }

        vm.currentMessage;
        vm.sendMessage = function (message) {
            signalService.sendMessage(message);
        }



        vm.stop;
        vm.signalInterval = function () {
            if (angular.isDefined(vm.stop)) return;

            vm.stop = $interval(function () {
                if (signalService.getDriverInfo().pickupSignal == true) {
                    vm.setCenterMarker();
                }
                if (signalService.getDriverInfo().dropOffSignal == true && vm.stopAll == true) {
                    accountService.withdraw(vm.fare) 
                    vm.goToEnd();
                }

            }, 50);
        };
        vm.setCenterMarker = function () {
            var temp = angular.copy(signalService.getMyCoords());

            vm.onCenterChanged(temp);
            vm.map.setCenter({ lat: temp[0], lng: temp[1] })
        }
        vm.stopAll = true;
        vm.goToEnd = function () {
            vm.stopAll = false;
            $location.path('/userDetails');

        }


        vm.signalInterval();

        vm.stopInterval = function () {
            if (angular.isDefined(stop)) {
                $interval.cancel(stop);
                stop = undefined;
            }
        };

        $scope.$on('$destroy', function () {
            // Make sure that the interval is destroyed too
            vm.stopInterval();
        });











        function containsAny(source, target) {
            for (var i = 0; i < target.length; i++) {
                for (var j = 0; j < source.length; j++) {
                    if (target[i] === source[j])
                        return true;
                }
            }
            return false;
        };

        vm.toLogin = function () {
            if (!authService.authentication.isAuth) {
                $location.path('/login');
            }
        }
        vm.toLogin();

        vm.roles = [];

        vm.authentication = authService.authentication;

        authService.getUserRoles().then(function (resRoles) {
            vm.roles = resRoles.data;
        });

        vm.allUsersList;
        authService.getAllUsers().then(function (users) {
 //           console.log(users);
            vm.allUsersList = users.data;

        });

        vm.isUserInRoles = function (roles) {
            var authorizedRoles = [];
            if (!angular.isArray(roles)) {
                authorizedRoles = [roles];
            } else {
                authorizedRoles = roles;
            }
            var result = containsAny($scope.authentication.roles, authorizedRoles);
            return result;
        };
    }]);