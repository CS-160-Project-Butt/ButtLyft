'use strict';
/*
    Obsolete: 05/10/2016
    Use departmentListViewModel instead
*/
app.factory('departmentViewModel', function () {
    return kendo.data.Model.define({
        id: 'id',
        fields: {
            id: { type: 'string', editable: false, nullable: false },
            name: { type: 'string', editable: true, nullable: false },
            companyId: { type: 'string', editable: false, nullable: false },
            company: { 
                type: 'companyViewModel',
                editable: false,
                defaultValue: {
                    id: '',
                    name: ''
                }
            },
            parentDepartmentId: { type: 'string', editable: false, nullable: true },
            parentDepartment: {                 
                //id: { type: 'string' },
                //name: { type: 'string' }
                type: 'departmentDisplayViewModel',
                editable: false,                
                defaultValue: {
                    id: '',
                    name: '',
                }
            },

            departmentHeadEmployeeId: { type: 'string', editable: false, nullable: true },
            departmentHead: {
                type: 'employeeViewModel',
                editable: false,
                defaultValue: {
                    id: '',
                    applicationUser: {
                        id: '',
                        username: '',
                        firstName: '',
                        lastName: '',
                        email: ''
                    }
                }
            },
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
            },
            childrenDepartments: []
        }
    });
});