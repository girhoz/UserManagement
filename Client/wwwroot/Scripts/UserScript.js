var table = null;
var Roles = [];
var Apps = [];
var Religions = [];
$(document).ready(function () {
    //Load Data Table
    table = $('#Users').DataTable({ //Nama table pada index
        "ajax": {
            url: "/Users/LoadUser", //Nama controller/fungsi pada index controller
            type: "GET",
            dataType: "json",
            dataSrc: ""
        },
        "columnDefs": [
            { "orderable": false, "targets": 8 },
            { "searchable": false, "targets": 8 },
            { "searchable": false, "orderable": false, "targets": 0 }
        ],
        "columns": [
            {
                data: null, render: function (data, type, row, meta) {
                    return meta.row + meta.settings._iDisplayStart + 1;
                }
            },
            { "data": "email", "name": "Email" },
            { "data": "fullName", "name": "Name" },
            { "data": "roleName", "name": "Role" },
            { "data": "appName", "name": "Application" },
            { "data": "address", "name": "Address" },
            { "data": "phoneNumber", "name": "Phone Number" },
            {
                "data": "birthDate", "render": function (data) {
                    return moment(data).format('DD/MM/YYYY');
                }
            },
            { "data": "religionName", "name": "Religion" },
            {
                data: null, render: function (data, type, row) {
                    return " <td><button type='button' class='btn btn-warning' Id='Update' onclick=GetById('" + row.id + "');>Edit</button> <button type='button' class='btn btn-danger' Id='Delete' onclick=Delete('" + row.id + "');>Delete</button ></td >";
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

function ShowPass() {
    var x = document.getElementById("Password");
    if (x.type === "password") {
        x.type = "text";
    } else {
        x.type = "password";
    }
}

function ShowModal() {
    $("#createModal").modal('show');
    $('#Email').show();
    $('#EmailLabel').show();
    $('#Id').val('');
    $('#Email').val('');
    $('#Password').val('');
    $('#FullName').val('');
    $('#FirstName').val('');
    $('#LastName').val('');
    $('#BirthDate').val('');
    $('#PhoneNumber').val('');
    $('#Address').val('');
    $('#RoleOption').val(0);
    $('#AppOption').val(0);
    $('#ReligionOption').val(0);
    $("#Save").show();
    $("#Edit").hide();
}

function Save() {
    //debugger;
    var UserVM = new Object();
    UserVM.Email = $('#Email').val();
    UserVM.Password = $('#Password').val();
    UserVM.FullName = $('#FullName').val();
    UserVM.FirstName = $('#FirstName').val();
    UserVM.LastName = $('#LastName').val();
    UserVM.BirthDate = $('#BirthDate').val();
    UserVM.PhoneNumber = $('#PhoneNumber').val();
    UserVM.Address = $('#Address').val();
    UserVM.RoleId = $('#RoleOption').val();
    UserVM.App_Type = $('#AppOption').val();
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
                title: 'User Added Succesfully'
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