// File: toggleMachineStatus.js

// Declare ToggleMachineStatus namespace
var ToggleMachineStatus = {};

$(document).ready(function () {
    $(".status > button").click(function () {
        var $imagePath = $(this).closest(".machine-info").attr("id");
        $(this).addClass("transition");
        $(this).html("?");
        $(this).attr("disabled", "disabled");

        toggleMachineStatus($imagePath, this);
    });
});

function toggleMachineStatus(machineName, button) {
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "VirtualMachine/ToggleStatus",
        data: "{'image': '" + machineName + "'}",
        dataType: "json",
        success: function (data) { ToggleMachineStatus.successCallback(data, button); },
        error: function (error) { ToggleMachineStatus.failureCallback(error, machineName, button); }
    });
}

ToggleMachineStatus.successCallback = function (data, button) {
    var status = ((String)(data.Status)).toLowerCase();
    var $machine = $(button).closest(".machine-info");

    var milli = data.LastShutdownTime.replace(/\/Date\((-?\d+)\)\//, '$1');
    var stopped = new Date(parseInt(milli));
    
    milli = data.LastStartTime.replace(/\/Date\((-?\d+)\)\//, '$1');
    var started = new Date(parseInt(milli));

    $(button).attr("status", status);
    $machine.find(".tStopped").text(dateFormat(stopped, "m/dd/yyyy HH:MM:ss"));
    $machine.find(".tStarted").text(dateFormat(started, "m/dd/yyyy HH:MM:ss"));
    setStatusTooltips(button);
    resetTransitionButton(button);
}

ToggleMachineStatus.failureCallback = function (error, machineName, button) {
    alert("Failed to change machine " + machineName + "'s status: " + error.Status);
    resetTransitionButton(button);
}

function resetTransitionButton(button) {
    $(button).removeClass("transition");
    $(button).html("");
    $(button).removeAttr("disabled");
}

function setStatusTooltips(button) {
    $(button).attr("title", function () {
        status = $(this).attr("status");

        if (status === "running")
            return "Power Off";
        else if (status === "stopped")
            return "Power On";
        else if (status === "poweringon")
            return "Powering On";
        else if (status === "poweringoff")
            return "Powering Off";
    });
}
