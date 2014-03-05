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

    addItemPreview(id: number, photo: string, alt: string) {
        var itemPreview = '<li id="item-preview-' + id + '"><img class="cand-preview" src="' + photo + '" alt="' + alt + '"></img></li>';
        //TODO: let chosen-previews id be specified in the constructor
        $("#chosen-previews").html($("#chosen-previews").html() + itemPreview);
        if ($("#footer-noitems").css("display") != "none") {
            $("#footer-noitems").fadeOut(function () {
                $("#footer-items").fadeIn();
            });
        }
    }

    removeItemPreview(id: number) {
        $("#item-preview-" + id).remove();
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
        newItem += '<div id="picker-overlay-' + item.id + '" cand-id="' + item.id + '" cand-pic="' + item.profilePic + '" cand-selected="' + item.chosen.toLowerCase() + '" cand-name="' + item.name + '" class="picker-overlay">' + hoverText + '</div>';
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

        var self = this;
        $(".picker-overlay").click(function () {
            if ($(this).attr("cand-selected") == "false") { //Add candidate to chosen list
                $(this).attr("cand-selected", "true");
                $(this).text("REMOVE");
                $.getJSON("/API/Chosen/Add/" + $(this).attr("cand-id"), function (data) {
                });
                self.addItemPreview(parseInt($(this).attr("cand-id")), $(this).attr("cand-pic"), $(this).attr("cand-name"));
            }
            else { //Remove candidate from chosen list
                $(this).attr("cand-selected", "false");
                $.getJSON("/API/Chosen/Remove/" + $(this).attr("cand-id"), function (data) {
                });
                self.removeItemPreview(parseInt($(this).attr("cand-id")));
                $(this).text("ADD");
            }
        });

        if (item.chosen == "true") {
            setTimeout(function () {
                $("#picker-overlay-" + item.id).stop().animate({ opacity: 0.7 });
            }, 50);
            this.addItemPreview(item.id, item.profilePic, item.name);
        }
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