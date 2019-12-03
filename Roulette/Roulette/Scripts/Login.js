var app = angular.module("Roulette", []);

app.controller("HomeController", function ($scope, $http) {
    $scope.LoginFailed = false;
    $scope.userId = '';
    $scope.password = '';
    $scope.userIdNew = '';
    $scope.passwordNew = '';
    $scope.createNewUser = false;
    $scope.login = false;
    $scope.userCreated = false;
    $scope.userCreationFailed = false;
    Login = function () {
        var loginModel = { "Username": $scope.userId, "Password": $scope.password };
        $http.post('/api/Account/Login', loginModel).then(
            function successCallback(response) {
                if (response.status == 200) {
                    var authToken = response.data.AuthToken;
                    window.location.href = response.data.RedirectUrl;
                }
            },
            function failureCallback(response) {
                $scope.LoginFailed = true;
            });
    };
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
    LogOff = function () {
        var loginModel = { "Username": $scope.userIdNew, "Password": $scope.passwordNew };
        $http.post('/api/Account/LogOff', authToken).then(
            function successCallback(response) {
                if (response.status == 200)
                    window.location.href = response.data.RedirectUrl;
            },
            function failureCallback(response) {
            });
    }
});
