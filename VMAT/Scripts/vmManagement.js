// File: vmManagement.js

var $projectName;

var toggleDetailsClick = function () {
    var $detailsDiv = $(this).closest(".machine-info").children(".details");

    if ($(this).text() === "Show Less") {
        $(this).text("Show More");
    } else {
        $(this).text("Show Less");
    }

    $detailsDiv.slideToggle(300);
}

var projectCloseClick = function () {
    $projectName = $(this).closest(".project").attr("id");
    Popup.loadPopup("Archive Project " + $projectName + "?", "#project-close-form");

    $("#popup-content button.confirm").click(function () {
        archiveProject($projectName);
        Popup.disablePopup();
    });

    $("#popup-content button.deny").click(function () {
        Popup.disablePopup();
    });
}

var projectCollapseClick = function () {
    if ($(this).text() == "v") {
        $(this).text(">");
    } else {
        $(this).text("v");
    }

    $(this).closest(".project").children(".machine-list").slideToggle(300);
}

var vmArchiveClick = function () {
	var $container = $(this).closest(".machine-info");
	archiveVm($container.attr("id"));
}

var undoPendingCreateClick = function () {
    var $container = $(this).closest(".machine-info");
    undoPendingCreateOperation($container);
}

var undoPendingArchiveClick = function () {
    var $container = $(this).closest(".machine-info");
    undoPendingArchiveOperation($container);
}


$(document).ready(function () {
	// Activate items if JavaScript is enabled
	$(".machine-info .details").hide();
	$(".status button").attr("title", function () {
		setStatusTooltips($(this));
	});

	$(".toggle-details").click(toggleDetailsClick);

	$(".project-close").click(projectCloseClick);

	$(".project-collapse").click(projectCollapseClick);

	$(".archive-vm-button button").click(vmArchiveClick);

	$(".pending-vm .undo-pending").click(undoPendingCreateClick);

	$(".pending-archive-vm .undo-pending").click(undoPendingArchiveClick);
});


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
            $(this).find(".machine-info .details").hide();
            $(this).find(".status button").attr("title", function () {
                setStatusTooltips($(this));
            });

            $(this).find(".status > button").click(toggleStatusClick);
            $(this).find(".toggle-details").click(toggleDetailsClick);
            $(this).find(".pending-archive-vm .undo-pending").click(undoPendingArchiveClick);
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
