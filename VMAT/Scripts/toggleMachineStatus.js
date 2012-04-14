﻿// File: toggleMachineStatus.js

var toggleStatusClick = function () {
    var id = $(this).closest(".machine-info").attr("machineid");
    $(this).addClass("transition");
    $(this).attr("disabled", "disabled");

    toggleMachineStatus(id, this);
}

$(document).ready(function () {
    $(".status > button").click(toggleStatusClick);
});

function toggleMachineStatus(id, button) {
    var successCallback = function(data, button) {
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
    };

    var failureCallback = function (error, id, button) {
        alert("Failed to change machine " + id + "'s status: " + error.status);
        resetTransitionButton(button);
    };

    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: $.url("toggleMachineStatus"),
        data: "{'id': '" + id + "'}",
        dataType: "json",
        success: function (data) { successCallback(data, button); },
        error: function (error) { failureCallback(error, id, button); }
    });
}

function resetTransitionButton(button) {
    $(button).removeClass("transition");
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
