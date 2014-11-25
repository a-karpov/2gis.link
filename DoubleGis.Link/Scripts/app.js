(function (app) {
    function IndexViewModel() {
        var self = this;

        this.what = ko.observable();
        this.where = ko.observable('Новосибирск');

        this.action = function () {
            location =  self.what() + '/' + self.where();
        }
    };

    function SearchViewModel() {
        var self = this;

        if (Modernizr.localstorage) {
            // window.localStorage is available!
        }
    };

    app.applyIndexViewModel = function() {
        ko.applyBindings(new IndexViewModel());
    }

    app.applySearchViewModel = function () {
        ko.applyBindings(new SearchViewModel());
    }
})(window.app = {});