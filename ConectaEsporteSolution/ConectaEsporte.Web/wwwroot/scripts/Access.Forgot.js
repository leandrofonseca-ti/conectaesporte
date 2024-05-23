PORTALAPP.controller('ForgotController', function ($scope) {
    $scope.clsErrorEmailAddress = '';
    $scope.objEmailAddress = '';
    $scope.validateForm = function () {
        var hasError = false;

        if ($scope.objEmailAddress == "" || $scope.objEmailAddress == undefined) {
            $scope.clsErrorEmailAddress = 'error-field';
            hasError = true;
        } else {
            $scope.clsErrorEmailAddress = '';
        }



        ///console.log("hasError: " + hasError +" = $scope.Email: [" + $scope.objEmailAddress + "] $scope.Email: [" + $scope.objPassword + "]");

        if (hasError) {

            JSBASE.SHOW_MESSAGE("Atenção", "Informe o e-mail para recuperação da senha", "error");
 
            return false;
        }
        else {
            return true;
        }
    }
});

