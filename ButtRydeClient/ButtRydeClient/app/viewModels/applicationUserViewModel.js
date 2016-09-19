'use strict';

app.factory('applicationUserViewModel', function () {
    return kendo.data.Model.define({
        id: 'id',
        fields: {
            id: { type: 'string', editable: false, nullable: false },
            userName: { type: 'string', editable: true, nullable: false },
            firstName: { type: 'string', editable: true, nullable: false },
            lastName: { type: 'string', editable: true, nullable: false },
            email: { type: 'string', editable: true, nullable: false },
            isActive: { type: 'boolean', editable: true, nullable: false }
        }
    });
});