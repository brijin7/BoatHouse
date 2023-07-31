
function ShowOtherGSTCalculation(Charge, TaxPer, TaxCount, Output1, Output2) {
    var inputnum = document.getElementById(Charge).value;
    var amt = parseInt(inputnum);
    var rowerchr = 0;

    if (document.getElementById(TaxPer).value == '') {
        alert('Select Tax');

        document.getElementById(Charge).value = ""; 
        document.getElementById(Output1).value = "0";
        document.getElementById(Output2).value = "0";

        return
    }

    if (document.getElementById(TaxPer).value != 'Nil Tax') {

        function round(value, decimals) {
            let sum = Number(Math.round(value + 'e' + decimals) + 'e-' + decimals);
            let gst = parseFloat(sum) * parseFloat(document.getElementById(TaxCount).value);
            roundamount(inputnum - (parseFloat(rowerchr) + parseFloat(gst)), 2);
            document.getElementById(Output2).value = gst;
            return Number(Math.round(value + 'e' + decimals) + 'e-' + decimals);
        }

        round((inputnum / (100 + (parseFloat(document.getElementById(TaxPer).value)
            * parseFloat(document.getElementById(TaxCount).value)))) * parseFloat(document.getElementById(TaxPer).value), 2);

        function roundamount(value, decimals) {
            let amount = Number(Math.round(value + 'e' + decimals) + 'e-' + decimals);
            document.getElementById(Output1).value = amount;
            return Number(Math.round(value + 'e' + decimals) + 'e-' + decimals);
        }
    }
    else {
        document.getElementById(Output1).value = inputnum;
        document.getElementById(Output2).value = "0";
    }
} 

function ShowBoatGSTCalculation(Charge, Rower, TaxPer, TaxCount, Output1, Output2) {
    var inputnum = document.getElementById(Charge).value;
    var amt = parseInt(inputnum);
    var rowerchr = document.getElementById(Rower).value;

    if (!(inputnum)) {
        inputnum = 0;
    }

    if (!(rowerchr)) {
        rowerchr = 0;
    }

    if (parseFloat(rowerchr) >= parseFloat(inputnum)) {

        document.getElementById(Output1).value = "0";
        document.getElementById(Output2).value = "0";
        return;
    }

    if (document.getElementById(TaxPer).value == '') {
        alert('Select Tax');

        document.getElementById(Charge).value = "";
        document.getElementById(Output1).value = "0";
        document.getElementById(Output2).value = "0";

        return
    }

    if (document.getElementById(TaxPer).value != 'Nil Tax') {

        function round(value, decimals) {
            let sum = Number(Math.round(value + 'e' + decimals) + 'e-' + decimals);
            let gst = parseFloat(sum) * parseFloat(document.getElementById(TaxCount).value);
            roundamount(inputnum - (parseFloat(rowerchr) + parseFloat(gst)), 2);
            document.getElementById(Output2).value = gst;
            return Number(Math.round(value + 'e' + decimals) + 'e-' + decimals);
        }

        round((inputnum / (100 + (parseFloat(document.getElementById(TaxPer).value)
            * parseFloat(document.getElementById(TaxCount).value)))) * parseFloat(document.getElementById(TaxPer).value), 2);

        function roundamount(value, decimals) {
            let amount = Number(Math.round(value + 'e' + decimals) + 'e-' + decimals);
            document.getElementById(Output1).value = amount;
            return Number(Math.round(value + 'e' + decimals) + 'e-' + decimals);
        }
    }
    else {
        document.getElementById(Output1).value = inputnum;
        document.getElementById(Output2).value = "0";
    }
} 
