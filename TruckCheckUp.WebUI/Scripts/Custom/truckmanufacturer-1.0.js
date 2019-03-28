//Load Data in Table when documents is ready
$(document).ready(function () {
    //validateSearchTextBox();
    validateManufacturerTextbox();
   
    loadData();
});

function validateSearchTextBox()
{
    var $regexname = /^([0-9a-zA-Z]{2,30})$/;
    $('#manufacturerSearch_textbox').on('keypress keydown keyup', function () {
        if (!$(this).val().match($regexname)) {
            // there is a mismatch, hence show the error message
            $('#manufacturerSearch_textbox').css('border-color', 'Red');
            $('#manufacturerSearch_error').text("Please only letters or numbers");
            $("#manufacturerSearch_button").prop('disabled', true);
        }
        else {
            // else, do not display message
            $('#manufacturerSearch_textbox').css('border-color', 'lightgrey');
            $('#manufacturerSearch_error').text("");
            $("#manufacturerSearch_button").prop('disabled', false);
        }
    });
}

function validateManufacturerTextbox()
{
    var $regexname = /^([0-9a-zA-Z]{2,30})$/;
    $('#manufacturerDescription_textbox').on('keypress keydown keyup', function () {
        if (!$(this).val().match($regexname)) {
            // there is a mismatch, hence show the error message
            $('#manufacturerDescription_textbox').css('border-color', 'Red');
            $('#manufacturerDescription_error').text("Please only letters or numbers");
            $("#manufacturerAdd_button").prop('disabled', true);
            $("#manufacturerUpdate_button").prop('disabled', true);
        }
        else {
            // else, do not display message
            $('#manufacturerDescription_textbox').css('border-color', 'lightgrey');
            $('#manufacturerDescription_error').text("");
            $("#manufacturerAdd_button").prop('disabled', false);
            $("#manufacturerUpdate_button").prop('disabled', false);
        }
    });
}

function searchManufacturer()
{
    //Search only if search textbox not empty
    var txtManufacturer = jQuery.trim($("#manufacturerSearch_textbox").val());
    if (txtManufacturer.length != 0)
    {
      retrieveManufacturerRecord();
    }
    else
    {
      resetManufacturerSearchTextBoxt()
    }
}

function clearManufacturerSearch() {
    resetManufacturerSearchTextBoxt();
    loadData();
}

