'user strict';

app.factory('dialogService', function ($q) {
    return {
        showDialog: function (title, message) {
            var deferred = $q.defer();

            var html = 
              '<div id="myDialogWindow"> ' +
              ' <div style="text-align: center; width:100%"> ' +
              '   <div style="margin:10px 0 15px 0">' + message + '</div> ' +
              '   <button class="k-button k-primary" id="yesButton">Yes</button> ' +
              '   <button class="k-button" id="noButton"">No</button> ' +
              '   </div> ' +
              '</div> ';

            $('body').append(html);

            var windowDiv = $('#myDialogWindow');
            windowDiv.kendoWindow({
                width: "250px",
                title: title,
                modal: true,
                visible: false
            });

            var dialog = windowDiv.data('kendoWindow');

            $('#yesButton').click(function (e) {
                dialog.close();
                $('#myDialogWindow').remove();
                deferred.resolve();
            });

            $('#noButton').click(function (e) {
                dialog.close();
                $('#myDialogWindow').remove();
                deferred.reject();
            });

            dialog.center();
            dialog.open();

            return deferred.promise;
        }
    };
});