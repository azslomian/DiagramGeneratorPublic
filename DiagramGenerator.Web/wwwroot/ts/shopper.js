var Shopper = /** @class */ (function () {
    function Shopper(first, last) {
        this.firstName = "";
        this.lastName = "";
        this.firstName = first;
        this.lastName = last;
    }
    Shopper.prototype.showName = function () {
        alert(this.firstName + " " + this.lastName);
    };
    return Shopper;
}());
//# sourceMappingURL=shopper.js.map