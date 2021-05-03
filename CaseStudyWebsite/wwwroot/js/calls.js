
$(function () {
	//releases the key off the keyboard then this event will fire 
	//check to see if all the fields in the form are valid 
	document.addEventListener("click", e => {
		changeModalStatus();
	}); // click eventlistener 


	// change modal status upon validation (handled by click & keyup events)
	function changeModalStatus() {
		$("#modalstatus").removeClass();//remove any existing css on div
		if ($('#theModal').is(':visible')) {
			//if its valid then it will print green (valid) or red(invalid)
			if ($("#CallModalForm").valid()) {
				$("#modalstatus").attr("class", "badge badge-success");
				$("#modalstatus").text("The data entered is valid.");
				$("#actionbutton").prop("disabled", false);
			}
			else {
				$("#modalstatus").attr("class", "badge badge-danger");
				$("#modalstatus").text("Please Enter Valid Data!");
				$("#actionbutton").prop("disabled", true);
			}
		}
	} // change modal status

	$("#CallModalForm").validate({
		 //main property that looks after each field
        //has to have a length, has to have something in it, and a validtitle
        //validtitle is a custom validator
		rules: {
			ddlProblems: { required: true },
			ddlEmployees: { required: true },
			ddlTechs: { required: true },
			textAreaNotes: { maxlength: 250, required: true }
		},
		//as these rules get broken then these messages below will fire 
		errorElement: "div",
		messages: {
			ddlProblems: {
				required: "Select problem."
			},
			ddlEmployees: {
				required: "Select employee."
			},
			ddlTechs: {
				required: "Select technician."
			},
			textAreaNotes: {
				required: "Required 1-250 characters.", maxlength: "250 characters max length."
			}
		}
	}); // CallModalForm.validate

	//when they hit a key, the keyup event will fire and parse it into an object array and run the filter method
	//go through the object and look at the lastname and match it with a regular expression 
	$("#srch").keyup(() => {
		let allData = JSON.parse(sessionStorage.getItem("allcalls"));
		let filteredData = allData.filter((call) => call.employeeName.match(new RegExp($("#srch").val(), 'i')));
		buildCallList(filteredData, false);
	}); // callsearch

		//populates the list with all the calls and their details in the database 
	const getAll = async (msg) => {
		try {

			// build call list 
			let response = await fetch(`api/call`); //gets call data
			if (response.ok) {
				let payload = await response.json();
				buildCallList(payload);
				msg === "" ?
					$("#status").text("Calls Loaded") : $("#status").text(`${msg} - Calls Loaded`);
			} else if (response.status !== 404) { // then it's probably some other client-side error
				let problemJson = await response.json();
				errorRtn(problemJson, response.status);
			} else {
				$("#status").text("no such path on server"); 
			}

			// get problem data
			response = await fetch(`api/problem`);//gets problem data for dropdown list 
			if (response.ok) {
				let problems = await response.json(); // a promise 
				sessionStorage.setItem("allProblems", JSON.stringify(problems));
			}
			else if (response.status !== 404) { // then it's probably some other client-side error
				let problemJson = await response.json();
				errorRtn(problemJson, response.status);
			}
			else {
				$("#status").text("no such path on server"); 
			}

			// get employee data
			response = await fetch(`api/employee`);//gets employee data for dropdown list 
			if (response.ok) {
				let emps = await response.json(); // a promise 
				sessionStorage.setItem("allEmployees", JSON.stringify(emps));
			}
			else if (response.status !== 404) { // then it's probably some other client-side error
				let problemJson = await response.json();
				errorRtn(problemJson, response.status);
			}
			else {
				$("#status").text("no such path on server"); 
			}

		}
		catch (error) { // catastrophic :( 
			$("#status").text(error.message);
		}
	}; // getAll

	//builds the call list 
	const buildCallList = (data, useAllData = true) => {
		$("#callList").empty();
		divHeader = $(`<div class="list-group-item text-white bg-dark row d-flex" id="status">Call Info</div>`);
		divHeader.appendTo($("#callList"));
		div = $(`<div class= "list-group-item row d-flex text-center" id="heading">
				<div class= "col-4 h4 text-dark">Date</div>
				<div class= "col-4 h4 text-dark">For</div>
				<div class= "col-4 h4 text-dark">Problem</div>
				 </div>`);
		div.appendTo($("#callList"));
		btn = $(`<button class="list-group=item row d-flex" id="0"><divclass="col-12 text-left">...Click to Add Call</div></button>`);
		btn.appendTo($("#callList"));
		useAllData ? sessionStorage.setItem("allcalls", JSON.stringify(data)) : null;
		data.map(call => {
			btn = $(`<button class="list-group-item row d-flex hover" id="${call.id}">`);
			btn.html(`<div class="col-4" id="employeetitle${call.id}">${unformatDate(call.dateOpened)}</div>
                      <div class="col-4" id="employeefname${call.id}">${call.employeeName}</div>
                      <div class="col-4" id="employeelastnam${call.id}">${call.problemDescription}</div>`
			);
			btn.appendTo($("#callList"));
		});
	}; // buildCallList

	$("#callList").click((e) => {
		if (!e) e = window.event;
		let id = e.target.parentNode.id;
		if (id === "callList" || id === "") {
			id = e.target.id;
		}
		if (id !== "status" && id !== "heading") {
			let data = JSON.parse(sessionStorage.getItem("allcalls"));
			id === "0" ? setupForAdd() : setupForUpdate(id, data);
		} else {
			return false;
		}
	}); // callList

	//if checkbox is closed then get the current date it closed and change the necessary statuses 
	$("#checkBoxClose").click(() => {
		if ($("#checkBoxClose").is(":checked")) {
			$("#divDateClosed").text(formatDate().replace("T", " "));
			sessionStorage.setItem("openStatus", false);
			sessionStorage.setItem("dateClosed", formatDate());
		}
		else {
			$("#divDateClosed").text("");
			sessionStorage.setItem("openStatus", true);
			sessionStorage.setItem("dateClosed", "");
		}
	}); // checkBoxClose

	// reformats the date in a server-friendly format to send to database 
	const formatDate = (date) => {
		let d;
		(date === undefined) ? d = new Date() : d = new Date(Date.parse(date));
		let _day = d.getDate();
		if (_day < 10) { _day = "0" + _day };
		let _month = d.getMonth() + 1;
		if (_month < 10) { _month = "0" + _month; }
		let _year = d.getFullYear();
		let _hour = d.getHours();
		if (_hour < 10) { _hour = "0" + _hour; }
		let _min = d.getMinutes();
		if (_min < 10) { _min = "0" + _min; }
		return _year + "-" + _month + "-" + _day + "T" + _hour + ":" + _min;
	}// formatDate

	// reformats date to include year-month-day for CallList
	const unformatDate = (date) => {
		let d;
		(date === undefined) ? d = new Date() : d = new Date(Date.parse(date));
		let _day = d.getDate();
		if (_day < 10) { _day = "0" + _day };
		let _month = d.getMonth() + 1;
		if (_month < 10) { _month = "0" + _month; }
		let _year = d.getFullYear();
		return _year + "-" + _month + "-" + _day;
	}// unformatDate 

	// reformats data to include year-month-time-day for Modal
	const formatDateWithTime = (date) => {
		let d;
		(date === undefined) ? d = new Date() : d = new Date(Date.parse(date));
		let _day = d.getDate();
		if (_day < 10) { _day = "0" + _day };
		let _month = d.getMonth() + 1;
		if (_month < 10) { _month = "0" + _month; }
		let _year = d.getFullYear();
		let _hour = d.getHours();
		if (_hour < 10) { _hour = "0" + _hour; }
		let _min = d.getMinutes();
		if (_min < 10) { _min = "0" + _min; }
		return _year + "-" + _month + "-" + _day + " " + _hour + ":" + _min;
	}// formatDateWithTime

	//clear the modal fields everytime a user opens a modal 
	const clearModalFields = () => {
		$("#CallModalForm").validate().resetForm();
		loadProblemDDL(-1);
		loadEmployeeDDL(-1);
		loadTechDDL(-1);
		$("#divDateOpened").html("");
		$("#divDateClosed").html("");
		$("#textAreaNotes").val("");
		$("#checkBoxClose").prop('checked', false);
		sessionStorage.removeItem("id");
		sessionStorage.removeItem("employeeId");
		sessionStorage.removeItem("problemId");
		sessionStorage.removeItem("techId");
		sessionStorage.removeItem("timer");
		sessionStorage.removeItem("dateOpened");
		sessionStorage.removeItem("dateClosed");
		sessionStorage.removeItem("openStatus");
		sessionStorage.removeItem("notes");
		sessionStorage.removeItem("timer");
		$("#actionbutton").prop("disabled", false);
		$("#actionbutton").prop("disabled", false);
		$("#modalstatus").removeAttr("class", "text-success");
	}; // clearModalFields

	//checks to see if ticket is opened and if not then disable all the fields since its closed 
	const checkOpenStatus = (openStatus) => {
		if (openStatus) {
			$("#ddlProblems").attr('disabled', false);
			$("#ddlEmployees").attr('disabled', false);
			$("#ddlTechs").attr('disabled', false);
			$("#checkBoxClose").attr('disabled', false);
			$("#textAreaNotes").attr('disabled', false);
			$("#modalstatus").show();
			$("#actionbutton").show();
		}
		else {
			$("#ddlProblems").attr('disabled', true);
			$("#ddlEmployees").attr('disabled', true);
			$("#ddlTechs").attr('disabled', true);
			$("#checkBoxClose").attr('disabled', true);
			$("#textAreaNotes").attr('disabled', true);
			$("#modalstatus").hide();
			$("#actionbutton").hide();
		}
	}

	const setupForUpdate = (id, data) => {
		$("#actionbutton").val("update");
		$(".modal-title").html("Update and/or Delete Call");
		$("#deletebutton").show();
		$("#labelDateClosed").show();
		$("#divDateClosed").show();
		$("#labelCloseCall").show();
		$("#checkBoxClose").show();

		clearModalFields();

		data.map(call => {
			if (call.id === parseInt(id)) {

				// enable/disable fields based on call open status
				checkOpenStatus(call.openStatus);

				// only tick checkbox if call is closed
				if (call.openStatus) {
					$("#checkBoxClose").prop('checked', false);
				}
				else {
					$("#checkBoxClose").prop('checked', true);
				}

				loadProblemDDL(call.problemDescription.toString());
				loadEmployeeDDL(call.employeeName.toString());
				loadTechDDL(call.techName.toString());

				$('#ddlProblems').val(call.problemId);
				$('#ddlEmployees').val(call.employeeId);
				$('#ddlTechs').val(call.techId);

				$('#textAreaNotes').val(call.notes);

				$("#divDateOpened").html(`${formatDateWithTime(call.dateOpened)}`);

				sessionStorage.setItem("id", call.id);
				sessionStorage.setItem("dateOpened", formatDate(call.dateOpened));

				if (call.dateClosed !== null) {
					sessionStorage.setItem("dateClosed", formatDate(call.dateClosed));
					$("#divDateClosed").html(`${formatDateWithTime(call.dateClosed)}`);
				}

				sessionStorage.setItem("openStatus", call.openStatus);

				sessionStorage.setItem("timer", call.timer);

				$("#modalstatus").text("Update information.");
				$("#theModal").modal("toggle");
			}
		});
	}; // setupForUpdate

	const update = async () => {
		try {
			call = new Object();

			call.id = parseInt(sessionStorage.getItem("id"));
			call.employeeId = parseInt($("#ddlEmployees").val());
			call.problemId = parseInt($("#ddlProblems").val());
			call.techId = parseInt($("#ddlTechs").val());

			if (sessionStorage.getItem("openStatus") === "true") {
				call.openStatus = true;
			}
			else {
				call.openStatus = false;
			}

			call.dateOpened = sessionStorage.getItem("dateOpened");

			sessionStorage.getItem("dateClosed")
				? call.dateClosed = sessionStorage.getItem("dateClosed")
				: call.dateClosed = null;

			call.notes = $("#textAreaNotes").val();
			call.timer = sessionStorage.getItem("timer");

			let response = await fetch("/api/call", {
				method: "PUT",
				headers: { "Content-Type": "application/json; charset=utf-8" },
				body: JSON.stringify(call)
			});
			if (response.ok) {
				let data = await response.json();
				getAll(data.msg);
			} else if (response.status !== 404) {
				let problemJson = await response.json();
				errorRtn(problemJson, repsonse.status);
			} else {
				$("#status").text("no such path on server");
			}
		} catch (error) {
			$("#status").text(error.message);
		}
		$("#theModal").modal("toggle");
	} // update 


	const setupForAdd = () => {
		clearModalFields();
		checkOpenStatus(true);
		$("#actionbutton").val("add");
		let d = new Date();
		$("#divDateOpened").html(`${formatDateWithTime(d)}`);
		sessionStorage.setItem("dateOpened", formatDate(d));
		$("#labelDateClosed").hide();
		$("#divDateClosed").hide();
		$("#labelCloseCall").hide();
		$("#checkBoxClose").hide();
		$(".modal-title").html("Add Call");
		$("#theModal").modal("toggle");
		$("#modalstatus").text("Please enter the call information.");
		$("#deletebutton").hide();
	}; // setupForAdd

	//add a new call
	const add = async () => {
		try {
			call = new Object();
			call.id = -1;
			call.employeeId = parseInt($("#ddlEmployees").val());
			call.problemId = parseInt($("#ddlProblems").val());
			call.techId = parseInt($("#ddlTechs").val());
			call.dateOpened = sessionStorage.dateOpened;
			call.dateClosed = null;
			call.openStatus = true;
			call.notes = $("#textAreaNotes").val();
			call.timer = null;

			let response = await fetch("/api/call", {
				method: "POST",
				headers: {
					"Content-Type": "application/json; charset=utf-8"
				},
				body: JSON.stringify(call)
			});
			if (response.ok) {
				let data = await response.json();
				getAll(data.msg);
			} else if (response.status !== 404) {
				let problemJson = await response.json();
				errorRtn(problemJson, response.status);
			} else {
				$("#status").text("no such path on server");
			}
		} catch (error) {
			$("#status").text(error.message);
		}
		$("#theModal").modal("toggle");
	}; // add

	$("#actionbutton").click(() => {
		$("#actionbutton").val() === "update" ? update() : add();
	});

	//delete a call! 
	const _delete = async () => {
		try {
			let response = await fetch(`/api/call/${sessionStorage.getItem('id')}`, {
				method: 'DELETE',
				headers: { 'Content-Type': 'application/json; charset=utf-8' }
			});
			if (response.ok) // or check for response.status
			{
				let data = await response.json();
				getAll(data.msg);
			} else {
				$('#status').text(`Status - ${response.status}, Problem on delete server side, see server console`);
			} // else 
			$('#theModal').modal('toggle');
		} catch (error) {
			$('#status').text(error.message);
		}
	}; // delete 

	//event handler for the confirmation click - passes a JSON object to the click event and the object of the user's choice 
	$('[data-toggle=confirmation]').confirmation({ rootSelector: '[data-toggle=confirmation]' });
	//if yes was chosen, if no then ignored 
	$('#deletebutton').click(() => _delete());



	getAll("");
}); // ready method

