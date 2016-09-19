'use strict';
/*
    Obsolete: 05/10/2016
    Use companyListViewModel instead
*/
app.factory('companyViewModel', function () {
    return kendo.data.Model.define({
        id: 'id',
        fields: {
            id: { type: 'string', editable: false, nullable: false },
            name: { type: 'string', editable: true, nullable: false },
            createdById: { type: 'string', editable: false, nullable: false, defaultValue: '00000000-0000-0000-0000-000000000000' },
            createdDate: {
                type: 'date',
                editable: false,
                nullable: true                
            },
            createdBy: {
                type: 'displayUserBindingModel',
                editable: false,
                defaultValue: {
                    id: '',
                    email: '',
                    username: '',
                    firstName: 'firstName',
                    lastName: 'lastName'
                }
            }
        }
    });
});