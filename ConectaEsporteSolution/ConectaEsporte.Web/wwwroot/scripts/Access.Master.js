PORTALAPP.controller('MasterController', function ($scope, $http) {
    $scope.clsErrorEmailAddress = '';
    $scope.clsErrorPassword = '';
    $scope.objEmailAddress = '';
    $scope.objPassword = '';
    $scope.validateForm = function () {
        var hasError = false;

        if ($scope.objEmailAddress == "" || $scope.objEmailAddress == undefined) {
            $scope.clsErrorEmailAddress = 'error-field';
            hasError = true;
        } else {
            $scope.clsErrorEmailAddress = '';
        }

        if ($scope.objPassword == "") {
            $scope.clsErrorPassword = 'error-field';
            hasError = true;
        } else {
            $scope.clsErrorPassword = '';
        }

        ///console.log("hasError: " + hasError +" = $scope.Email: [" + $scope.objEmailAddress + "] $scope.Email: [" + $scope.objPassword + "]");

        if (hasError) {
            JSBASE.SHOW_MESSAGE("Atenção", "Preencha corretamente os campos abaixo!", "error");
            return false;
        }
        else {
            var json = [];
            json.push({ "name": "Name", "value": "" });
            json.push({ "name": "Phone", "value": "" });
            json.push({ "name": "Email", "value": $scope.objEmailAddress });
            json.push({ "name": "Password", "value": $scope.objPassword });
            json.push({ "name": "KeepLogin", "value": true });
            JSBASE.AJAX_POST("LoginMaster", "Access", "", json, true, function (resposta) {
                console.log(JSON.stringify(resposta));
                if (resposta.data) {
                    JSBASE.SHOW_MESSAGE("Atenção", "Acesso realizado com sucesso :)", "success");
                    window.open(resposta.redirect, "_parent");
                }
                else {
                    JSBASE.SHOW_MESSAGE("Atenção", "Dados incorretos, não foi possível realizar o login :(", "error");
                }

            });
            return true;
        }
    }
});

