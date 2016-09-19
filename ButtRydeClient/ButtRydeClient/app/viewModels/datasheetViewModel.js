'use strict';

app.factory('datasheetViewModel', function () {
    return kendo.data.Model.define({
        id: 'id',
        fields: {
            id: { type: 'string', editable: false, nullable: false },
            link: { type: 'string', editable: true, nullable: true },
            fileUpload: { type: 'resourceViewModel', editable: true, nullable: true }          
        }
    });
});