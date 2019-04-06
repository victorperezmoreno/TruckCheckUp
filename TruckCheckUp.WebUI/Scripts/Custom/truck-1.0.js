$(document).ready(function () {
    //Ensure that only letters and numbers are entered in VIN textbox
    validateTextboxContainsOnlyAlphanumericCharacters('vinNumber');
    //Ensure that only numbers are entered in truck number textbox
    validateTextboxContainsOnlyNumericCharacters('truckNumber');
    //Disable autocomplete for textboxes
    $("input:text,form").attr("autocomplete", "off");
    //Load Data in Table when documents is ready
    loadTruckData();
});

function validateSearchTextBox() {
    var $regexname = /^([0-9a-zA-Z]{2,30})$/;
    $('#truckSearch_textbox').on('keypress keydown keyup', function () {
        if (!$(this).val().match($regexname)) {
            // there is a mismatch, hence show the error message
            $('#truckSearch_textbox').css('border-color', 'Red');
            $('#truckSearch_error').text("Please only letters or numbers");
            $("#truckSearch_button").prop('disabled', true);
        }
        else {
            // else, do not display message
            $('#truckSearch_textbox').css('border-color', 'lightgrey');
            $('#truckSearch_error').text("");
            $("#truckSearch_button").prop('disabled', false);
        }
    });
}

function validateManufacturerDropDownList(manufacturerSelected) {
    //Ensures a valid value is selected
    var validValueSelectedByUser = ValidateUserSelectedAValueInDropDownList("manufacturer");

    if (validValueSelectedByUser == true)
    {
        populateModelAndYearDropDownBoxes(manufacturerSelected.value);
    }
}

