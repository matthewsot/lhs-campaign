/// <reference path="../typings/jquery/jquery.d.ts" />
var Flauntly = (function () {
    function Flauntly() {
    }
    //returns true if the browser is neither IE nor Chrome
    /*
    * Note that most of the time this is bad practice - use modernizr or another library to detect
    * if the browser supports what you're doing. This will only be used if there's a browser-specific
    * quirk.
    */
    Flauntly.isProblemBrowser = function () {
        if (navigator.appName.indexOf("Internet Explorer") != -1 || navigator.appName.indexOf("Chrome") != -1) {
            return false;
        } else {
            return true;
        }
    };

    //gets the height of an element not current shown
    Flauntly.getUndisplayHeight = function (item) {
        var currdsply = $(item.parentElement.parentElement).css("display");
        var currvis = $(item.parentElement.parentElement).css("visibility");
        $(item.parentElement.parentElement).css("visibility", "none");
        $(item.parentElement.parentElement).css("display", "block");
        var height = $(item).height();
        $(item.parentElement.parentElement).css("display", currdsply);
        $(item.parentElement.parentElement).css("visibility", currvis);
        return height;
    };

    //Toggles an item between a link and a selected link
    Flauntly.linkToggle = function (item, forceSelected, isLeft, forceUnselected) {
        if (typeof forceSelected === "undefined") { forceSelected = false; }
        if (typeof isLeft === "undefined") { isLeft = false; }
        if (typeof forceUnselected === "undefined") { forceUnselected = false; }
        if ($(item).attr("isLink") != "true" && !forceSelected || forceUnselected) {
            item.style.color = "#0026ff";
            item.style.cursor = "pointer";
            $(item).attr("isLink", "true");
        } else {
            item.style.color = "#fff";
            item.style.cursor = "auto";
            $(item).attr("isLink", "false");
        }
    };

    //gets the width of the browser's scrollbar
    //code thanks to MLM @ http://visualpulse.net/forums/index.php?topic=120.0
    Flauntly.getScrollBarWidth = function () {
        var wide_scroll_html = '<div id="wide_scroll_div_one" style="width:50px;height:50px;overflow-y:scroll;position:absolute;top:-200px;left:-200px;"><div id="wide_scroll_div_two" style="height:100px;width:100%"></div></div>';
        $("body").append(wide_scroll_html);
        var scroll_w1 = $("#wide_scroll_div_one").width();
        var scroll_w2 = $("#wide_scroll_div_two").innerWidth();
        var scroll_bar_width = scroll_w1 - scroll_w2;
        $("#wide_scroll_div_one").remove();
        return scroll_bar_width;
    };

    //gets a boolean representing whether or not the element is overflowing.
    //code thanks to Shog9 @ http://stackoverflow.com/questions/143815/how-to-determine-using-javascript-if-html-element-has-overflowing-content
    Flauntly.checkOverflow = function (el) {
        var curOverflow = el.style.overflow;
        if (!curOverflow || curOverflow === "visible")
            el.style.overflow = "hidden";

        var isOverflowing = el.clientWidth < el.scrollWidth || el.clientHeight < el.scrollHeight;

        el.style.overflow = curOverflow;

        return isOverflowing;
    };
    return Flauntly;
})();

var topNav = (function () {
    function topNav(Layers, RightLayers, LayerIDs) {
        if (typeof LayerIDs === "undefined") { LayerIDs = new Array(); }
        this.contents = new Array();
        for (var layer = 0; layer < Layers.length; layer++) {
            var newHeading = document.createElement("h1");
            if (RightLayers.indexOf(layer) == -1) {
                newHeading.className = "link";
                newHeading.style.cssFloat = "left";
            } else {
                newHeading.className = "rightLink";
                newHeading.style.cssFloat = "right";
            }
            newHeading.style.marginLeft = "6px";
            newHeading.style.marginTop = "0px";
            if (LayerIDs[layer] != null && LayerIDs[layer] != undefined) {
                newHeading.id = LayerIDs[layer];
            } else {
                newHeading.id = Layers[layer].toLowerCase() + "Layer";
            }
            newHeading.textContent = Layers[layer];
            this.contents.push(newHeading);
        }
    }
    topNav.prototype.applyTo = function (divID) {
        if (typeof divID === "undefined") { divID = "topNav"; }
        var div = document.getElementById(divID);
        div.textContent = "";
        for (var i = 0; i < this.contents.length; i++) {
            div.appendChild(this.contents[i]);
        }
    };
    topNav.applyLinks = function (selected, divID) {
        if (typeof selected === "undefined") { selected = new Array(); }
        if (typeof divID === "undefined") { divID = "topNav"; }
        $(".link").each(function () {
            Flauntly.linkToggle(this, false, false, true);
        });

        $(".rightLink").each(function () {
            Flauntly.linkToggle(this, false, false, true);
        });

        $(".leftLink").each(function () {
            Flauntly.linkToggle(this, false, false, true);
        });
        for (var i = 0; i < selected.length; i++) {
            Flauntly.linkToggle(document.getElementById(selected[i]), false);
        }
    };
    return topNav;
})();
//# sourceMappingURL=Core.js.map
