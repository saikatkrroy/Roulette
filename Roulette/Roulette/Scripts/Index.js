var app = angular.module("Roulette", []);
app.controller("HomeController", function ($scope, $http) {
    $scope.bet = '';
    $scope.money = '';
    $scope.Numbers = [];
    $scope.HotNumbers = [];
    $scope.CoolNumbers = [];
    $scope.OddEvenStats = [];
    $scope.ColorStats = [];
    $scope.RouletteEvent = '';
    $scope.BetPlaced = false;
    $scope.ActionFailed = false;
    $scope.NumberLoadFailed = false;
    $scope.UpdateUserInput = false;
    $scope.DeleteUserInput = false;
    $scope.Clicked = 0;
    $scope.userSelectedBet = '';
    $scope.range = '';
    $http.get('/api/RouletteEntry/RetrieveNumbers')
        .then(function successCallback(response) {
            if (response.data == null)
                $scope.NumberLoadFailed = true;
            else
                $scope.Numbers = JSON.parse(JSON.stringify(response.data));
        },
            function failureCallback(response) {
                var data = response.Data;
            });
    PlaceYourBet = function () {
        if ($scope.Clicked == 0 && $scope.UpdateUserInput == false) {
            $scope.userSelectedBet = $scope.bet;
            var betModel = { "value": $scope.bet, "rouletteEventName": $scope.RouletteEvent, "betPlaced": $scope.money };
            $http.post('/api/RouletteEntry/CreateUserInput/', betModel).then(
                function successCallback(response) {
                    if (response.status == 204)
                        $scope.BetPlaced = true;
                    $scope.Clicked += 1;

                },
                function failureCallback(response) {
                    $scope.ActionFailed = true;
                });
        }
        if ($scope.Clicked >= 0 && $scope.UpdateUserInput == true) {
            UpdateBet();
        }
    };
    UpdateBet = function () {
        $http.put('/api/RouletteEntry/UpdateUserInput/' + $scope.bet + '/' + $scope.userSelectedBet).then(
                function successCallback(response) {
                    if (response.status == 204)
                        $scope.BetPlaced = true;
                    $scope.UpdateUserInput = false;
                    $scope.userSelectedBet = $scope.bet;
                    $scope.Clicked += 1;
                },
                function failureCallback(response) {
                    $scope.ActionFailed = true;
                    $scope.UpdateUserInput = false;
                });
    };
    $scope.DeleteBet = function () {
        $http.delete('/api/RouletteEntry/DeleteUserInput/'+ $scope.userSelectedBet).then(
            function successCallback(response) {
                if (response.status == 204)
                    $scope.DeleteUserInput = true;
            },
            function failureCallback(response) {
                $scope.DeleteUserInput = false;
            });
    };
    DisplayMinMaxValue = function () {
        if ($scope.RouletteEvent == "RA 01") {
            $scope.range = 'Min Euro 2, Max Euro 20';
        }
        if ($scope.RouletteEvent == "RA 11") {
            $scope.range = 'Min Euro 5, Max Euro 50';
        }
        if ($scope.RouletteEvent == "RA 02") {
            $scope.range = 'Min Euro 2, Max Euro 20';
        }
        if ($scope.RouletteEvent == "RA 12") {
            $scope.range = 'Min Euro 5, Max Euro 50';
        }
    };
    $scope.LogOff = function () {
        var response = confirm("Are you sure you want to LogOut");
        if (response == true) {
            $http.post('/api/Account/Logoff').then(function successCallback(response) {
                if (response.status == 200) {
                    window.location.href = response.data.RedirectUrl;
                }
            },
                function failureCallback(response) {
                    $scope.LoginFailed = true;
                });
        }
    }
});
