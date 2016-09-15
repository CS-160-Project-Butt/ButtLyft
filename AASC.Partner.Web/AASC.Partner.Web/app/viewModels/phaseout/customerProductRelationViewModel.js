'use strict';

app.factory('customerProductRelationViewModel', function () {
    return kendo.data.Model.define({
        id: 'id',
        fields: {
            id: { type: 'string', editable: false, nullable: false },
            customerId: { type: 'string', editable: false, nullable: false },
            productId: { type: 'string', editable: false, nullable: false },
            customerName: { type: 'boolean', editable: true, nullable: false },
            createdDate: { type: 'date', editable: true, nullable: false }
        }
    });
});