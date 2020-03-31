var app = angular.module("Roulette", []);
app.controller("HomeController", function ($scope, $http) {
    $scope.bet = '';
    $scope.money = '';
    $scope.Data = [];
    $scope.HotNumbers = [];
    $scope.CoolNumbers = [];
    $scope.OddEvenStats = [];
    $scope.ColorStats = [];
    $scope.LastTwelveBets = [];
    $scope.RouletteEvent = '';
    $scope.BetPlaced = false;
    $scope.ActionFailed = false;
    $scope.NumberLoadFailed = false;
    $scope.updateUserInput = false;
    $scope.DeleteUserInput = false;
    $scope.userSelectedBet = '';
    $scope.range = '';
    $scope.formValidated = true;
    $scope.HotNumbers = [];
    $scope.CoolNumbers = [];
    $scope.OddEvenStats = [];
    $scope.ColorStats = [];
    $scope.OddEvenStatsFailure = false;
    $scope.ColorStatsFailure = false;
    $scope.HotNumberFailure = false;
    $scope.CoolNumberFailure = false;
    $scope.LastTwelveBetFailure = false;
    $scope.ZeroFailure = false;
    $scope.StatsVisible = false;
    $scope.Zero = '';
    $http.get('/api/RouletteEntry/RetrieveData')
        .then(function successCallback(response) {
            if (response.data == null)
                $scope.NumberLoadFailed = true;
            else {
                $scope.Data = JSON.parse(JSON.stringify(response.data));
            }
        },
            function failureCallback(response) {
                var data = response.Data;
            });
    $http.get('/api/RouletteEntry/UserInputUpdateData').then(
        function successCallback(response) {
            if (response.status == 200) {
                $scope.bet = response.data.Number.Number;
                $scope.RouletteEvent = response.data.RouletteEvent.EventName;
                $scope.money = response.data.BetPlaced;
            }

        },
        function failureCallback(response) {
            $scope.ActionFailed = true;
        });
    PlaceYourBet = function () {
        $scope.formValidated=ValidateUserInput();

        if ($scope.updateUserInput === false && $scope.formValidated === true) {
            $scope.userSelectedBet = $scope.bet;
            var betModel = { "value": $scope.bet, "rouletteEventName": $scope.RouletteEvent };
            $http.post('/api/RouletteEntry/CreateUserInput/', betModel).then(
                function successCallback(response) {
                    if (response.status === 204)
                        $scope.BetPlaced = true;

                },
                function failureCallback(response) {
                    $scope.ActionFailed = true;
                });
        }
        if ($scope.updateUserInput === true) {
            UpdateBet();
        }
    };
    $scope.UserInputUpdate = function () {
        $scope.updateUserInput = true;
        $http.get('/api/RouletteEntry/UserInputUpdate').then(
            function successCallback(response) {
                if (response.status === 200)
                {
                    $scope.BetPlaced = true;
                }

            },
            function failureCallback(response) {
                $scope.ActionFailed = true;
            });
    };
    ValidateUserInput = function () {
        if ($scope.bet === "" || $scope.RouletteEvent === "" )
            return false;
        //if ($scope.RouletteEvent == "RA 01" || $scope.RouletteEvent == "RA 02") {
        //    if ($scope.money < 2 || $scope.money > 20) {
        //        DisplayMinMaxValue();
        //        return false;
        //    }
        //}
        //if ($scope.RouletteEvent == "RA 11" |$scope.RouletteEvent == "RA 12") {
        //    if ($scope.money < 5 || $scope.money > 50) {
        //        DisplayMinMaxValue();
        //        return false;
        //    }
        //}
        return true;
    };
    UpdateBet = function () {
        $http.put('/api/RouletteEntry/UpdateUserInput/' + $scope.bet + '/' + $scope.userSelectedBet).then(
                function successCallback(response) {
                    if (response.status === 204)
                        $scope.BetPlaced = true;
                    $scope.updateUserInput = false;
                    $scope.userSelectedBet = $scope.bet;
                },
                function failureCallback(response) {
                    $scope.ActionFailed = true;
                    $scope.updateUserInput = false;
                });
    };
    $scope.DeleteBet = function () {
        $http.delete('/api/RouletteEntry/DeleteUserInput/'+ $scope.userSelectedBet).then(
            function successCallback(response) {
                if (response.status === 204)
                    $scope.DeleteUserInput = true;
            },
            function failureCallback(response) {
                $scope.DeleteUserInput = false;
            });
    };
    DisplayMinMaxValue = function () {
        if ($scope.RouletteEvent === "RA 01") {
            $scope.range = 'Min Euro 2, Max Euro 20';
        }
        if ($scope.RouletteEvent === "RA 11") {
            $scope.range = 'Min Euro 5, Max Euro 50';
        }
        if ($scope.RouletteEvent === "RA 02") {
            $scope.range = 'Min Euro 2, Max Euro 20';
        }
        if ($scope.RouletteEvent === "RA 12") {
            $scope.range = 'Min Euro 5, Max Euro 50';
        }
    };
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
    RetrieveStats = function () {

        $http.get('/api/RouletteEntry/RetrieveOddEvenStats/true')
            .then(function successCallback(response) {
                if (response.data.Odd == undefined && response.data.Even == undefined)
                    $scope.OddEvenStatsFailure = true;
                else
                    $scope.OddEvenStats = JSON.parse(JSON.stringify(response.data));
            },
                function failureCallback(response) {
                    $scope.OddEvenStatsFailure = true;
                });
        $http.get('/api/RouletteEntry/RetrieveColorStats/true')
            .then(function successCallback(response) {
                if (response.data.Black == undefined && response.data.Red == undefined)
                    $scope.ColorStatsFailure = true;
                else
                    $scope.ColorStats = JSON.parse(JSON.stringify(response.data));
            },
                function failureCallback(response) {
                    $scope.ColorStatsFailure = true;
                });
        $http.get('/api/RouletteEntry/RetrieveCoolNumber/true')
            .then(function successCallback(response) {
                if (response.data.length === 0)
                    $scope.CoolNumberFailure = true;
                else
                    $scope.CoolNumbers = JSON.parse(JSON.stringify(response.data));
            },
                function failureCallback(response) {
                    $scope.CoolNumberFailure = true;
                });
        $http.get('/api/RouletteEntry/RetrieveHotNumber/true')
            .then(function successCallback(response) {
                if (response.data.length === 0)
                    $scope.HotNumberFailure = true;
                else
                    $scope.HotNumbers = JSON.parse(JSON.stringify(response.data));
            },
                function failureCallback(response) {
                    $scope.HotNumberFailure = true;
                });
        $http.get('/api/RouletteEntry/RetrieveZeroPercentage/true')
            .then(function successCallback(response) {
                $scope.Zero = JSON.parse(JSON.stringify(response.data));
            },
                function failureCallback(response) {
                    $scope.ZeroFailure = true;

            });
        $http.get('/api/RouletteEntry/LastTwelveBet/true')
            .then(function successCallback(response) {
                if (response.data.length === 0)
                    $scope.LastTwelveBetFailure = true;
                else
                    $scope.LastTwelveBets = JSON.parse(JSON.stringify(response.data));
            },
                function failureCallback(response) {
                    $scope.LastTwelveBetFailure = true;
                });
        $scope.StatsVisible = true;

    }
});
