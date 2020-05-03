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
                colors: ['#009efb', '#55ce63', '#2f3d4a', '#D93117', '#CBBA0D', '#47CB0D', '#018284', '#720DCB', '#CB0D0D', '#9C9191']
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
                colors: ['#009efb', '#55ce63', '#2f3d4a', '#D93117', '#CBBA0D', '#47CB0D', '#018284', '#720DCB', '#CB0D0D', '#9C9191']
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
                colors: ['#009efb', '#55ce63', '#2f3d4a', '#D93117', '#CBBA0D', '#47CB0D', '#018284', '#720DCB', '#CB0D0D', '#9C9191']
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
                colors: ['#009efb', '#55ce63', '#2f3d4a', '#D93117', '#CBBA0D', '#47CB0D', '#018284', '#720DCB', '#CB0D0D', '#9C9191']
            });
        }
    })
};