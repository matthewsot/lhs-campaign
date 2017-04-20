//$(".tab-content").hide();

$("body").on("click", "#position-tabs div", function (e) {
    $(this).addClass("selected").siblings(".selected").removeClass("selected");
    $(".tab-content").hide();

    var thisFirstText = $(this).clone().children().remove().end().text().trim(); //Thanks! http://viralpatel.net/blogs/jquery-get-text-element-without-child-element/
    $.cookie("last-tab", $(this).index());
    thisFirstText = thisFirstText.replace("rep.", "representative");
    if ($(".tab-content[data-tab=\"" + thisFirstText + "\"]").length > 0 && $(".tab-content[data-tab=\"" + thisFirstText + "\"]").children("article").length > 0) {
        $(".tab-content[data-tab=\"" + thisFirstText + "\"]").show();
    } else {
        $(".tab-content[data-tab=\"no-peeps\"]").show();
    }

    $("#position-tabs div span").hide();
    if ($(this).find("span").length != 0) {
        $(this).find("span").show();
    }
});

if ($.cookie("last-tab") !== undefined) {
    $("#position-tabs div").get($.cookie("last-tab")).click();
} else {
    $("#position-tabs div").first().click();
}

$.cookie.json = true;
function setSelected(val) {
    $.cookie("selected-attendees", val, { path: "/", expires: 365 });
    $.cookie("fresh-elections", true, { path: "/", expires: 365 });
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
    $(".add-pic-button").text("mash picture");
    for (var i = 0; i < selected.length; i++) {
        var person = selected[i];
        if (person["initials"] == null) {
            person["initials"] = "";
        }
        console.log(person);
        $(".candidate[data-id=\"" + person["id"] + "\"]").find(".add-pic-button").text("unmash picture");
        $("#selected-people").append($("<div></div>").append($("<span>x</span>")).append($("<img></img>").attr("src", person["img"]).attr("data-id", person["id"]).attr("alt", person["initials"])));
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
        var toPush = { "id": thisId, "img": $(this).closest(".candidate").find("img").attr("src"), "initials": $(this).closest(".candidate").find("img").attr("alt") };
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

$("#close-wedge").click(function() {
    wedge.close(function() {
        $.cookie("has-shown", true, { path: "/", expires: 100 });
    });
});

$("#class-options a").click(function() {
    $.cookie("has-shown", true, { path: "/", expires: 100 });
    window.location.href = $(this).attr("data-href");
});

//function updateCountdown() {
//    var now = moment();
//    var then = moment("March 29, 2016 9:00 PM");

//    var days = then.diff(now, "days");
//    var hours = then.diff(now, "hours") - (days * 24);
//    var minutes = then.diff(now, "minutes") - (days * 24 * 60) - (hours * 60);
//    var seconds = then.diff(now, "seconds") - (days * 24 * 60 * 60) - (hours * 60 * 60) - (minutes * 60);

//    function format(num) {
//        if (num.toString().length < 2) {
//            num = "0" + num.toString();
//        }
//        return num;
//    }

//    $("#count").text(format(hours) + ":" + format(minutes) + ":" + format(seconds));
//}

//updateCountdown();
//setInterval(updateCountdown, 500);