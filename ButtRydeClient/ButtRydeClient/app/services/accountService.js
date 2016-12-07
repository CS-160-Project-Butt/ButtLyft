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
        if (amount >= 0 && typeof (amount) != 'undefined') {
            amount = amount.toFixed(2)
            amount = Number(amount)
            account.balance = account.balance + amount
            return true;
        }
        else
            return false;
    }

    this.setBalance = function (amount) {
        account.balance += amount
    }

    this.withdraw = function (amount) {
        console.log(amount)
        if (account.balance >= amount && amount >= 0 && typeof (amount) != 'undefined') {
            amount = amount.toFixed(2)
            amount = Number(amount)
            account.balance -= amount
            return true;
        }
        else
            return false;

    }
}]);