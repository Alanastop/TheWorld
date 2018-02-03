// tripEditorController.js
(function () {
    "use strict";

    angular.module("app-trips")
        .controller("tripEditorController", tripEditorController);

    function tripEditorController($routeParams, $filter, $scope, $http) {
        var viewModel = this;

        viewModel.tripName = $routeParams.tripName;
        viewModel.stops = [];
        viewModel.errorMessage = "";
        viewModel.isBusy = true;
        viewModel.newStop = {};
        var username = document.getElementById('userName').innerHTML;

        debugger;
        $http.get("http://localhost:10816/api/stops/" + viewModel.tripName + "/" + username + "/GetEntityByName/")
            .then(function (response) {
                //success
                angular.copy(response.data, viewModel.stops);
                showMap(viewModel.stops);
            }, function (error) {
                //failure
                viewModel.error = "Failed to load stops" + error.statusText;
            })
            .finally(function () {
                viewModel.isBusy = false;
            });

        viewModel.CreateOrUpdateStop = function () {
            debugger;
            if (viewModel.newStop.id === undefined || viewModel.newStop.id === 0)
                viewModel.addStop();
            else
                viewModel.updateStop();
        }

        viewModel.addStop = function () {
            viewModel.isBusy = true;   
            var url = "http://localhost:10816/api/stops/" + viewModel.tripName + "/" + username + "/post/";
            debugger;
            
            var data = {
                Name: viewModel.newStop.name,
                Arrival: viewModel.newStop.arrival                
            }
            
            $http({
                method: "POST",
                url: url,
                data: data
            })           
                .then(function (response) {
                    //success                    
                    viewModel.stops.push(response.data);
                    showMap(viewModel.stops);
                    viewModel.newStop = {};
                    showMap(viewModel.stops);
                }, function (error) {
                    //failure
                    viewModel.error = "Failed to add the new stop" + error.statusText;
                })
                .finally(function () {
                    viewModel.isBusy = false;
                });
        }

        viewModel.updateStop = function () {
            viewModel.isBusy = true;
            var url = "http://localhost:10816/api/stops/" + viewModel.tripName + "/" + username + "/update/";
            debugger;

            var data = {
                Name: viewModel.newStop.name,
                Arrival: viewModel.newStop.arrival,
                Id: viewModel.newStop.id
            }

            $http({
                method: "POST",
                url: url,
                data: data
            })
                .then(function (response) {
                    //success
                    var index = viewModel.stops.indexOf(viewModel.newStop.id);
                    viewModel.stops.splice(index, 1);
                    viewModel.stops.push(response.data);                 
                    viewModel.newStop = {};                    
                    showMap(viewModel.stops);
                }, function (error) {
                    //failure
                    viewModel.error = "Failed to update the new stop" + error.statusText;
                })
                .finally(function () {
                    viewModel.isBusy = false;
                });
        }

        viewModel.deleteStop = function (index) {
            debugger;
            var stops = viewModel.stops;
            viewModel.errorMessage = "";
            viewModel.isBusy = true;

            var data = {
                Id: viewModel.stop.id,
            }
            var url = "http://localhost:10816/api/stops/delete";

            $http({
                method: "POST",
                url: url,
                data: data
            })
                .then(function (response) {
                    //success
                    //var index = viewModel.stops.indexOf(viewModel.stop.id);
                    viewModel.stops.splice(index, 1);
                    viewModel.newStop = {}; 
                    showMap(viewModel.stops);
                }, function (error) {
                    //failure
                    viewModel.errorMessage = "Failed to delete Stop: " + error.statusText;
                })
                .finally(function () {
                    viewModel.isBusy = false;
                });
        }

        viewModel.UpdateStop = function (stop, index) {
            debugger;
            stop.arrival = $filter('date')(stop.arrival, "MM/dd/yyyy")
            viewModel.newStop = stop;
            viewModel.deleteStop(index)
        }
    }

 

    function showMap(stops) {
        debugger;
        if (stops && stops.length > 0) {

            var mapStops = _.map(stops, function (item) {
                return {
                    lat: item.latitude,
                    long: item.longtitude,
                    info: item.name
                };
            });
            //show Map
            travelMap.createMap({
                stops: mapStops,
                selector: "#map",
                currentStop: 1,
                initialZoom: 3
            });
        }
    }

})();