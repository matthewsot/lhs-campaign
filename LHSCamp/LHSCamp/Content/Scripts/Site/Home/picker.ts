/// <reference path="../../typings/jquery/jquery.d.ts" />
class pickerBox {
    pickerId: string;
    parentId: string;
    picker: JQuery;
    constructor(pickerId, parentId = null) {
        this.pickerId = pickerId;
        this.picker = $("#" + pickerId);
        if (parentId == null)
            parentId = pickerId;

        this.parentId = parentId;
    }

    addItem(item, row: number, col: number) {
        row = row - 1;
        col = col - 1;
        var positionClass = "";
        item.chosen = item.chosen.toString().toLowerCase();
        var hoverText = "ADD";
        if (item.chosen == "true") {
            hoverText = "REMOVE";
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
        newItem += '<div cand-id="' + item.id + '" cand-selected="' + item.chosen.toLowerCase() + '" class="picker-overlay">' + hoverText + '</div>';
        newItem += '<div class="picker-name">' + item.name + '</div>';
        newItem += '</div>';
        this.picker.html(this.picker.html() + newItem);

        $("#" + this.parentId).css("min-height", (((row + 1) * 340) + 40) + "px");
        $(".picker-overlay").unbind('mouseenter mouseleave click'); //Thanks! http://stackoverflow.com/questions/805133/how-do-i-unbind-hover-in-jquery
        $(".picker-overlay").hover(function () {
            $(this).stop().animate({
                opacity: 0.7
            });
        }, function () {
            if ($(this).attr("cand-selected") == "false") { //Only fade out for unselected items
                $(this).stop().animate({
                    opacity: 0
                });
            }
        });

        $(".picker-overlay").click(function () {
            if ($(this).attr("cand-selected") == "false") { //Add candidate to chosen list
                $(this).attr("cand-selected", "true");
                $(this).text("REMOVE");
                $.getJSON("/API/Chosen/Add/" + $(this).attr("cand-id"), function (data) {

                });
            }
            else { //Remove candidate from chosen list
                $(this).attr("cand-selected", "false");
                $.getJSON("/API/Chosen/Remove/" + $(this).attr("cand-id"), function (data) {

                });
                $(this).text("ADD");
            }
        });

        $('[cand-selected="true"]').stop().animate({
            opacity: 0.7
        });
    }

    switchTo(position) {
        var self = this;
        $.getJSON("/API/Candidates/" + position, function (data) {
            self.picker.html("");
            for (var i = 0; i < data.length; i++) {
                var col = (i + 1) % 3;
                if (col == 0) col = 3;

                self.addItem(data[i], Math.ceil((i + 1) / 3), col);
            }
        });
    }
}