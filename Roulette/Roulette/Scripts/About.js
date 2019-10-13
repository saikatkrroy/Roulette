var app = angular.module("Roulette", []);

app.controller("HomeController", function ($scope, $http) {
    $scope.HotNumbers = [];
    $scope.CoolNumbers = [];
    $scope.OddEvenStats = [];
    $scope.ColorStats = [];

    $http.get('/api/RouletteEntry/RetrieveOddEvenStats')
        .then(function successCallback(response) {
            $scope.OddEvenStats = JSON.parse(JSON.stringify(response.data));
        },
            function failureCallback(response) {
                var data = response.Data;
            });
    $http.get('/api/RouletteEntry/RetrieveColorStats')
        .then(function successCallback(response) {
            $scope.ColorStats = JSON.parse(JSON.stringify(response.data));
        },
            function failureCallback(response) {
                var data = response.Data;
            });
    $http.get('/api/RouletteEntry/RetrieveCoolNumber')
        .then(function successCallback(response) {
            $scope.CoolNumbers = JSON.parse(JSON.stringify(response.data));
        },
            function failureCallback(response) {
                var data = response.Data;
            });
    $http.get('/api/RouletteEntry/RetrieveHotNumber')
        .then(function successCallback(response) {
            $scope.HotNumbers = JSON.parse(JSON.stringify(response.data));
        },
            function failureCallback(response) {
                var data = response.Data;
            });
    });
