'use strict';

app.factory('phaseOutExecutionViewModel', function () {
    return kendo.data.Model.define({
        id: 'id',
        fields: {
            id: { type: 'string', editable: false, nullable: false },
            scheduledDate: { type: 'date', editable: true, nullable: false },
            partNumber: { type: 'string', editable: true, nullable: false },
            status: { type: 'string', editable: true, nullable: false },
            description: { type: 'string', editable: true, nullable: false },
            plmStatus: { type: 'date', editable: true, nullable: false },
            productFamily: { type: 'string', editable: true, nullable: false },
            lastBuyTime: { type: 'date', editable: true, nullable: false },
            replacement: { type: 'string', editable: true, nullable: false },
        }
    });
});