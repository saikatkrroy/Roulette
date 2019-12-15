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
    $scope.ZeroFailure = false;
    $scope.authToken = '';
    $scope.authenticated = false;
    $scope.Zero = '';
    
    $http.get('/api/RouletteEntry/RetrieveOddEvenStats/false')
        .then(function successCallback(response) {
            if (response.data.Odd == undefined && response.data.Even == undefined)
                $scope.OddEvenStatsFailure = true;
            else
                $scope.OddEvenStats = JSON.parse(JSON.stringify(response.data));
        },
            function failureCallback(response) {
                $scope.OddEvenStatsFailure = true;
            });
    $http.get('/api/RouletteEntry/RetrieveColorStats/false')
        .then(function successCallback(response) {
            if (response.data.Black == undefined && response.data.Red == undefined)
                $scope.ColorStatsFailure = true;
            else
                $scope.ColorStats = JSON.parse(JSON.stringify(response.data));
        },
            function failureCallback(response) {
                $scope.ColorStatsFailure = true;
            });
    $http.get('/api/RouletteEntry/RetrieveCoolNumber/false')
        .then(function successCallback(response) {
            if (response.data.length == 0)
                $scope.CoolNumberFailure = true;
            else
                $scope.CoolNumbers = JSON.parse(JSON.stringify(response.data));
        },
            function failureCallback(response) {
                $scope.CoolNumberFailure = true;
            });
    $http.get('/api/RouletteEntry/RetrieveHotNumber/false')
        .then(function successCallback(response) {
            if (response.data.length == 0)
                $scope.HotNumberFailure = true;
            else
                $scope.HotNumbers = JSON.parse(JSON.stringify(response.data));
        },
            function failureCallback(response) {
                $scope.HotNumberFailure = true;
            });
    $http.get('/api/RouletteEntry/RetrieveZeroPercentage/false')
        .then(function successCallback(response) {
             $scope.Zero = JSON.parse(JSON.stringify(response.data));
        },
            function failureCallback(response) {
                $scope.ZeroFailure = true;

            });
    
});
