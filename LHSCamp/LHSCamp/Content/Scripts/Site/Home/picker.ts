/// <reference path="../../typings/jquery/jquery.d.ts" />

class pickerBox {
    pickerId: string;
    picker: JQuery;
    constructor(pickerId) {
        this.pickerId = pickerId;
        this.picker = $("#" + pickerId);
    }

    addItem(item, row: number, col: number) {
        row = row - 1;
        col = col - 1;
        var positionClass = "";
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
        var newItem = '<div class="picker-item ' + positionClass + '" id="picker-item-' + item.id + '" style="margin-top:' + row * 300 + 'px;">';
        newItem += '<img class="picker-img" src="' + item.profilePic + '" alt="' + item.name + '"></img>';
        newItem += '<br/><div class="picker-name">' + item.name + '</div>';
        newItem += '</div>';
        this.picker.html(this.picker.html() + newItem);
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