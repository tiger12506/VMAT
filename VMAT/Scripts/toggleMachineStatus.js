// File: toggleMachineStatus.js

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
        url: "/VirtualMachine/ToggleStatus",
        data: "{'image': '" + machineName + "'}",
        dataType: "json",
        success: function (data) { successCallback(data, button); },
        error: function (error) { failureCallback(error, machineName, button); }
    });
}

function successCallback(data, button) {
    var status = ((String) (data.Status)).toLowerCase();
    $(button).attr("status", status);
    resetTransitionButton(button);
}

function failureCallback(error, machineName, button) {
    alert("Failed to change machine " + machineName + "'s status: " + error.Status);
    resetTransitionButton(button);
}

function resetTransitionButton(button) {
    $(button).removeClass("transition");
    $(button).html("");
    $(button).removeAttr("disabled");
}