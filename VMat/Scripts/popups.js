// File: popups.js
// Display a 'div' component within the page like a popup window, centered horizontally in the window.

// False: Disabled, True: Enabled
var popupStatus = false;

$(document).ready(function () {
    $("#popup-header").mousedown(function () {
        $("#popup").draggable();
    });

    $("#popup-header button.cancel").click(function () {
        disablePopup();
    });

    $(document).keypress(function (e) {
        if (e.keyCode == 27 && popupStatus)
            disablePopup();
    });

    $(".project-close").click(function () {
        loadPopup("Close Project?", "#project-complete-form");
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


// ***** The following is credited to http://qfox.nl/notes/115 *****

jQuery.prototype.draggable = function () {
    var 
		$obj = this,
		dragging = false,
		startMouseX,
		startMouseY,
		startObjectX,
		startObjectY;

    ondragstart = function (e) {
        dragging = true;
        startMouseX = e.pageX;
        startMouseY = e.pageY;
        var pos = $obj.offset();
        startObjectX = pos.left;
        startObjectY = pos.top;
    },
		ondragging = move = function (e) {
		    if (dragging) {
		        $obj.css({
		            left: startObjectX + (e.pageX - startMouseX) + 'px',
		            top: startObjectY + (e.pageY - startMouseY) + 'px'
		        });
		    }
		},
		ondragstop = function () {
		    dragging = false;
		};

    // hook events
    this.mousedown(ondragstart);
    $(document)
	.mousemove(ondragging)
	.mouseup(ondragstop);

    // unhook events, function is attached to returned object...
    $obj.undraggable = function () {
        $obj.unbind('mousedown', ondragstart);
        $(document)
		.unbind('mousemove', ondragging)
		.unbind('mouseup', ondragstop);
        delete $obj.undraggable;
        ondragstart = ondragstop = ondragging = null;
        return $obj;
    };

    return $obj;
};

// *****************************************************************
