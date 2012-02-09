var $projectName;

$(document).ready(function () {
    enableProjectControls();

    enableToggleDetails();

    enablePendingOperationControls();
});


// Create event handlers for DOM elements
function enableProjectControls() {
    $(".project-close").click(function () {
        $projectName = $(this).closest(".project").attr("id");
        Popup.loadPopup("Close Project " + $projectName + "?", "#project-close-form");

        $("#popup-content button.archive").click(function () {
            archiveProject($projectName);
            Popup.disablePopup();
        });

        $("#popup-content button.delete").click(function () {
            deleteProject($projectName);
            Popup.disablePopup();
        });
    });

    $(".project-display").click(function () {
        if ($(this).text() === "v") {
            $(this).text(">");
        } else {
            $(this).text("v");
        }

        $(this).closest(".project").children(".project-machines").slideToggle(300);
    });
}

function enableToggleDetails() {
    // Activate items if JavaScript is enabled
    $(".machine-info .details").hide();
    $(".status button").attr("title", function () {
        setStatusTooltips($(this));
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
}

function enablePendingOperationControls() {
    $(".pending-vm .undo-pending").click(function () {
        var $container = $(this).closest(".machine-info");
        undoPendingCreateOperation($container);
    });

    $(".pending-archive-vm .undo-pending").click(function () {
        var $container = $(this).closest(".machine-info");
        undoPendingArchiveOperation($container);
    });
}

function undoPendingCreateOperation($container) {
    var imagePath = $container.attr("id");

    var successCallback = function($container) {
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

    var failureCallback = function(error, imagePath) {
        alert("Failed to undo operation on " + imagePath + ": " + error.status);
    };

    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: $.url("undoPendingCreateOperation"),
        data: "{'image': '" + imagePath + "'}",
        dataType: "json",
        success: function (data) { successCallback($container); },
        error: function (error) { failureCallback(error, imagePath); }
    });
    //successCallback($container); FOR TESTING
}

function undoPendingArchiveOperation($container) {
    var imagePath = $container.attr("id");

    var successCallback = function (item) {
        $container.closest("li").fadeOut(200, function () {
            $(this).html(item);
            enableToggleDetails();
            enablePendingOperationControls();
            $(this).fadeIn(200);
        });
    };

    var failureCallback = function (error, imagePath) {
        alert("Failed to undo operation on " + imagePath + ": " + error.status);
    };

    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: $.url("undoPendingArchiveOperation"),
        data: "{'image': '" + imagePath + "'}",
        dataType: "html",
        success: function (data) { successCallback(data); },
        error: function (error) { failureCallback(error, imagePath); }
    });
    //successCallback($container); FOR TESTING
}
