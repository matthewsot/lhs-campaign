$("body").on("click", "#position-tabs li", function () {
    $(this).addClass("selected").siblings(".selected").removeClass("selected");
    $(".tab-content").hide();
    $(".tab-content[data-tab=\"" + $(this).text().trim() + "\"]").show();
});
$("#position-tabs li").first().click();

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
    var thisId = parseInt($(this).closest(".candidate").attr("data-id"));

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

$("body").on("click", ".platform-button", function () {
    $("#cand-wedge-reasons").text($(this).closest(".candidate").find("p").text());

    var fbLink = $(this).closest(".candidate").attr("data-fb");
    if (fbLink.trim().length > 0) {
        $("#cand-wedge-fb").unbind("click").click(function() {
            window.open(fbLink);
        });
    } else {
        $("#cand-wedge-fb").hide();
    }

    var cover = $(this).closest(".candidate").attr("data-cover");
    if (cover.trim().length > 0) {
        $("#cand-wedge-cover").unbind("click").click(function () {
            window.open(cover);
        });
    } else {
        $("#cand-wedge-cover").hide();
    }

    var img = $(this).closest(".candidate").find("img").attr("src");
    $("#cand-wedge-download").unbind("click").click(function () {
        window.open(img);
    });
    $("#cand-wedge-img").attr("src", img);

    window.location.hash = "cand=" + $(this).closest(".candidate").attr("data-id");

    wedge.show("#cand-wedge");
});

var splitted = window.location.hash.split("#");
if (splitted.length > 1) {
    var pathed = splitted[1].trim().split("cand=");
    if (pathed.length > 1) {
        var cand = pathed[1].split("&")[0].split("?")[0].trim();
        var candidate = $(".candidate[data-id=\"" + cand + "\"]");
        if (candidate.length) {
            candidate.find(".platform-button").click();
        }
    }
}

$("body").on("click", "#gplus-btn", function() {
    window.open("GPlus");
});

$("body").on("click", "#wedge-overlay", function() {
    window.location.hash = "";
});