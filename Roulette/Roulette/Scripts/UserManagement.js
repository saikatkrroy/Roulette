var app = angular.module("Roulette", []);
app.controller("HomeController", function ($scope, $http) {
    $scope.LoginFailed = false;
    $scope.userIdNew = '';
    $scope.passwordNew = '';
    $scope.createNewUser = false;
    $scope.deleteUser = false;
    $scope.userCreated = false;
    $scope.userDeleted = false;
    $scope.userCreationFailed = false;
    $scope.failedDelete = false;
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
    $scope.DisplayStats = false;
    $scope.Zero = '';
    $scope.userList = [];
    $scope.selectedUser = '';
    CreateNewUser = function () {
        var loginModel = { "Username": $scope.userIdNew, "Password": $scope.passwordNew };
        $http.post('/api/Account/CreateNewUser', loginModel).then(
            function successCallback(response) {
                if (response.status == 200)
                    $scope.userCreated = true;
            },
            function failureCallback(response) {
                $scope.userCreationFailed = true;
            });
    };
    RetrieveUsers = function () {
        $scope.createNewUser = false;
        $scope.deleteUser = true;
        $scope.DisplayStats = false;
        $http.get('/api/Accout/RetrieveUsers').then(
            function successCallback(response) {
                if (response.status == 200)
                    $scope.userList = response.data;
            },
            function failureCallback(response) {
                
            }
        );
    };
    DeleteUser = function () {
        var response = confirm("Are you sure you want to Delete this user");
        if (response == true) {
            $http.delete('/api/Accout/DeleteUser/' + $scope.selectedUser).then(
                function successCallback(response) {
                    if (response.status == 200)
                        $scope.userDeleted = true;
                },
                function failureCallback(response) {
                    $scope.failedDelete = true;

                });
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
    };
    RetrieveStats = function () {
        $scope.createNewUser = false;
        $scope.deleteUser = false;
        $scope.DisplayStats = true;
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
                if (response.data.length == 0)
                    $scope.CoolNumberFailure = true;
                else
                    $scope.CoolNumbers = JSON.parse(JSON.stringify(response.data));
            },
                function failureCallback(response) {
                    $scope.CoolNumberFailure = true;
                });
        $http.get('/api/RouletteEntry/RetrieveHotNumber/true')
            .then(function successCallback(response) {
                if (response.data.length == 0)
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
                if (response.data.length == 0)
                    $scope.LastTwelveBetFailure = true;
                else
                    $scope.LastTwelveBets = JSON.parse(JSON.stringify(response.data));
            },
                function failureCallback(response) {
                    $scope.LastTwelveBetFailure = true;
                });
        $scope.StatsVisible = true;

    };
    EnableUserCreation = function () {
        $scope.DisplayStats = false;
        $scope.deleteUser = false;
        $scope.createNewUser = true;
        $scope.$digest();
    };
});