// File: closeProject.js

// Declare CloseProject namespace
var CloseProject = {};

$(document).ready(function () {
    $("button.archive").click(function () {
        var project = $(this).closest("#project-complete-form").attr("project");
        archiveProject(project);
    });

    $("button.delete").click(function () {
        var project = $(this).closest("#project-complete-form").attr("project");
        deleteProject(project);
    });
});

function archiveProject(projectName) {
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "/VirtualMachine/ArchiveProject",
        data: "{'project': '" + projectName + "'}",
        dataType: "json",
        success: function (data) { successCallback(data, button); },
        error: function (error) { failureCallback(error, machineName, button); }
    });
}

function deleteProject(projectName) {
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "/VirtualMachine/DeleteProject",
        data: "{'project': '" + projectName + "'}",
        dataType: "json",
        success: function (data) { CloseProject.successCallback(data, projectName); },
        error: function (error) { CloseProject.failureCallback(error, projectName); }
    });
}

CloseProject.successCallback = function (data, project) {
    disablePopup();
}

CloseProject.failureCallback = function (error, project) {
    disablePopup();
    alert("Failed to close project " + project + ": " + error);
}