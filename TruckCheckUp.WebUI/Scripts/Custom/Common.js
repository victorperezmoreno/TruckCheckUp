function DisplayWarningMessageForTextBox(message, elementId)
{
    initializeDropDownList("model");
    let elementFromUI = document.getElementById(elementId);
    $(elementFromUI).css('border-color', 'Red');

    let errorElement = "#" + elementId + "TextBox_error";
    $(errorElement).text(message);
}

function ResetWarningMessageForTextBox(resetMessage, resetElementId) {
    let elementFromUI = document.getElementById(resetElementId);
    $(elementFromUI).css('border-color', 'lightgrey');

    let errorElement = "#" + resetElementId + "TextBox_error";
    $(errorElement).text(resetMessage);
}

function ValidateUITextBoxIsNotEmpty(textBoxId) {
    let textBoxElement = document.getElementById(textBoxId);
    let textBoxValue = document.getElementById(textBoxId).value.trim();
    let errorId = "#" + textBoxId + "TextBox_error";
    if (textBoxValue == "") {
        $(textBoxElement).css('border-color', 'Red');
        let textBoxIdWithFirstLetterCapitalized = capitalizeFirstLetter(textBoxId);
        $(errorId).text(textBoxIdWithFirstLetterCapitalized + " is required");
        //$("#truckAdd_button").prop('disabled', true);
        //$("#truckUpdate_button").prop('disabled', true);
        return false;
    }
    else {
        $(textBoxElement).css('border-color', 'lightgrey');
        $(errorId).text("");
        //$("#truckAdd_button").prop('disabled', false);
        //$("#truckUpdate_button").prop('disabled', false);
        return true;
    }
}

function ValidateUserSelectedAValueInDropDownList(dropDownListId) {
    let dropDownListElement = document.getElementById(dropDownListId);
    let selectedValueInDropDownList = dropDownListElement.options[dropDownListElement.selectedIndex].value;
    let errorId = "#" + dropDownListId + "DropDownList_error";
    if (selectedValueInDropDownList == "" || selectedValueInDropDownList == "- Please Select -")
    {
        $(dropDownListElement).css('border-color', 'Red');
        let dropDownListIdWithFirstLetterCapitalized = capitalizeFirstLetter(dropDownListId);
        $(errorId).text(dropDownListIdWithFirstLetterCapitalized + " is required");
        //$("#truckAdd_button").prop('disabled', true);
        //$("#truckUpdate_button").prop('disabled', true);
        return false;
    }
    else {
        $(dropDownListElement).css('border-color', 'lightgrey');
        $(errorId).text("");
        //$("#truckAdd_button").prop('disabled', false);
        //$("#truckUpdate_button").prop('disabled', false);
        return true;
    }
}

function capitalizeFirstLetter(string) {
    return string.charAt(0).toUpperCase() + string.slice(1);
}

function validateTextboxContainsOnlyNumericCharacters(txtBxNumericId) {
    var $regexNumericName = /^[0-9]{1,5}$/; 
    var txtBxNumericElement = document.getElementById(txtBxNumericId);
    var errorMessageIdForNumeric = "#" + txtBxNumericId + "TextBox_error";
    $(txtBxNumericElement).on('keypress keydown keyup', function () {
        if (!$(this).val().match($regexNumericName)) {
            // there is a mismatch, hence show the error message
            $(txtBxNumericElement).css('border-color', 'Red');
            $(errorMessageIdForNumeric).text("Please only numbers");
            //$("#truckAdd_button").prop('disabled', true);
            //$("#truckUpdate_button").prop('disabled', true);
        }
        else {
            // else, do not display message
            $(txtBxNumericElement).css('border-color', 'lightgrey');
            $(errorMessageIdForNumeric).text("");
            //$("#truckAdd_button").prop('disabled', false);
            //$("#truckUpdate_button").prop('disabled', false);
        }
    });
}


function validateTextboxContainsOnlyAlphanumericCharacters(txtBxAlphanumericId) {
    let $regexAlphanumericName = /^([0-9a-zA-Z]{2,30})$/;
    let txtBxAlphanumericElement = document.getElementById(txtBxAlphanumericId);
    let errorMessageIdForAlphanumeric = "#" + txtBxAlphanumericId + "TextBox_error";
    $(txtBxAlphanumericElement).on('keypress keydown keyup', function () {
        if (!$(this).val().match($regexAlphanumericName)) {
            // there is a mismatch, hence show the error message
            $(txtBxAlphanumericElement).css('border-color', 'Red');
            $(errorMessageIdForAlphanumeric).text("Please only letters or numbers");
            //$("#truckAdd_button").prop('disabled', true);
            //$("#truckUpdate_button").prop('disabled', true);
        }
        else {
            // else, do not display message
            $(txtBxAlphanumericElement).css('border-color', 'lightgrey');
            $(errorMessageIdForAlphanumeric).text("");
            //$("#truckAdd_button").prop('disabled', false);
            //$("#truckUpdate_button").prop('disabled', false);
        }
    });
}

function validateTextboxContainsOnlyCharacters(txtBxOnlyLettersId) {
    var $regexLetterName = /^[A-Za-z]+$/;
    var txtBxLettersElement = document.getElementById(txtBxOnlyLettersId);
    var errorMessageIdForLetters = "#" + txtBxOnlyLettersId + "TextBox_error";
    $(txtBxLettersElement).on('keypress keydown keyup', function () {
        if (!$(this).val().match($regexLetterName)) {
            // there is a mismatch, hence show the error message
            $(txtBxLettersElement).css('border-color', 'Red');
            $(errorMessageIdForLetters).text("Please only letters without spaces");
            $("#situationAdd_button").prop('disabled', true);
            $("#situationUpdate_button").prop('disabled', true);
        }
        else {
            // else, do not display message
            $(txtBxLettersElement).css('border-color', 'lightgrey');
            $(errorMessageIdForLetters).text("");
            $("#situationAdd_button").prop('disabled', false);
            $("#situationUpdate_button").prop('disabled', false);
        }
    });
}


function initializeDropDownList(dropDownListIdentifier) {
    let dDLIdWithFirstLetterCapitalized = capitalizeFirstLetter(dropDownListIdentifier);
    let dDlDivId = "#" + dropDownListIdentifier + "_dropdownlist";
    let initList = "<select id=" + dropDownListIdentifier + " class = 'form-control' onchange='validate" + dDLIdWithFirstLetterCapitalized + "DropDownList(this)'>";
    initList = initList + '<option value="">- Please Select -</option>';
    initList = initList + '</select>';
    $(dDlDivId).html(initList)
}

