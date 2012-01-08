//File: forms.js

$(document).ready(function () {
    $("form input").change(updatePreviewPane);
    $("form select").change(updatePreviewPane);

    $("#project-menu").change(function () {
        $("#project-menu-field").val($(this).val());
    });
});


// Update the preview display on the 'Create Machine' page with the information
// currently entered in the form inputs.
function updatePreviewPane() {
    var projectNumber = $("#ProjectName").val();
    var machineSuffix = $("#MachineNameSuffix").val();
    
    $(".pProject").text("G" + projectNumber);
    $(".pHostname").text("gapdev.com");
    $(".pMachinename").text("gapdev" + projectNumber + machineSuffix);
    $(".pIP").text("192.168.1.1");
    
    try {
        var imageName = $("#BaseImageName option:selected").val();
        $(".pImage").text(imageName);
    } catch (e) {
        // Ignore if the field does not exist
    }
};
