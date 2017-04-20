if ($.cookie("2017-fresh") === undefined) {
    $.removeCookie("selected-attendees");
    $.removeCookie("has-shown");
    $.removeCookie("2017-fresh");
    $.cookie("2017-fresh", true, { path: "/", expires: 365 });
}