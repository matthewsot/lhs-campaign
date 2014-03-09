﻿/// <reference path="../../typings/jquery/jquery.d.ts" />

class picker {
    static updateChosenPreviews() {
        $.getJSON("/API/Chosen", function (data) {
            var newChosenPreviews = "";
            for (var i = 0; i < data.length; i++) {
                var item = data[i];
                var itemPreview = '<li id="item-preview-' + item.id + '">';
                itemPreview += '<img class="cand-preview" data-cand-id="' + item.id + '" src="' + item.profilePic + '" alt="' + item.name + '"></img>';
                itemPreview += '</li>';
                newChosenPreviews += itemPreview;
            }
            $("#chosen-previews").html(newChosenPreviews);
            if ($("#footer-noitems").css("display") != "none") {
                $("#footer-noitems").fadeOut(function () {
                    $("#footer-items").fadeIn();
                });
            }
        });
    }

    static addItem(item, row: number, col: number) {
        row = row - 1;
        col = col - 1;
        var positionClass = "";
        item.chosen = item.chosen.toString().toLowerCase();
        var hoverText = "Select";
        if (item.chosen == "true") {
            hoverText = "Unselect";
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
        newItem += '<button class="picker-info-btn" ';
        newItem += 'data-cand-id="' + item.id + '" data-cand-selected="' + item.chosen.toLowerCase() + '"';
        newItem += '>' + hoverText + ' </button > ';
        newItem += '<div class="picker-name half-vert">' + item.name + '</div>';
        newItem += '</div>';
        newItem += '</div>';
        $("#picker-box").html($("#picker-box").html() + newItem);

        $("#picker-container").css("min-height", (((row + 1) * 340) + 40) + "px");

        $(".picker-item").unbind('mouseenter mouseleave'); //Thanks! http://stackoverflow.com/questions/805133/how-do-i-unbind-hover-in-jquery
        $(".picker-item").hover(function () {
            $(this).children().first().next().stop().animate({ bottom: 0 }, 100);
            $(this).children().first().stop().animate({ marginTop: -75 }, 300);
        }, function () {
            $(this).children().first().next().stop().animate({ bottom: -75 }, 500);
            $(this).children().first().stop().animate({ marginTop: 0 }, 300);
        });
        $(".picker-info-btn").unbind("click");
        $(".picker-info-btn").click(function () {
            if ($(this).attr("data-cand-selected") == "false") { //Add candidate to chosen list
                $(this).attr("data-cand-selected", "true");
                $(this).text("Unselect");
                $.getJSON("/API/Chosen/Add/" + $(this).attr("data-cand-id"), function (data) {
                    picker.updateChosenPreviews();
                });
            }
            else { //Remove candidate from chosen list
                $(this).attr("data-cand-selected", "false");
                $.getJSON("/API/Chosen/Remove/" + $(this).attr("data-cand-id"), function (data) {
                    picker.updateChosenPreviews();
                });
                $(this).text("Select");
            }
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