﻿var app = angular.module("Roulette", []);

app.controller("HomeController", function ($scope, $http) {
    $scope.bet = '';
    $scope.Numbers = [];
    $scope.HotNumbers = [];
    $scope.CoolNumbers = [];
    $scope.OddEvenStats = [];
    $scope.ColorStats = [];
    $scope.BetPlaced = false;
    $scope.ActionFailed = false;

    $http.get('/api/RouletteEntry/RetrieveNumbers')
        .then(function successCallback(response) {
            $scope.Numbers = JSON.parse(JSON.stringify(response.data));
        },
            function failureCallback(response) {
                var data = response.Data;
            });
},
    this.PlaceYourBet = function ($scope, $http) {
        $http.post('/api/RouletteEntry/CreateUserInput', bet).then(
            function successCallback(response) {
                if (response.status == 200)
                    $scope.BetPlaced = true;
            },
            function failureCallback(response) {
                $scope.ActionFailed = true;
            });
});
