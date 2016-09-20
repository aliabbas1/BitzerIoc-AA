app.controller('gatewaysController', function ($scope, $filter, $location, BitzerIocAdminService) {

    BindGatewayGrid();
    
    /* bind Gateway Grid*/
    /*Angular-Ui-Grid version 3.1.1*/
    /*PR version 1.0*/
    $scope.gatewayGridOpts =
        {
        enableHorizontalScrollbar: 0,
        enableVerticalScrollbar: 0,
        columnDefs: [
            { field: 'gatewayName', displayName: 'Gateway Name' },
            { field: 'gatewayMAC', displayName: 'Gateway Id' },
            {
                field: 'connectionStatus', displayName: 'Status', cellTemplate:
                       '<div ng-if="row.entity.connectionStatus == true"><img src="/images/Connection.png"></div>' +
                       '<div ng-if="row.entity.connectionStatus == false"><img src="/images/ConnectionLost.svg"></div>'
            },
            {
                field: 'isEnable', displayName: 'Enabled', enableColumnMenu: false, cellTemplate:
                       '<div ng-if="row.entity.isEnable == true"><img src="/images/GreenDotIcon.svg"></div>' +
                       '<div ng-if="row.entity.isEnable == false"><img src="/images/GrayDotIcon.svg"></div>'
            },
            {
                field: 'Edit', displayName: 'Edit', enableColumnMenu: false, width:'8%', cellTemplate:
                    '<a class="glyphicon glyphicon-pencil edit-btn" ng-click=grid.appScope.EditGateway(row.entity.gatewayId)></a>'
            },
            {
                field: 'Delete', displayName: 'Delete', enableColumnMenu: false, width: '8%', cellTemplate:
                       '<a class="glyphicon glyphicon-remove edit-btn" data-target="#DeleteGatewayModal" ng-click="grid.appScope.DeleteGatewayModal(row.entity.gatewayId,row.entity.gatewayName)"></a>'
            }

        ]
    }
    /*  end binding of gateway ui-grid*/




    /*------------  Functions ------------------*/

    /* Search Gateways from ui-grid*/
    /* this function is temporary and will be remove when we enable filter in $scope.gatewayGridOpts. */
    /*PR version 1.0*/
    $scope.searchGateways = function () {
        $scope.gatewayGridOpts.data = $filter('filter')($scope.searchGateway, $scope.filterText, undefined);
    };


    /* Function Bind the Gateway Grid*/
    /*PR version 1.0*/
    function BindGatewayGrid() {
        var gateways = BitzerIocAdminService.GetGateways();
        gateways.then(function (result) {
            $scope.gatewayGridOpts.data = result.data;
            $scope.searchGateway = result.data;
        });
    }


    /* this function open delete gateway bootstrap popup*/
    /*PR version 1.0*/
    $scope.DeleteGatewayModal = function (gatewayId, gatewayName) {
        $('#modalTitle').html("Delete gateway?");
        $scope.hdnGatewayId = gatewayId;
        $scope.hdnGatewayName = gatewayName;
        $scope.DeviceExist = false;
        $('#DeleteGatewayModal').modal();
    }

    /* this function open update gateway page*/
    /*PR version 1.0*/
    $scope.EditGateway = function (gatewayId) {
        $location.path("/admin/updategateway/" + gatewayId);
    }

    /* this function delete gateway if no device associate with particular gateway*/
    /* retun true if no device found in particular gateway else return false*/
    /*PR version 1.0*/
    $scope.DeleteGateway = function () {
        var GatewayId = $('#hdnGatewayId').text();
        var Params = encodeURIComponent(GatewayId);
        $scope.DeviceExist = false;
        var gateways = BitzerIocAdminService.DeleteGateway(Params);
        gateways.then(function (result) {
            if (result.data == true) {
                $('#DeleteGatewayModal').modal('hide');
                BindGatewayGrid();
            }
            else {
                $scope.DeviceExist = true;
            }

        });
    }
    /*------------ / Functions ------------------*/


});