$(document).ready(function () {

    //Disable autocomplete for textboxes
    $("input:text,form").attr("autocomplete", "off");
    
     //Update report when user selects trucknumber in DropDownList
    $(document.body).on('change', '#TruckNumberId', function () {
       retrieveInspectionReportsSortedByTruckIdInAscendingOrder($('#TruckNumberId').val());
    });

});

//Retrieve List of Inspections  
function retrieveInspectionReportsSortedByTruckIdInAscendingOrder (truckNumber) {
    varUrl = "/InspectionReport/RetrieveInspectionReportsListSortedByUserRequest";
    varType = "GET";
    varSortField = "Id";
    varSortDirection = "ascending";
    varPageSize = 10;
    varPageCount = 0;
    varCurrentPageIndex = 0;
    varTruckNumber = truckNumber;
    var userSortingRequest = {
        SortField: varSortField,
        SortDirection: varSortDirection,
        PageSize: varPageSize,
        PageCount: varPageCount,
        CurrentPageIndex: varCurrentPageIndex,
        TruckNumberId: varTruckNumber
    };
    $.ajax({
        url: varUrl,
        type: varType,
        data: userSortingRequest,
        success: successLoadingStudents,
        error: failLoadingStudents
    })
}

//Load reports into inspections table
function successLoadingStudents(partialViewResultObject) {
    
    $("#tblInpectionReportsList").html(partialViewResultObject);
}

//Error retrieving inspections data
function failLoadingStudents(result) {
    alert('Failure loading Inspection Reports : ' + result.status + result.statusText);
}
