var table = null;
var Roles = [];
var Apps = [];
var Religions = [];
var id = null;
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
    //Load Data Table
    table = $('#Users').DataTable({ //Nama table pada index
        "ajax": {
            url: "/Users/LoadUser", //Nama controller/fungsi pada index controller
            type: "GET",
            dataType: "json",
            dataSrc: ""
        },
        "columnDefs": [
            { "orderable": false, "targets": 15 },
            { "searchable": false, "targets": 15 },
            { "searchable": false, "orderable": false, "targets": 0 }
        ],
        "columns": [
            {
                data: null, render: function (data, type, row, meta) {
                    return meta.row + meta.settings._iDisplayStart + 1;
                }
            },
            { "data": "email", "name": "Email" },
            { "data": "name", "name": "Name" },
            { "data": "gender", "name": "Gender" },
            { "data": "roleName", "name": "Role" },
            { "data": "appName", "name": "Application" },
            { "data": "address", "name": "Address" },
            { "data": "stateName", "name": "State" },
            { "data": "districtName", "name": "District" },
            { "data": "zipcodeName", "name": "Zipcode" },
            { "data": "phoneNumber", "name": "Phone Number" },
            {
                "data": "birthDate", "render": function (data) {
                    return moment(data).format('DD/MM/YYYY');
                }
            },
            { "data": "religionName", "name": "Religion" },
            {
                "data": "batchName", "name": "Batch", "render": function (data) {
                    if (data === null) {
                        return "Not Bootcamp";
                    } else {
                        return data;
                    }
                }
            },
            {
                "data": "className", "name": "Class", "render": function (data) {
                    if (data === null) {
                        return "Not Bootcamp";
                    } else {
                        return data;
                    }
                }
            },
            {
                data: null, render: function (data, type, row) {
                    return '<button type="button" class="btn btn-warning" id="BtnEdit" data-toggle="tooltip" data-placement="top" title="Edit" onclick="return GetById(' + row.id + ')"><i class="mdi mdi-pencil"></i></button> &nbsp; <button type="button" class="btn btn-danger" id="BtnDelete" data-toggle="tooltip" data-placement="top" title="Delete" onclick="return Delete(' + row.id + ')"><i class="mdi mdi-delete"></i></button>';
                }
            }
        ]
    });
    table.on('order.dt search.dt', function () {
        table.column(0, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
            cell.innerHTML = i + 1;
        });
    }).draw();
});

//Tampung dan tampilkan role kedalam selectoption
function LoadRole(element) {
    $.ajax({
        type: "Get",
        url: "/Roles/LoadRole",
        success: function (data) {
            Roles = data;
            var $option = $(element);
            $option.empty();
            $option.append($('<option/>').val('0').text('Select Role').hide());
            $.each(Roles, function (i, val) {
                $option.append($('<option/>').val(val.id).text(val.name));
            });
        }
    });
}
LoadRole($('#RoleOption'));

//Tampung dan tampilkan application kedalam selectoption
function LoadApp(element) {
    $.ajax({
        type: "Get",
        url: "/Applications/LoadApplication",
        success: function (data) {
            //debugger;
            Apps = data;
            var $option = $(element);
            $option.empty();
            $option.append($('<option/>').val('0').text('Select Application').hide());
            $.each(Apps, function (i, val) {
                //debugger;
                $option.append($('<option/>').val(val.id).text(val.name));
            });
        }
    });
}
LoadApp($('#AppOption'));

//Tampung dan tampilkan religion kedalam selectoption
function LoadReligion(element) {
    $.ajax({
        type: "Get",
        url: "/Religions/LoadReligion",
        success: function (data) {
            Religions = data;
            var $option = $(element);
            $option.empty();
            $option.append($('<option/>').val('0').text('Select Religion').hide());
            $.each(Religions, function (i, val) {
                $option.append($('<option/>').val(val.id).text(val.name));
            });
        }
    });
}
LoadReligion($('#ReligionOption'));

//Tampung dan tampilkan batch kedalam selectoption
function LoadBatch(element) {
    $.ajax({
        type: "Get",
        url: "/Batches/LoadBatch",
        success: function (data) {
            Batches = data;
            var $option = $(element);
            $option.empty();
            $option.append($('<option/>').val('0').text('Select Batch').hide());
            $.each(Batches, function (i, val) {
                $option.append($('<option/>').val(val.id).text(val.name));
            });
        }
    });
}
LoadBatch($('#BatchOption'));

//Tampung dan tampilkan class kedalam selectoption
function LoadClass(element) {
    $.ajax({
        type: "Get",
        url: "/Classes/LoadClass",
        success: function (data) {
            Classes = data;
            var $option = $(element);
            $option.empty();
            $option.append($('<option/>').val(State).text(StateName).hide());
            $.each(Classes, function (i, val) {
                $option.append($('<option/>').val(val.id).text(val.name));
            });
        }
    });
}
LoadClass($('#ClassOption'));