function retrieveManufacturerRecord() {
    varUrl = "/TruckManufacturerManagement/SearchManufacturerName";
    var manufacturerObj = {
        Id : "",
        ManufacturerDescription: $('#manufacturerSearch_textbox').val(),
        ManufacturerExistInDB : true,
        ManufacturerIsValid: true
        //Country: $('#Country').val()
    };   
    varType = "POST";
    varContentType = "application/json;charset=utf-8";
    varDataType = "json";
    $.ajax({
        url: varUrl,
        data: JSON.stringify(manufacturerObj),
        type: varType,
        traditional : true,
        contentType: varContentType,
        dataType: varDataType,
        success: successRetrievingManufacturerRecord,
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function successRetrievingManufacturerRecord(manufacturerRecord)
{
    if (manufacturerRecord.ManufacturerIsValid == false)
    {
        invalidManufacturerNameSearchTextBox();
    }
    else {
        if (manufacturerRecord.ManufacturerExistInDB == true) {
            var html = '';
            $.each([manufacturerRecord], function (key, item) {
                html += '<tr>';
                html += '<td>' + item.Id + '</td>';
                html += '<td>' + item.ManufacturerDescription + '</td>';
                //html += '<td>' + item.Age + '</td>';
                //html += '<td>' + item.State + '</td>';
                //html += '<td>' + item.Country + '</td>';
                html += '<td><a href="#" onclick="return getManufacturerbyId(\'' + item.Id + '\')">Edit</a> | <a href="#" onclick="deleteManufacturer(\'' + item.Id + '\')">Delete</a></td>';
                html += '</tr>';
            });
            $('.tbodyManufacturer').html(html);
        }
        else {
            $('.tbodyManufacturer').html('');
            html += '<tr>';
            html += '<td>' + 'No records match search criterion' + '</td>';
            html += '<td>' + '' + '</td>';
            html += '<td>' + '' + '</td>';
            html += '</tr>';
            $('.tbodyManufacturer').html(html);
        }
        resetManufacturerSearchTextBoxt();
    }  
}

//Load Data function
function loadData() {
    varUrl = "/TruckManufacturerManagement/ListOfManufacturers";
    varType = "GET";
    varContentType = "application/json;charset=utf-8";
    varDataType = "json";
    $.ajax({
        url: varUrl,
        type: varType,
        contentType: varContentType,
        dataType: varDataType,
        success: successRetrievingAllManufacturers,
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function successRetrievingAllManufacturers(allManufacturers)
{
    var html = '';
    $.each(allManufacturers, function (key, item) {
        html += '<tr>';
        html += '<td>' + item.Id + '</td>';
        html += '<td>' + item.ManufacturerDescription + '</td>';
        //html += '<td>' + item.Age + '</td>';
        //html += '<td>' + item.State + '</td>';
        //html += '<td>' + item.Country + '</td>';
        html += '<td><a href="#" onclick="return getManufacturerbyId(\'' + item.Id + '\')">Edit</a> | <a href="#" onclick="deleteManufacturer(\'' + item.Id + '\')">Delete</a></td>';
        html += '</tr>';
    });
    $('.tbodyManufacturer').html(html);
}

//Add Data Function
function addManufacturer() {
    var res = validate();
    if (res == false) {
        return false;
    }
   
    varUrl = "/TruckManufacturerManagement/Add";
    var manufacturerObj = {
        Id : "",
        ManufacturerDescription: $('#manufacturerDescription_textbox').val(),
        ManufacturerExistInDB : true,
        ManufacturerIsValid: true
        //Country: $('#Country').val()
        };
    varType = "POST";
    varContentType = "application/json;charset=utf-8";
    varDataType = "json";   
    $.ajax({
        url: varUrl,
        data: JSON.stringify(manufacturerObj),
        type: varType,
        contentType: varContentType,
        dataType: varDataType,
        success: successAttemptToAddManufacturer,
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function successAttemptToAddManufacturer(truckManufacturer)
{
    if (truckManufacturer.ManufacturerIsValid == false)
    {
        invalidManufacturerName();
    }
    else
        if (truckManufacturer.ManufacturerExistInDB) {
            manufacturerAlreadyInDB();
        }
        else {
            loadData();
            $('#myModal').modal('hide');
        }
}

function getManufacturerbyId(Id) {
    $('#manufacturerDescription_textbox').css('border-color', 'lightgrey');
    //$('#Age').css('border-color', 'lightgrey');
    //$('#State').css('border-color', 'lightgrey');
    //$('#Country').css('border-color', 'lightgrey');
    varUrl = "/TruckManufacturerManagement/GetManufacturerbyId/" + Id;
    varType = "GET";
    varContentType = "application/json;charset=utf-8";
    varDataType = "json";   
    $.ajax({
        url: varUrl,
        type: varType,
        contentType: varContentType,
        dataType: varDataType,
        success: successReturningManufacturerById,
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    return false;
}

function successReturningManufacturerById(manufacturer)
{
    $('#Id').val(manufacturer.Id);
    $('#manufacturerDescription_textbox').val(manufacturer.ManufacturerDescription);
    //$('#Age').val(result.Age);
    //$('#State').val(result.State);
    //$('#Country').val(result.Country);
    $('#myModal').modal('show');
    $('#manufacturerUpdate_button').show();
    $('#manufacturerAdd_button').hide();
}

//function for updating manufacturer's record
function updateManufacturer() {
    var res = validate();
    if (res == false) {
        return false;
    }

    varUrl = "/TruckManufacturerManagement/Update";
    var manufacturerObj = {
        Id: $('#Id').val(),
        ManufacturerDescription: $('#manufacturerDescription_textbox').val(),
        ManufacturerExistInDB: true,
        ManufacturerIsValid: true
        //Age: $('#Age').val(),
        //State: $('#State').val(),
        //Country: $('#Country').val(),
    };
    varType = "POST";
    varContentType = "application/json;charset=utf-8";
    varDataType = "json";
    $.ajax({
        url: varUrl,
        data: JSON.stringify(manufacturerObj),
        type: varType,
        contentType: varContentType,
        dataType: varDataType,
        success: successAttemptToUpdateManufacturer,
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function successAttemptToUpdateManufacturer(truckManufacturer)
{
    if (truckManufacturer.ManufacturerIsValid == false) {
        invalidManufacturerName();
    }
    else
        if (truckManufacturer.ManufacturerExistInDB) {
            manufacturerAlreadyInDB();
        }
        else {
            loadData();
            $('#myModal').modal('hide');
            $('#Id').val("");
            $('#manufacturerDescription_textbox').val("");
            //$('#Age').val("");
            //$('#State').val("");
            //$('#Country').val("");
        }
}

//function for deleting manufacturer's record
function deleteManufacturer(Id) {
    var userResponse = confirm("Are you sure you want to delete this Record?");
    varUrl = "/TruckManufacturerManagement/Delete/" + Id;
    varType = "POST";
    varContentType = "application/json;charset=utf-8";
    varDataType = "json";

    if (userResponse) {
        $.ajax({
            url: varUrl,
            type: varType,
            contentType: varContentType,
            dataType: varDataType,
            success: function (result) {
                loadData();
            },
            error: function (errormessage) {
                alert(errormessage.responseText);
            }
        });
    }
}

//Function for clearing the textboxes
function clearTextBoxes() {
    $('#Id').val("");
    $('#manufacturerDescription_textbox').val("");
    //$('#Age').val("");
    //$('#State').val("");
    //$('#Country').val("");
    $('#manufacturerDescription_error').text("");
    $('#manufacturerUpdate_button').hide();
    $('#manufacturerAdd_button').show();
    $('#manufacturerDescription_textbox').css('border-color', 'lightgrey');
    //$('#Age').css('border-color', 'lightgrey');
    //$('#State').css('border-color', 'lightgrey');
    //$('#Country').css('border-color', 'lightgrey');
}

//Validation using jquery
function validate() {
    var isValid = true;
    if ($('#manufacturerDescription_textbox').val().trim() == "") {
        $('#manufacturerDescription_textbox').css('border-color', 'Red');
        $('#manufacturer_error').text("Manufacturer is required");
        $('#manufacturerDescription_textbox').focus();
        isValid = false;
    }
    else {
        $('#manufacturerDescription_textbox').css('border-color', 'lightgrey');
    }
    //if ($('#Age').val().trim() == "") {
    //    $('#Age').css('border-color', 'Red');
    //    isValid = false;
    //}
    //else {
    //    $('#Age').css('border-color', 'lightgrey');
    //}
    //if ($('#State').val().trim() == "") {
    //    $('#State').css('border-color', 'Red');
    //    isValid = false;
    //}
    //else {
    //    $('#State').css('border-color', 'lightgrey');
    //}
    //if ($('#Country').val().trim() == "") {
    //    $('#Country').css('border-color', 'Red');
    //    isValid = false;
    //}
    //else {
    //    $('#Country').css('border-color', 'lightgrey');
    //}
    return isValid;
}

function invalidManufacturerName()
{
    $('#manufacturerDescription_textbox').css('border-color', 'Red');
    $('#manufacturerDescription_error').text("Invalid Manufacturer Name");
    $('#manufacturerDescription_textbox').focus();
}

function resetManufacturerSearchTextBoxt() {
    $('#manufacturerSearch_textbox').css('border-color', 'lightgrey');
    $('#manufacturerSearch_error').text("");
    $('#manufacturerSearch_textbox').val("");
}

function invalidManufacturerNameSearchTextBox() {
    $('#manufacturerSearch_textbox').css('border-color', 'Red');
    $('#manufacturerSearch_error').text("Invalid Manufacturer Name");
    $("#manufacturerSearch_textbox").focus();
}

function manufacturerAlreadyInDB() {
    $('#manufacturerDescription_textbox').css('border-color', 'Red');
    $('#manufacturerDescription_error').text("Manufacturer is already in Database");
    $('#manufacturerDescription_textbox').focus();
}