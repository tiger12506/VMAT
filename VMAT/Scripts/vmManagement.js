$(document).ready(function () {
    // Activate items if JavaScript is enabled
    $(".machine-info .details").hide();
    $(".status button").attr("title", function () {
        setStatusTooltips($(this));
    });

    $(".project-display").click(function () {
        if ($(this).text() === "V") {
            $(this).text(">");
        } else {
            $(this).text("V");
        }

        $(this).closest(".project").children(".project-machines").slideToggle(300);
    });

    $(".toggle-details").click(function () {
        var $detailsDiv = $(this).closest(".machine-info").children(".details");

        if ($(this).text() === "Show Less") {
            $(this).text("Show More");
        } else {
            $(this).text("Show Less");
        }

        $detailsDiv.slideToggle(300);
    });
});
