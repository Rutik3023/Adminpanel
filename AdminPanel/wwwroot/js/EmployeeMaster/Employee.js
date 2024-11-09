


//document.addEventListener('DOMContentLoaded', function () {

//    document.addEventListener('click', function (event) {
//        if (event.target && event.target.matches('.e-icons.e-file-remove-btn')) {
//            console.log("event.target");
//            console.log(event.target);
//            //clearImage();
//        }
//    });

//});


document.addEventListener('DOMContentLoaded', function () {
    var container1 = document.getElementById('BrowseDocuments');
    var container2 = document.getElementById('BrowseImg');

    console.log(container1);
    container1.addEventListener('click', function (event) {
        if (event.target && event.target.matches('.e-icons.e-file-remove-btn')) {
            console.log("Clicked on element in container 1");
            console.log(event.target);
            // Perform actions specific to container 1
        }
    });

    container2.addEventListener('click', function (event) {
        if (event.target && event.target.matches('.e-icons.e-file-remove-btn')) {
            console.log("Clicked on element in container 2");
            console.log(event.target);
            clearImage();
            // Perform actions specific to container 2
        }
    });
});

$(document).ready(function () {
    function OnLastName() {
        var FirstValue = $('#First_Name').val();
        var SecondValue = $('#Middle_Name').val();
        var LastdValue = $('#Last_Name').val();


        var AliceName = document.getElementById('Alice_Name').ej2_instances[0];
        AliceName.value = FirstValue + " " + SecondValue + " " + LastdValue;
        console.log(FirstValue);
        console.log(SecondValue);
        console.log(LastdValue);
        console.log(FirstValue + " " + SecondValue + " " + LastdValue);
    }
    OnLastName();


    //// Your code here
    //var tabObj = new ej.navigations.Tab({
    //    heightAdjustMode: "Auto"
    //});
    //tabObj.appendTo('#ej2Tab');




    var tabObj = new ej.navigations.Tab({
        heightAdjustMode: "Content" // Adjust the height based on content
    });
    tabObj.appendTo('#ej2Tab');


    var options = {
        rules: {

            'First_Name': { required: [true, "Please enter first name"], regex: ['^[A-Za-z ]+$', "Please enter valid Name numeric value & special Character can not allowed"] },
            'Middle_Name': { required: [true, "Please enter middle name"], regex: ['^[A-Za-z ]+$', "Please enter valid Name numeric value & special Character can not allowed"] },
            'Last_Name': { required: [true, "Please enter last name"], regex: ['^[A-Za-z ]+$', "Please enter valid Name numeric value & special Character can not allowed"] },
            'Emp_Status': { required: [true, "Please select employment status"] },
            'Co_Email_ID': { required: [true, "Please enter co-emailid"], email: [true, "Please Enter Valid Co emailid"] },
            'Mobile_No': { required: [true, "Please enter mobile no"], regex: /^[0-9]{10}$/ },
            'Salesmen': { required: [true, "Please select salesmen"] },
            'Status': { required: [true, "Please select status"] },
            'OrgCode': { required: [true, "Please select Organization"] }
          


        },
        errorPlacement: function (error, element) {
            var errorId = element.id + 'Error';
            var errorSpan = document.getElementById(errorId);
            if (errorSpan) {
                errorSpan.textContent = error.textContent;
            }
        }

    };


    var saveButton = document.getElementById('EmpsubmitBtn');
    var formObject = new ej.inputs.FormValidator('#EmpForm');

    formObject.rules = options.rules;
    // Places error label outside the TextBox using the customPlacement event of FormValidator
    formObject.customPlacement = function (element, errorElement) {
        console.log(element.parentElement);
        if (element.parentElement && element.parentElement.parentElement) {

            element.parentElement.parentElement.appendChild(errorElement);
        }
    };

    let isValids = true;

    // onSelect function to validate file size
    function onSelect(args) {
        const maxFileSize = 3145728; // 3 MB in bytes
        isValid = true; // Reset isValid to true on each file selection

        // Iterate over selected files and check their size
        for (let file of args.filesData) {
            if (file.size > maxFileSize) {
                isValids = false; // Set validation flag to false
                args.cancel = true; // Cancel the upload
                // Create and show the toast notification on successful deletion
                var toastObj = new ej.notifications.Toast({
                    content: 'The file size exceeds 3 MB. Please select a smaller file.',
                    position: { X: 'Right', Y: 'Top' },
                    cssClass: 'Red-toast' // Add custom class for styling
                });
                toastObj.appendTo('#toast'); // Append to the toast container
                toastObj.show(); // Show the toast

                //alert('The file size exceeds 3 MB. Please select a smaller file.');
                return; // Exit after cancelling
            }
        }
    }

    // Add event listener for click event
    saveButton.addEventListener('click', function (event) {

        var uploader = document.getElementById('Emp_photoCode').ej2_instances[0];
        var fileList = uploader.getFilesData();
     
        // Check if any file exists and validate its size
        if (fileList.length > 0) {
            var file = fileList[0]; // Since multiple is false, we only expect one file
            var fileSize = file.size; // File size in bytes

            // Max file size in bytes (3 MB = 3145728 bytes)
            var maxFileSize = 3145728;

            if (fileSize > maxFileSize) {
                // File size exceeds the limit, prevent form submission
               
                event.preventDefault(); // Stop form submission
                return;
            }
        }
        var IsValid = formObject.validate();
        var img = document.getElementById('UploadImg');
        var IsValid = formObject.validate();
        $("#m").remove();
        if (img.src === '' || img.src === 'https://localhost:7119/HRM/EmployeeMaster/Employee_Form') {
            $("<span class='e-error' id='m' style='display: block;'>Select Emp Photo</span>").insertAfter("#m1");
            IsValid = false;
            } 
        

        

      
        if (IsValid ) {


        } else {

            event.preventDefault();

        }
    });

    //-----------------------validation for Tabs ----------------------------------------------------------------

    var professionalFormRules = {
        rules: {
            'BranchCode': { required: [true, "Please select branch location"], regex: [/^[a-zA-Z0-9]+$/, "Special characters are not allowed in Branch Code"] },
            'DepartmentCode': { required: [true, "Please select department"], regex: [/^[a-zA-Z0-9]+$/, "Special characters are not allowed in Department Code"] },
            'Department_Head_Code': { required: [true, "Please select department head"], regex: [/^[a-zA-Z0-9]+$/, "Special characters are not allowed in Department Head Code"] },
            'Reporting_To_Code': { required: [true, "Please select reporting to"] },
            'DesignationCode': { required: [true, "Please select designation"], regex: [/^[a-zA-Z0-9]+$/, "Special characters are not allowed indesignation Code"] }
        }
    };

    var personalFormRules = {
        rules: {

            // Personal form validation

            'Date_Of_Birth': { required: [true, "Please enter date of birth"] },
            'Aadhar_No': { required: [true, "Please enter aadhar number"], regex: [/^\d{12}$/, "Please Enter Valid Adhar No"] } ,
            'PAN_No': { required: [true, "Please enter pan number"], regex: ['^[A-Z]{5}[0-9]{4}[A-Z]$',"Please Enter Valid Pan No" ]},
            'Nationality': { required: [true, "Please enter nationality"] },
            'Personal_Email_ID': { required: [true, "Please enter personal email id"], email: [true, "Please Enter Valid Personal Email Id"] },
            'Personal_Mobile_No': { required: [true, "Please enter personal mobile number"], regex: [/^[0-9]{10}$/, "Please Enter Valid mobile Number"] },
            'Personal_Address': { required: [true, "Please enter personal address"] },
            'Emergency_Contact_No': { required: [true, "Please fill emergency contact number"], regex:[ /^[0-9]{10}$/,"Please Enter Valid Contact Number" ]},
            'Emergency_Contact_Person': { required: [true, "Please enter emergency contact person"], regex: ['^[A-Za-z ]+$', "Please enter valid Name numeric value & special Character can not allowed"] },
            'Office_Phone_No': { regex: [/^[0-9]{10}$/, "Please Enter Valid Office Phone No"] },
            'Phone_Number': { regex: [/^[0-9]{10}$/, "Please Enter Valid Office Phone Number"] }

        }
    };



    var AccountFormRules = {
        rules: {

            // Personal form validation

            'Bank_AC_No': { required: [true, "Please enter Bank AC No."],regex: ['^\\d{9,18}$', "Please Enter Valid Bank Account Number"] },
            'IFSC_Code': { required: [true, "Please enter IFSC Code"], regex: ['^[A-Z]{4}0\\d{6}$', "Please Enter Valid IFSC Code"] },
            'Bank_Name': { required: [true, "Please enter bank name"], regex: [/^[a-zA-Z ]+$/, "Plase Enter The Vaild Bank Name"] }


        }
    };





    // Initialize FormValidator for each form
    var professionalFormValidator = new ej.inputs.FormValidator('#Professional_Form');
    professionalFormValidator.rules = professionalFormRules.rules;
    professionalFormValidator.customPlacement = customPlacementFunction;

    var personalFormValidator = new ej.inputs.FormValidator('#Personal_Form');
    personalFormValidator.rules = personalFormRules.rules;
    personalFormValidator.customPlacement = customPlacementFunction;


    var AccountFormValidator = new ej.inputs.FormValidator('#EmpAccount');
    AccountFormValidator.rules = AccountFormRules.rules;
    AccountFormValidator.customPlacement = customPlacementFunction;

    function customPlacementFunction(inputElement, errorElement) {
        var elementType = inputElement.tagName.toLowerCase();
        var parentElement = inputElement.parentElement.parentElement;

        if (elementType === "input" || elementType === "textarea" || elementType === "select") {
            parentElement.appendChild(errorElement);
        } else if (elementType.startsWith("ejs-")) {
            parentElement.parentElement.appendChild(errorElement);
        }
    }

    // Handle form submission
    var saveButton = document.getElementById('TabsSubmitBtn');



    saveButton.addEventListener('click', function (event) {


        var isProfessionalFormValid = professionalFormValidator.validate();
        var isPersonalFormValid = personalFormValidator.validate();
        var isAccountFormValid = AccountFormValidator.validate();

      


        if (isProfessionalFormValid && isPersonalFormValid && isAccountFormValid) {
            // Gather data from both forms



            //Professional Details

            var syscodee = document.getElementById("Sys_Code").value;

            var dateOfJoining = document.getElementById("Date_of_Joining").value;
            var transferDate = document.getElementById("Transfer_Date").value;
            var employmentEndDate = document.getElementById("Employment_End_Date").value;
            var probationPeriod = document.getElementById("Probation_Period").value;
            var professionalStatus = document.getElementById("Professional_Status").value;
            var nationalID = document.getElementById("National_ID").value;

            var branchCode = document.getElementById("BranchCode").ej2_instances[0].value;
            var departmentCode = document.getElementById("DepartmentCode").ej2_instances[0].value;
            var departmentHeadCode = document.getElementById("DepartmentCode").ej2_instances[0].value;
            var reportingToCode = document.getElementById("Reporting_To_Code").ej2_instances[0].value;


            var designationStartDate = document.getElementById("Designation_Start_Date").value;
            var designationCode = document.getElementById("DesignationCode").ej2_instances[0].value;
            var designationQualifier = document.getElementById("Designation_Qualifier").value;
            var jobDescription = document.getElementById("Job_Description").value;
            var payrollEmployeeID = document.getElementById("Payroll_Employee_ID").value;


            //Personal Details

            var Date_Of_Birth = document.getElementById('Date_Of_Birth').value;
            var Aadhar_No = document.getElementById('Aadhar_No').value;
            var PAN_No = document.getElementById('PAN_No').value;
            var Gender = document.getElementById("Gender").ej2_instances[0].value;/* document.getElementById('Gender').value;*/
            var UAN_No = document.getElementById('UAN_No').value;
            var Nationality = document.getElementById("Nationality").ej2_instances[0].value; /*document.getElementById('Nationality').value;*/
            var Religion = document.getElementById('Religion').value;
            var Marital_Status = document.getElementById("Marital_Status").ej2_instances[0].value;/* document.getElementById('Marital_Status').value;*/
            var Wedding_Annive = document.getElementById('Wedding_Annive').value;
            var Office_Phone_No = document.getElementById('Office_Phone_No').value;
            var Personal_Email_ID = document.getElementById('Personal_Email_ID').value;
            var Personal_Mobile_No = document.getElementById('Personal_Mobile_No').value;
            var Home_Town = document.getElementById('Home_Town').value;
            var Airport_Name = document.getElementById('Airport_Name').value;
            var Personal_Address = document.getElementById('Personal_Address').value;
            var Phone_Number = document.getElementById('Phone_Number').value;
            var Contact_Address = document.getElementById('Contact_Address').value;
            var Emergency_Contact_Person = document.getElementById('Emergency_Contact_Person').value;
            var Emergency_Contact_Address = document.getElementById('Emergency_Contact_Address').value;
            var Emergency_Contact_No = document.getElementById('Emergency_Contact_No').value;


            //Account Details

            var BankACNo = document.getElementById('Bank_AC_No').value;
            var IFSCCode = document.getElementById('IFSC_Code').value;
            var BankName = document.getElementById('Bank_Name').value;
            var BankAddress = document.getElementById("Bank_Address").ej2_instances[0].value;


            var postData = {


                "Sys_Code": syscodee,

                "Date_of_Joining": dateOfJoining,
                "Transfer_Date": transferDate,
                "Employment_End_Date": employmentEndDate,
                "Probation_Period": probationPeriod,
                "Professional_Status": professionalStatus,
                "National_ID": nationalID,


                "BranchCode": branchCode,
                "DepartmentCode": departmentCode,
                "Department_Head_Code": departmentHeadCode,
                "Reporting_To_Code": reportingToCode,


                "Designation_Start_Date": designationStartDate,
                "DesignationCode": designationCode,
                "Designation_Qualifier": designationQualifier,
                "Job_Description": jobDescription,
                "Payroll_Employee_ID": payrollEmployeeID,



                // Personal Details
                "Date_Of_Birth": Date_Of_Birth,
                "Aadhar_No": Aadhar_No,
                "PAN_No": PAN_No,
                "Gender": Gender,
                "UAN_No": UAN_No,
                "Nationality": Nationality,
                "Religion": Religion,
                "Marital_Status": Marital_Status,
                "Wedding_Annive": Wedding_Annive,
                "Office_Phone_No": Office_Phone_No,
                "Personal_Email_ID": Personal_Email_ID,
                "Personal_Mobile_No": Personal_Mobile_No,
                "Home_Town": Home_Town,
                "Airport_Name": Airport_Name,
                "Personal_Address": Personal_Address,
                "Phone_Number": Phone_Number,
                "Contact_Address": Contact_Address,
                "Emergency_Contact_Person": Emergency_Contact_Person,
                "Emergency_Contact_Address": Emergency_Contact_Address,
                "Emergency_Contact_No": Emergency_Contact_No,


                //Account Details
                "Bank_AC_No": BankACNo,
                "IFSC_Code": IFSCCode,
                "Bank_Name": BankName,
                "Bank_Address": BankAddress

            };


            //Mandatory PopUp
            var EmpMandatorydialog = new ej.popups.Dialog({
                header: 'Warning',
                content: '<div>Please Fill Employee Details</div>',
                buttons: [
                    { buttonModel: { content: 'Yes', isPrimary: true }, click: HideDialogEmpMandatory }

                ]

            });


            function HideDialogEmpMandatory() {
                console.log(EmpMandatorydialog);
                if (EmpMandatorydialog) { clearDialogEmpMandatory(); EmpMandatorydialog.hide(); }
                //

            }

            function clearDialogEmpMandatory() {

                var dialogDiv = document.getElementById("DilogEmpMandate");
                if (dialogDiv) {
                    while (dialogDiv.firstChild) {
                        dialogDiv.removeChild(dialogDiv.firstChild);
                    }
                }
            }

            console.log(postData);

            var Sys_Code = document.getElementById("Sys_Code").ej2_instances[0].value;


            if (syscodee == null || syscodee == "") {
                if (EmpMandatorydialog) {
                    clearDialogEmpMandatory();
                    EmpMandatorydialog.appendTo('#DilogEmpMandate');
                    EmpMandatorydialog.show();
                }

            } else {      TabsDataPosting(postData); }


     











        } else {

            clearDialogDiv();
            var dialogDiv = document.getElementById("Dilogbox");
            if (dialogDiv) {
                while (dialogDiv.firstChild) {
                    dialogDiv.removeChild(dialogDiv.firstChild);
                }
            }
            //var dialog = new ej.popups.Dialog({
            //    header: 'ALERT',
            //    content: '<div>Fill All Tabs Requied Data</div>',
            //    buttons: [
            //        { buttonModel: { content: 'OK', isPrimary: true }, click: onOkButtonClick, id: 'confirmOk' }
            //    ],
            //    showCloseIcon: true,  // Optional: adds a close icon to the dialog
            //    width: '250px',
            //    target: document.body
            //});
            //dialog.appendTo('#Dilogbox');
            //dialog.show();


            // Create and show the toast notification on successful deletion
            var toastObj = new ej.notifications.Toast({
                content: 'Fill All Tabs Requied Data',
                position: { X: 'Right', Y: 'Top' },
                cssClass: 'Red-toast' // Add custom class for styling
            });
            toastObj.appendTo('#toast'); // Append to the toast container
            toastObj.show(); // Show the toast

            event.preventDefault();
        }
    });



});

