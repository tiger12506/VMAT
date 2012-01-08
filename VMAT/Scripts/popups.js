// File: popups.js
// Display a 'div' component within the page like a popup window, centered horizontally in the window.

// False: Disabled, True: Enabled
var popupStatus = false;

$(document).ready(function () {
    $("#popup-header").mousedown(function () {
        $("#popup").draggable({ cursor: 'move' });
    });

    $("#popup-header button.cancel").click(function () {
        disablePopup();
    });

    $(document).keypress(function (e) {
        if (e.keyCode == 27 && popupStatus)
            disablePopup();
    });

    $(".project-close").click(function () {
        var $projectName = $(this).closest(".project").attr("id");

        loadPopup("Close Project G" + $projectName + "?", "#project-complete-form");
    });
});

// ****** The following borrows code from
// http://yensdesign.com/2008/09/how-to-create-a-stunning-and-smooth-popup-using-jquery/

function loadPopup(title, element) {
    if (!popupStatus) {
        $("#popup-header > h2").text(title);
        $("#popup-content").html($(element).html());

        centerPopup();
        $("#popup").fadeIn("fast", function () {
            popupStatus = true;
        });
    }
}

function disablePopup() { 
    if (popupStatus) {
        $("#popup").fadeOut("fast", function () {
            $("#popup-header > h2").text("");
            $("#popup-content").html("");
            popupStatus = false;
        });
    }
}

function centerPopup() {
    var windowWidth = document.documentElement.clientWidth;
    var windowHeight = document.documentElement.clientHeight;
    var popupHeight = $("#popup").height();
    var popupWidth = $("#popup").width();

    $("#popup").css({
        "top": windowHeight / 2 - popupHeight / 2,
        "left": windowWidth / 2 - popupWidth / 2
    });
}  

// *****************************************************************
