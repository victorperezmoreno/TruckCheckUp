$(document).ready(function () {
    //Ensure that only letters are entered in situation textbox located in modal window
    validateTextboxContainsOnlyCharacters('situation');
    //Ensure that only letters are entered in description Search textbox 
    validateTextboxContainsOnlyCharacters('description');
    //Disable autocomplete for textboxes
    $("input:text,form").attr("autocomplete", "off");
    //Load Data in Table when documents is ready
    loadSituationData();
});

function searchSituation() {
    //Search only if search textbox not empty
    if (ValidateUITextBoxIsNotEmpty("situation")) {
        retrieveSituationRecord();
    }
}

function retrieveSituationRecord() {
    varUrl = "/SituationManagement/SearchSituationDescription";
    let situationSearchObj = {
        Id: "",
        Description: $('#situation').val(),
        ExistInDB: true,
        IsValid: true,
        Status : true
    };
    varType = "POST";
    varContentType = "application/json;charset=utf-8";
    varDataType = "json";
    $.ajax({
        url: varUrl,
        data: JSON.stringify(situationSearchObj),
        type: varType,
        traditional: true,
        contentType: varContentType,
        dataType: varDataType,
        success: situationRecordRetrieved,
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function situationRecordRetrieved(situationRecord) {
    if (situationRecord.IsValid == false) {
        DisplayWarningMessageForTextBox("Invalid Situation Description", "situation");;
    }
    else {
        if (situationRecord.ExistInDB == true) {
            let html = '';
            $.each([situationRecord], function (key, item) {
                html += '<tr>';
                html += '<td>' + item.Id + '</td>';
                html += '<td>' + item.Description + '</td>';
                //html += '<td>' + item.Status + '</td>';

                if (item.Status == true) {
                    html += '<td class="text-success"><strong>' + item.StatusLabel + '</strong></td>';
                }
                else {
                    html += '<td class="text-danger"><strong>' + item.StatusLabel + '</strong></td>';
                }
                html += '<td><a href="#" onclick="return getSituationById(\'' + item.Id + '\')">Edit</a> | <a href="#" onclick="deleteSituation(\'' + item.Id + '\')">Delete</a></td>';
                html += '</tr>';
            });
            $('.tbodySituation').html(html);
        }
        else {
            noRecordsFoundInSituationDatabaseMessage();
        }
        ResetWarningMessageForTextBox("", "situation");
    }
}

function noRecordsFoundInSituationDatabaseMessage() {
    let html = '';
    $('.tbodySituation').html('');
    html += '<tr>';
    html += '<td>' + 'No records found in Database' + '</td>';
    html += '<td>' + '' + '</td>';
    html += '<td>' + '' + '</td>';
    html += '<td>' + '' + '</td>';
    html += '</tr>';
    $('.tbodySituation').html(html);
}

function clearSituationSearch() {
    ResetWarningMessageForTextBox("", "situation");
    document.getElementById("situation").value = "";
    loadSituationData();
}


//Load Data function
function loadSituationData() {
    varUrl = "/SituationManagement/ListOfSituations";
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

function dataRetrievedFromDatabase(allSituations) {
    if (Object.keys(allSituations).length == 0) {
        noRecordsFoundInSituationDatabaseMessage();
    }
    else {
        let html = '';
        $.each(allSituations, function (key, item) {
            html += '<tr>';
            html += '<td>' + item.Id + '</td>';
            html += '<td>' + item.Description + '</td>';
            if (item.Status == true) {
                html += '<td class="text-success"><strong>' + item.StatusLabel + '</strong></td>';
            }
            else {
                html += '<td class="text-danger"><strong>' + item.StatusLabel + '</strong></td>';
            }
            html += '<td><a href="#" onclick="return getSituationById(\'' + item.Id + '\')">Edit</a> | <a href="#" onclick="deleteSituation(\'' + item.Id + '\')">Delete</a></td>';
            html += '</tr>';
        });
        $('.tbodySituation').html(html);
    }
}

//Add Data Function
function addSituation() {
    if (ValidateUITextBoxIsNotEmpty("description")) {
        varUrl = "/SituationManagement/Add";
        let situationObj = {
            Id: "",
            Description: $('#description').val(),
            Status: $('#descriptionStatus').val(),
            IsValid: true,
            ExistInDB: false
        };
        varType = "POST";
        varContentType = "application/json;charset=utf-8";
        varDataType = "json";
        $.ajax({
            url: varUrl,
            data: JSON.stringify(situationObj),
            type: varType,
            contentType: varContentType,
            dataType: varDataType,
            success: situationAdded,
            error: function (errormessage) {
                alert(errormessage.responseText);
            }
        });
    }
}

function situationAdded(situationObject) {
    if (situationObject.IsValid == false) {
        DisplayWarningMessageForTextBox("Invalid Situation Description", "description");
    }
    else {
        if (situationObject.ExistInDB) {
            DisplayWarningMessageForTextBox("Situation Description already in Database", "description");
        }
        else {
            loadSituationData();
            $('#situationModal').modal('hide');
        }
    }
}

function getSituationById(Id) {

    varUrl = "/SituationManagement/GetSituationById/" + Id;
    varType = "GET";
    varContentType = "application/json;charset=utf-8";
    varDataType = "json";
    $.ajax({
        url: varUrl,
        type: varType,
        contentType: varContentType,
        dataType: varDataType,
        success: situationByIdRetrieved,
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    return false;
}

function situationByIdRetrieved(situation) {

    $('#Id').val(situation.Id);
    $('#description').val(situation.Description);
    //Enable checkbox, so user can enable/disable truck
    document.getElementById("descriptionStatus").disabled = false;
    $('#descriptionStatus').val(situation.Status);
    $('#statusText').val(situation.StatusLabel)
    $('#situationModal').modal('show');
    $('#situationUpdate_button').show();
    $('#situationAdd_button').hide();
}

function updateSituation()
{
    if (ValidateUITextBoxIsNotEmpty("description"))
    {
        varUrl = "/SituationManagement/Update";
        let situationObjToUpdate = {
            Id: $('#Id').val(),
            Description: $('#description').val(),
            Status: $('#descriptionStatus').is(':checked'),
            IsValid: true,
            ExistInDB: true
        };
        varType = "POST";
        varContentType = "application/json;charset=utf-8";
        varDataType = "json";
        $.ajax({
            url: varUrl,
            data: JSON.stringify(situationObjToUpdate),
            type: varType,
            contentType: varContentType,
            dataType: varDataType,
            success: situationUpdated,
            error: function (errormessage) {
                alert(errormessage.responseText);
            }
        });
    }   
}

function situationUpdated(situation) {
    if (situation.IsValid == false) {
        DisplayWarningMessageForTextBox("Invalid Situation Description", "description");
    }
    else {
        loadSituationData();
        $('#situationModal').modal('hide');
        $('#Id').val("");
    }
}

//function for deleting manufacturer's record
function deleteSituation(Id) {
    let userResponseYes = confirm("Are you sure you want to delete this Record?");

    if (userResponseYes) {
        varUrl = "/SituationManagement/Delete/" + Id;
        varType = "POST";
        varContentType = "application/json;charset=utf-8";
        varDataType = "json";

        $.ajax({
            url: varUrl,
            type: varType,
            contentType: varContentType,
            dataType: varDataType,
            success: loadSituationData,
            error: function (errormessage) {
                alert(errormessage.responseText);
            }
        });
    }
}

//Function for clearing the textboxes
function clearSituationTextBoxes() {
    let element = document.getElementById('statusText');
    let currentStatus = document.getElementById('descriptionStatus');
    $('#Id').val("");
    $('#description').val("");
    $('#descriptionTextBox_error').text("");
    $('#description').css('border-color', 'lightgrey');
    //if status checkbox false from last operation then returned to true
    if (currentStatus.checked == false) {
        currentStatus.checked = true;
        element.classList.remove("text-danger");
        element.classList.add("text-success");
        element.innerHTML = "<strong>Active</strong>";
    }
    //Disable Status checkbox, so when user inserts new truck we saved as active by default
    document.getElementById("descriptionStatus").disabled = true;
    $('#situationUpdate_button').hide();
    $('#situationAdd_button').show();
}

function changeSituationStatus(currentStatus) {
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