function TabsDataPosting(postData) {


   

    $.ajax({
        url: '/HRM/EmployeeMaster/SaveData', // Replace with your action and controller
        type: 'POST',
        contentType: 'application/json',
        dataType: 'json',
        data: JSON.stringify(postData),
        success: function (response) {

            console.log(response);
            if (response != null) {
                clearDialogDiv();
                var dialogDiv = document.getElementById("Dilogbox");
                if (dialogDiv) {
                    while (dialogDiv.firstChild) {
                        dialogDiv.removeChild(dialogDiv.firstChild);
                    }
                }
                //var dialog = new ej.popups.Dialog({
                //    header: 'ALERT',
                //    content: '<div>Record Updated SucsessFully</div>',
                //    buttons: [
                //        { buttonModel: { content: 'OK', isPrimary: true }, click: onOkButtonClick, id: 'confirmOk' }
                //    ],
                //    showCloseIcon: true,  // Optional: adds a close icon to the dialog
                //    width: '250px',
                //    target: document.body
                //});
                //dialog.appendTo('#Dilogbox');
                //dialog.show();


                // Create and show the toast notification on successful deletion
                var toastObj = new ej.notifications.Toast({
                    content: 'Record Saved successfully.',
                    position: { X: 'Right', Y: 'Top' },
                    cssClass: 'green-toast' // Add custom class for styling
                });
                toastObj.appendTo('#toast'); // Append to the toast container
                toastObj.show(); // Show the toast

                


            } else { resetForm(); }


        },
        error: function (xhr, status, error) {
            // Handle errors
            console.error(xhr.responseText);
        }
    });
}

