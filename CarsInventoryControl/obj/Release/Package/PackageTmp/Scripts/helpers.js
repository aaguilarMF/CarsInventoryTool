Paging = function () {
    var currentPageNumber = ko.observable(1),
        previousPageNumber = ko.observable(0),
        nextPage = function () {
            previousPageNumber(parseInt(currentPageNumber()));
            currentPageNumber(parseInt(currentPageNumber()) + 1);
        },
        backPage = function () {
            currentPageNumber(parseInt(previousPageNumber()));
            previousPageNumber(parseInt(currentPageNumber()) - 1);
        }
    return {
        currentPageNumber: currentPageNumber,
        previousPageNumber: previousPageNumber,
        nextPage: nextPage,
        backPage: backPage
    }
}
isNullUndefinedOrWhiteSpace = function (str) {
    if (str === undefined || str === null) {
        return true;
    }
    if (!(str instanceof String)) {
        str = str.toString();
    }
    return str.toString().match(/^ *$/) !== null
};