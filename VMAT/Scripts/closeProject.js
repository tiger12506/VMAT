// File: closeProject.js

// Declare CloseProject namespace
var CloseProject = {};

$(document).ready(function () {
    $("#project-close-form button.archive").click(function () {

        var project = $(this).closest("#project-close-form").attr("project");
        alert("ok"); //outdated code; no longer gets called
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
    alert("ok:" + projectName);

    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: $.url("archiveProject"),
        data: "{'project': '" + projectName + "'}",
        dataType: "json",
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
        dataType: "json",
        success: function (data) { CloseProject.successCallback(data, projectName); },
        error: function (error) { CloseProject.failureCallback(error, projectName); }
    });
}

CloseProject.successCallback = function (data, project) {
    var $projectContainer = $("#" + project + " .project-machines");
    $projectContainer.children(".project-closing h4").text("Project " + project + " will be " + data.Action + "d at " + data.Time.toString());
    $projectContainer.children(".project-closing button").attr({ "action": data.Action, value: "Undo" });
    
}

CloseProject.failureCallback = function (error, project) {
    alert("Failed to close project " + project + ": " + error);
}
