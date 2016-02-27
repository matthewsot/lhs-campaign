if ($.cookie("2016-asb") === undefined) {
    $.removeCookie("selected-attendees");
    $.cookie("2016-asb", true, { path: "/", expires: 365 });
}