const loadProblemDDL = (problem) => {
	html = '';
	$('#ddlProblems').empty();
	let allProblems = JSON.parse(sessionStorage.getItem("allProblems"));
	allProblems.map(problem => html += `<option value="${problem.id}">${problem.description}</option>}`);
	$("#ddlProblems").append(html);
	$("#ddlProblems").val(problem);
}; // loadProblemDDL

const loadEmployeeDDL = (emp) => {
	html = '';
	$('#ddlEmployees').empty();
	let allEmployees = JSON.parse(sessionStorage.getItem("allEmployees"));
	allEmployees.map(emp => html += `<option value="${emp.id}">${emp.lastName}</option>}`);
	$("#ddlEmployees").append(html);
	$("#ddlEmployees").val(emp);
}; // loadEmployeeDDL

const loadTechDDL = (tech) => {
	html = '';
	$('#ddlTechs').empty();
	let allTechs = JSON.parse(sessionStorage.getItem("allEmployees"));
	// only add employees to the list if they are also a technician
	allTechs.map(tech => { if (tech.isTech) { html += `<option value="${tech.id}">${tech.lastName}</option>}` } });
	$("#ddlTechs").append(html);
	$("#ddlTechs").val(tech);
}; // loadTechDDL

const errorRtn = (problemJson, status) => {
	if (status > 499) {
		$("#status").text("Problem server side, see debug console");
	} else {
		let keys = Object.keys(problemJson.errors)
		problem = {
			status: status,
			statusText: problemJson.errors[keys[0]][0],
		};
		$("#status").text("Problem client side, see browser console");
		console.log(problem);
	}
} // errorRtn