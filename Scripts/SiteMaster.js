$(document).ready(function () {
    $('.frmDate').keypress(function (e) { e.preventDefault(); });
    $('.toDate').keypress(function (e) { e.preventDefault(); });
    $('.frmDate1').keypress(function (e) { e.preventDefault(); });
    $('.toDate1').keypress(function (e) { e.preventDefault(); });
    $('.datepicker').keypress(function (e) { e.preventDefault(); });
    $('.empdatepicker').keypress(function (e) { e.preventDefault(); });
    $('.dtpicker').keypress(function (e) { e.preventDefault(); });
    $('.clkpicker').keypress(function (e) { e.preventDefault(); });
    $('.txtPrevent').keypress(function (e) { e.preventDefault(); });

    $(".datepicker").datepicker(
        {
            dateFormat: 'dd/mm/yy',
            changeMonth: true,
            changeYear: true,
            yearRange: "-70:+0",
            maxDate: 0
        });

    $('.txtdec').keyup(function () {
        var val = $(this).val();
        if (isNaN(val)) {
            val = val.replace(/[^0-9\.]/g, '');
            if (val.split('.').length > 2)
                val = val.replace(/\.+$/, "");
        }
        $(this).val(val);
    });

    $(".empdatepicker").datepicker(
        {
            dateFormat: 'dd/mm/yy',
            yearRange: "-50:+0",
            changeMonth: true,
            changeYear: true,
            maxDate: 0
        });
    $(".frmDate").datepicker({
        dateFormat: 'dd/mm/yy',
        maxDate: 0,
        numberOfMonths: 1,
        changeMonth: true,
        changeYear: true,
        onSelect: function (selected) {
            $(".toDate").datepicker("option", "minDate", selected)
        }
    });
    $(".toDate").datepicker({
        dateFormat: 'dd/mm/yy',
        minDate: 0,
        maxDate: 0,
        changeMonth: true,
        changeYear: true,
        onSelect: function (selected) {
            $(".frmDate").datepicker("option", "maxDate", selected)
        }
    });
    $(".frmDate1").datepicker({
        dateFormat: 'dd/mm/yy',
        maxDate: 0,
        numberOfMonths: 1,
        changeMonth: true,
        changeYear: true,
        onSelect: function (selected) {
            $(".toDate1").datepicker("option", "minDate", selected)
        }
    });
    $(".toDate1").datepicker({
        dateFormat: 'dd/mm/yy',
        minDate: 0,
        maxDate: 365,
        changeMonth: true,
        changeYear: true,
        onSelect: function (selected) {
            $(".frmDate1").datepicker("option", "maxDate", selected)
        }
    });

    //$('.clkpicker').clockpicker({
    //    placement: 'bottom',
    //    align: 'left',
    //    donetext: 'Done'
    //});

    //$('.clkpicker1').clockpicker({
    //    placement: 'bottom',
    //    align: 'left',
    //    donetext: 'Done'
    //});


    $(".dtpicker").datepicker(
        {
            dateFormat: 'dd/mm/yy',
            changeMonth: true,
            changeYear: true,
            maxDate: 365
        });
    $(".sidebar-dropdown > a").click(function () {
        $(".sidebar-submenu").slideUp(200);
        if ($(this).parent().hasClass("active")) {
            $(".sidebar-dropdown").removeClass("active");
            $(this).parent().removeClass("active");
        } else {
            $(".sidebar-dropdown").removeClass("active");
            $(this).next(".sidebar-submenu").slideDown(200);
            $(this).parent().addClass("active");
        }
    });

    $("#close-sidebar").click(function () {
        $(".page-wrapper").removeClass("toggled");
    });
    $("#show-sidebar").click(function () {
        $(".page-wrapper").addClass("toggled");
    });

    $('#table').dataTable({
        aoColumnDefs: [{
            bSortable: false,
            aTargets: [0, -1, -2, 2]
        }]
    });
    //$('.customSelect').fSelect();
});


function sidebar_toggle() {
    $(".page-wrapper").toggleClass("toggled");
}

function prle_d_tgle() {
    $(".profile_card_design").slideToggle("slow");
    $('.profilelogostyle').toggleClass('toglebdecls');
}


/************* End ********************/

