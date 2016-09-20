'use strict';

app.factory('employeeViewModel', function () {
    return kendo.data.Model.define({
        id: 'id',
        fields: {
            id: { type: 'string', editable: false, nullable: false },
            jobTitle: { type: 'string', editable: false, nullable: true },
            companyId: { type: 'string', editable: false, nullable: true },
            company: {
                type: 'companyViewModel',
                editable: false,
                defaultValue: {
                    id: '',
                    name: ''
                }
            },
            applicationUserId: { type: 'string', editable: false, nullable: false, defaultValue: '00000000-0000-0000-0000-000000000000' },
            applicationUser: {
                type: 'displayUserBindingModel',
                editable: false,
                defaultValue: {
                    id: '',
                    username: '',
                    firstName: '',
                    lastName: '',
                    email: ''
                }
            },
            createdDate: {
                type: 'date',
                editable: false,
                nullable: true
            },
            createdById: { type: 'string', editable: false, nullable: false, defaultValue: '00000000-0000-0000-0000-000000000000' },
            createdBy: {
                type: 'displayUserBindingModel',
                editable: false,
                nullable: true
            }
        }
    });
});