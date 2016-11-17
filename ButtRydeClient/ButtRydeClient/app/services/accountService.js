'use strict';
app.service('accountService', ['$http', '$q', function ($http, $q) {


    var account = {
        balance: 0
    };



    this.getBalance = function () {
        console.log(account.balance)
        return account.balance;
    };

    this.deposit = function (amount) {
        account.balance += amount
    }

    this.setBalance = function (amount) {
        account.balance += amount
    }

    this.withdraw = function (amount) {
        if (account.balance >= amount) {
            account.balance -= amount
            return true;
        }
        else
            return false;

    }
}]);