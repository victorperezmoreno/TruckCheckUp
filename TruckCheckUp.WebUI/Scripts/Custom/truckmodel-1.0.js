$(document).ready(function () {
	//Ensure that only alphanumerics are entered in model field
	validateModelTextbox();
	//Disable autocomplete for textboxes
	$("input:text,form").attr("autocomplete", "off");
	//Load Data in Table when documents is ready
	loadModelData();
});

function validateSearchTextBox() {
	var $regexname = /^([0-9a-zA-Z]{2,30})$/;
	$('#modelSearch_textbox').on('keypress keydown keyup', function () {
		if (!$(this).val().match($regexname)) {
			// there is a mismatch, hence show the error message
			$('#modelSearch_textbox').css('border-color', 'Red');
			$('#modelSearch_error').text("Please only letters or numbers");
			$("#modelSearch_button").prop('disabled', true);
		}
		else {
			// else, do not display message
			$('#modelSearch_textbox').css('border-color', 'lightgrey');
			$('#modelSearch_error').text("");
			$("#modelSearch_button").prop('disabled', false);
		}
	});
}

function validateModelTextbox() {
	var $regexname = /^([0-9a-zA-Z]{2,30})$/;
	$('#modelDescription_textbox').on('keypress keydown keyup', function () {
		if (!$(this).val().match($regexname)) {
			// there is a mismatch, hence show the error message
			$('#modelDescription_textbox').css('border-color', 'Red');
			$('#modelDescription_error').text("Please only letters or numbers");
			$("#modelAdd_button").prop('disabled', true);
			$("#modelUpdate_button").prop('disabled', true);
		}
		else {
			// else, do not display message
			$('#modelDescription_textbox').css('border-color', 'lightgrey');
			$('#modelDescription_error').text("");
			$("#modelAdd_button").prop('disabled', false);
			$("#modelUpdate_button").prop('disabled', false);
		}
	});
}

function validateManufacturerDropDownList(manufacturerDDL)
{
	if (manufacturerDDL.value == "") {
		$('#manufacturer').css('border-color', 'Red');
		$('#manufacturerDescription_error').text("Manufacturer is required");
		$("#modelAdd_button").prop('disabled', true);
		$("#modelUpdate_button").prop('disabled', true);

		isValid = false;
	}
	else {
		$('#manufacturer').css('border-color', 'lightgrey');
		$('#manufacturerDescription_error').text("");
		$("#modelAdd_button").prop('disabled', false);
		$("#modelUpdate_button").prop('disabled', false);
	}
}

function searchModel() {
	//Search only if search textbox not empty
	var txtModel = jQuery.trim($("#modelSearch_textbox").val());
	if (txtModel.length != 0) {
		retrieveModelRecord();
	}
	else {
		resetModelSearchTextBox()
	}
}

function clearModelSearch() {
	resetModelSearchTextBox();
	loadModelData();
}

function retrieveModelRecord() {
	varUrl = "/TruckModelManagement/SearchModelName";
	var modelObj = {
		Id: "",
		Description: $('#modelSearch_textbox').val(),
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
				html += '<td><a href="#" onclick="return getModelById(\'' + item.Id + '\')">Edit</a> | <a href="#" onclick="deleteModel(\'' + item.Id + '\')">Delete</a></td>';
				html += '</tr>';
			});
			$('.tbodyModel').html(html);
		}
		else {
			noRecordsFoundInDatabaseMessage();
		}
		resetModelSearchTextBox();
	}
}

function noRecordsFoundInDatabaseMessage()
{
	$('.tbodyModel').html('');
	html += '<tr>';
	html += '<td>' + 'No records found in Database' + '</td>';
	html += '<td>' + '' + '</td>';
	html += '<td>' + '' + '</td>';
	html += '<td>' + '' + '</td>';
	html += '</tr>';
	$('.tbodyModel').html(html);
}

//Load Data function
function loadModelData() {
	varUrl = "/TruckModelManagement/ListOfModels";
	varType = "GET";
	varContentType = "application/json;charset=utf-8";
	varDataType = "json";
	$.ajax({
		url: varUrl,
		type: varType,
		contentType: varContentType,
		dataType: varDataType,
		success: dataFromDatabaseRetrieved,
		error: function (errormessage) {
			alert(errormessage.responseText);
		}
	});
}

function dataFromDatabaseRetrieved(allModels) {
	if (Object.keys(allModels).length == 0)
	{
		noRecordsFoundInDatabaseMessage();
	}
	else
	{
		var html = '';
		$.each(allModels, function (key, item) {
			html += '<tr>';
			html += '<td>' + item.Id + '</td>';
			html += '<td>' + item.Description + '</td>';
			html += '<td>' + item.ManufacturerDescription + '</td>';
			html += '<td><a href="#" onclick="return getModelById(\'' + item.Id + '\')">Edit</a> | <a href="#" onclick="deleteModel(\'' + item.Id + '\')">Delete</a></td>';
			html += '</tr>';
		});
		$('.tbodyModel').html(html);
	}  
}

//Add Data Function
function addModel() {
	var res = validateModelWhenUserPostToServer();
	if (res == false) {
		return false;
	}

	varUrl = "/TruckModelManagement/Add";
	var modelObj = {
		Id: "",
		Description: $('#modelDescription_textbox').val(),
		ManufacturerId: $('#manufacturer').val(),
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
		contentType: varContentType,
		dataType: varDataType,
		success: truckModelAdded,
		error: function (errormessage) {
			alert(errormessage.responseText);
		}
	});
}

