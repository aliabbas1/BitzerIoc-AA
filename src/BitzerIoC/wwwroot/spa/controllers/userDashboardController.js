app.controller('userDashboardController', function ($scope)
{
    
});





//$scope.makeNotSelected = true;
//$scope.searchText = "";
//LoadStates();
//LoadMakes();
//LoadPrices();


//$scope.makeChange = function () {
//    var makeId = $scope.selectedMake.MakeId;
//    $scope.makeNotSelected = $scope.selectedMake.MakeId != 0 ? false : true;
//    LoadModels(makeId);

//};

//$scope.searchByQuery = function () {
//    var query = $scope.searchText;
//    var q = "";
//    if (query != "") {
//        q = query.replace(/ /g, "_");
//        $location.path("/used-cars/carlisting/all/all/all/all/q-" + q);
//    }
//};


//$scope.advanceSearch = function () {

//    var make = $scope.selectedMake;
//    var model = $scope.selectedModel;
//    var state = $scope.selectedState;
//    var price = $scope.selectedPrice;

//    var prms = "";
//    var defaultValue = "all";

//    prms += "/make-";
//    prms += make != null ? make.MakeName : defaultValue;

//    prms += "/model-";
//    prms += model != null ? model.ModelName : defaultValue;

//    prms += "/state-";
//    prms += state != null ? state.StateName : defaultValue;

//    prms += "/price-";
//    prms += price != null ? price.Value : defaultValue;

//    prms = prms.replace(/ /g, "_");
//    console.info(prms);
//    $location.path("/used-cars/carlisting" + prms);


//}


//// --------------- Fill Dropdown Lists ------------
//function LoadMakes() {
//    //Call service to get makes
//    var makes = usedVehiclesService.getMakes(true);
//    makes.then(function (result) { $scope.makes = result.data; $scope.selectedMake = null; }, function (error_make) { $scope.error = error_make; });

//}

//function LoadModels(makeId) {
//    var models = usedVehiclesService.getModels(makeId); //result.data[0];
//    models.then(function (result) { $scope.models = result.data; $scope.selectedModel = null; }, function (error_models) { $scope.error = error_models; });
//}

//function LoadPrices() {
//    var prices = usedVehiclesService.getPriceRange();
//    prices.then(function (result) { $scope.prices = result.data; $scope.selectedPrice = null; }, function (error_prices) { $scope.error = error_prices; });
//}

//function LoadStates() {
//    var states = usedVehiclesService.getStates();
//    states.then(function (result) { $scope.states = result.data; $scope.selectedState = null; }, function (error_states) { $scope.error = error_states; });
//}