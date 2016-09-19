'use strict';

app.factory('departmentDisplayViewModel', function () {
    return kendo.data.Model.define({
        id: 'id',
        fields: {
            id: { type: 'string', editable: true, nullable: true },
            name: { type: 'string', editable: true, nullable: true },
        }
    });
});