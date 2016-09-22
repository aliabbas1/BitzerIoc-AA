app.controller('updateGatewayController', function ($scope, $routeParams, $location, BitzerIocAdminService) {

    var gatewayId = $routeParams.gatewayId;
    $scope.hiddenGatewayId = gatewayId;
    GetGateway(gatewayId);

    /*------------  Functions ------------------*/
    /* Function get the Gateway on the basis of gatewayId*/
    /* version 1.0*/
    /*ToDo: remove hardcoded boundaryid=1 */

    function GetGateway(gatewayId) {
        var params = gatewayId + "/" + 1;
        var gateways = BitzerIocAdminService.GetGatewayById(params);
        gateways.then(function (result) {
            $scope.txtGatewayName = result.data.gatewayName;
            $scope.txtGatewayMac = result.data.gatewayMAC;
            $scope.chkIsEnable = result.data.isEnable;
        });
    }
    /*------------ / Functions ------------------*/


    /*------------  Functions ------------------*/
    /* Function UpdateGateway the Gateway on the basis of gatewayId*/
    /* version 1.0*/
    /*ToDo: remove hardcoded boundaryid=1 */
    $scope.UpdateGateway = function () {
        var GatewayId = $('#hdnGatewayId').text();
        var UpdatedBy = $('#hdnUpdatedBy').val();
        var GatewayName = $scope.txtGatewayName;
        var GatewayStatus = $scope.chkIsEnable;

        var Params = GatewayId + "/" + encodeURIComponent(GatewayName) + "/" + GatewayStatus + "/" + UpdatedBy;
        var response = BitzerIocAdminService.UpdateGateway(Params).then(function (result) {
            if (result.data == true)
                $location.path("/admin/gateways");
        });
    }
    /*------------ / Functions ------------------*/
});