function truckModelAdded(truckModel) {
	if (truckModel.IsValid == false) {
		validateModel("Invalid Model Name");
	}
	else
		if (truckModel.ExistInDB) {
			validateModel("Model is already in Database");
		}
		else {
			loadModelData();
			$('#modelModal').modal('hide');
		}
}

function getModelById(Id) {
	$('#modelDescription_textbox').css('border-color', 'lightgrey');

	varUrl = "/TruckModelManagement/GetModelById/" + Id;
	varType = "GET";
	varContentType = "application/json;charset=utf-8";
	varDataType = "json";
	$.ajax({
		url: varUrl,
		type: varType,
		contentType: varContentType,
		dataType: varDataType,
		success: modelByIdReturned,
		error: function (errormessage) {
			alert(errormessage.responseText);
		}
	});
	return false;
}

function modelByIdReturned(model) {
	
	$('#Id').val(model.Id);
	$('#modelDescription_textbox').val(model.Description);
	populateManufacturersDropDownList(model);
	$('#manufacturer').val(model.ManufacturerId);
	$('#modelModal').modal('show');
	$('#Update_button').show();
	$('#Add_button').hide();
}

//function for updating manufacturer's record
function updateModel() {
	var res = validateModelWhenUserPostToServer();
	if (res == false) {
		return false;
	}

	varUrl = "/TruckModelManagement/Update";
	var manufacturerObj = {
		Id: $('#Id').val(),
		Description: $('#modelDescription_textbox').val(),
		ManufacturerId: $('#manufacturer').val(),
		ExistInDB: true,
		IsValid: true
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
		success: truckModelUpdated,
		error: function (errormessage) {
			alert(errormessage.responseText);
		}
	});
}

function truckModelUpdated(truckModel) {
	if (truckModel.IsValid == false) {
		validateModel("Invalid Model Name");
	}
	else
		if (truckModel.ExistInDB) {
			validateModel("Model is already in Database");
		}
		else {
			loadModelData();
			$('#modelModal').modal('hide');
			$('#Id').val("");
			$('#modelDescription_textbox').val("");
		}
}

//function for deleting manufacturer's record
function deleteModel(Id) {
	var userResponse = confirm("Are you sure you want to delete this Record?");
	varUrl = "/TruckModelManagement/Delete/" + Id;
	varType = "POST";
	varContentType = "application/json;charset=utf-8";
	varDataType = "json";

	if (userResponse) {
		$.ajax({
			url: varUrl,
			type: varType,
			contentType: varContentType,
			dataType: varDataType,
			success: loadModelData,
			error: function (errormessage) {
				alert(errormessage.responseText);
			}
		});
	}
}

//Function for clearing the textboxes
function clearModelTextBoxes() {
	$('#Id').val("");
	retrieveAllManufacturers();
	$('#modelDescription_textbox').val("");
	$('#modelDescription_error').text("");
	$('#modelUpdate_button').hide();
	$('#modelAdd_button').show();
	$('#modelDescription_textbox').css('border-color', 'lightgrey');
}

//Function to retrieve manufacturers
function retrieveAllManufacturers()
{
	varUrl = "/TruckModelManagement/GetManufacturersList";
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

function populateManufacturersDropDownList(manufacturers)
{
	//Load manufacturers into dropdownlist
	var manufacturerList = "<select id='manufacturer' class = 'form-control' onchange='validateManufacturerDropDownList(this)'>";
	manufacturerList = manufacturerList + '<option value="">- Select Manufacturer -</option>';
	for (var i = 0; i < manufacturers.ManufacturerDropDownList.length; i++) {
		manufacturerList = manufacturerList + '<option value=' + manufacturers.ManufacturerDropDownList[i].Id + '>' + manufacturers.ManufacturerDropDownList[i].Description + '</option>';
	}
	manufacturerList = manufacturerList + '</select>';
	$('#manufacturer_dropdownlist').html(manufacturerList);
}

//Validation using jquery
function validateModelWhenUserPostToServer() {
	var isValid = true;
	if ($('#modelDescription_textbox').val().trim() == "") {
		modelValidationMessage("Model is required");
		isValid = false;
	}
	else
	{
	  $('#modelDescription_textbox').css('border-color', 'lightgrey');   
	}
	//Validate that user selected a manufacturer in DropDownList
	var manufacturerDDL = document.getElementById("manufacturer");
	var manufacturerSelectedValue = manufacturerDDL.options[manufacturerDDL.selectedIndex].value;
	if (manufacturerSelectedValue == "") {
		$('#manufacturer').css('border-color', 'Red');
		$('#manufacturerDescription_error').text("Manufacturer is required");
		$('#manufacturer').focus();
		isValid = false;
	}
	else {
		$('#manufacturer').css('border-color', 'lightgrey');
		$('#manufacturerDescription_error').text("");
	}
	return isValid;
}

function validateModel(message) {
	$('#modelDescription_textbox').css('border-color', 'Red');
	$('#modelDescription_error').text(message);
	$('#modelDescription_textbox').focus();
}

function resetModelSearchTextBox() {
	$('#modelSearch_textbox').css('border-color', 'lightgrey');
	$('#modelSearch_error').text("");
	$('#modelSearch_textbox').val("");
}

function invalidModelNameSearchTextBox() {
	$('#modelSearch_textbox').css('border-color', 'Red');
	$('#modelSearch_error').text("Invalid Model Name");
	$("#modelSearch_textbox").focus();
}



