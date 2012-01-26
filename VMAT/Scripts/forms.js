//File: forms.js

// Declare GetNextAvailableIP namespace
var GetNextAvailableIP = {};

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

    $(".project-add").click(function () {
        displayAddProjectNumberField();
    });

    $(".project-add-field").keypress(function (e) {
        if (e.keyCode == 13) {
            updateProjectNumberList($(this).val());
            return false;
        }
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
        getNextAvailableIP();
    
    try {
        var imageFile = $("#BaseImageFile option:selected").val();
        $(".pImage").text(imageFile);
    } catch (e) {
        // Ignore if the field does not exist
    }
};

function getNextAvailableIP() {
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "/VirtualMachine/GetNextIP",
        data: "{ }",
        dataType: "json",
        success: function (data) { GetNextAvailableIP.successCallback(data); },
        error: function (error) { GetNextAvailableIP.failureCallback(error); }
    });
}

GetNextAvailableIP.successCallback = function (data) {
    $(".pIP").text(data);
}

GetNextAvailableIP.failureCallback = function (error) {
    alert("Failed to get next available IP address: " + error.status + " - " + 
        JSON.parse(error.responseText));
}

function displayAddProjectNumberField() {
    $(".project-add").hide();
    $("#ProjectName").hide();
    $(".project-add-field").show();
    $(".project-add-field").focus();
}

function updateProjectNumberList(projNumber) {
    $("#ProjectName").html();
    $("#ProjectName").show();
    $(".project-add").show();
    $(".project-add-field").val("");
    $(".project-add-field").hide();
}