const lineChartDrow = data => {
    const {
        disposalCount,
        dryWeight,
        totalWeight,
        wetWeight
    } = data;
    $(document).ready(function () {
        // line Chart
        Highcharts.chart('lineChart', {
            chart: {
                type: 'areaspline'
            },
            title: {
                text: 'Wastage collected graph '
            },
            xAxis: {
                allowDecimals: false,
                categories: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'],
                labels: {
                    formatter: function (e) {
                        return this.value;
                    }
                }
            },
            yAxis: {
                title: {
                    text: 'Wastage'
                },
                labels: {
                    formatter: function () {
                        return this.value + 'kg';
                    }
                }
            },
            tooltip: {
                pointFormat: '{series.name} had stockpiled <b>{point.y:,.0f}</b><br/>Kg'
            },
            series: [{
                name: 'Dry Wastage',
                data: dryWeight
            }, {
                name: 'Wet Wastage',
                data: wetWeight
            },
            {
                name: 'Disposed Wastage',
                data: disposalCount
            },
            {
                name: 'Total Wastage',
                data: totalWeight
            }

            ]
        });
        //piChart

    });

}


const pieChartDrow = data => {
    console.log(data)
    Highcharts.chart('piChart', {
        chart: {
            plotBackgroundColor: null,
            plotBorderWidth: null,
            plotShadow: false,
            type: 'pie'
        },
        title: {
            text: 'Month to Date – Wastage collected graph '
        },
        tooltip: {
            pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>'
        },
        plotOptions: {
            pie: {
                allowPointSelect: true,
                cursor: 'pointer',
                dataLabels: {
                    enabled: true,
                    format: '<b>{point.name}</b>: {point.text} Kg',
                    style: {
                        color: (Highcharts.theme && Highcharts.theme.contrastTextColor) || 'black'
                    },
                    connectorColor: 'silver'
                }
            }
        },
        series: [{
            data,
            name: 'Share',

        }]
    });

}



const BarChartDrow = data => {
    const {
        disposalCount,
        dryWeight,
        totalWeight,
        wetWeight
    } = data;
    Highcharts.chart('barChart', {
        chart: {
            type: 'column'
        },
        title: {
            text: 'Last 4 Monthly wastage collected graph '
        },

        xAxis: {
            categories: [
                'Jan',
                'Feb',
                'Mar',
                'Apr',

            ],
            crosshair: true
        },
        yAxis: {
            title: {
                text: 'Wastage'
            },
            labels: {
                formatter: function () {
                    return this.value + 'kg';
                }
            }
        },
        tooltip: {
            headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
            pointFormat: '<tr><td style="color:{series.color};padding:0">{series.name}: </td>' +
                '<td style="padding:0"><b>{point.y:.1f} Kg</b></td></tr>',
            footerFormat: '</table>',
            shared: true,
            useHTML: true
        },
        plotOptions: {
            column: {
                pointPadding: 0.2,
                borderWidth: 0
            }
        },
        series: [{
            name: 'Wet Wastage',
            data: wetWeight

        }, {
            name: 'Dry Wastage',
            data: dryWeight

        }, {
            name: 'Disposed Wastage',
            data: disposalCount

        }, {
            name: 'Total Wastage',
            data: totalWeight

        }]
    });

}

$('.datepicker1').keypress(function (e) { e.preventDefault(); });
$(".datepicker1").datepicker(
    {
        dateFormat: 'dd/mm/yy',
        changeMonth: false,
        changeYear: false,
        minDate: 0,

    });



$('.taxdatepicker').keypress(function (e) { e.preventDefault(); });

$(".taxdatepicker").datepicker(
    {
        dateFormat: 'dd/mm/yy',
        changeMonth: true,
        changeYear: true,
        yearRange: "-70:+0"
    });


$('.OfferfrmDate').keypress(function (e) { e.preventDefault(); });
$('.OffertoDate').keypress(function (e) { e.preventDefault(); });

$(".OfferfrmDate").datepicker({
    dateFormat: 'dd/mm/yy',
    minDate: 0,
    numberOfMonths: 1,
    changeMonth: true,
    changeYear: true,
    onSelect: function (selected) {
        $(".OffertoDate").datepicker("option", "minDate", selected)
    }
});
$(".OffertoDate").datepicker({
    dateFormat: 'dd/mm/yy',
    minDate: 0,
    changeMonth: true,
    changeYear: true,
    onSelect: function (selected) {
        $(".OfferfrmDate").datepicker("option", "maxDate", selected)
    }
});