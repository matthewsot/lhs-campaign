$("#tabs li a").click(function (e) {
    e.stopPropagation();
});
$("#tabs li").click(function () {
    $("#tabs li").removeClass("selected");
    $(this).addClass("selected");

    $(".tab.selected").removeClass("selected");
    $(".tab#" + $(this).text().toLowerCase().trim()).addClass("selected");
});

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
    $("#add-pic-button").text("MASH PICTURE");
    for (var i = 0; i < selected.length; i++) {
        var person = selected[i];
        try {
            if (person["id"].toString() == $("main").attr("data-id").toString()) {
                $("#add-pic-button").text("UNMASH PICTURE");
            }
        } catch (e) { }
        $("#selected-people").append($("<div></div>").append($("<span>x</span>")).append($("<img></img>").attr("src", person["img"]).attr("data-id", person["id"])));
    }
}

refreshSelected();

$("body").on("click", "#selected-people div", function () {
    var img = $(this).first("img");
    var foundIndex = -1;
    var selected = getSelected();
    for (var i = 0; i < selected.length; i++) {
        if (selected[i]["id"] === $(img).attr("data-id")) {
            foundIndex = i;
            break;
        }
    }
    selected.splice(foundIndex, 1);
    setSelected(selected);
    refreshSelected();
});

$("#add-pic-button").click(function () {
    var selected = getSelected();
    var thisId = $(this).closest("main").attr("data-id");

    var foundId = -1;
    for (var i = 0; i < selected.length; i++) {
        if (selected[i]["id"] == thisId) {
            foundId = i;
            break;
        }
    }

    if (foundId === -1) {
        var toPush = { "id": thisId, "img": $(this).closest("#info").find("img").attr("src") };
        if ($(this).closest("main").attr("data-cover").trim().length > 0) {
            toPush["cover"] = $(this).closest("main").attr("data-cover");
        }
        selected.push(toPush);
    } else {
        selected.splice(foundId, 1);
    }
    setSelected(selected);
    refreshSelected();
});

if (window.location.href.indexOf("?tab=questions")) {
    $("#tabs li[nth-child=2]").click();
}