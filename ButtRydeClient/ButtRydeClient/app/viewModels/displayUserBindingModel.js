'use strict';

app.factory('displayUserBindingModel', function () {
    return kendo.data.Model.define({
        id: 'id',
        fields: {
            id: { type: 'string', editable: false, nullable: true, defaultValue: '' },
            email: { type: 'string', editable: false, nullable: true, defaultValue: '' },
            userName: { type: 'string', editable: false, nullable: true, defaultValue: '' },
            firstName: { type: 'string', editable: false, nullable: true, defaultValue: '' },
            lastName: { type: 'string', editable: false, nullable: true, defaultValue: '' },
        }
    });
});