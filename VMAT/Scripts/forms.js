﻿//File: forms.js

$(document).ready(function () {
    // Update on load
    updatePreviewPane();

    $("form input").bind('input', updatePreviewPane);
    $("form select").change(updatePreviewPane);

    // IE <=8 Compatibility
    $("form input").keyup(updatePreviewPane);
    $("form input").change(updatePreviewPane);
    
    $(".project-add").click(function () {
        displayAddProjectNumberField();
    });

    $(".project-add-field").keypress(function (e) {
        if (e.keyCode == 13) {
            updateProjectNumberList($(this).val());
            return false;
        }
    });

    $(".project-add-field").focusout(function () {
        updateProjectNumberList($(this).val());
    });
});


// Update the preview display on the 'Create Machine' page with the information
// currently entered in the form inputs.
function updatePreviewPane() {
    updateImageFilePreview();
    updateIpPreview();
    updateBaseImagePreview();
};

function updateImageFilePreview() {
    var machineSuffix = $("#MachineName").val();
    var projectNumber = $("#ProjectName").val();

    $(".pProject").text(projectNumber);

    // HACK: Project naming convention nonsense
    projectNumber = projectNumber.replace("G", "");

    if (machineSuffix && machineSuffix.length <= 5)
        $(".pMachinename").text("gapdev" + projectNumber + machineSuffix);
    else
        $(".pMachinename").text("gapdev" + projectNumber + "yyyyy");
}

function updateIpPreview() {
    var ip = $("#IP").val();

    if (ip && ip.length <= 15)
        $(".pIP").text(ip);
    else
        getNextAvailableIP();
}

function updateBaseImagePreview() {
    try {
        var imageFile = $("#BaseImageFile option:selected").val();
        $(".pImage").text(imageFile);
    } catch (e) {
        // Ignore if the field does not exist
    }
}

function getNextAvailableIP() {
    var successCallback = function (data) {
        $(".pIP").text(data);
    }

    var failureCallback = function (error) {
        alert("Failed to get next available IP address: " + error.status);
    }

    $.ajax({
        type: "GET",
        contentType: "application/json; charset=utf-8",
        url: $.url("getNextIP"),
        data: "{ }",
        dataType: "json",
        success: function (data) { successCallback(data); },
        error: function (error) { failureCallback(error); }
    });
}

function displayAddProjectNumberField() {
    $(".project-add").hide();
    $("#ProjectName").hide();
    $(".project-add-field").show();
    $(".project-add-field").focus();
}

function updateProjectNumberList(projNumber) {
    if (projNumber) {
        var exists = false;

        $("#ProjectName option").each(function () {
            if ($(this).val() == projNumber)
                exists = true;
        });

        if (!exists) {
            $('#ProjectName').append('<option value=' + projNumber + ' selected="selected">' +
            projNumber + '</option>');
            updateImageFilePreview();
        }
    }

    $("#ProjectName").show();
    $(".project-add").show();
    $(".project-add-field").val("");
    $(".project-add-field").hide();
}
