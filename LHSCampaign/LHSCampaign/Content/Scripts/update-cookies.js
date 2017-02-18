if ($.cookie("2017-asb") === undefined) {
    $.removeCookie("selected-attendees");
    $.removeCookie("has-shown");
    $.removeCookie("2017-asb");
    $.cookie("2017-asb", true, { path: "/", expires: 365 });
}