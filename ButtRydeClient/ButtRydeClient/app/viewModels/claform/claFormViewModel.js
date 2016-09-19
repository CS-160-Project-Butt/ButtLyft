'use strict';

app.factory('claFormViewModel', function () {
    return kendo.data.Model.define({
        id: 'id',
        fields: {
            id: { type: 'string', editable: false, nullable: false },
            companyName: { type: 'string', editable: true, nullable: false },
            taxID: { type: 'string', editable: true, nullable: false },
            salesContact: { type: 'string', editable: true, nullable: false },
            address: { type: 'string', editable: true, nullable: false },
            city: { type: 'string', editable: true, nullable: false },
            country: { type: 'string', editable: true, nullable: false },
            postCode: { type: 'string', editable: true, nullable: false },
            signerFirstName: { type: 'string', editable: true, nullable: false },
            signerLastName: { type: 'string', editable: true, nullable: false },
            signerEmail: { type: 'string', editable: true, nullable: false },
            signerJobTitle: { type: 'string', editable: true, nullable: true },
            signerPhoneNumber: { type: 'string', editable: true, nullable: false },
            technicalFirstName: { type: 'string', editable: true, nullable: false },
            technicalLastName: { type: 'string', editable: true, nullable: false },
            technicalEmail: { type: 'string', editable: true, nullable: false },
            technicalJobTitle: { type: 'string', editable: true, nullable: true},
            technicalPhoneNumber: { type: 'string', editable: true, nullable: false },

            deviceCategories: { type: 'string', editable: true, nullable: false },
            productList: { type: 'string', editable: true, nullable: false },

            otherType: { type: 'string', editable: true, nullable: false },
            otherQuantity: { type: 'string', editable: true, nullable: false },
            claNumber: { type: 'string', editable: true, nullable: true },
            claStatus: { type: 'string', editable: true, nullable: false },
            claStatusDate: { type: 'date', editable: true, nullable: false },
            customerERPID: { type: 'string', editable: true, nullable: false },
            createdDate: { type: 'date', editable: false, nullable: true }
        }
    });
});