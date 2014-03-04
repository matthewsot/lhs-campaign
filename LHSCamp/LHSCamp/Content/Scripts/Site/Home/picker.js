/// <reference path="../../typings/jquery/jquery.d.ts" />
var pickerBox = (function () {
    function pickerBox(pickerId, parentId) {
        if (typeof parentId === "undefined") { parentId = null; }
        this.pickerId = pickerId;
        this.picker = $("#" + pickerId);
        if (parentId == null)
            parentId = pickerId;

        this.parentId = parentId;
    }
    pickerBox.prototype.addItem = function (item, row, col) {
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
        var newItem = '<div class="picker-item ' + positionClass + '" id="picker-item-' + item.id + '" style="margin-top:' + row * 340 + 'px;">';
        newItem += '<img class="picker-img" src="' + item.profilePic + '" alt="' + item.name + '"></img>';
        newItem += '<div class="picker-overlay"></div>';
        newItem += '<br/><div class="picker-name">' + item.name + '</div>';
        newItem += '</div>';
        this.picker.html(this.picker.html() + newItem);

        $("#" + this.parentId).css("min-height", (((row + 1) * 340) + 40) + "px");
        $(".picker-img").unbind('mouseenter mouseleave'); //Thanks! http://stackoverflow.com/questions/805133/how-do-i-unbind-hover-in-jquery
        $(".picker-img").hover(function () {
            $(this).next().stop().fadeIn();
        }, function () {
            $(this).next().stop().fadeOut();
        });
    };

    pickerBox.prototype.switchTo = function (position) {
        var self = this;
        $.getJSON("/API/Candidates/" + position, function (data) {
            self.picker.html("");
            for (var i = 0; i < data.length; i++) {
                var col = (i + 1) % 3;
                if (col == 0)
                    col = 3;

                self.addItem(data[i], Math.ceil((i + 1) / 3), col);
            }
        });
    };
    return pickerBox;
})();
//# sourceMappingURL=picker.js.map
