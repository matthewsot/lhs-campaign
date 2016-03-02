if ($.cookie("2016-asb") === undefined) {
    $.removeCookie("selected-attendees");
    $.removeCookie("has-shown");
    $.cookie("2016-asb", true, { path: "/", expires: 365 });
}