'use strict';

app.factory('employeeListViewModel', function () {
    return kendo.data.Model.define({
        id: 'id',
        fields: {
            id: { type: 'string', editable: false, nullable: false },
            jobTitle: { type: 'string', editable: false, nullable: true },
            companyId: { type: 'string', editable: false, nullable: true },
            companyName: { type: 'string', editable: false, nullable: true },
            applicationUserId: { type: 'string', editable: false, nullable: false, defaultValue: '00000000-0000-0000-0000-000000000000' },
            applicationUserName: { type: 'string', editable: false, nullable: true },
            applicationUserEmail: { type: 'string', editable: false, nullable: true },
            applicationUserFirstName: { type: 'string', editable: false, nullable: true },
            applicationUserLastName: { type: 'string', editable: false, nullable: true },
            createdDate: {
                type: 'date',
                editable: false,
                nullable: true
            },
            createdById: { type: 'string', editable: false, nullable: false, defaultValue: '00000000-0000-0000-0000-000000000000' },
            createdByUserName: { type: 'string', editable: false, nullable: true },
            activeFrom: {
                type: 'date',
                editable: true,
                nullable: true
            },
            deactiveDate: {
                type: 'date',
                editable: true,
                nullable: true
            },
        }
    });
});