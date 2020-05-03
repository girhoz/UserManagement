Religion = 0;
ReligionName = "";
$(document).ready(function () {
    //Load Select Box Religion
    $.ajax({
        url: "/Religions/LoadReligion",
        type: "GET",
        success: function (data) {
            //debugger;
            var ReligionOption = data;
            var $option = $('#ReligionOption');
            $option.empty();
            $option.append($('<option/>').val(Religion).text(ReligionName).hide());
            $.each(ReligionOption, function (i, val) {
                //debugger;
                $option.append($('<option/>').val(val.id).text(val.name));
            });
        }
    });
    //Load Profile From User Login
    $.ajax({
        url: "/Users/LoadProfile/",
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        async: false,
        success: function (result) {
            //debugger;
            $('#Id').val(result.id);
            $('#Email').attr('readonly', true);
            $('#Email').val(result.email);
            $('#Password').val(result.password);
            $('#Role').val(result.roleName);
            $('#Application').val(result.appName);
            $('#FullName').val(result.fullName);
            $('#FirstName').val(result.firstName);
            $('#LastName').val(result.lastName);
            $('#BirthDate').val(moment(result.birthDate).format('YYYY-MM-DD'));
            $('#PhoneNumber').val(result.phoneNumber);
            $('#Address').val(result.address);
            $('#Batch').val(result.batchName);
            $('#Class').val(result.className);
            Religion = result.religionId;
            ReligionName = result.religionName;
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
});

function Edit() {
    var status = checkForm();
    if (status === true) {
        var UserVM = new Object();
        UserVM.Id = $('#Id').val();
        UserVM.Email = $('#Email').val();
        UserVM.Password = $('#Password').val();
        UserVM.FullName = $('#FullName').val();
        UserVM.FirstName = $('#FirstName').val();
        UserVM.LastName = $('#LastName').val();
        UserVM.BirthDate = $('#BirthDate').val();
        UserVM.PhoneNumber = $('#PhoneNumber').val();
        UserVM.Address = $('#Address').val();
        UserVM.ReligionId = $('#ReligionOption').val();
        $.ajax({
            type: 'POST',
            url: '/Users/InsertOrUpdate/',
            data: UserVM
        }).then((result) => {
            //debugger;
            if (result.statusCode === 200) {
                Swal.fire({
                    position: 'center',
                    type: 'success',
                    title: 'Profile Updated'
                }).then((result) => {
                    if (result.value) {
                        location.reload();
                    }
                });
            }
            else {
                Swal.fire('Error', 'Failed to Update Profile', 'error');
                ShowModal();
            }
        });
    }
}

function ShowPass() {
    var x = document.getElementById("Password");
    if (x.type === "password") {
        x.type = "text";
    } else {
        x.type = "password";
    }
}

function checkForm() {
    //debugger;
    var val = $('#Password').val();
    if (val !== "") {
        if (val.length < 6) {
            Swal.fire('Error', 'Password Must Contain At Least Six Characters!', 'error');
            return false;
        }
        re = /[0-9]/;
        if (!re.test(val)) {
            Swal.fire('Error', 'Password Must Contain At Least One Number (0-9)!', 'error');
            return false;
        }
        re = /[a-z]/;
        if (!re.test(val)) {
            Swal.fire('Error', 'Password Must Contain At Least One Lowercase Letter (a-z)!', 'error');
            return false;
        }
        re = /[A-Z]/;
        if (!re.test(val)) {
            Swal.fire('Error', 'Password Must Contain At Least One Uppercase Letter (A-Z)!', 'error');
            return false;
        }
        re = /[@$!%*#?&]/;
        if (!re.test(val)) {
            Swal.fire('Error', 'Password Must Contain At Least One Special Character (@$!%*#?&)!', 'error');
            return false;
        }
    } else {
        Swal.fire('Error', 'Password Cannot Be Empty!', 'error');
        return false;
    }
    return true;
}