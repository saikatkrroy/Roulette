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
        $http.post('/api/RouletteEntry/CreateUserInput', $scope.bet).then(
            function successCallback(response) {
                if (response.status == 204)
                    $scope.BetPlaced = true;
            },
            function failureCallback(response) {
                $scope.ActionFailed = true;
            });
    }
});
