var app = angular.module("Roulette", []);

app.controller("HomeController", function ($scope, $http) {
    $scope.HotNumbers = [];
    $scope.CoolNumbers = [];
    $scope.OddEvenStats = [];
    $scope.ColorStats = [];
    $scope.LastTwelveBets = [];
    $scope.OddEvenStatsFailure = false;
    $scope.ColorStatsFailure = false;
    $scope.HotNumberFailure = false;
    $scope.CoolNumberFailure = false;
    $scope.LastTwelveBetFailure = false;
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
    $http.get('/api/RouletteEntry/LastTwelveBet/false')
        .then(function successCallback(response) {
            if (response.data.length == 0)
                $scope.LastTwelveBetFailure = true;
            else
                $scope.LastTwelveBets = JSON.parse(JSON.stringify(response.data));
        },
            function failureCallback(response) {
                $scope.LastTwelveBetFailure = true;
            });
    $scope.LogOff = function () {
        var response = confirm("Are you sure you want to LogOut");
        if (response === true) {
            $http.post('/api/Account/Logoff').then(function successCallback(response) {
                    if (response.status === 200) {
                        window.location.href = response.data.RedirectUrl;
                    }
                },
                function failureCallback(response) {
                    $scope.LoginFailed = true;
                });
        }
    };
    window.onunload = function () {
        LogOff();
    };
});