function LoadState() {
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
LoadState();

function ShowPass() {
    var x = document.getElementById("Password");
    if (x.type === "password") {
        x.type = "text";
    } else {
        x.type = "password";
    }
}

function ShowModal() {
    $('#createModal').modal('show');
    $('#check').prop('checked', false);
    $('#Email').attr('readonly', false);
    $('#RoleOption').attr('disabled', false);
    $('#BatchOption').attr('disabled', false);
    $('#ClassOption').attr('disabled', false);
    $('#Id').val('');
    $('#Email').val('');
    $('#FirstName').val('');
    $('#LastName').val('');
    $('#BirthDate').val('');
    $('#PhoneNumber').val('');
    $('#Address').val('');
    $("input[name= Gender][value= Male]").prop('checked', true);
    $('#RoleOption').val(0);
    $('#AppOption').val(0);
    $('#ReligionOption').val(0);
    $('#BatchOption').val(0);
    $('#ClassOption').val(0);
    $('#StateOption').val(0);
    $('#DistrictOption').val(0);
    $('#ZipcodeOption').val(0);
    $("#Save").show();
    $("#Edit").hide();
}

function Save() {
    //debugger;
    var UserVM = new Object();
    UserVM.Email = $('#Email').val();
    UserVM.Password = $('#Password').val();
    UserVM.FirstName = $('#FirstName').val();
    UserVM.LastName = $('#LastName').val();
    UserVM.Gender = $("input[name=Gender]:checked").val();
    UserVM.BirthDate = $('#BirthDate').val();
    UserVM.PhoneNumber = $('#PhoneNumber').val();
    UserVM.Address = $('#Address').val();
    UserVM.RoleId = $('#RoleOption').val();
    UserVM.App_Type = $('#AppOption').val();
    UserVM.ReligionId = $('#ReligionOption').val();
    UserVM.BatchId = $('#BatchOption').val();
    UserVM.ClassId = $('#ClassOption').val();
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
                title: 'User Added and Password Sended To Email'
            }).then((result) => {
                if (result.value) {
                    table.ajax.reload();
                }
            });
        }
        else {
            Swal.fire('Error', 'Failed to Add User', 'error');
            ShowModal();
        }
    });
}

function GetById(Id) {
    $.ajax({
        url: "/Users/GetById/" + Id,
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        async: false,
        success: function (result) {
            //debugger;
            $('#check').prop('checked', false);
            $('#Id').val(result.id);
            $('#Email').attr('readonly', true);
            $('#Email').val(result.email);
            //$('#Password').val(result.password).attr('type',"password");
            $('#FirstName').val(result.firstName);
            $('#LastName').val(result.lastName);
            $("input[name= Gender][value= " + result.gender + "]").prop('checked', true);
            $('#BirthDate').val(moment(result.birthDate).format('YYYY-MM-DD'));
            $('#PhoneNumber').val(result.phoneNumber);
            $('#Address').val(result.address);
            $('#RoleOption').attr('disabled', true);
            $('#RoleOption').val(result.roleId);
            $('#AppOption').val(result.app_Type);
            $('#ReligionOption').val(result.religionId);
            $('#BatchOption').attr('disabled', true)
            $('#BatchOption').val(result.batchId);
            $('#ClassOption').attr('disabled', true)
            $('#ClassOption').val(result.classId);
            State = result.stateId;
            StateName = result.stateName;
            LoadState();
            District = result.districtId;
            DistrictName = result.districtName;
            LoadDistrict(State);
            Zipcode = result.zipcodeId;
            ZipcodeName = result.zipcodeName;
            LoadZipcode(District);
            $("#createModal").modal('show');
            $("#Save").hide();
            $('#Edit').show();
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function Edit() {
    var UserVM = new Object();
    UserVM.Id = $('#Id').val();
    UserVM.Email = $('#Email').val();
    UserVM.Password = $('#Password').val();
    UserVM.FirstName = $('#FirstName').val();
    UserVM.LastName = $('#LastName').val();
    UserVM.Gender = $("input[name=Gender]:checked").val();
    UserVM.BirthDate = $('#BirthDate').val();
    UserVM.PhoneNumber = $('#PhoneNumber').val();
    UserVM.Address = $('#Address').val();
    UserVM.RoleId = $('#RoleOption').val();
    UserVM.App_Type = $('#AppOption').val();
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
                title: 'User Edit Succesfully'
            }).then((result) => {
                if (result.value) {
                    table.ajax.reload();
                }
            });
        }
        else {
            Swal.fire('Error', 'Failed to Edit User', 'error');
            ShowModal();
        }
    });
}

function Delete(Id) {
    Swal.fire({
        title: "Are you sure?",
        text: "You won't be able to revert this!",
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        //debugger;
        if (result.value) {
            $.ajax({
                url: "/Users/Delete/" + Id,
                data: { Id: Id }
            }).then((result) => {
                if (result.statusCode === 200) {
                    Swal.fire({
                        position: 'center',
                        type: 'success',
                        title: 'User Deleted Succesfully'
                    }).then((result) => {
                        if (result.value) {
                            table.ajax.reload();
                        }
                    });
                }
                else {
                    Swal.fire('Error', 'Failed to Delete User', 'error');
                    ShowModal();
                }
            });
        }
    });
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
