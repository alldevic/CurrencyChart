$(function () {

    var ViewModel = function (first, last) {
        this.firstName = ko.observable(first);
        this.lastName = ko.observable(last);

        this.fullName = ko.pureComputed(function () {
            return this.firstName() + " " + this.lastName();
        }, this);

        this.addSeat = function() {
            var self = this;
            self.firstName("Hello");
        }
    };

    ko.applyBindings(new ViewModel("Planet", "Earth"));
});