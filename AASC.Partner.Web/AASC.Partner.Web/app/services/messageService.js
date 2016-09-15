'user strict';

app.factory('messageService', function ($rootScope) {
    return {
        prepareResponseMessage: function (response) {
            var msg = '';
            var messageType = 'info';
            var messageDetails = [];
            if (response.status && response.statusText) {
                if (response.status >= '400')
                    messageType = 'authorization';
                else if (response.status >= '300')
                    messageType = 'error';
                msg = 'Status Code: [' + response.status + '] - Text: [' + response.statusText + ']';
            }                
            if (response.message)
                msg += response.message;
            if (response.data) {
                if (response.data.message) msg += '- Extra Message: [' + response.data.message + '] ';
                if (response.data.exceptionMessage) msg += '- Exception: [' + response.data.exceptionMessage + ']';
                if (response.data.modelState) {
                    var modelState = response.data.modelState;
                    for (var key in modelState) {
                        if (modelState.hasOwnProperty(key)) {
                            messageDetails.push(' key: ' + key + ' Error: ' + modelState[key]);
                        }
                    }
                }
            }
            return { status: response.status, messageType: messageType, message: msg, messageDetails: messageDetails };
        },
        prepareResultMessage: function (result) {
            var msg = 'Error: ' + result.status + ' StatusText: ' + result.statusText + ' ResponseText: ' + result.responseText;
            return msg;
        }
    };
});