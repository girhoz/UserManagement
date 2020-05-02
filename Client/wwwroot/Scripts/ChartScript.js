$(document).ready(function () {
    //Load Chart
    UserAppChart();
    UserReligionChart();
    UserBatchChart();
    UserClassChart();
});

function UserAppChart() {
    $.ajax({
        type: 'GET',
        url: '/Charts/GetUserApp/',
        success: function (data) {
            Morris.Donut({
                element: 'UserAppChart',
                data: $.each(JSON.parse(data), function (index, val) {
                    //debugger;
                    [{
                        label: val.label,
                        value: val.value
                    }]
                }),
                resize: true,
                colors: ['#009efb', '#55ce63', '#2f3d4a', '#0051FF', '#00FFEC', '#1AD700', '#018284', '#01847C', '#258401', '#19BDFF']
            });
        }
    })
};

function UserReligionChart() {
    $.ajax({
        type: 'GET',
        url: '/Charts/GetUserReligion/',
        success: function (data) {
            Morris.Donut({
                element: 'UserReligionChart',
                data: $.each(JSON.parse(data), function (index, val) {
                    [{
                        label: val.label,
                        value: val.value
                    }]
                }),
                resize: true,
                colors: ['#009efb', '#55ce63', '#2f3d4a', '#0051FF', '#00FFEC', '#1AD700', '#018284', '#01847C', '#258401', '#19BDFF']
            });
        }
    })
};

function UserBatchChart() {
    $.ajax({
        type: 'GET',
        url: '/Charts/GetUserBatch/',
        success: function (data) {
            Morris.Donut({
                element: 'UserBatchChart',
                data: $.each(JSON.parse(data), function (index, val) {
                    [{
                        label: val.label,
                        value: val.value
                    }]
                }),
                resize: true,
                colors: ['#009efb', '#55ce63', '#2f3d4a', '#0051FF', '#00FFEC', '#1AD700', '#018284', '#01847C', '#258401', '#19BDFF']
            });
        }
    })
};

function UserClassChart() {
    $.ajax({
        type: 'GET',
        url: '/Charts/GetUserClass/',
        success: function (data) {
            Morris.Donut({
                element: 'UserClassChart',
                data: $.each(JSON.parse(data), function (index, val) {
                    [{
                        label: val.label,
                        value: val.value
                    }]
                }),
                resize: true,
                colors: ['#009efb', '#55ce63', '#2f3d4a', '#0051FF', '#00FFEC', '#1AD700', '#018284', '#01847C', '#258401', '#19BDFF']
            });
        }
    })
};