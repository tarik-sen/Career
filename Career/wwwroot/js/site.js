function CityOf(CountrySelector, citySelectionSelector, activeCity) {
    $.ajax({
        url: "GetData?handler=CityOf",
        method: "POST",
        beforeSend: function (xhr) {
            xhr.setRequestHeader("XSRF-TOKEN", $('input:hidden[name="__RequestVerificationToken"]').val());
        },
        data: { countryId: $(CountrySelector).val() },
        success: function (data) {
            var citySelect = $(citySelectionSelector);

            var s = "<option value>--</option>";
            data.forEach(function (item) {
                s += `<option value=${item.value}>${item.text}</option>`;
            });

            citySelect.html(s);

            if (activeCity !== undefined)
                citySelect.val(activeCity);

        },
        error: function (error) {
            console.error(error);
        }

    });
}

function GetCitiesOf(CountrySelector, citySelectionSelector, activeCity) {
    $.ajax({
        url: '/Broad/GetCitiesOf',
        method: "GET",
        data: { countryId: $(CountrySelector).val() },
        success: function (data) {
            var citySelect = $(citySelectionSelector);

            var s = "<option value>--</option>";
            data.forEach(function (item) {
                s += `<option value=${item.value}>${item.text}</option>`;
            });

            citySelect.html(s);

            if (activeCity !== undefined)
                citySelect.val(activeCity);

        },
        error: function (error) {
            console.error(error);
        }

    });
}



function GetChartDataFrom(url) {
    return new Promise((resolve, reject) => {
        $.ajax({
            url: url,
            method: "GET",
            success: function (data) {
                resolve(data);
            },
            error: function (error) {
                reject(error);
            }
        });
    });
}

function DrawPieChart(title, elementId, jsonData) {
    var data = new google.visualization.DataTable(jsonData);

    var startColor = chroma('#42c3ca'); // --primary
    var endColor = chroma('#6610f2');   // --indigo

    var colorScale = chroma.scale([startColor, endColor]).mode('lch').colors(data.getNumberOfRows());

    var options = {
        title: title,
        titleTextStyle: { fontSize: 16 },
        width: '100%',
        height: '100%',
        chartArea: {
            width: '100%',
            height: '100%',
            top: 56,
            bottom: 16
        },
        fontSize: 13,
        legend: {
            position: 'top'
        },
        colors: colorScale,
    };

    var chart = new google.visualization.PieChart(document.getElementById(elementId));
    chart.draw(data, options);
}


function SetApplicantNumber(jobId, resultSelector) {
    $.ajax({
        url: "/Broad/GetNumberOfApplicants",
        method: "GET",
        data: { jobId: jobId },
        success: function (data) {
            $(resultSelector).text(data.count);
        },
        error: function (error) {
            console.error(error);
        }
    });
}

// Dependent to Admin/Jobs/ 
function NotifyApplicant(appliedJobId, containerSelector) {
    $.ajax({
        url: "Applicant?Handler=SendNotification",
        method: "POST",
        beforeSend: function (xhr) {
            xhr.setRequestHeader("XSRF-TOKEN", $('input:hidden[name="__RequestVerificationToken"]').val());
        },
        data: {
            appliedJobId: appliedJobId
        },
        success: function (data) {
            $(containerSelector).prepend(data);
        },
        error: function (error) {
            console.error(error);
        }
    });
}