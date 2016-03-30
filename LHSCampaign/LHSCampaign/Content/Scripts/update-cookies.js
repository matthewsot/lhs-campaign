if ($.cookie("2016-class") === undefined) {
    $.removeCookie("selected-attendees");
    $.removeCookie("has-shown");
    $.removeCookie("2016-asb");
    $.cookie("2016-class", true, { path: "/", expires: 365 });
}