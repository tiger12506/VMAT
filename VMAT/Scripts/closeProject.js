// File: closeProject.js

// Declare CloseProject namespace
var CloseProject = {};

$(document).ready(function () {
    $(".project-close-form button.archive").click(function () {
        alert("ok");
        var project = $(this).closest(".project-close-form").attr("project");
        archiveProject(project);
    });

    $(".project-close-form button.delete").click(function () {
        var project = $(this).closest(".project-close-form").attr("project");
        deleteProject(project);
    });
});

function archiveProject(projectName) {
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

    disablePopup();
}

CloseProject.successCallback = function (data, project) {
    var $projectContainer = $("#" + project + " .project-machines");

    $.ajax({
        url: $.url("_ClosingProject.cshtml"), // TODO: What is this getting?
        type: 'GET',
        dataType: 'html',
        success: function (result) { $projectContainer.html(result); }
    });

    $projectContainer.children(".project-closing h4").text("Project G" + project + " will be " + data.Action + "d at " + data.Time.toString());
    $projectContainer.children(".project-closing button").attr({ "action": data.Action, value: "Undo" });
    
}

CloseProject.failureCallback = function (error, project) {
    alert("Failed to close project " + project + ": " + error);
}
