$(".tab-content").hide();

$("body").on("click", "#position-tabs div", function (e) {
    $(this).addClass("selected").siblings(".selected").removeClass("selected");
    $(".tab-content").hide();

    var thisFirstText = $(this).clone().children().remove().end().text().trim(); //Thanks! http://viralpatel.net/blogs/jquery-get-text-element-without-child-element/
    thisFirstText = thisFirstText.replace("rep.", "representative");
    if ($(".tab-content[data-tab=\"" + thisFirstText + "\"]").length > 0 && $(".tab-content[data-tab=\"" + thisFirstText + "\"]").children("article").length > 0) {
        $(".tab-content[data-tab=\"" + thisFirstText + "\"]").show();
    } else {
        $(".tab-content[data-tab=\"no-peeps\"]").show();
    }
});
$("#position-tabs div").first().click();

$.cookie.json = true;
function setSelected(val) {
    $.cookie("selected-attendees", val, { path: "/", expires: 365 });
    $.cookie("asb-elections", true, { path: "/", expires: 365 });
}
function getSelected() {
    var selected = $.cookie("selected-attendees");
    if (typeof selected === "undefined") {
        setSelected([]);
        return [];
    }
    return selected;
}

function refreshSelected() {
    $("#selected-people").html("");
    var selected = getSelected();
    $(".add-pic-button").text("(+) add picture");
    for (var i = 0; i < selected.length; i++) {
        var person = selected[i];
        console.log(person);
        $(".candidate[data-id=\"" + person["id"] + "\"]").find(".add-pic-button").text("(-) rm. picture");
        $("#selected-people").append($("<img></img>").attr("src", person["img"]).attr("data-id", person["id"]));
    }
}

refreshSelected();

$("body").on("click", "#selected-people img", function () {
    var foundIndex = -1;
    var selected = getSelected();
    for (var i = 0; i < selected.length; i++) {
        if (selected[i]["id"] === $(this).attr("data-id")) {
            foundIndex = i;
            break;
        }
    }
    selected.splice(foundIndex, 1);
    setSelected(selected);
    refreshSelected();
});

$("body").on("click", ".add-pic-button", function() {
    var selected = getSelected();
    var thisId = $(this).closest(".candidate").attr("data-id");

    var foundId = -1;
    for (var i = 0; i < selected.length; i++) {
        if (selected[i]["id"] == thisId) {
            foundId = i;
            break;
        }
    }

    if (foundId === -1) {
        var toPush = { "id": thisId, "img": $(this).closest(".candidate").find("img").attr("src") };
        if ($(this).closest(".candidate").attr("data-cover").trim().length > 0) {
            toPush["cover"] = $(this).closest(".candidate").attr("data-cover");
        }
        selected.push(toPush);
    } else {
        selected.splice(foundId, 1);
    }
    setSelected(selected);
    refreshSelected();
});

var hasShown = $.cookie("has-shown");
if (typeof hasShown === "undefined" || !hasShown) {
    wedge.show("#welcome-popup", { allowExit: false });
}

//$("#close-wedge").click(function() {
//    wedge.close(function() {
//        $.cookie("has-shown", true, { path: "/", expires: 100 });
//    });
//});

$("#class-options a").click(function() {
    $.cookie("has-shown", true, { path: "/", expires: 100 });
    window.location.href = $(this).attr("data-href");
});