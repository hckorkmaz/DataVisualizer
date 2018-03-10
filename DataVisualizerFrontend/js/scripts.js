var apiAddress = "http://localhost:1881/api";

var server, databaseName, username, password;
var databaseObjects = [];
var selectedObjectsData = [];
var selectedObject;
var chart;

var dbInformationFormValidate;

$(function () {
    $("#ddDatabaseObjects").chosen({ width: '100%', search_contains: true });
    $("#ddLabelColumn").chosen({ width: '100%', search_contains: true });
    $("#ddDataColumn").chosen({ width: '100%', search_contains: true });
    $("#ddChartType").chosen({ width: '100%', search_contains: true });

    // Send SQL Information through the form.
    dbInformationFormValidate = $('#databaseInformationForm').validate({
        rules: {
            Server: {
                required: true
            },
            DatabaseName: {
                required: true
            },
            Username: {
                required: true
            },
            Password: {
                required: true
            }
        },
        messages: {
            Server: {
                required: "Please enter a server IP address"
            },
            DatabaseName: {
                required: "Please enter a database name"
            },
            Username: {
                required: "Please enter a username"
            },
            pasPasswordsword: {
                required: "Please enter a password"
            }
        },
        errorElement: "span",
        errorPlacement: function (error, element) {
            error.addClass("help-block");

            if (element.prop("type") === "checkbox") {
                error.insertAfter(element.parent("label"));
            } else {
                error.insertAfter(element);
            }
        },
        highlight: function (element, errorClass, validClass) {
            $(element).parents(".form-group").addClass("has-error").removeClass("has-success");
        },
        unhighlight: function (element, errorClass, validClass) {
            $(element).parents(".form-group").addClass("has-success").removeClass("has-error");
        },
        submitHandler: function () {
            server = $('#Server').val();
            databaseName = $('#DatabaseName').val();
            username = $('#Username').val();
            password = $('#Password').val();

            var input = {
                Server: server,
                DatabaseName: databaseName,
                Username: username,
                Password: password
            };

            postData(apiAddress + "/DatabaseObject/GetAll", input, function (data) {
                databaseObjects = data;

                var ddDatabaseObjects = $("#ddDatabaseObjects");
                $.each(data, function (idx, obj) {
                    ddDatabaseObjects.append('<option value="' + obj.Name + '">' + obj.Name + '</option>');
                });

                ddDatabaseObjects.trigger("chosen:updated");

                $('#databaseInformation').hide(500, function () {
                    $('#databaseObjects').show(500);
                });
            });
        }
    });
})

// Gets database object's data by providing required data
function getDatabaseData() {

    var input = {
        DatabaseConfig: {
            Server: server,
            DatabaseName: databaseName,
            Username: username,
            Password: password
        },
        DbObjectName: selectedObject.Name,
        DbObjectType: selectedObject.Type
    }

    postData(apiAddress + "/DatabaseData/GetData", input, function (data) {
        selectedObjectsData = data;

        var ddLabelColumn = $("#ddLabelColumn"); ddLabelColumn.empty();
        var ddDataColumn = $("#ddDataColumn"); ddDataColumn.empty();

        if (data.length == 0) {
            ddLabelColumn.trigger("chosen:updated");
            ddDataColumn.trigger("chosen:updated");

            $("#labelColumnFormGroup").hide();
            $("#dataColumnFormGroup").hide();
            $("#chartTypeFormGroup").hide();
            $("#visualizeData").hide();

            toastr.warning("Data not found", "Warning");
            return;
        }

        var exampleRecord = data[0];
        var keys = Object.keys(exampleRecord);

        keys.forEach(function (k) {
            ddLabelColumn.append('<option value="' + k + '">' + k + '</option>');
            ddDataColumn.append('<option value="' + k + '">' + k + '</option>');
        });

        ddLabelColumn.trigger("chosen:updated");
        ddDataColumn.trigger("chosen:updated");

        $("#labelColumnFormGroup").show();
        $("#dataColumnFormGroup").show();
        $("#chartTypeFormGroup").show();
        $("#visualizeData").show();
        toastr.remove()
    });
}

// Calls renderChart method and shows chartModal
function visualizeData() {
    renderChart();
    $('#chartModal').modal('show');
}

// Renders chart with provided chart options.
function renderChart() {
    if (chart != null)
        chart.destroy();

    var ctx = document.getElementById('myChart').getContext('2d');

    var chartType = $('#ddChartType').val();
    var labelColumn = $('#ddLabelColumn').val();
    var dataColumn = $('#ddDataColumn').val();

    var labelList = [];
    var dataList = [];
    var colors = (chartType == 'pie' || chartType == 'doughnut') ? [] : null;

    var dynamicColors = function () {
        var r = Math.floor(Math.random() * 255);
        var g = Math.floor(Math.random() * 255);
        var b = Math.floor(Math.random() * 255);
        return "rgb(" + r + "," + g + "," + b + ")";
    };

    for (var i = 0; i < selectedObjectsData.length; i++) {
        var record = selectedObjectsData[i];

        labelList.push(record[labelColumn]);
        dataList.push(record[dataColumn]);

        if (chartType == 'pie' || chartType == 'doughnut') {
            colors.push(dynamicColors());
        } else {
            if (i == 0)
                colors = dynamicColors();
        }

    }

    chart = new Chart(ctx, {
        type: chartType,
        data: {
            labels: labelList,
            datasets: [{
                label: dataColumn,
                backgroundColor: colors,
                borderColor: 'rgba(200, 200, 200, 0.75)',
                hoverBorderColor: 'rgba(200, 200, 200, 1)',
                data: dataList,
            }]
        },
        options: {}
    });
}

// Triggered when database object dropdown changed.
function selectObject(objectName) {
    var filteredObjects = databaseObjects.filter(function (obj) {
        return obj.Name == objectName;
    });

    if (filteredObjects && filteredObjects.constructor === Array && filteredObjects.length > 0) {
        selectedObject = filteredObjects[0];
        getDatabaseData();
    };
}

// Posts data using Ajax request
function postData(_url, _data, onSuccess) {
    $('#loading').modal('show');
    $.ajax({
        type: "POST",
        url: _url,
        data: _data,
        dataType: "json",
        success: onSuccess,
        error: function (err) {
            console.error("AJAX post request failed. Server response: ", err);
            if (err.status == 500) {
                toastr.error(err.responseJSON.ExceptionMessage, 'SQL Error');
            } else if (err.status == 400) {
                App.ModelState.showResponseErrors($('#databaseInformationForm'), err.responseJSON);
            }
        },
        complete: function () {
            $('#loading').modal('hide');
        }
    })
}