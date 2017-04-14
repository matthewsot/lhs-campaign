if ($.cookie("2017-class") === undefined) {
    $.removeCookie("selected-attendees");
    $.removeCookie("has-shown");
    $.removeCookie("2017-class");
    $.cookie("2017-class", true, { path: "/", expires: 365 });
}