Religion = 0;
ReligionName = "Select Religion";
State = 0;
StateName = "Select State";
District = 0;
DistrictName = "Select District";
Zipcode = 0;
ZipcodeName = "Select Zipcode";
$(document).ready(function () {
    SetDate();
    $('#StateOption').change(function () {
        District = 0;
        DistrictName = "Select District";
        LoadDistrict($(this).find(':selected').val());
    });
    $('#DistrictOption').change(function () {
        Zipcode = 0;
        ZipcodeName = "Select Zipcode";
        LoadZipcode($(this).find(':selected').val());
    });
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
    //Load State
    $.ajax({
        type: "Get",
        url: "/States/LoadState",
        success: function (data) {
            States = data;
            var $option = $('#StateOption');
            $option.empty();
            $option.append($('<option/>').val(State).text(StateName).hide());
            $.each(States, function (i, val) {
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
            $('#FirstName').val(result.firstName);
            $('#LastName').val(result.lastName);
            $("input[name= Gender][value= " + result.gender + "]").prop('checked', true);
            $('#BirthDate').val(moment(result.birthDate).format('YYYY-MM-DD'));
            $('#PhoneNumber').val(result.phoneNumber);
            $('#Address').val(result.address);
            $('#Batch').val(result.batchName);
            $('#Class').val(result.className);
            Religion = result.religionId;
            ReligionName = result.religionName;
            State = result.stateId;
            StateName = result.stateName;
            District = result.districtId;
            DistrictName = result.districtName;
            LoadDistrict(State);
            Zipcode = result.zipcodeId;
            ZipcodeName = result.zipcodeName;
            LoadZipcode(District);
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
});

function Edit() {
    var UserVM = new Object();
    UserVM.Id = $('#Id').val();
    UserVM.Email = $('#Email').val();
    UserVM.Password = $('#Password').val();
    UserVM.Gender = $("input[name=Gender]:checked").val();
    UserVM.FirstName = $('#FirstName').val();
    UserVM.LastName = $('#LastName').val();
    UserVM.BirthDate = $('#BirthDate').val();
    UserVM.PhoneNumber = $('#PhoneNumber').val();
    UserVM.Address = $('#Address').val();
    UserVM.ReligionId = $('#ReligionOption').val();
    UserVM.StateId = $('#StateOption').val();
    UserVM.DistrictId = $('#DistrictOption').val();
    UserVM.ZipcodeId = $('#ZipcodeOption').val();
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

function ChangePass() {
    var status = checkForm();
    if (status === true) {
        var UserVM = new Object();
        UserVM.Id = $('#Id').val();
        UserVM.CurrentPassword = $('#CurrentPassword').val();
        UserVM.NewPassword = $('#NewPassword').val();
        $.ajax({
            type: 'POST',
            url: '/Users/ChangePassword/',
            data: UserVM
        }).then((result) => {
            //debugger;
            if (result.statusCode === 200) {
                Swal.fire({
                    position: 'center',
                    type: 'success',
                    title: 'Password Changed'
                }).then((result) => {
                    if (result.value) {
                        location.reload();
                    }
                });
            }
            else {
                Swal.fire('Error', 'Current Password is Wrong', 'error');
                ShowModal();
            }
        });
    }
}

function ShowPass1() {
    var x = document.getElementById("CurrentPassword");
    if (x.type === "password") {
        x.type = "text";
    } else {
        x.type = "password";
    }
}

function ShowPass2() {
    var x = document.getElementById("NewPassword");
    if (x.type === "password") {
        x.type = "text";
    } else {
        x.type = "password";
    }
}

function LoadDistrict(stateId) {
    //debugger;
    $('#DistrictOption').children().remove();
    $('#ZipcodeOption').children().remove();
    $.ajax({
        type: "Get",
        url: "/Districts/LoadDistrict/" + stateId,
        success: function (data) {
            Districts = data;
            var $option = $('#DistrictOption');
            $option.empty();
            $option.append($('<option/>').val(District).text(DistrictName).hide());
            $.each(Districts, function (i, val) {
                $option.append($('<option/>').val(val.id).text(val.name));
            });
        }
    });
    Zipcode = 0;
    ZipcodeName = "Select Zipcode";
}

function LoadZipcode(districtId) {
    $.ajax({
        type: "Get",
        url: "/Zipcodes/LoadZipcode/" + districtId,
        success: function (data) {
            Zipcodes = data;
            var $option = $('#ZipcodeOption');
            $option.empty();
            $option.append($('<option/>').val(Zipcode).text(ZipcodeName).hide());
            $.each(Zipcodes, function (i, val) {
                $option.append($('<option/>').val(val.id).text(val.name));
            });
        }
    });
}

function ShowModal() {
    $('#createModal').modal('show');
    $('#check').prop('checked', false);
    $('#check2').prop('checked', false);
    $('#CurrentPassword').val('').attr('type', "password");
    $('#NewPassword').val('').attr('type', "password");
    $('#CurrentPassword').val('');
    $('#NewPassword').val('');
}

function checkForm() {
    //debugger;
    var val = $('#NewPassword').val();
    if (val !== "") {
        if (val.length < 6) {
            Swal.fire('Error', 'Password Must Contain At Least Six Characters!', 'error');
            ShowModal();
            return false;
        }
        re = /[0-9]/;
        if (!re.test(val)) {
            Swal.fire('Error', 'Password Must Contain At Least One Number (0-9)!', 'error');
            ShowModal();
            return false;
        }
        re = /[a-z]/;
        if (!re.test(val)) {
            Swal.fire('Error', 'Password Must Contain At Least One Lowercase Letter (a-z)!', 'error');
            ShowModal();
            return false;
        }
        re = /[A-Z]/;
        if (!re.test(val)) {
            Swal.fire('Error', 'Password Must Contain At Least One Uppercase Letter (A-Z)!', 'error');
            ShowModal();
            return false;
        }
        re = /[@$!%*#?&]/;
        if (!re.test(val)) {
            Swal.fire('Error', 'Password Must Contain At Least One Special Character (@$!%*#?&)!', 'error');
            ShowModal();
            return false;
        }
    } else {
        Swal.fire('Error', 'Password Cannot Be Empty!', 'error');
        ShowModal();
        return false;
    }
    return true;
}

function SetDate() {
    var today = new Date();
    var dd = today.getDate();
    var mm = today.getMonth() + 1; //January is 0!
    var yyyy = today.getFullYear();
    if (dd < 10) {
        dd = '0' + dd;
    }
    if (mm < 10) {
        mm = '0' + mm;
    }
    today = yyyy + '-' + mm + '-' + dd;
    yyyy = yyyy - 50;
    min = yyyy + '-' + mm + '-' + dd;
    document.getElementById("BirthDate").setAttribute("max", today);
    document.getElementById("BirthDate").setAttribute("min", min);
}
