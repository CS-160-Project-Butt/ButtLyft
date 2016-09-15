'use strict';

app.service('messageDataService', function () {
    var msg = {};

    return {
        getMessage: function () {
            return msg;
        },
        setMessage: function (value) {
            msg = value;
        }
    };
});
