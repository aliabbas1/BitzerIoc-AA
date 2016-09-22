app.controller('createGatewayController', function ($scope, $location, BitzerIocAdminService) {


    /// ValidateGateway method validate gateway mac address, if gateway mac is already exist then it return true 
    /// and disabled field and save button
    /// parameter = gatewayMac
    /// return true or false.
    /// version 1.0 

    /*------------  Functions ------------------*/
    $scope.ValidateGateway = function () {
        var gatewayMac = $scope.txtGatewayMac;
        if (gatewayMac) {
            var gateway = BitzerIocAdminService.ValidateGateway(gatewayMac);
            gateway.then(function (result) {
                if (result.data == true) {
                    $('#txtGatewayName').attr('disabled', true);
                    $('#chkIsEnable').attr('disabled', true);
                    $scope.alreadyExistGateway = true;
                    $('#btnCreateGateway').attr('disabled', true);
                }
                else {
                    $('#txtGatewayName').attr('disabled', false);
                    $('#chkIsEnable').attr('disabled', false);
                    $scope.alreadyExistGateway = false;
                    if ($scope.txtGatewayMac && $scope.txtGatewayName)
                        $('#btnCreateGateway').attr('disabled', false);
                }
            });
        }
    }

    /// SaveGateway method is used for insertion of gateway
    /// parameter =gatewayName, gatewayMac,user Email address,BoundaryId,IsEnable
    /// after successfully insertion of gateway, redirect to gateways page. 
    /// version 1.0 
    /// ToDo: boundaryId=1

    $scope.SaveGateway = function () {
        
        var params = $scope.txtGatewayName + "/" + $scope.txtGatewayMac + "/" + $('#hiddenEmail').val() + "/" + 1 + "/" + $scope.chkIsEnable

        BitzerIocAdminService.SaveGateway(params).then(function (result) {
            $location.path("/admin/gateways");
        });
    }

    /*------------ / Functions ------------------*/

});