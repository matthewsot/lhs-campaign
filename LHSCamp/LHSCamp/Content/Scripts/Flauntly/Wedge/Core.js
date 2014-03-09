/// <reference path="../../typings/jquery/jquery.d.ts" />
/*
* Displays the Wedge Lightbox.
* link: A link to the youtube video or picture.
* title: Text displayed below the content
* type: The type of link provided - youtube or pic
*/
function initWedge(link, title, type, opacity, allowExit) {
    /*
    * Shows the overlay
    */
    if (typeof opacity === "undefined") { opacity = 0.9; }
    if (typeof allowExit === "undefined") { allowExit = true; }
    $("body").append('<div id="lightOverlay" style="overflow:hidden;top:0px;left:0px;position:fixed;z-index:2147483630;opacity:' + opacity.toString() + ';background-color:#000000;display:none;" />');
    $("#lightOverlay").css("height", window.innerHeight);
    $("#lightOverlay").css("width", window.innerWidth);
    $("#lightOverlay").fadeIn("slow");

    if (allowExit) {
        $("#lightOverlay").click("slow", function () {
            var goodMarginLeft = $("#lightContent").css("margin-left");
            var toGoLeft = (parseInt(goodMarginLeft.replace('px', '')) - 50);
            $("#lightContent").animate({
                opacity: 0,
                marginLeft: toGoLeft
            }, function () {
                if (type == "div") {
                    $("#" + link).appendTo($("body"));
                    $("#" + link).hide();
                }
                $("#lightContent").remove();
                $("#lightOverlay").fadeOut(function () {
                    $("#lightOverlay").remove();
                });
            });
        });
    }

    /*
    * Shows the content
    */
    $("body").append('<div id="lightContent" style="text-align:center;z-index:2147483641;opacity:0;position:fixed;">');
    switch (type) {
        case "youtube":
            $("#lightContent").append('<iframe id="youtubeFrame" width="853" height="480" src="' + link.replace("/watch?v=", "/embed/").replace(/&.*/, "") + '" frameborder="0" allowfullscreen></iframe><h3 style="color:#A0A0A0;">' + title + '</h3>');
            break;
        case "pic":
            $("#lightContent").append('<img src="' + link + '" style="max-height:' + (window.innerHeight - 100) + 'px;max-width:' + (window.innerWidth - 100) + 'px;"/><h3 style="color:#A0A0A0;">' + title + '</h3>');
            break;
        case "div":
            $("#" + link).show();
            $("#lightContent").append($("#" + link));
    }
    $("#lightContent").css({ top: '50%', left: '50%', margin: '-' + ($('#lightContent').height() / 2) + 'px 0 0 -' + ($('#lightContent').width() / 2) + 'px' }); //courtesy of http://archive.plugins.jquery.com/project/autocenter
    var goodMarginLeft = $("#lightContent").css("margin-left");
    $("#lightContent").css("margin-left", (parseInt(goodMarginLeft.replace('px', '')) - 50) + "px");
    $("#lightContent").animate({
        "opacity": 1,
        marginLeft: goodMarginLeft
    }, "slow");
}
//# sourceMappingURL=Core.js.map
