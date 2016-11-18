'use strict';
app.service('accountService', ['$http', '$q', function ($http, $q) {


    var account = {
        balance: 0
    };



    this.getBalance = function () {
        console.log(account)
        return account.balance;
    };

    this.deposit = function (amount) {
        console.log(amount)
        account.balance = account.balance + 0 + amount
    }

    this.setBalance = function (amount) {
        account.balance += amount
    }

    this.withdraw = function (amount) {
        console.log(amount)
        if (account.balance >= amount) {
            account.balance -= amount
            return true;
        }
        else
            return false;

    }
}]);