function populateModelAndYearDropDownBoxes(manufacturerId)
{
    varUrl = "/TruckManagement/GetModelAndYearLists/" + manufacturerId;
    varType = "GET";
    varContentType = "application/json;charset=utf-8";
    varDataType = "json";
    $.ajax({
        url: varUrl,
        type: varType,
        contentType: varContentType,
        dataType: varDataType,
        success: populateModelAndYearDropDownLists,
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function populateModelAndYearDropDownLists(truck)
{
    populateModelsDropDownList(truck);
    populateYearsDropDownList(truck);
}

function validateModelDropDownList(modelSelected) {
    //Ensures "Select Model" or "Please Select" is not selected 
    ValidateUserSelectedAValueInDropDownList("model");
}

function validateYearDropDownList(yearSelected) {
    //Ensures "Select Model" or "Please Select" is not selected 
    ValidateUserSelectedAValueInDropDownList("year");
}

function searchTruck() {
    //Search only if search textbox not empty
    var txtModel = jQuery.trim($("#truckSearch_textbox").val());
    if (txtModel.length != 0) {
        retrieveModelRecord();
    }
    else {
        resetTruckSearchTextBox()
    }
}

function clearTruckSearch() {
    resetTruckSearchTextBox();
    loadTruckData();
}

function retrieveTruckRecord() {
    varUrl = "/TruckModelManagement/SearchTruckNumber";
    var modelObj = {
        Id: "",
        Description: $('#truckSearch_textbox').val(),
        ExistInDB: true,
        IsValid: true
    };
    varType = "POST";
    varContentType = "application/json;charset=utf-8";
    varDataType = "json";
    $.ajax({
        url: varUrl,
        data: JSON.stringify(modelObj),
        type: varType,
        traditional: true,
        contentType: varContentType,
        dataType: varDataType,
        success: modelRecordRetrieved,
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function modelRecordRetrieved(modelRecord) {
    if (modelRecord.IsValid == false) {
        invalidModelNameSearchTextBox();
    }
    else {
        if (modelRecord.ExistInDB == true) {
            var html = '';
            $.each([modelRecord], function (key, item) {
                html += '<tr>';
                html += '<td>' + item.Id + '</td>';
                html += '<td>' + item.Description + '</td>';
                html += '<td>' + item.ManufacturerDescription + '</td>';
                html += '<td><a href="#" onclick="return getModelById(\'' + item.Id + '\')">Edit</a> | <a href="#" onclick="deleteTruck(\'' + item.Id + '\')">Delete</a></td>';
                html += '</tr>';
            });
            $('.tbodyTruck').html(html);
        }
        else {
            noRecordsFoundInTruckDatabaseMessage();
        }
        resetTruckSearchTextBox();
    }
}

function noRecordsFoundInTruckDatabaseMessage() {
    var html = '';
    $('.tbodyTruck').html('');
    html += '<tr>';
    html += '<td>' + 'No records found in Database' + '</td>';
    html += '<td>' + '' + '</td>';
    html += '<td>' + '' + '</td>';
    html += '<td>' + '' + '</td>';
    html += '<td>' + '' + '</td>';
    html += '<td>' + '' + '</td>';
    html += '<td>' + '' + '</td>';
    html += '<td>' + '' + '</td>';
    html += '</tr>';
    $('.tbodyTruck').html(html);
}

//Load Data function
function loadTruckData() {
    varUrl = "/TruckManagement/ListOfTrucks";
    varType = "GET";
    varContentType = "application/json;charset=utf-8";
    varDataType = "json";
    $.ajax({
        url: varUrl,
        type: varType,
        contentType: varContentType,
        dataType: varDataType,
        success: dataRetrievedFromDatabase,
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function dataRetrievedFromDatabase(allTrucks) {
    if (Object.keys(allTrucks).length == 0) {
        noRecordsFoundInTruckDatabaseMessage();
    }
    else {
        var html = '';
        $.each(allTrucks, function (key, item) {
            html += '<tr>';
            html += '<td>' + item.Id + '</td>';
            html += '<td>' + item.TruckNumber + '</td>';
            html += '<td>' + item.VIN + '</td>';
            html += '<td>' + item.Manufacturer + '</td>';
            html += '<td>' + item.Model + '</td>';
            html += '<td>' + item.Year + '</td>';
            if (item.Status == true)
            {
                html += '<td class="text-success"><strong>' + item.StatusLabel + '</strong></td>';
            }
            else
            {
                html += '<td class="text-danger"><strong>' + item.StatusLabel + '</strong></td>';
            }
            html += '<td><a href="#" onclick="return getTruckById(\'' + item.Id + '\')">Edit</a> | <a href="#" onclick="deleteTruck(\'' + item.Id + '\')">Delete</a></td>';
            html += '</tr>';
        });
        $('.tbodyTruck').html(html);
    }
}

//Add Data Function
function addTruck() {
    var res = validateTruckWhenUserPostToServer();
    if (res == false) {
        return false;
    }

    varUrl = "/TruckManagement/Add";
    var truckObj = {
        Id: "",
        Manufacturer: $('#manufacturer').val(),
        Model: $('#model').val(),
        Year: $('#year').val(),
        VIN: $('#vinNumber').val(),
        TruckNumber : $('#truckNumber').val(),
        Status: $('#truckStatus').val(),
        TruckNumberIsValid: true,
        VinNumberIsValid: true,
        ExistInDB: true                                    
    };
    varType = "POST";
    varContentType = "application/json;charset=utf-8";
    varDataType = "json";
    $.ajax({
        url: varUrl,
        data: JSON.stringify(truckObj),
        type: varType,
        contentType: varContentType,
        dataType: varDataType,
        success: truckAdded,
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function truckAdded(truck) {
    if (truck.TruckNumberIsValid == false) {
        DisplayWarningMessageForTextBox("Invalid Truck Number", "truckNumber");
    }

    if (truck.VinNumberIsValid == false) {
        DisplayWarningMessageForTextBox("Invalid VIN Number", "vinNumber");
    }

    if (truck.ExistInDB) {
        DisplayWarningMessageForTextBox("Truck number already in Database", "truckNumber");
    }
    else {
        loadTruckData();
        $('#truckModal').modal('hide');
    }
}

function getTruckById(Id) {

    varUrl = "/TruckManagement/GetTruckById/" + Id;
    varType = "GET";
    varContentType = "application/json;charset=utf-8";
    varDataType = "json";
    $.ajax({
        url: varUrl,
        type: varType,
        contentType: varContentType,
        dataType: varDataType,
        success: truckByIdReturned,
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    return false;
}

function truckByIdReturned(truck) {

    $('#Id').val(truck.Id);
    populateManufacturersDropDownList(truck);
    $('#manufacturer').val(truck.Manufacturer);
    populateModelsDropDownList(truck);
    $('#model').val(truck.Model);
    populateYearsDropDownList(truck);
    $('#year').val(truck.Year);
    $('#vinNumber').val(truck.VIN);
    $('#truckNumber').val(truck.TruckNumber);
    //Enable checkbox, so user can enable/disable truck
    document.getElementById("truckStatus").disabled = false;
    $('#truckStatus').val(truck.Status);
    $('#statusText').val(truck.StatusLabel)
    $('#truckModal').modal('show');
    $('#truckUpdate_button').show();
    $('#truckAdd_button').hide();
}

//function for updating manufacturer's record
function updateTruck() {
    var res = validateTruckWhenUserPostToServer();
    if (res == false) {
        return false;
    }

    varUrl = "/TruckManagement/Update";
    var truckObjToUpdate = {
        Id: $('#Id').val(),
        Manufacturer: $('#manufacturer').val(),
        Model: $('#model').val(),
        Year: $('#year').val(),
        VIN: $('#vinNumber').val(),
        TruckNumber: $('#truckNumber').val(),
        Status: $('#truckStatus').is(':checked'),
        TruckNumberIsValid: true,
        VinNumberIsValid: true,
        ExistInDB: true
    };
    varType = "POST";
    varContentType = "application/json;charset=utf-8";
    varDataType = "json";
    $.ajax({
        url: varUrl,
        data: JSON.stringify(truckObjToUpdate),
        type: varType,
        contentType: varContentType,
        dataType: varDataType,
        success: truckUpdated,
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function truckUpdated(truck) {
    if (truck.TruckNumberIsValid == false) {
        DisplayWarningMessageForTextBox("Invalid Truck Number", "truckNumber");
    }

    if (truck.VinNumberIsValid == false) {
        DisplayWarningMessageForTextBox("Invalid VIN Number", "vinNumber");
    }

    //if (truck.ExistInDB) {
    //    DisplayWarningMessageForTextBox("Truck number already in Database", "truckNumber");
    //}
    //else {
        loadTruckData();
        $('#truckModal').modal('hide');
        $('#Id').val("");
        //$('#modelDescription_textbox').val("");
    //}   
}

//function for deleting manufacturer's record
function deleteTruck(Id) {
    var userResponse = confirm("Are you sure you want to delete this Record?");
    varUrl = "/TruckManagement/Delete/" + Id;
    varType = "POST";
    varContentType = "application/json;charset=utf-8";
    varDataType = "json";

    if (userResponse) {
        $.ajax({
            url: varUrl,
            type: varType,
            contentType: varContentType,
            dataType: varDataType,
            success: loadTruckData,
            error: function (errormessage) {
                alert(errormessage.responseText);
            }
        });
    }
}

//Function for clearing the textboxes
function clearTruckTextBoxes() {
    var element = document.getElementById('statusText');
    var currentStatus = document.getElementById('truckStatus');
    $('#Id').val("");
    retrieveAllManufacturers();
    $('#vinNumber').val("");
    $('#vinNumberTextBox_error').text("");
    $('#vinNumber').css('border-color', 'lightgrey');
    
    if (currentStatus.checked == false) {
        currentStatus.checked = true;
        element.classList.remove("text-danger");
        element.classList.add("text-success");
        element.innerHTML = "<strong>Active</strong>";
    }
    //Disable Status checkbox, so when user inserts new truck we saved as active by default
    document.getElementById("truckStatus").disabled = true;
    $('#truckNumber').val("");
    $('#truckNumberTextBox_error').text("");
    $('#truckNumber').css('border-color', 'lightgrey');
    $('#truckUpdate_button').hide();
    $('#truckAdd_button').show();
}

//Function to retrieve manufacturers
function retrieveAllManufacturers() {
    varUrl = "/TruckManagement/GetManufacturersList";
    varType = "GET";
    varContentType = "application/json;charset=utf-8";
    varDataType = "json";
    $.ajax({
        url: varUrl,
        type: varType,
        contentType: varContentType,
        dataType: varDataType,
        success: populateManufacturersDropDownList,
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function populateManufacturersDropDownList(truck) {
    //Load manufacturers into dropdownlist
    var manufacturerList = "<select id='manufacturer' class = 'form-control' onchange='validateManufacturerDropDownList(this)'>";
    manufacturerList = manufacturerList + '<option value="">- Select Manufacturer -</option>';
    for (var i = 0; i < truck.ManufacturerDropDownList.length; i++) {
        manufacturerList = manufacturerList + '<option value=' + truck.ManufacturerDropDownList[i].Id + '>' + truck.ManufacturerDropDownList[i].Manufacturer + '</option>';
    }
    manufacturerList = manufacturerList + '</select>';
    $('#manufacturer_dropdownlist').html(manufacturerList);
}

function populateModelsDropDownList(truck) {
    //Load models into dropdownlist
    var modelList = "<select id='model' class = 'form-control' onchange='validateModelDropDownList(this)'>";
    modelList = modelList + '<option value="">- Select Model -</option>';
    for (var i = 0; i < truck.ModelDropDownList.length; i++) {
        modelList = modelList + '<option value=' + truck.ModelDropDownList[i].Id + '>' + truck.ModelDropDownList[i].Model + '</option>';
    }
    modelList = modelList + '</select>';
    $('#model_dropdownlist').html(modelList);
}

function populateYearsDropDownList(truck) {
    //Load manufacturers into dropdownlist
    var yearList = "<select id='year' class = 'form-control' onchange='validateYearDropDownList(this)'>";
    yearList = yearList + '<option value="">- Select Year -</option>';
    for (var i = 0; i < truck.YearDropDownList.length; i++) {
        yearList = yearList + '<option value=' + truck.YearDropDownList[i].Id + '>' + truck.YearDropDownList[i].Year + '</option>';
    }
    yearList = yearList + '</select>';
    $('#year_dropdownlist').html(yearList);
}

//Validation using jquery
function validateTruckWhenUserPostToServer() {
    var isValid = true;
    //Validate that user selected a manufacturer in DropDownList
    isValid = ValidateUserSelectedAValueInDropDownList("manufacturer");
    //Validate that user selected a model in DropDownList
    isValid = ValidateUserSelectedAValueInDropDownList("model");
    //Validate that user selected a year in DropDownList
    isValid = ValidateUserSelectedAValueInDropDownList("year");
    //Validate that user entered a VIN number
    isValid = ValidateUITextBoxIsNotEmpty("vinNumber");
    //Validate that user entered a truck number
    isValid = ValidateUITextBoxIsNotEmpty("truckNumber");
    
    return isValid;
}

//function validateTruck(message) {
//    $('#modelDescription_textbox').css('border-color', 'Red');
//    $('#modelDescription_error').text(message);
//    $('#modelDescription_textbox').focus();
//}

function resetTruckSearchTextBox() {
    $('#truckSearch_textbox').css('border-color', 'lightgrey');
    $('#truckSearch_error').text("");
    $('#truckSearch_textbox').val("");
}

function invalidModelNameSearchTextBox() {
    $('#truckSearch_textbox').css('border-color', 'Red');
    $('#truckSearch_error').text("Invalid Truck Number");
    $("#truckSearch_textbox").focus();
}

function changeTruckStatus(currentStatus)
{
    var element = document.getElementById('statusText');
    var statusElement = document.getElementById(currentStatus);
    //Inspect checkbox to determine whether is checked or not
    if (currentStatus.checked == true) {
        element.classList.remove("text-danger");
        element.classList.add("text-success");
        element.innerHTML = "<strong>Active</strong>";
    } else {
        element.classList.remove("text-success");
        element.classList.add("text-danger");
        element.innerHTML = "<strong>Inactive</strong>";
    }
}



