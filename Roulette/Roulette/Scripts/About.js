var app = angular.module("Roulette", []);

app.controller("HomeController", function ($scope, $http) {
    $scope.HotNumbers = [];
    $scope.CoolNumbers = [];
    $scope.OddEvenStats = [];
    $scope.ColorStats = [];
    $scope.Users = [];
    $scope.userId = '';
    $scope.OddEvenStatsFailure = false;
    $scope.ColorStatsFailure = false;
    $scope.HotNumberFailure = false;
    $scope.CoolNumberFailure = false;
    $scope.ZeroFailure = false;
    $scope.authToken = '';
    $scope.authenticated = false;

    $http.get('/api/RouletteEntry/RetrieveUsers')
        .then(function successCallback(response) {
            if (response.data.length > 0) {
                $scope.authenticated = true;
                $scope.Users = JSON.parse(JSON.stringify(response.data));
            }
        },
            function failureCallback(response) {
            });
    RetrieveStats = function () {
        $http.get('/api/RouletteEntry/RetrieveOddEvenStats', $scope.userId)
            .then(function successCallback(response) {
                if (response.data.Odd == undefined && response.data.Even == undefined)
                    $scope.OddEvenStatsFailure = true;
                else
                    $scope.OddEvenStats = JSON.parse(JSON.stringify(response.data));
            },
                function failureCallback(response) {
                    $scope.OddEvenStatsFailure = true;
                });
        $http.get('/api/RouletteEntry/RetrieveColorStats', $scope.userId)
            .then(function successCallback(response) {
                if (response.data.Black == undefined && response.data.Red == undefined)
                    $scope.ColorStatsFailure = true;
                else
                    $scope.ColorStats = JSON.parse(JSON.stringify(response.data));
            },
                function failureCallback(response) {
                    $scope.ColorStatsFailure = true;
                });
        $http.get('/api/RouletteEntry/RetrieveCoolNumber', $scope.userId)
            .then(function successCallback(response) {
                if (response.data.length == 0)
                    $scope.CoolNumberFailure = true;
                else
                    $scope.CoolNumbers = JSON.parse(JSON.stringify(response.data));
            },
                function failureCallback(response) {
                    $scope.CoolNumberFailure = true;
                });
        $http.get('/api/RouletteEntry/RetrieveHotNumber', $scope.userId)
            .then(function successCallback(response) {
                if (response.data.length == 0)
                    $scope.HotNumberFailure = true;
                else
                    $scope.HotNumbers = JSON.parse(JSON.stringify(response.data));
            },
                function failureCallback(response) {
                    $scope.HotNumberFailure = true;
                });
        $http.get('/api/RouletteEntry/RetrieveZeroPercentage', $scope.userId)
            .then(function successCallback(response) {
                if (response.data.Zero == 0)
                    $scope.ZeroFailure = true;
                else
                    $scope.Zero = JSON.parse(JSON.stringify(response.data));
            },
                function failureCallback(response) {
                    $scope.ZeroFailure = true;

                });
    }
});
