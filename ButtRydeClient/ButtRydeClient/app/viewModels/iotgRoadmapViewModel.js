'use strict';

app.factory('iotgRoadmapViewModel', function () {
    return kendo.data.Model.define({
        id: 'id',
        fields: {
            id: { type: 'string', editable: false, nullable: true },
            model: { type: 'string', nullable: false },
            status: { type: 'string', nullable: true },
            platform: { type: 'string', nullable: false },
            trim: { type: 'string', nullable: false },
            year: { type: 'number', nullable: false, validation: { min: 2010, required: true }, defaultValue: 2015 },
            codeName: { type: 'string', nullable: false },
            level: { type: 'string', nullable: false },
            category: { type: 'string', nullable: false },
            subcategory: { type: 'string', nullable: false },
            datasheet: { type: 'datasheetViewModel', nullable: true },
            marketSegment: { type: 'string', nullable: true },
            availabilityESSample: { type: 'string', nullable: true },
            availabilityMP: { type: 'string', nullable: true },
            contact: { type: 'employeeViewModel', nullable: true }
        }
    });
});