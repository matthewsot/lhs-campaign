/// <reference path="../../typings/jquery/jquery.d.ts" />
/// <reference path="../../Flauntly/Wedge/Core.ts" />

class picker {
    static updateChosenPreviews() {
        $.getJSON("/API/Chosen", function (data) {
            if (data.length == 0) {
                $("#footer-items").fadeOut(function () {
                    $("#footer-noitems").fadeIn();
                });
            }
            else {
                var newChosenPreviews = "";
                for (var i = 0; i < data.length; i++) {
                    var item = data[i];
                    var itemPreview = '<li id="item-preview-' + item.id + '">';
                    itemPreview += '<img class="cand-preview" data-cand-id="' + item.id + '" src="' + item.profilePic + '" alt="' + item.name + '"></img>';
                    itemPreview += '</li>';
                    newChosenPreviews += itemPreview;
                }
                $("#chosen-previews").html(newChosenPreviews);
                $(".cand-preview").unbind("click");
                $(".cand-preview").click(function () {
                    var id = $(this).attr("data-cand-id");
                    $.get("/API/Chosen/Remove/" + id, function (data) {
                        picker.updateChosenPreviews();
                        var pickerBoxButton = $("[data-cand-id=" + id + "]");
                        if (pickerBoxButton.length > 0) {
                            pickerBoxButton.attr("data-cand-selected", "false");
                            pickerBoxButton.text("add picture");
                        }
                    });
                });
                $("#footer-noitems").fadeOut(function () {
                    $("#footer-items").fadeIn();
                });
            }
        });
    }
    static showWedge(id) {
        $.getJSON("/API/Candidate/Details/" + id, function (data) {
            

            $("#cand-wedge-img").attr("src", data.profilePic);
            $("#cand-wedge-name").text(data.name);
            $("#cand-wedge-position").text(data.position);
            if (data.email != null) {
                $("#cand-wedge-email").text(data.email);
            }
            else {
                $("#cand-wedge-email").text("(no email provided)");
            }
            $("#facebook-btn").attr("data-cand-id", id);
            if (data.facebook == null) {
                $("#cand-wedge-fb").hide();
            }
            else {
                $("#cand-wedge-fb").unbind("click");
                $("#cand-wedge-fb").click(function () {
                    window.open(data.facebook);
                });
                $("#cand-wedge-fb").show();
            }
            if (data.coverPhoto == null) {
                $("#cand-wedge-cover").hide();
            }
            else {
                $("#cand-wedge-cover").unbind("click");
                $("#cand-wedge-cover").click(function () {
                    window.open(data.coverPhoto);
                });
                $("#cand-wedge-cover").show();
            }
            if (data.reasons != null && data.reasons.length > 0) {
                $("#cand-wedge-reasons").text(data.reasons);
            } else {
                $("#cand-wedge-reasons").text("This candidate hasn't filled in his or her campaign summary!");
            }
            initWedge("cand-wedge", '', 'div', false, 0.7);
        });
    }

    static addItem(item, row: number, col: number) {
        row = row - 1;
        col = col - 1;
        var positionClass = "";
        item.chosen = item.chosen.toString().toLowerCase();
        var hoverText = "add picture";
        if (item.chosen == "true") {
            hoverText = "remove picture";
        }
        switch (col) {
            case 0:
                positionClass = "left-item";
                break;
            case 1:
                positionClass = "mid-item";
                break;
            case 2:
                positionClass = "right-item";
                break;
        }
        var newItem = '<div class="picker-item ' + positionClass + '" id="picker-item-' + item.id + '" style="margin-top:' + row * 340 + 'px;">';
        newItem += '<img class="picker-img" src="' + item.profilePic + '" alt="' + item.name + '"></img>';
        //newItem += '<div id="picker-overlay-' + item.id + '" data-cand-id="' + item.id + '" data-cand-pic="' + item.profilePic + '" data-cand-selected="' + item.chosen.toLowerCase() + '" data-cand-name="' + item.name + '" class="picker-overlay">' + hoverText + '</div>';
        newItem += '<div class="picker-info">';
        newItem += '<button class="default-btn picker-info-btn picker-add-btn" ';
        newItem += 'data-cand-id="' + item.id + '" data-cand-selected="' + item.chosen.toLowerCase() + '"';
        newItem += '>' + hoverText + ' </button>';
        newItem += '<button class="default-btn picker-info-btn picker-read-btn" data-cand-id="' + item.id + '">read more</button>';
        newItem += '<div class="picker-name half-vert">' + item.name.toUpperCase() + '</div>';
        newItem += '</div>';
        newItem += '</div>';
        $("#picker-box").append(newItem);

        $("#picker-container").css("min-height", (((row + 1) * 340) + 40) + "px");

        $(".picker-item").unbind('mouseenter mouseleave'); //Thanks! http://stackoverflow.com/questions/805133/how-do-i-unbind-hover-in-jquery
        $(".picker-item").hover(function () {
            $(this).children().first().next().stop().animate({ bottom: 0 }, 200);
            $(this).children().first().stop().animate({ marginTop: -75 }, 300);
        }, function () {
                $(this).children().first().next().stop().animate({ bottom: -100 }, 500);
                $(this).children().first().stop().animate({ marginTop: 0 }, 300);
            });
        $(".picker-add-btn").unbind("click");
        $(".picker-add-btn").click(function () {
            if ($(this).attr("data-cand-selected") == "false") { //Add candidate to chosen list
                $(this).attr("data-cand-selected", "true");
                $(this).text("remove picture");
                $.getJSON("/API/Chosen/Add/" + $(this).attr("data-cand-id"), function (data) {
                    picker.updateChosenPreviews();
                });
            }
            else { //Remove candidate from chosen list
                $(this).attr("data-cand-selected", "false");
                $.getJSON("/API/Chosen/Remove/" + $(this).attr("data-cand-id"), function (data) {
                    picker.updateChosenPreviews();
                });
                $(this).text("add picture");
            }
        });
        var self = this;
        $(".picker-read-btn").unbind("click");
        $(".picker-read-btn").click(function () {
            self.showWedge($(this).attr("data-cand-id"));
        });

        if (item.chosen == "true") {
            setTimeout(function () {
                $("#picker-overlay-" + item.id).stop().animate({ opacity: 0.7 });
            }, 50);
        }
    }

    static switchTo(position) {
        $.getJSON("/API/Candidates/" + position, function (data) {
            $("#picker-box").html("");
            for (var i = 0; i < data.length; i++) {
                var col = (i + 1) % 3;
                if (col == 0) col = 3;

                picker.addItem(data[i], Math.ceil((i + 1) / 3), col);
            }
        });
        this.updateChosenPreviews();
    }
}