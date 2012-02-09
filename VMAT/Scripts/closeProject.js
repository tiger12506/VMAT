// File: closeProject.js

// Declare CloseProject namespace
var CloseProject = {};

$(document).ready(function () {
    $("#project-close-form button.archive").click(function () {
        var project = $(this).closest("#project-close-form").attr("project");
        archiveProject(project);
        Popup.disablePopup();
    });

    $("#project-close-form button.delete").click(function () {
        var project = $(this).closest("#project-close-form").attr("project");
        deleteProject(project);
        Popup.disablePopup();
    });
});

function archiveProject(projectName) {
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: $.url("archiveProject"),
        data: "{'project': '" + projectName + "'}",
        dataType: "html",
        success: function (data) { CloseProject.successCallback(data, projectName); },
        error: function (error) { CloseProject.failureCallback(error, projectName); }
    });
}

function deleteProject(projectName) {
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: $.url("deleteProject"),
        data: "{'project': '" + projectName + "'}",
        dataType: "html",
        success: function (data) { CloseProject.successCallback(data, projectName); },
        error: function (error) { CloseProject.failureCallback(error, projectName); }
    });
}

CloseProject.successCallback = function (item, project) {
    $("#" + project).closest("li.project-item").fadeOut(200, function () {
        $(this).html(item);
        enableToggleDetails();
        enablePendingOperationControls();
        enableProjectControls();
        $(this).fadeIn(200);
    });
    
}

CloseProject.failureCallback = function (error, project) {
    alert("Failed to close project " + project + ": " + error.status);
}
