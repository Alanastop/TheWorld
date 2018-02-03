//tripsController
(function () {

    "use strict";
    //Getting the existing module
    angular.module("app-trips")
        .controller("tripsController", tripsController);

    function tripsController($http, $scope, $routeParams) {
        var viewModel = this;

        viewModel.trips = [];
        viewModel.newTrip = {};
        viewModel.errorMessage = "";
        viewModel.isBusy = true;
        var username = document.getElementById('userName').innerHTML;       
        debugger;
        
        $http({
            url: "//localhost:10816/api/trips/",
            method: "GET",
            params: { username }
        })
            .then(function (response) {
                //Succes
                angular.copy(response.data, viewModel.trips)
            }, function (error) {
                //failure
                viewModel.errorMessage = "Failed to load data: " + error.statusText;
            })
            .finally(function () {
                viewModel.isBusy = false;
            });

        viewModel.CreateOrUpdateTrip = function () {
            debugger;
            if (viewModel.newTrip.id === undefined || viewModel.newTrip.id === 0)
                viewModel.addTrip();
            else
                viewModel.updateTrip();
        }

        viewModel.addTrip = function () {
            debugger;
            viewModel.errorMessage = "";
            viewModel.isBusy = true;
            viewModel.newTrip.username = username;
            
            $http.post("//localhost:10816/api/trips", viewModel.newTrip)
                .then(function (response) {
                    //success
                    viewModel.trips.push(response.data);
                    viewModel.newTrip = {};
                }, function (error) {
                    //failure
                    viewModel.errorMessage = "Failed to save data: " + error.statusText;
                })
                .finally(function () {
                    viewModel.isBusy = false;
                });
        }        

        viewModel.updateTrip = function () {
            debugger;
            viewModel.errorMessage = "";
            viewModel.isBusy = true;
            viewModel.newTrip.username = username;

            $http.post("//localhost:10816/api/trips/update", viewModel.newTrip)
                .then(function (response) {
                    //success
                    //viewModel.trips.push(response.data);
                    var index = viewModel.trips.indexOf(viewModel.newTrip.id);
                    viewModel.trips.splice(index, 1);
                    viewModel.trips.push(response.data); 
                    viewModel.newTrip = {};
                }, function (error) {
                    //failure
                    viewModel.errorMessage = "Failed to save data: " + error.statusText;
                })
                .finally(function () {
                    viewModel.isBusy = false;
                });
        }

        viewModel.deleteTrip = function () {
            debugger;
            var trips = viewModel.trips;
            viewModel.errorMessage = "";
            viewModel.isBusy = true;           

            var data = {
                Id: viewModel.trip.id,
                Username: username
            }
            var url = "http://localhost:10816/api/trips/delete";
            
            //$http.post("//localhost:10816/api/trips/delete", viewModel.newTrip)

            $http({
                method: "POST",
                url: url,
                data: data
            })         
                .then(function (response) {
                    //success
                    viewModel.trips = [];
                    viewModel.trips = response.data;          
                    viewModel.newTrip = {};
                }, function (error) {
                    //failure
                    viewModel.errorMessage = "Failed to delete Trip: " + error.statusText;
                })
                .finally(function () {
                    viewModel.isBusy = false;
                });
        }

        viewModel.UpdateTrip = function (trip, index) {
            debugger;
            viewModel.newTrip = trip;
        }
    }
})();

