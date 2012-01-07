//File: forms.js

$(document).ready(function () {
    $("form input").change(updatePreviewPane);

    $("#project-menu").change(function () {
        $("#project-menu-field").val($(this).val());
    });

    $("input.cancel").click(function () {
        window.location = "/";
    });
});


// Update the preview display on the 'Create Machine' page with the information
// currently entered in the form inputs.
function updatePreviewPane() {
    var projectNumber = $("#project-menu-field").val();
    var machineSuffix = $("#machine-suffix").val();
    
    $("#pProject").text("G" + projectNumber);
    $("#pHostname").text("gapdev.com");
    $("#pMachinename").text("gapdev" + projectNumber + machineSuffix);
    $("#pIP").text("192.168.1.1");
    

    try {
        var imageName = $("#image-menu option:selected").val();
        $("#pImage").text(imageName);
    } catch (e) {
        
    }
};
