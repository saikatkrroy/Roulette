var app = angular.module("Roulette", []);

app.controller("HomeController", function ($scope, $http) {
    $scope.HotNumbers = [];
    $scope.CoolNumbers = [];
    $scope.OddEvenStats = [];
    $scope.ColorStats = [];
    $scope.OddEvenStatsFailure = false;
    $scope.ColorStatsFailure = false;
    $scope.HotNumberFailure = false;
    $scope.CoolNumberFailure = false;

    $http.get('/api/RouletteEntry/RetrieveOddEvenStats')
        .then(function successCallback(response) {
            if (response.data.Odd == undefined && response.data.Even == undefined)
                $scope.OddEvenStatsFailure = true;
            else
                $scope.OddEvenStats = JSON.parse(JSON.stringify(response.data));
        },
        function failureCallback(response) {
            $scope.OddEvenStatsFailure = true;
            });
    $http.get('/api/RouletteEntry/RetrieveColorStats')
        .then(function successCallback(response) {
            if (response.data.Black == undefined && response.data.Red == undefined)
                $scope.ColorStatsFailure = true;
            else
                $scope.ColorStats = JSON.parse(JSON.stringify(response.data));
        },
            function failureCallback(response) {
                $scope.ColorStatsFailure = true;
            });
    $http.get('/api/RouletteEntry/RetrieveCoolNumber')
        .then(function successCallback(response) {
            if (response.data.length == 0)
                $scope.CoolNumberFailure = true;
            else
                $scope.CoolNumbers = JSON.parse(JSON.stringify(response.data));
        },
            function failureCallback(response) {
                $scope.CoolNumberFailure = true;
            });
    $http.get('/api/RouletteEntry/RetrieveHotNumber')
        .then(function successCallback(response) {
            if (response.data.length == 0)
                $scope.HotNumberFailure = true;
            else
                $scope.HotNumbers = JSON.parse(JSON.stringify(response.data));
        },
            function failureCallback(response) {
                $scope.HotNumberFailure = true;

            });
    });
