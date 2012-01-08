﻿//File: forms.js

$(document).ready(function () {
    // Update on load
    updatePreviewPane();

    $("form input").bind('input', updatePreviewPane);
    $("form select").change(updatePreviewPane);

    // IE <=8 Compatibility
    $("form input").keyup(updatePreviewPane);
    $("form input").change(updatePreviewPane);

    $("#project-menu").change(function () {
        $("#project-menu-field").val($(this).val());
    });
});


// Update the preview display on the 'Create Machine' page with the information
// currently entered in the form inputs.
function updatePreviewPane() {
    var projectNumber = $("#ProjectName").val();
    var machineSuffix = $("#MachineNameSuffix").val();
    var ip = $("#IP").val();

    $(".pProject").text("G" + projectNumber);
    $(".pHostname").text("vmat.rose-hulman.edu");

    if (machineSuffix && machineSuffix.length <= 5)
        $(".pMachinename").text("gapdev" + projectNumber + machineSuffix);
    else
        $(".pMachinename").text("gapdev" + projectNumber + "yyyyy");

    if (ip && ip.length <= 15)
        $(".pIP").text(ip);
    else
        $(".pIP").text("192.168.1.1");
    
    try {
        var imageFile = $("#BaseImageFile option:selected").val();
        $(".pImage").text(imageFile);
    } catch (e) {
        // Ignore if the field does not exist
    }
};
