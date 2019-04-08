$(document).ready(function () {
    
    //Ensure that only numbers are entered in search truck textbox 
    validateTextboxContainsOnlyNumericCharacters('truck');
    //Ensure that only numbers are entered in truck number textbox 
    validateTextboxContainsOnlyNumericCharacters('truckNumber');
    //Ensure that only letters and numbers are entered in VIN textbox
    validateTextboxContainsOnlyAlphanumericCharacters('vinNumber');
    //Disable autocomplete for textboxes
    $("input:text,form").attr("autocomplete", "off");
    //Load Data in Table when documents is ready
    loadTruckData();
});

function validateManufacturerDropDownList(manufacturerSelected) {
    //Ensures a valid value is selected
    let validValueSelectedByUser = ValidateUserSelectedAValueInDropDownList("manufacturer");

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
    if (ValidateUITextBoxIsNotEmpty("truck"))
    {
        retrieveTruckRecord();
    }
}

function clearTruckSearch() {
    ResetWarningMessageForTextBox("", "truck");
    document.getElementById("truck").value = "";
    loadTruckData();
}

function retrieveTruckRecord() {
    varUrl = "/TruckManagement/SearchTruckNumber";
    let truckSearchObj = {
        Id: "",
        TruckNumber: $('#truck').val(),
        ExistInDB: true,
        TruckNumberIsValid: true
    };
    varType = "POST";
    varContentType = "application/json;charset=utf-8";
    varDataType = "json";
    $.ajax({
        url: varUrl,
        data: JSON.stringify(truckSearchObj),
        type: varType,
        traditional: true,
        contentType: varContentType,
        dataType: varDataType,
        success: truckRecordRetrieved,
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function truckRecordRetrieved(truckRecord) {
    if (truckRecord.TruckNumberIsValid == false) {
        DisplayWarningMessageForTextBox("Invalid Truck Number", "truck");;
    }
    else {
        if (truckRecord.ExistInDB == true) {
            let html = '';
            $.each([truckRecord], function (key, item) {
                html += '<tr>';
                html += '<td>' + item.Id + '</td>';
                html += '<td>' + item.TruckNumber + '</td>';
                html += '<td>' + item.VIN + '</td>';
                html += '<td>' + item.Manufacturer + '</td>';
                html += '<td>' + item.Model + '</td>';
                html += '<td>' + item.Year + '</td>';
                if (item.Status == true) {
                    html += '<td class="text-success"><strong>' + item.StatusLabel + '</strong></td>';
                }
                else {
                    html += '<td class="text-danger"><strong>' + item.StatusLabel + '</strong></td>';
                }
                html += '<td><a href="#" onclick="return getTruckById(\'' + item.Id + '\')">Edit</a> | <a href="#" onclick="deleteTruck(\'' + item.Id + '\')">Delete</a></td>';
                html += '</tr>';
            });
            $('.tbodyTruck').html(html);
        }
        else {
            noRecordsFoundInTruckDatabaseMessage();
        }
        ResetWarningMessageForTextBox("", "truck");
    }
}

function noRecordsFoundInTruckDatabaseMessage() {
    let html = '';
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
        let html = '';
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
    let res = validateTruckWhenUserPostToServer();
    if (res == false) {
        return false;
    }

    varUrl = "/TruckManagement/Add";
    let truckObj = {
        Id: "",
        Manufacturer: $('#manufacturer').val(),
        Model: $('#model').val(),
        Year: $('#year').val(),
        VIN: $('#vinNumber').val(),
        TruckNumber : $('#truckNumber').val(),
        Status: $('#truckStatus').val(),
        TruckNumberIsValid: true,
        VinNumberIsValid: true,
        ExistInDB: false                                    
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
    if (truck.VinNumberIsValid == false) {
        DisplayWarningMessageForTextBox("Invalid VIN Number", "vinNumber");
    }
    if (truck.TruckNumberIsValid == false) {
        DisplayWarningMessageForTextBox("Invalid Truck Number", "truckNumber");
    }
    else {
        if (truck.ExistInDB) {
            DisplayWarningMessageForTextBox("Truck number already in Database", "truckNumber");
        }
        else {
            loadTruckData();
            $('#truckModal').modal('hide');
        }
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
    let res = validateTruckWhenUserPostToServer();
    if (res == false) {
        return false;
    }

    varUrl = "/TruckManagement/Update";
    let truckObjToUpdate = {
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
    if (truck.VinNumberIsValid == false) {
        DisplayWarningMessageForTextBox("Invalid VIN Number", "vinNumber");
    }

    if (truck.TruckNumberIsValid == false) {
        DisplayWarningMessageForTextBox("Invalid Truck Number", "truckNumber");
    }
    else
    {
        loadTruckData();
        $('#truckModal').modal('hide');
        $('#Id').val("");
    }
}

//function for deleting manufacturer's record
function deleteTruck(Id) {
    let userResponse = confirm("Are you sure you want to delete this Record?");
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
    let element = document.getElementById('statusText');
    let currentStatus = document.getElementById('truckStatus');
    $('#Id').val("");
    retrieveAllManufacturers();
    initializeDropDownList("model");
    initializeDropDownList("year");
    $('#vinNumber').val("");
    $('#vinNumberTextBox_error').text("");
    $('#vinNumber').css('border-color', 'lightgrey');
    //if status checkbox false from last operation then returned to true
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
    let manufacturerList = "<select id='manufacturer' class = 'form-control' onchange='validateManufacturerDropDownList(this)'>";
    manufacturerList = manufacturerList + '<option value="">- Select Manufacturer -</option>';
    for (let i = 0; i < truck.ManufacturerDropDownList.length; i++) {
        manufacturerList = manufacturerList + '<option value=' + truck.ManufacturerDropDownList[i].Id + '>' + truck.ManufacturerDropDownList[i].Manufacturer + '</option>';
    }
    manufacturerList = manufacturerList + '</select>';
    $('#manufacturer_dropdownlist').html(manufacturerList);
}

function populateModelsDropDownList(truck) {
    //Load models into dropdownlist
    let modelList = "<select id='model' class = 'form-control' onchange='validateModelDropDownList(this)'>";
    modelList = modelList + '<option value="">- Select Model -</option>';
    for (let i = 0; i < truck.ModelDropDownList.length; i++) {
        modelList = modelList + '<option value=' + truck.ModelDropDownList[i].Id + '>' + truck.ModelDropDownList[i].Model + '</option>';
    }
    modelList = modelList + '</select>';
    $('#model_dropdownlist').html(modelList);
}

function populateYearsDropDownList(truck) {
    //Load manufacturers into dropdownlist
    let yearList = "<select id='year' class = 'form-control' onchange='validateYearDropDownList(this)'>";
    yearList = yearList + '<option value="">- Select Year -</option>';
    for (let i = 0; i < truck.YearDropDownList.length; i++) {
        yearList = yearList + '<option value=' + truck.YearDropDownList[i].Id + '>' + truck.YearDropDownList[i].Year + '</option>';
    }
    yearList = yearList + '</select>';
    $('#year_dropdownlist').html(yearList);
}

//Validation using jquery
function validateTruckWhenUserPostToServer() {
    let truckIsValid;
    //Validate that user selected a manufacturer in DropDownList
    let manufacturerIsValid = ValidateUserSelectedAValueInDropDownList("manufacturer");
    //Validate that user selected a model in DropDownList
    let modelIsValid = ValidateUserSelectedAValueInDropDownList("model");
    //Validate that user selected a year in DropDownList
    let yearIsValid = ValidateUserSelectedAValueInDropDownList("year");
    //Validate that user entered a VIN number
    let vinNumberIsValid = ValidateUITextBoxIsNotEmpty("vinNumber");
    //Validate that user entered a truck number
    let truckNumberIsValid = ValidateUITextBoxIsNotEmpty("truckNumber");
    /*Validates that all textboxes have values and a values is selected in DropDownLists*/
    if (manufacturerIsValid && modelIsValid &&  yearIsValid && vinNumberIsValid && truckNumberIsValid)
    {
        truckIsValid = true;
    }
    else
    {
        truckIsValid = false;
    }

    return truckIsValid;
}

function changeTruckStatus(currentStatus)
{
    let element = document.getElementById('statusText');
    let statusElement = document.getElementById(currentStatus);
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



