$(function () { //introduces the jQuery ready method - only runs once 

	//releases the key off the keyboard then this event will fire 
	//check to see if all the fields in the form are valid 
	document.addEventListener("keyup", e => {
		$("#modalstatus").removeClass();//remove any existing css on div
		if ($("#EmployeeModalForm").valid()) {
			//if its valid then it will print green (valid) or red(invalid)
			$("#modalstatus").attr("class", "badge badge-success");
			$("#modalstatus").text("data entered is valid");
			$("#actionbutton").prop('disabled', false);
		}
		else {
			$("#modalstatus").attr("class", "badge badge-danger");
			$("#modalstatus").text("fix errors");
			$("#actionbutton").prop('disabled', true);
		}
	});

	$("#EmployeeModalForm").validate({
		//main property that looks after each field
		//has to have a length, has to have something in it, and a validtitle
		//validtitle is a custom validator
		rules: {
			TextBoxTitle: { maxlength: 4, required: true, validTitle: true },
			TextBoxFirstname: { maxlength: 25, required: true },
			TextBoxLastname: { maxlength: 25, required: true },
			TextBoxEmail: { maxlength: 40, required: true, email: true },
			TextBoxPhone: { maxlength: 15, required: true },
			ddlDepartments: { required: true }

		},
		//as these rules get broken then these messages below will fire 
		errorElement: "div",
		messages: {
			TextBoxTitle: {
				required: "required 1-4 chars.", maxlength: "required 1-4 chars.", validTitle: "Mr. Ms. Mrs. or Dr."
			},
			TextBoxFirstname: {
				required: "required 1-25 chars.", maxlength: "required 1-25 chars."
			},
			TextBoxLastname: {
				required: "required 1-25 chars.", maxlength: "required 1-25 chars."
			},
			TextBoxPhone: {
				required: "required 1-15 chars.", maxlength: "required 1-15 chars."
			},
			TextBoxEmail: {
				required: "required 1-40 chars.", maxlength: "required 1-40 chars.", email: " needs valid email format"
			},
			ddlDepartments: {
				required: "Select department."
			}
		}
	});//employeemodalform .validate 

	//defining a custom validator
	$.validator.addMethod("validTitle", (value) => {// custom rules
		return (value === "Mr." || value === "Ms." || value === "Mrs." || value === "Dr.");
	}, "");//.validator.addmethod


	//populates the list with all the employees and their details in the database 
	const getAll = async (msg) => {
		try {
			$("#employeeList").text("Finding employee Information...");
			let response = await fetch(`api/employee`); //try to get the employee data, asynchronous call 
			if (response.ok) {
				let payload = await response.json(); //this returns a promise, so we await it 
				buildEmployeeList(payload);
				msg === "" ? //are we appending to an existing message 
					$("#status").text("Employees Loaded") : $("#status").text(`${msg} - Employees Loaded`);
			} else if (response.status !== 404) {
				let problemJson = await response.json();
				errorRtn(problemJson, response.status);
			} else {
				$("#status").text("no such path on server");
			}
			response = await fetch(`api/department`); //try to get the list of departments  using the fetch web request 
			if (response.ok) {
				let divs = await response.json(); //this returns a promise, so we await it 
				sessionStorage.setItem("alldepartments", JSON.stringify(divs));
			} else if (reponse.status !== 404) { //probably some other client side error
				let problemJson = await response.json();
				errorRtn(problemJson, response.status);
			} else { //else 404 not found
				$("#status").text("no such path on server");
			}
		} catch (error) { //catatrosphic 
			$("#status").text(error.message);
		}
	};//get all 

	//when they hit a key, the keyup event will fire and parse it into an object array and run the filter method
	//go through the object and look at the lastname and match it with a regular expression 
	$("#srch").keyup(() => {
		let alldata = JSON.parse(sessionStorage.getItem("allemployees"));
		//optional parameter, test for case sensitivity: doesnt care if its upper or lower case 
		let filtereddata = alldata.filter((emp) => emp.lastName.match(new RegExp($("#srch").val(), 'i')));
		buildEmployeeList(filtereddata, false);
	}); // search keyup 


	const buildEmployeeList = (data, usealldata = true) => {
		$("#employeeList").empty();
		
		//add in the headers 
		div = $(`<div class="list-group-item text-white bg-dark row d-flex" id="status">employee Info</div>
				<div class= "list-group-item row d-flex text-center" id="heading">
				<div class= "col-4 h4">Title</div>
				<div class= "col-4 h4">First</div>
				<div class= "col-4 h4">Last</div>
				 </div>`);
		div.appendTo($("#employeeList"));
		usealldata ? sessionStorage.setItem("allemployees", JSON.stringify(data)) : null;
		//row that user clicks that initiates the add process (id = "0")
		btn = $(`<button class="list-group=item row d-flex" id="0"><divclass="col-12 text-left">...Click to Add Employee</div></button>`);
		btn.appendTo($("#employeeList"));
		//iterate over employees and add one button for each employee
		//the id of the butotn is the same as the id for the student
		//we make use of that id when we do the update 
		data.map(emp => {
			btn = $(`<button class="list-group-item row d-flex" id="${emp.id}">`);
			btn.html(`<div class="col-4" id="employeetitle${emp.id}">${emp.title}</div>
                      <div class="col-4" id="employeefname${emp.id}">${emp.firstName}</div>
                      <div class="col-4" id="employeelastnam${emp.id}">${emp.lastName}</div>`
			);

			btn.appendTo($("#employeeList"));

		}); //map

	};//buildStudentList 

	
	//clear everything 
	const clearModalFields = () => {
		loadDepartmentDDL(-1);
		$("#TextBoxTitle").val("");
		$("#TextBoxFirstname").val("");
		$("#TextBoxLastname").val("");
		$("#TextBoxPhone").val("");
		$("#TextBoxEmail").val("");
		$("#ImageHolder").html("");
		$("input:file").val("");
		sessionStorage.removeItem("id");
		sessionStorage.removeItem("departmentId");
		sessionStorage.removeItem("timer");
		sessionStorage.removeItem("staffPicture64");
		$("#EmployeeModalForm").validate().resetForm(); 
	};// clearModalFields 


	//call this if they click on the add button (if the id from our student list click handler is 0)
	const setupForAdd = () => {
		$("#actionbutton").val("add");
		$("#modaltitle").html("<h3>Add employee</h3>");
		$("#theModal").modal("toggle");
		$("#deletebutton").hide();
		clearModalFields();
	};//setup for add 


	const add = async () => {
		try {
			emp = new Object();
			//pull the values from the modal textboxes 
			emp.title = $("#TextBoxTitle").val();
			emp.firstName = $("#TextBoxFirstname").val();
			emp.lastName = $("#TextBoxLastname").val();
			emp.phoneNo = $("#TextBoxPhone").val();
			emp.email = $("#TextBoxEmail").val();
			//pull out div id out of ddl
			emp.departmentID = parseInt($("#ddlDepartments").val()); 
			//id set by database but need to be set because the server can't accept them as nulls 
			emp.id = -1;
			emp.timer = null;
			emp.staffPicture64 = sessionStorage.staffPicture64;
			//send the employee info to the server asynchronously using POST 
			let response = await fetch("api/employee", {
				method: "POST", //using a POST now isntead of a PUT (POST for new data, PUT for updating existing data)
				headers: {
					"Content-Type": "application/json; charset=utf-8"
				},
				body: JSON.stringify(emp)
			});
			if (response.ok) {
				let data = await response.json();
				getAll(data.msg); //data msg holds the reposne from the server, we'll put that string in the header of the table 
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
	};

	const setupForUpdate = (id, data) => {
		$("#actionbutton").val("update");
		$("#modaltitle").html("<h3>Update employee</h3>");

		clearModalFields();
		data.map(employee => {
			if (employee.id === parseInt(id)) {
				//set modal textbox info
				$("#TextBoxTitle").val(employee.title);
				$("#TextBoxFirstname").val(employee.firstName);
				$("#TextBoxLastname").val(employee.lastName);
				$("#TextBoxPhone").val(employee.phoneNo);
				$("#TextBoxEmail").val(employee.email);
				loadDepartmentDDL(employee.departmentID.toString());
				$("#ImageHolder").html(`<img height="120" width="110" src="data:img/png;base64,${employee.staffPicture64}"/>`);
				//store sensitive employee information for updating 
				sessionStorage.setItem("id", employee.id);
				sessionStorage.setItem("timer", employee.timer);
				sessionStorage.setItem("staffPicture64", employee.staffPicture64);

				//toggle modal to make it appear
				$("#theModal").modal("toggle");
				$("#deletebutton").show();
			} //if
		});//data.map
	};//setupForUpdate 

	const update = async () => {
		try {

			//setup a new client side instance of employee
			emp = new Object();
			//populate the properties 
			emp.title = $("#TextBoxTitle").val();
			emp.firstName = $("#TextBoxFirstname").val();
			emp.lastName = $("#TextBoxLastname").val();
			emp.phoneNo = $("#TextBoxPhone").val();
			emp.email = $("#TextBoxEmail").val();

			//stored these three values earlier in session storage and grab them 
			emp.id = parseInt(sessionStorage.getItem("id"));
			emp.departmentID = parseInt($("#ddlDepartments").val()); 
			emp.timer = sessionStorage.getItem("timer");
			//emp.StaffPicture64 = null;
			sessionStorage.getItem("staffPicture64")
				? emp.staffPicture64 = sessionStorage.getItem("staffPicture64")
				: emp.staffPicture64 = null;

			//send the updated back to the server asynchronously using PUT 
			let response = await fetch("api/employee", {
				method: "PUT",
				headers: {
					"Content-Type": "application/json; charset=utf-8"
				},
				body: JSON.stringify(emp)
			});
			if (response.ok) { //or check for response.staus 
				let data = await response.json();
				getAll(data.msg);
			} else if (response.status !== 404) { //probably some other client side error 
				let problemJson = await response.json();
				errorRtn(problemJson, repsonse.status);
			} else { //else 404 notfound
				$("#status").text("no such path on server");
			}//else
		} catch (error) {
			$("#status").text(error.message);
		}
		$("#theModal").modal("toggle");
	}
	// do we have a picture?
	$("input:file").change(() => {
		const reader = new FileReader();
		const file = $("#uploader")[0].files[0];

		file ? reader.readAsBinaryString(file) : null;

		reader.onload = (readerEvt) => {
			// get binary data then convert to encoded string
			const binaryString = reader.result;
			const encodedString = btoa(binaryString);
			sessionStorage.setItem('staffPicture64', encodedString);
		};
	}); // input file change

	//populates the html select with department options , retrieves data from the session storage and parse it into a JSON array 
	const loadDepartmentDDL = (empdiv) => {
		html = '';
		$('#ddlDepartments').empty();
		let alldepartments = JSON.parse(sessionStorage.getItem('alldepartments'));
		alldepartments.map(div => html += `<option value= "${div.id}">${div.name}</option>`);
		$('#ddlDepartments').append(html);
		$('#ddlDepartments').val(empdiv);
	};//loadDivisionDDL

	$("#actionbutton").click(() => {
		$("#actionbutton").val() === "update" ? update() : add();
	});

	//event handler for the confirmation click - passes a JSON object to the click event and the object of the user's choice 
	$('[data-toggle=confirmation]').confirmation({ rootSelector: '[data-toggle=confirmation]' });
	//if yes was chosen, if no then ignored 
	$('#deletebutton').click(() => _delete());


	const _delete = async () => {
		try {
			//send the request to delete to the server asynchronously using DELETE 
			let response = await fetch(`api/employee/${sessionStorage.getItem('id')}`, {
				method: 'DELETE',
				headers: { 'Content-Type': 'application/json; chartset=utf-8' }
			});
			if (response.ok)//or check for response status 
			{
				let data = await response.json();
				getAll(data.msg);
			} else {
				$('#status').text(`Status - ${response.status}, Problem on delete server side, see server console`);
			}//else
			$('#theModal').modal('toggle');

		}
		catch (error) {
			$('#status').text(error.message);
		}
	};//delete 
	$("#employeeList").click((e) => {
		if (!e) e = window.event;
		let id = e.target.parentNode.id;
		if (id === "employeeList" || id === "") {
			id = e.target.id;
		}//clicked on row somewhere else 
		if (id !== "status" && id !== "heading") {
			let data = JSON.parse(sessionStorage.getItem("allemployees"));
			//if id is 0, then we are adding a employee, otherwise we'll pass the id to the setupForUpdate function 
			id === "0" ? setupForAdd() : setupForUpdate(id, data);
		} else {
			return false; //ignore if they clicked on heading or status
		}
	});//employeeList click 

	

	getAll("");//first grab the data from the server
});//jQuery ready method 

//server was reached but server had problem with the call
const errorRtn = (problemJson, status) => {
	if (status > 499) {
		$("#status").text("Problem server side, see debug console");
	} else {
		let keys = Object.keys(problemJson.errors)
		problem = {
			status: status,
			statusText: problemJson.errors[keys[0]][0], //first error 
		};
		$("#status").text("Problem client side, see browser console");
		console.log(problem);
	}//else
}