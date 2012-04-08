// File: archiveMachine.js

/*$(document).ready(function () {
	$("#project-close-form button.confirm").click(function () {
		var id = $(this).closest("#project-close-form").attr("project");
		archiveMachine(id);
		Popup.disablePopup();
	});

	$("#project-close-form button.deny").click(function () {
		Popup.disablePopup();
	});
});*/

function archiveMachine(id) {
	var successCallback = function (item, id) {
		$('.machine-info[machineid="' + id + '"]').closest("li").fadeOut(200, function () {
			$(this).html(item);

			$(this).find(".machine-info .details").hide();
			$(this).find(".status button").attr("title", function () {
				setStatusTooltips($(this));
			});

			// Reattach event listeners to new DOM elements
			$(this).find(".status > button").click(toggleStatusClick);
			$(this).find(".toggle-details").click(toggleDetailsClick);
			$(this).find(".pending-archive-vm .undo-pending").click(undoPendingArchiveClick);

			$(this).fadeIn(200);
		});
	}

	var failureCallback = function (error, id) {
		alert("Failed to archive machine " + id + ": " + error.status);
	}
	
	$.ajax({
		type: "POST",
		contentType: "application/json; charset=utf-8",
		url: $.url("archiveMachine"),
		data: "{'id': " + id + "}",
		dataType: "html",
		success: function (data) { successCallback(data, id); },
		error: function (error) { failureCallback(error, id); }
	});
}
