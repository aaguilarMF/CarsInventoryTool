ViewModels.CarsInventory.Car = function (GridState) {
    var dummy = ko.observable('guat'),
        trueCarEntry = ko.observable(null),
        changed = ko.observable(false),
        carEntry = new function () {
            var locationNo = ko.observable(),
                availabilityControlId = ko.observable(),
                requestedBy = ko.observable(),
                warehouse = ko.observable(),
                merge = function (car) {
                    locationNo(car.LocationNo);
                    availabilityControlId(car.AvailabilityControlId);
                    requestedBy(car.RequestedBy);
                    warehouse(car.Warehouse);
                },
                asJson = function () {
                    return {
                        LocationNo: locationNo(),
                        AvailabilityControlId: availabilityControlId(),
                        RequestedBy: requestedBy(),
                        Warehouse: warehouse()
                    }
                }
            return {
                LocationNo: locationNo,
                AvailabilityControlId: availabilityControlId,
                RequestedBy: requestedBy,
                Warehouse: warehouse,
                merge: merge,
                asJson: asJson
            }
        },
        edit = function () {
            save(carEntry, false);
        },
        add = function () {
            save(carEntry, true);
        },
        save = function (carEntry, isAdd) {
            $.ajax({
                type: 'POST',
                url: '/CarsInventoryAPI/Inventory/Save/' + isAdd,
                data: carEntry.asJson(),
                success: function (result) {
                    carEntry.merge(result);
                    trueCarEntry(result);
                },
                error: function (result) {
                    alert('error saving');
                },
                complete: function () {
                    changed(false);
                },
                beforeSend: function () {

                }
            });
        },
        deleteEntry = function () {
            $.ajax({
                type: 'POST',
                url: '/CarsInventoryAPI/Inventory/Delete',
                data: trueCarEntry(),
                success: function (result) {
                    alert('Success');
                    back();
                },
                error: function (result) {
                    alert('error deleting');
                },
                complete: function () {
                    changed(false);
                },
                beforeSend: function () {

                }
            });
        },
        back = function () {

            $.ajax({
                type: 'POST',
                url: '/CarsInventory/Template/Home/',
                data: {
                    SearchCriteria: GridState != null ? GridState.SearchCriteria : null,
                    PageCount: GridState != null ? GridState.PageCount : null,
                },
                success: function (result) {

                    window.document.write(result);
                    window.document.close();

                },
                error: function (result) {
                    alert(result.statusText);
                },
                complete: function () {
                    //searchingStructure(false);
                    //if (!processRunning()) {
                    //    cursorDefault();
                    //}
                },
                beforeSend: function () {
                    //searchingStructure(true);
                    //cursorWait();
                }
            });
        },
        navigateToEdit = function (car) {
            $.ajax({
                type: 'POST',
                url: '/CarsInventory/Template/Edit/' + encodeURIComponent(car().LocationNo),
                success: function (result) {
                    window.document.close();
                    window.document.write(result);
                    window.document.close();

                },
                error: function (result) {
                    alert(result.statusText);
                },
                complete: function () {
                },
                beforeSend: function () {
                }
            });
        },
        getCarEntry = function (locationNo) {
            $.ajax({
                type: 'POST',
                url: '/CarsInventoryAPI/Inventory/Car/' + encodeURIComponent(locationNo),
                success: function (result) {
                    carEntry.merge(result);
                    trueCarEntry(result);
                    changed(false);
                },
                error: function (result) {
                    alert('Twas an unfortunate error');
                },
                complete: function () {
                },
                beforeSend: function () {

                }
            })
        },
        onEnter = function (d, e) {
            if (e.keyCode === 13) {
                edit();
            }
            else {
                changed(true);
                return true;
            }
        },
        init = function (locationNo) {
            if (!isNullUndefinedOrWhiteSpace(locationNo)) { //should be add
                getCarEntry(locationNo);
            }
            carEntry.LocationNo.subscribe(function (value) {
                changed(true);
            });
            carEntry.AvailabilityControlId.subscribe(function (value) {
                changed(true);
            });
            carEntry.RequestedBy.subscribe(function (value) {
                changed(true);
            });
            carEntry.Warehouse.subscribe(function (value) {
                changed(true);
            });
        };
    init(GridState != null ? (GridState.CurrentObjectView != null ? GridState.CurrentObjectView.LocationNo : null) : null);

    return {
        trueCarEntry: trueCarEntry,
        carEntry: carEntry,
        edit: edit,
        add: add,
        navigateToEdit: navigateToEdit,
        deleteEntry: deleteEntry,
        changed: changed,
        onEnter: onEnter,
        back: back
    }
}