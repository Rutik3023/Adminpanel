$(document).ready(function () {


    //Dialog To show Enter Email for forget User
    var dialogForgetUser = new ej.popups.Dialog({
        header: 'Enter Registered Mail',
        content: document.getElementById('dialogContent'),
        visible: false,
        buttons: [{

            buttonModel: { content: 'Close', isPrimary: true }, click: oncloseButtonClick
        }]
    });

    //Dialog To show Enter Email for forget Password
    var dialogForgetPass = new ej.popups.Dialog({
        header: 'Enter Username and Registered Mail',
        content: document.getElementById('dialogContent2'),
        visible: false,
        buttons: [{

            buttonModel: { content: 'Close', isPrimary: true }, click: onclosePassBtnClick
        }]
    });

    //Dynamic Dialog to show alert Message Dialog
    var dialogMsg2 = new ej.popups.Dialog({
        header: '',
        content: '',
        buttons: [
            { buttonModel: { content: 'Ok', isPrimary: true }, click: onOkButtonClick }

        ]

    });

    function onOkButtonClick() {

        dialogMsg2.hide();

    }

    //Function to clear dialog of forget Password from parent 
    function cleardialogMsg() {

        var dialogDiv = document.getElementById("Login2Dilog");

        if (dialogDiv) {

            while (dialogDiv.firstChild) {
                dialogDiv.removeChild(dialogDiv.firstChild);
            }
        }
    }

    //Function to submit email and check response and show alert Dialog in case of forget user
    var SubmitButton = document.getElementById('submitMail');

    if (SubmitButton != null) {
        SubmitButton.addEventListener('click', function (event) {

            var emailInput = document.getElementById('Regist_Email');
            var emailError = document.getElementById('ValidationError');
            var isValid = true;

            emailError.textContent = '';

            if (!emailInput.value.trim()) {
                emailError.textContent = 'Please enter email ';
                isValid = false;

            }
            else if (!validateEmail(emailInput.value.trim())) {
                isValid = false;
                emailError.textContent = 'Please enter valid email';

            }

            if (isValid) {
                var emailValue = document.getElementById('Regist_Email').value;
                console.log(emailValue);
                if (emailValue != null) {
                    $.ajax({
                        "url": '/Login/Forget_User_validation',
                        method: "GET",
                        data: { Mail: emailValue },
                        dataType: 'json',
                        success: function (data) {

                            console.log(data.code);
                            if (data.code == 0) {

                                //console.log(data.message);

                                dialogMsg2.header = '';
                                dialogMsg2.content = '';

                                cleardialogMsg();

                                dialogMsg2.header = '<div class="text-danger " style="font-weight:bold">Alert Message</div>';
                                dialogMsg2.content = '<div style="font-weight:bold">Username has been sent to your resgistered email. Kindly check email</div>'; // Use Html.Raw to render @resp as raw HTML
                                dialogMsg2.appendTo('#Login2Dilog');
                                oncloseButtonClick();
                                dialogMsg2.show();

                            } else {

                                dialogMsg2.header = '';
                                dialogMsg2.content = '';

                                cleardialogMsg();

                                dialogMsg2.header = '<div class="text-danger " style="font-weight:bold">Alert Message</div>';
                                dialogMsg2.content = '<div style="font-weight:bold">Entered email is invalid Please enter the registered email ID or contact administration.</div>'; // Use Html.Raw to render @resp as raw HTML
                                dialogMsg2.appendTo('#Login2Dilog');
                                oncloseButtonClick();
                                dialogMsg2.show();


                                //document.getElementById('emailError').innerText = 'Enter email not found';
                            }

                        },
                        error: function (err) {
                            console.log("error: " + err)
                        }

                    })
                }


            } else {

                event.preventDefault();

            }
        });
    }


    //Function to submit email and check response and show alert Dialog in case of forget password
    var SubmitFrgtPass = document.getElementById('Mailfrgt');
    if (SubmitFrgtPass != null) {
        SubmitFrgtPass.addEventListener('click', function (event) {
            var emailInput = document.getElementById('Regist_Email_Pass');
            var UserName = document.getElementById('UserName');
            var emailError = document.getElementById('ValidationErrorUserName');
            var UserNameError = document.getElementById('ValidationErrorPass');
            var isValidPass = true;

            emailError.textContent = '';
            UserNameError.textContent = '';

            if (!emailInput.value.trim() & !UserName.value.trim()) {
                emailError.textContent = 'Please enter email ';
                UserNameError.textContent = 'Please User Name Name ';

                isValidPass = false;

            }
            else if (!validateEmail(emailInput.value.trim())) {
                isValidPass = false;
                emailError.textContent = 'Please enter valid email';

            }

            if (isValidPass) {
                document.getElementById('emailError2').innerText = '';

                var emailValue = document.getElementById('Regist_Email_Pass').value;
                var Username = document.getElementById('UserName').value;
                console.log(emailValue);
                if (emailValue != null) {
                    $.ajax({
                        "url": 'Login/Forget_Password_Validation',
                        method: "GET",
                        data: {
                            Mail: emailValue, Username: Username
                        },
                        dataType: 'json',
                        success: function (data) {
                            /*console.log(data.code);*/
                            if (data.code == 0) {
                                console.log(data.message
                                );
                                dialogForgetPass.hide();
                                cleardialogMsg();
                                dialogMsg2.header = '<div class="text-danger " style="font-weight:bold">Alert Message</div>';
                                dialogMsg2.content = '<div style="font-weight:bold">Link to reset password has been sent to your resgistered email. Kindly check email</div>'; // Use Html.Raw to render @resp as raw HTML
                                dialogMsg2.appendTo('#Login2Dilog');
                                /* onclosePassBtnClick()*/
                                dialogMsg2.show();

                            } else {
                                dialogForgetPass.hide();
                                cleardialogMsg();
                                dialogMsg2.header = '<div class="text-danger " style="font-weight:bold">Alert Message</div>';
                                dialogMsg2.content = '<div style="font-weight:bold">Username and email do not match. Please enter a valid username and email.</div>'; // Use Html.Raw to render @resp as raw HTML
                                dialogMsg2.appendTo('#Login2Dilog');
                                /* onclosePassBtnClick()*/
                                dialogMsg2.show();
                                //document.getElementById('emailError2').innerText = 'Username and email do not match. Please enter a valid username and email.';
                            }

                        },
                        error: function (err) {
                            console.log("error: " + err)
                        }

                    })
                }


            } else {

                event.preventDefault();

            }



        })
    }



    //Function to close Forget Password dialog
    function onclosePassBtnClick() {
        var emailError2 = document.getElementById('ValidationErrorUserName');
        if (emailError2.textContent != '') { emailError2.textContent = ''; }
        document.getElementById('emailError2').textContent = '';
        ClearFrgtPass();
        dialogForgetPass.hide();

    }



    //Function to close  forget user Dilaog
    function oncloseButtonClick() {
        var emailError = document.getElementById('ValidationError');
        emailError.textContent = '';
        document.getElementById('emailError').textContent = '';
        ClearFrgtUsern();
        //onOkButtonClick();
        dialogForgetUser.hide();

        //var obj3 = document.getElementById('Regist_Email').value = null;
    }

    //Function to validate email
    function validateEmail(email) {
        var emailRegex = /^[\w\.-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$/;;
        return emailRegex.test(email);
    }

    //Function to clear dialog of forget user from parent 
    function cleardialogForgetUser() {
        var dialogDiv = document.getElementById("dialogfrgtUser");
        if (dialogDiv) {
            while (dialogDiv.firstChild) {
                dialogDiv.removeChild(dialogDiv.firstChild);
            }
        }
    }


    //Function to clear dialog of forget Password from parent 
    function cleardialogForgetPass() {
        var dialogDiv = document.getElementById("dialogfrgtPass");
        if (dialogDiv) {
            while (dialogDiv.firstChild) {
                dialogDiv.removeChild(dialogDiv.firstChild);
            }
        }
    }


    // Function To clear Form
    function ClearFrgtUsern() {
        document.getElementById('frgtUser_Form').reset();
    }


    function ClearFrgtPass() {
        document.getElementById('frgtPass_Form').reset();
    }

    //On click event of forget Username Link or tag---Starts
    var frgtUUser = document.getElementById('ForgetUserTag');
    frgtUUser.addEventListener('click', function (event) {
        /*ClearFrgtUsern();*/
        cleardialogForgetUser();

        dialogForgetUser.appendTo('#dialogfrgtUser');
        document.getElementById('dialogContent').classList.remove('d-none');
        dialogForgetUser.show();

    })
    //On click event of forget Username Link or tag---Ends

    //On click event of forget Password Link or tag---Starts

    var frgtPass = document.getElementById('ForgetPassTag');
    frgtPass.addEventListener('click', function (event) {
        ClearFrgtPass();
        cleardialogForgetPass();
        dialogForgetPass.appendTo('#dialogfrgtPass');
        document.getElementById('dialogContent2').classList.remove('d-none');
        dialogForgetPass.show();

    })
    //On click event of forget Password Link or tag---Ends

});