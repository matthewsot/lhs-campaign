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
    
}

$("body").on("click", ".add-pic-button", function() {
    var selected = getSelected();
    var thisId = parseInt($(this).closest(".candidate").attr("data-id"));

    if (selected.indexOf(thisId) === -1) {
        selected.push(thisId);
    } else {
        selected.splice(selected.indexOf(thisId), 1);
    }
});