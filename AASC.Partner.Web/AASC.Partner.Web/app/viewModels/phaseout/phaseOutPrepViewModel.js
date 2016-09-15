'use strict';

app.factory('phaseOutPrepViewModel', function () {
    return kendo.data.Model.define({
        id: 'id',
        fields: {
            id: { type: 'string', editable: false, nullable: false },
            phased: { type: 'boolean', editable: true, nullable: false },
            partNumber: { type: 'string', editable: true, nullable: false },
            description: { type: 'string', editable: true, nullable: true },
            plmStatus: { type: 'string', editable: true, nullable: false },
            productFamily: { type: 'string', editable: true, nullable: false },
            lastBuyTime: { type: 'date', editable: true, nullable: true },
            replacement: { type: 'string', editable: true, nullable: true },
            createdBy: { type: 'string', editable: true, nullable: false },
            createdDate: { type: 'date', editable: true, nullable: false }
        }
    });
});