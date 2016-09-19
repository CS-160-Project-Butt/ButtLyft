'use strict';

app.factory('resourceViewModel', function () {
    return kendo.data.Model.define({
        id: 'id',
        fields: {
            id: { type: 'string', editable: false, nullable: false },
            fileFolder: { type: 'string', editable: false, nullable: false },
            fileName: { type: 'string', editable: false, nullable: false },
            mimeType: { type: 'string', editable: false, nullable: false },
            note: { type: 'string', nullable: true },
            isPublished: { type: 'boolean', nullable: true },
            createdDate: { type: 'date', editable: false, nullable: false },
            createdBy: { type: 'displayUserBindingModel', editable: false }
            /*
            createdBy: {
                id: { type: 'string', editable: false, nullable: false },
                username: { type: 'string', editable: false, nullable: false },
                email: { type: 'string', editable: false, nullable: false },
                firstName: { type: 'string', editable: false, nullable: false },
                lastName: { type: 'string', editable: false, nullable: false }
            }
            */
        }
    });
});