(function() {
    function IndexViewModel() {
        var self = this;

        self.what = ko.observable();

        self.onSearch = function() {
            if (self.what()) {
                location = self.what();
            }
        }
    }

    ko.applyBindings(new IndexViewModel);
})();