function clearDialogDiv() {
    var dialogDiv = document.getElementById("Dilogbox");
    if (dialogDiv) {
        while (dialogDiv.firstChild) {
            dialogDiv.removeChild(dialogDiv.firstChild);
        }
    }
}



function onOkButtonClick() {
    clearDialogDiv();
    this.hide();
}


function onSelect(args) {

    console.log(args);
    if (args.filesData.length > 0) {
        var file = args.filesData[0].rawFile;
        var reader = new FileReader();
        reader.onload = function (e) {
            var imgElement = document.getElementById('UploadImg');
            console.log(e.target.result);
            console.log(imgElement);
            imgElement.src = e.target.result;
            imgElement.style.display = 'block';
        };
        reader.readAsDataURL(file);
    }
}

function clearImage() {

    var imgElement = document.getElementById('UploadImg');
    if (imgElement) {
        imgElement.src = '';
        imgElement.style.display = 'none';
    }
}
function OnLastName() {
    var FirstValue = $('#First_Name').val();
    var SecondValue = $('#Middle_Name').val();
    var LastdValue = $('#Last_Name').val();


    var AliceName = document.getElementById('Alice_Name').ej2_instances[0];
    AliceName.value = FirstValue + " " + SecondValue + " " + LastdValue;
    console.log(FirstValue);
    console.log(SecondValue);
    console.log(LastdValue);
    console.log(FirstValue + " " + SecondValue + " " + LastdValue);
}


function DeptHead(args) {




    var syscode = args.itemData.Value;



    var xhr = new XMLHttpRequest();
    xhr.open('GET', 'Get_Dept_Head?syscode=' + syscode, true);
    xhr.onreadystatechange = function () {
        if (xhr.readyState == XMLHttpRequest.DONE) {
            if (xhr.status === 200) {
                var deptHeadData = JSON.parse(xhr.responseText); // Assuming dept head data is returned as JSON

                console.log(deptHeadData)

                // console.log(deptHeadData.department_Head);
                var departmentHeadTextBox = document.getElementById("Department_Head_Code").ej2_instances[0];


                console.log(departmentHeadTextBox);

                if (deptHeadData && deptHeadData.department_Head) {
                    departmentHeadTextBox.value = deptHeadData.code;
                } else {
                    console.error('Department head data not found or invalid.');
                }
            } else {
                console.error('Failed to fetch user roles. Status:', xhr.status);
            }
        }
    };
    xhr.send();
}


