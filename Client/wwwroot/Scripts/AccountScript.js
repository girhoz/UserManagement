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
            $('#FullName').val(result.fullName);
            $('#FirstName').val(result.firstName);
            $('#LastName').val(result.lastName);
            $('#BirthDate').val(moment(result.birthDate).format('YYYY-MM-DD'));
            $('#PhoneNumber').val(result.phoneNumber);
            $('#Address').val(result.address);
            Religion = result.religionId;
            ReligionName = result.religionName;
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