// File: vmManagement.js

var UndoPendingOperation = {};

$(document).ready(function () {
    // Activate items if JavaScript is enabled
    $(".machine-info .details").hide();
    $(".status button").attr("title", function () {
        setStatusTooltips($(this));
    });

    $(".project-display").click(function () {
        if ($(this).text() === "V") {
            $(this).text(">");
        } else {
            $(this).text("V");
        }

        $(this).closest(".project").children(".project-machines").slideToggle(300);
    });

    $(".toggle-details").click(function () {
        var $detailsDiv = $(this).closest(".machine-info").children(".details");

        if ($(this).text() === "Show Less") {
            $(this).text("Show More");
        } else {
            $(this).text("Show Less");
        }

        $detailsDiv.slideToggle(300);
    });

    $(".undo-pending").click(function () {
        var $container = $(this).closest(".machine-info");
        undoPendingOperation($container);
    });
});

function undoPendingOperation($container) {
    var imagePath = $container.attr("id");

    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "/VirtualMachine/UndoPendingOperation",
        data: "{'image': '" + imagePath + "'}",
        dataType: "json",
        success: function (data) { UndoPendingOperation.successCallback($container); },
        error: function (error) { UndoPendingOperation.failureCallback(error, imagePath); }
    });
    //UndoPendingOperation.successCallback($container); FOR TESTING
}

UndoPendingOperation.successCallback = function ($container) {
    var $project = $container.closest(".project");

    if ($container.closest("li").siblings().length >= 1) {
        $container.animate({ height: "toggle", opacity: "toggle" }, 400, function () {
            $container.closest("li").remove();
        });
    } else {
        $project.animate({ height: "toggle", opacity: "toggle" }, 400, function () {
            $project.closest("li").remove();
        });
    }
};

UndoPendingOperation.failureCallback = function (error, imagePath) {
    alert("Failed to undo operation on " + imagePath + ": " + error.status + " - " +
        JSON.parse(error.responseText));
};
