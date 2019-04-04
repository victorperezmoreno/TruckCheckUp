function DisplayWarningMessageForTextBox(message, elementId)
{
    var elementFromUI = document.getElementById(elementId);

    $(elementFromUI).css('border-color', 'Red');
    var errorElement = "#" + elementId + "TextBox_error";
    $(errorElement).text(message);
}

function ValidateUITextBoxIsNotEmpty(textBoxId) {
    var textBoxElement = document.getElementById(textBoxId);
    var textBoxValue = document.getElementById(textBoxId).value.trim();
    var errorId = "#" + textBoxId + "TextBox_error";
    if (textBoxValue == "") {
        $(textBoxElement).css('border-color', 'Red');
        var textBoxIdWithFirstLetterCapitalized = capitalizeFirstLetter(textBoxId);
        $(errorId).text(textBoxIdWithFirstLetterCapitalized + " is required");
        $("#truckAdd_button").prop('disabled', true);
        $("#truckUpdate_button").prop('disabled', true);
        return false;
    }
    else {
        $(textBoxElement).css('border-color', 'lightgrey');
        $(errorId).text("");
        $("#truckAdd_button").prop('disabled', false);
        $("#truckUpdate_button").prop('disabled', false);
        return true;
    }
}

function ValidateUserSelectedAValueInDropDownList(dropDownListId) {
    var dropDownListElement = document.getElementById(dropDownListId);
    var selectedValueInDropDownList = dropDownListElement.options[dropDownListElement.selectedIndex].value;
    var errorId = "#" + dropDownListId + "DropDownList_error";
    if (selectedValueInDropDownList == "" || selectedValueInDropDownList == "- Please Select -") {
        $(dropDownListElement).css('border-color', 'Red');
        var dropDownListIdWithFirstLetterCapitalized = capitalizeFirstLetter(dropDownListId);
        $(errorId).text(dropDownListIdWithFirstLetterCapitalized + " is required");
        $("#truckAdd_button").prop('disabled', true);
        $("#truckUpdate_button").prop('disabled', true);
        return false;
    }
    else {
        $(dropDownListElement).css('border-color', 'lightgrey');
        $(errorId).text("");
        $("#truckAdd_button").prop('disabled', false);
        $("#truckUpdate_button").prop('disabled', false);
        return true;
    }
}

function capitalizeFirstLetter(string) {
    return string.charAt(0).toUpperCase() + string.slice(1);
}

function validateTextboxContainsOnlyNumericCharacters(txtBxId) {
    var $regexname = /^([0-9]{1,5})$/;
    var txtBxElement = document.getElementById(txtBxId);
    var errorMessageId = "#" + txtBxId + "TextBox_error";
    $(txtBxElement).on('keypress keydown keyup', function () {
        if (!$(this).val().match($regexname)) {
            // there is a mismatch, hence show the error message
            $(txtBxElement).css('border-color', 'Red');
            $(errorMessageId).text("Please only numbers");
            $("#truckAdd_button").prop('disabled', true);
            $("#truckUpdate_button").prop('disabled', true);
        }
        else {
            // else, do not display message
            $(txtBxElement).css('border-color', 'lightgrey');
            $(errorMessageId).text("");
            $("#truckAdd_button").prop('disabled', false);
            $("#truckUpdate_button").prop('disabled', false);
        }
    });
}

function validateTextboxContainsOnlyAlphanumericCharacters(txtBxId) {
    var $regexname = /^([0-9a-zA-Z]{2,30})$/;
    var txtBxElement = document.getElementById(txtBxId);
    var errorMessageId = "#" + txtBxId + "TextBox_error";
    $(txtBxElement).on('keypress keydown keyup', function () {
        if (!$(this).val().match($regexname)) {
            // there is a mismatch, hence show the error message
            $(txtBxElement).css('border-color', 'Red');
            $(errorMessageId).text("Please only letters or numbers");
            $("#truckAdd_button").prop('disabled', true);
            $("#truckUpdate_button").prop('disabled', true);
        }
        else {
            // else, do not display message
            $(txtBxElement).css('border-color', 'lightgrey');
            $(errorMessageId).text("");
            $("#truckAdd_button").prop('disabled', false);
            $("#truckUpdate_button").prop('disabled', false);
        }
    });
}

