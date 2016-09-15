'use strict';
app.service('phaseOutService', ['$http', '$q', 'ngAuthSettings', 'localStorageService', function ($http, $q, ngAuthSettings, localStorageService) {

    var selectedProducts = [];
    var productDB = {
        "data": [{
            "id": "0ae715a5-0eed-4004-bc8c-957a51c7dcbb", "phased": false, "productName": "Assy PEC-4045 L2A Rev. A1 01-1", "partNumber": "10239124236534", "description": "Server Grade Network",
            "status": "Open", "plmStatus": "Service", "productFamily": "i7", "lastBuyTime": "8/16/2016", "replacement": "10923123523", "createdBy": "Jeffffffffff", "createdDate": "2016-07-19T23:59:23.143"
    }], "total": 1
    }

    var customerProductDb = {
        "data": [{
            "id": "7db8948e-b68d-4e49-a9fb-5f0c5c123fed",
            "customerId": "922f2263-2c97-4b71-b77a-230f482873a6",
            "productId": "0ae715a5-0eed-4004-bc8c-957a51c7dcbb",
            "customerName": "Customer1",
            "createdDate": "2016-07-19T23:59:23.143"
        }, {
            "id": "da8e8d39-9562-4799-9f74-b320203527cf",
            "customerId": "36e03274-732e-4a4a-83a2-1c445e6c631d",
            "productId": "0ae715a5-0eed-4004-bc8c-957a51c7dcbb",
            "customerName": "Customer2",
            "createdDate": "2016-07-19T23:59:23.143"
        }, {
            "id": "	d351a4ce-ddf7-4c41-98cf-332f76937501",
            "customerId": "0efcf9a2-d78a-4b7d-ad9a-34bf388c8e91",
            "productId": "0ae715a5-0eed-4004-bc8c-957a51c7dcbb",
            "customerName": "Customer3",
            "createdDate": "2016-07-19T23:59:23.143"
        }], "total": 2
    }


    generateProduct();

    this.addSelectedProduct = function (product) {
        selectedProducts.push(product);
    }
    this.getSelectedProducts = function () {
        return selectedProducts;
    };
    this.removeSelectedProduct = function (id) {

    }

    this.togglePhased = function (toggleId) {
        console.log(toggleId);
        angular.forEach(productDB.data, function (value, key) {

            if (value.id == toggleId) {
                value.phased = !value.phased;
            }

        });

    }

    this.getProduct = function (id) {
        var product;
        angular.forEach(productDB.data, function (value, key) {

            if (value.id == id) {

                product = value;
            }

        });
        return product;
    }

    this.getRelations = function (id) {
        return customerProductDb;
    };


    this.generateProduct = function () {
        //productDB.data.
        var product = {
            "id": generateUUID(),
            "phased": false,
            "productName": createRandomWord(5 + Math.random() * 20),
            "partNumber": createRandomNum(14),
            "description": createRandomWord(5 + Math.random() * 30),
            "plmStatus": "Service",
            "status": "Open",
            "productFamily": createRandomWord(3 + Math.random() * 4),
            "lastBuyTime": "8/16/2016",
            "replacement": createRandomNum(20),
            "createdBy": createRandomWord(5 + Math.random() * 7),
            "createdDate": "2016-07-19T23:59:23.143"
        }
        productDB.data.push(product);
        productDB.total++;

    }
    function generateProduct() {
        //productDB.data.
        var product = {
            "id": generateUUID(),
            "phased": false,
            "productName": createRandomWord(5 + Math.random() * 20),
            "partNumber": createRandomNum(14),
            "description": createRandomWord(5 + Math.random() * 30),
            "plmStatus": "Service",
            "productFamily": createRandomWord(3 + Math.random() * 4),
            "lastBuyTime": "8/16/2016",
            "replacement": createRandomNum(20),
            "createdBy": createRandomWord(5 + Math.random() * 7),
            "createdDate": "2016-07-19T23:59:23.143"
        }
        productDB.data.push(product);
        productDB.total++;


    }
    function createRandomStatus() {

    }
    function createRandomWord(length) {
        var consonants = 'bcdfghjklmnpqrstvwxyz',
            vowels = 'aeiou',
            rand = function (limit) {
                return Math.floor(Math.random() * limit);
            },
            i, word = '', length = parseInt(length, 10),
            consonants = consonants.split(''),
            vowels = vowels.split('');
        for (i = 0; i < length / 2; i++) {
            var randConsonant = consonants[rand(consonants.length)],
                randVowel = vowels[rand(vowels.length)];
            word += (i === 0) ? randConsonant.toUpperCase() : randConsonant;
            word += i * 2 < length - 1 ? randVowel : '';
        }
        return word;
    }

    function createRandomNum(length) {
        var consonants = '1234567890',
            vowels = '1234567890',
            rand = function (limit) {
                return Math.floor(Math.random() * limit);
            },
            i, word = '', length = parseInt(length, 10),
            consonants = consonants.split(''),
            vowels = vowels.split('');
        for (i = 0; i < length / 2; i++) {
            var randConsonant = consonants[rand(consonants.length)],
                randVowel = vowels[rand(vowels.length)];
            word += (i === 0) ? randConsonant.toUpperCase() : randConsonant;
            word += i * 2 < length - 1 ? randVowel : '';
        }
        return word;
    }

    function generateUUID() {
        var d = new Date().getTime();
        var uuid = 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
            var r = (d + Math.random() * 16) % 16 | 0;
            d = Math.floor(d / 16);
            return (c == 'x' ? r : (r & 0x3 | 0x8)).toString(16);
        });
        return uuid;
    };








    
    this.getProducts = function () {
        return productDB;
    };



    this.getApiData = function (apiString) {
        return $http.get(serviceBase + apiString).success(function (results) {
            return results.data;
        });
    };

    this.updateData = function (data) {
        return $http.post(serviceBase + 'api/cla/updateclaform', data).success(function (results) {
            return results.data;
        });
    };

    //var oauthToken = 'oauth/token';
    //var oauthToken = 'Token';

    var serviceBase = ngAuthSettings.apiServiceBaseUri;

    var claData = {};

    var submitCLA = function () {

        populateData();

        var deferred = $q.defer();

        $http.post(serviceBase + 'api/cla', cladata).success(function (response) {
            deferred.resolve(response);
        }).error(function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    };

}]);