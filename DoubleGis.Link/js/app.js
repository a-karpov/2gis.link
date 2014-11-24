(function($) {
    function ViewModel(){
        var self = this;
        
        self.query = ko.observable();
        self.cards = ko.observable();
        
        Sammy(function(){
            
            this.get('/', function() { 
                self.cards(null);
            });
            
            this.get('/#search/:what', function(context) {
                 $.get('api/search', {what: context.params.what, where: 'Новосибирск'}, function(data) {
                    self.cards(data.result);
                });
            });
            
        }).run();
    };
    

    
    $(function(){
        ko.applyBindings(new ViewModel());
    });
})(jQuery);
