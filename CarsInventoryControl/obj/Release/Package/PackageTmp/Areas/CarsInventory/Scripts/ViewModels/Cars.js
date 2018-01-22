ViewModels.CarsInventory.Cars = function (GridState) {
    var dummy = ko.observable(GridState.Message),
        loading = ko.observable(false),
        paging = new Paging(),
        nextPageExists = ko.observable(false),
        structure = ko.observableArray([]),
        searchingStructure = ko.observable(false),
        searchOptions = ko.observable(new function () {
            var locationNo = ko.observable(),
                availabilityControlId = ko.observable(),
                requestedBy = ko.observable(),
                warehouse = ko.observable(),
                caseSensitiveSearch = ko.observable(false),
                reset = function () {
                    locationNo(null);
                    availabilityControlId(null);
                    requestedBy(null);
                    warehouse(null);
                    caseSensitiveSearch(false);
                }
            return {
                LocationNo: locationNo,
                AvailabilityControlId: availabilityControlId,
                RequestedBy: requestedBy,
                Warehouse: warehouse,
                CaseSensitiveSearch: caseSensitiveSearch,
                Reset: reset
            }
        }),
        /*Methods*/
        getInventoryInfo = function (paging) {
            searchOptions();
            $.ajax({
                type: 'GET',
                url: '/CarsInventoryAPI/Inventory/Cars/' + paging.currentPageNumber()  + "/" + searchOptions().CaseSensitiveSearch() + "/" +
                    encodeURIComponent((isNullUndefinedOrWhiteSpace(searchOptions().LocationNo()) ? null : searchOptions().LocationNo())) + "/" +
                    encodeURIComponent((isNullUndefinedOrWhiteSpace(searchOptions().AvailabilityControlId()) ? null : searchOptions().AvailabilityControlId())) + "/" +
                    encodeURIComponent((isNullUndefinedOrWhiteSpace(searchOptions().RequestedBy()) ? null : searchOptions().RequestedBy())) + "/" +
                    encodeURIComponent((isNullUndefinedOrWhiteSpace(searchOptions().Warehouse()) ? null : searchOptions().Warehouse())),
                success: function (result) {
                    nextPageExists(result.NextPageExists);
                    structure(result.Inventory);
                },
                error: function (result) {
                    alert(result.statusText);
                },
                complete: function () {
                    searchingStructure(false);
                    if (!processRunning()) {
                        cursorDefault();
                    }
                },
                beforeSend: function () {
                    searchingStructure(true);
                    cursorWait();
                }
            });
        },
        edit = function (ediEntry) {
            //alert(ediEntry);
            //window.location.href = '/Template/Edit/' + ediEntry.RowId;
            $.ajax({
                type: 'POST',
                url: '/CarsInventory/Template/Edit/' + encodeURIComponent( ediEntry.LocationNo),
                data: {
                    PageCount: paging.currentPageNumber()
                },
                success: function (result) {
                    window.document.close();
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
                    searchingStructure(true);
                    cursorWait();
                }
            });
        },
         /////////////////general functions /////////////
        search = function () {
             paging.currentPageNumber(1);
             paging.previousPageNumber(0);
             getInventoryInfo(paging);
        },
        resetSearch = function () {
            paging.currentPageNumber(1);
            paging.previousPageNumber(0);
            searchOptions().Reset();
            getInventoryInfo(paging);
        }
        nextPage = function () {
             paging.nextPage();
             getInventoryInfo(paging);
         },
        backPage = function () {
            paging.backPage();
            getInventoryInfo(paging);
        },
        processRunning = ko.computed(function () {
            return searchingStructure();
        }),
        cursorWait = function () {
            loading(true);
            document.body.style.cursor = 'wait';
        },
        cursorDefault = function () {
            loading(false);
            document.body.style.cursor = 'default';
        },
        init = function (pageCount) {
            paging.currentPageNumber(pageCount);
            paging.previousPageNumber(pageCount > 1 ? pageCount - 1 : 0);
            getInventoryInfo(paging);
        };
    init(parseInt(GridState.PageCount));

    return {
        dummy: dummy,
        structure: structure,
        searchOptions: searchOptions,
        edit: edit,
        search: search,
        resetSearch: resetSearch,
        paging: paging,
        nextPage: nextPage,
        backPage: backPage,
        nextPageExists: nextPageExists
    }
}