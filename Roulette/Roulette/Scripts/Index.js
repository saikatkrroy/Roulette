var app = angular.module("Roulette", []);
app.controller("HomeController", function ($scope, $http) {
    $scope.bet = '';
    $scope.Numbers = [];
    $scope.HotNumbers = [];
    $scope.CoolNumbers = [];
    $scope.OddEvenStats = [];
    $scope.ColorStats = [];
    $scope.BetPlaced = false;
    $scope.ActionFailed = false;
    $scope.NumberLoadFailed = false;
    $scope.UpdateUserInput = false;
    $scope.Clicked = 0;
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
        var userSelectedBet = $scope.bet;
        if ($scope.Clicked == 0 && $scope.UpdateUserInput == false) {
            var betModel = { "value": $scope.bet, "rouletteEventName":"RA 01", "betPlaced": 10 };
            $http.post('/api/RouletteEntry/CreateUserInput/', betModel).then(
                function successCallback(response) {
                    if (response.status == 204)
                        $scope.BetPlaced = true;
                },
                function failureCallback(response) {
                    $scope.ActionFailed = true;
                });
        }
        if ($scope.Clicked == 0 && $scope.UpdateUserInput == true) {
            $http.post('/api/RouletteEntry/UpdateUserInput/'+ $scope.bet+'/'+ userSelectedBet).then(
                function successCallback(response) {
                    if (response.status == 204)
                        $scope.BetPlaced = true;
                    $scope.UpdateUserInput = false;
                },
                function failureCallback(response) {
                    $scope.ActionFailed = true;
                    $scope.UpdateUserInput = false;
                });
        }
        $scope.Clicked += 1;
    }
});
