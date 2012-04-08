// File: archiveProject.js

/*$(document).ready(function () {
	$("#project-close-form button.confirm").click(function () {
		var project = $(this).closest("#project-close-form").attr("project");
		archiveProject(project);
		Popup.disablePopup();
	});

	$("#project-close-form button.deny").click(function () {
		Popup.disablePopup();
	});
});*/

function archiveProject(id) {
	var successCallback = function (item, id) {
		$('.project[projectid="' + id + '"]').closest("li.project-item").fadeOut(200, function () {
			$(this).html(item);

			$(this).find(".machine-info .details").hide();
			$(this).find(".status button").attr("title", function () {
				setStatusTooltips($(this));
			});

			// Reattach event listeners to new DOM elements
			$(this).find(".project-close").click(projectCloseClick);
			$(this).find(".project-collapse").click(projectCollapseClick);
			$(this).find(".status > button").click(toggleStatusClick);
			$(this).find(".toggle-details").click(toggleDetailsClick);
			$(this).find(".pending-archive-vm .undo-pending").click(undoPendingArchiveClick);

			$(this).fadeIn(200);
		});
	}

	var failureCallback = function (error, id) {
		alert("Failed to archive project " + id + ": " + error.status);
	}

	$.ajax({
		type: "POST",
		contentType: "application/json; charset=utf-8",
		url: $.url("archiveProject"),
		data: "{'id': '" + id + "'}",
		dataType: "html",
		success: function (data) { successCallback(data, id); },
		error: function (error) { failureCallback(error, id); }
	});
}
