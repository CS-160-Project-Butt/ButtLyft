'use strict';

app.factory('employeeDisplayViewModel', function () {
    return kendo.data.Model.define({
        id: 'id',
        fields: {
            id: { type: 'string', editable: false, nullable: false },
            jobTitle: { type: 'string', editable: false, nullable: true },
            applicationUserId: { type: 'string', editable: false, nullable: false, defaultValue: '00000000-0000-0000-0000-000000000000' },
            username: { type: 'string', editable: false, nullable: false },
            firstName: { type: 'string', editable: false, nullable: false },
            lastName: { type: 'string', editable: false, nullable: false },
            email: { type: 'string', editable: false, nullable: false }
        }
    });
});