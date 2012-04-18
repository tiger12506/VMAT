// File: vmManagement.js

var toggleDetailsClick = function () {
	var $detailsDiv = $(this).closest(".machine-info").children(".details");

	if ($(this).text() === "Show Less") {
		$(this).text("Show More");
	} else {
		$(this).text("Show Less");
	}

	$detailsDiv.slideToggle(300);
}

var projectArchiveClick = function () {
	var id = $(this).closest(".project").attr("projectid");
	var projectName = $(this).closest(".project").find(".project-name").text();
	Popup.loadPopup("Archive Project " + projectName + "?", "#confirmation-form");

	$("#popup-content button.confirm").click(function () {
		archiveProject(id);
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

var machineArchiveClick = function () {
	var id = $(this).closest(".machine-info").attr("machineid");
	var machineName = $(this).closest(".machine-info").find(".tMachinename").text();
	Popup.loadPopup("Archive Virtual Machine " + machineName + "?", "#confirmation-form");

	$("#popup-content button.confirm").click(function () {
		archiveMachine(id);
		Popup.disablePopup();
	});

	$("#popup-content button.deny").click(function () {
		Popup.disablePopup();
	});
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

	$(".project-archive").click(projectArchiveClick);

	$(".project-collapse").click(projectCollapseClick);

	$(".archive-vm-button button").click(machineArchiveClick);

	$(".pending-vm .undo-pending").click(undoPendingCreateClick);

	$(".pending-archive-vm .undo-pending").click(undoPendingArchiveClick);
});


function undoPendingCreateOperation($container) {
	var id = $container.attr("machineid");

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

	var failureCallback = function(error, id) {
		alert("Failed to undo operation on " + id + ": " + error.status);
	};

	$.ajax({
		type: "POST",
		contentType: "application/json; charset=utf-8",
		url: $.url("undoPendingCreateOperation"),
		data: "{'id': '" + id + "'}",
		dataType: "json",
		success: function (data) { successCallback($container); },
		error: function (error) { failureCallback(error, id); }
	});
	//successCallback($container); FOR TESTING
}

function undoPendingArchiveOperation($container) {
	var id = $container.attr("machineid");

	var successCallback = function (item) {
		$container.closest("li").fadeOut(200, function () {
			$(this).html(item);
			$(this).find(".machine-info .details").hide();
			$(this).find(".status button").attr("title", function () {
				setStatusTooltips($(this));
			});

			$(this).find(".archive-vm-button button").click(machineArchiveClick);
			$(this).find(".status > button").click(toggleStatusClick);
			$(this).find(".toggle-details").click(toggleDetailsClick);
			$(this).find(".pending-archive-vm .undo-pending").click(undoPendingArchiveClick);
			$(this).fadeIn(200);
		});
	};

	var failureCallback = function (error, id) {
		alert("Failed to undo operation on " + id + ": " + error.status);
	};

	$.ajax({
		type: "POST",
		contentType: "application/json; charset=utf-8",
		url: $.url("undoPendingArchiveOperation"),
		data: "{'id': '" + id + "'}",
		dataType: "html",
		success: function (data) { successCallback(data); },
		error: function (error) { failureCallback(error, id); }
	});
	//successCallback($container); FOR TESTING
}
