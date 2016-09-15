'use strict';

app.factory('departmentListViewModel', function () {
    return kendo.data.Model.define({
        id: 'id',
        fields: {
            id: { type: 'string', editable: false, nullable: false },
            name: { type: 'string', editable: true, nullable: false },
            companyId: { type: 'string', editable: false, nullable: false },
            companyName: { type: 'string', editable: false, nullable: false },
            parentDepartmentId: { type: 'string', editable: true, nullable: true },
            parentDepartmentName: { type: 'string', editable: true, nullable: true },
            departmentHeadEmployeeId: { type: 'string', editable: false, nullable: true },
            departmentHeadUserName: { type: 'string', editable: true, nullable: true },
            createdById: { type: 'string', editable: false, nullable: false, defaultValue: '00000000-0000-0000-0000-000000000000' },
            createdByUserName: { type: 'string', editable: true, nullable: true },
            createdDate: {
                type: 'date',
                editable: false,
                nullable: true
            },            
            childrenDepartments: []
        }
    });
});