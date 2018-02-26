(function () {
    angular
        .module('app.rialto', [])
        .controller('rialtoController', rialtoController);

    rialtoController.$inject = ['$scope', '$rootScope', '$http'];

    function rialtoController($scope, $rootScope, $http) {
        var vm = this;
        activate();

        function activate() {
            vm.formatInvalidInputErrorMsg = false;
            vm.ballancetoaction = 0;
            vm.chartSteps = 100;
            vm.chatMessages = [];
            vm.chartArray = [];

            //vm.scrollpane = {};
            //vm.scrollpaneInit = false;

            vm.msgAdded = function () {
                $(".chat-display").scrollTop(1000);
            }

            var renewactualdata = function(redrawChart) {
                $http({ url: '/Rialto/GetActualData', method: 'POST' }).then(function (result) {
                    if (result.data.IsSuccess) {
                        vm.ballance = result.data.RialtoData.UserState.Ballance.toFixed(2);
                        vm.equity = result.data.RialtoData.UserState.Equity.toFixed(2);
                        vm.aveRate = result.data.RialtoData.AveRate;
                        vm.champion = result.data.Champion;
                        vm.benefitPersent = result.data.RialtoData.BenefitPersent;
                        vm.assetlist = result.data.RialtoData.Assets;
                        vm.chatMessages = result.data.ChatMessage;

                        if (redrawChart) vm.buildChart(result.data.ChartData);
                        $(".chat-display").scrollTop(1000);
                    }
                });
            }
            renewactualdata(true);

            vm.up = function () {
                if (vm.ballancetoaction == undefined) { vm.inputZeroErrorMsg = true; return; }
                var requestEnabled;
                var ballancetoaction = parseFloat(vm.ballancetoaction);
                var losses = parseFloat(vm.benefitPersent) * ballancetoaction / 100;
                if (vm.assetSign || vm.assetSign == null) {
                    requestEnabled = parseFloat(vm.ballance) >= ballancetoaction + losses;
                }
                else {
                    requestEnabled = parseFloat(vm.ballance) + parseFloat(vm.assets) >= ballancetoaction + losses;
                }
                if (requestEnabled) {
                    $http({ url: '/Rialto/Bid', method: 'POST', data: { data: vm.ballancetoaction } }).then(function(result) {
                        if (result.data.IsSuccess) {
                            vm.inputErrorMsg = false;
                            vm.assetlist = result.data.Assets;
                        } else {
                            vm.inputErrorMsg = true;
                        }
                    });
                }
                else { vm.inputErrorMsg = true; }
                vm.ballancetoaction = 0;
                vm.inputZeroErrorMsg = false;
            }

            vm.down = function () {
                if (vm.ballancetoaction == undefined) { vm.inputZeroErrorMsg = true; return; }
                var requestEnabled;
                var ballancetoaction = parseFloat(vm.ballancetoaction);
                var losses = parseFloat(vm.benefitPersent) * ballancetoaction / 100;
                if (vm.assetSign &&  vm.assetSign != null) {
                    requestEnabled = parseFloat(vm.ballance) + parseFloat(vm.assets) >= ballancetoaction + losses;
                }
                else {
                    requestEnabled = parseFloat(vm.ballance) >= ballancetoaction + losses;
                }
                if (requestEnabled) {
                    $http({ url: '/Rialto/Bid', method: 'POST', data: { data: (vm.ballancetoaction * -1) } }).then(function(result) {
                        if (result.data.IsSuccess) {
                            vm.inputErrorMsg = false;
                            vm.assetlist = result.data.Assets;
                        } else {
                            vm.inputErrorMsg = true;
                        }
                    });
                }
                else { vm.inputErrorMsg = true; }
                vm.ballancetoaction = 0;
                vm.inputZeroErrorMsg = false;
            }


            vm.close = function (i, assetid) {
                vm.assetlist.splice(i, 1);
                $http({ url: '/Rialto/CloseBid', method: 'POST', data: { bidId: assetid } }).then(function (result) {
                    if (result.data.IsSuccess) {
                        vm.assetlist.splice(i, 1);
                    }
                });
            }

            vm.ballancetoactionUp = function () {
                if (vm.ballancetoaction.toString().length === 1) vm.ballancetoaction = parseInt(vm.ballancetoaction) + 1;
                else if (vm.ballancetoaction.toString().length === 2) vm.ballancetoaction = parseInt(vm.ballancetoaction) + 10;
                else if (vm.ballancetoaction.toString().length === 3) vm.ballancetoaction = parseInt(vm.ballancetoaction) + 100;
                else if (vm.ballancetoaction.toString().length >= 4) vm.ballancetoaction = parseInt(vm.ballancetoaction) + 1000;
            }

            vm.ballancetoactionDown = function () {
                if (parseInt(vm.ballancetoaction) === 0) return;
                if (vm.ballancetoaction.toString().length === 1) vm.ballancetoaction = parseInt(vm.ballancetoaction) - 1;
                else if (vm.ballancetoaction.toString().length === 2) {
                    if (parseInt(vm.ballancetoaction) === 10) { vm.ballancetoaction = parseInt(vm.ballancetoaction) - 1; }
                    else { vm.ballancetoaction = parseInt(vm.ballancetoaction) - 10; }
                }
                else if (vm.ballancetoaction.toString().length === 3) {
                    if (parseInt(vm.ballancetoaction) === 100) { vm.ballancetoaction = parseInt(vm.ballancetoaction) - 10; }
                    else { vm.ballancetoaction = parseInt(vm.ballancetoaction) - 100; }
                }
                else if (vm.ballancetoaction.toString().length >= 4) {
                    if (parseInt(vm.ballancetoaction) === 1000) { vm.ballancetoaction = parseInt(vm.ballancetoaction) - 100; }
                    else { vm.ballancetoaction = parseInt(vm.ballancetoaction) - 1000; }
                }
            }
            
            Highcharts.theme = {
                colors: ['#2b908f', '#90ee7e', '#f45b5b', '#7798BF', '#aaeeee', '#ff0066', '#eeaaee', '#55BF3B', '#DF5353', '#7798BF', '#aaeeee'],
                chart: { backgroundColor: { linearGradient: { x1: 0, y1: 0, x2: 1, y2: 1 }, stops: [[0, '#323e60'], [1, '#323e60']] }, style: { fontFamily: '\'Unica One\', sans-serif' }, plotBorderColor: '#323e60' },
                title: { style: { color: '#E0E0E3',  textTransform: 'uppercase', fontSize: '20px' } },
                subtitle: { style: { color: '#E0E0E3', textTransform: 'uppercase' } },
                xAxis: { gridLineColor: '#707073', labels: { style: { color: '#E0E0E3' } }, lineColor: '#707073', minorGridLineColor: '#505053', tickColor: '#707073',title: { style: { color: '#A0A0A3' }} },
                yAxis: { gridLineColor: '#707073', labels: { style: { color: '#E0E0E3' } }, lineColor: '#707073', minorGridLineColor: '#505053', tickColor: '#707073', tickWidth: 1, title: { style: { color: '#A0A0A3' } } },
                tooltip: { backgroundColor: 'rgba(0, 0, 0, 0.85)', style: { color: '#F0F0F0' } },
                plotOptions: { series: { dataLabels: { color: '#B0B0B3' }, marker: { lineColor: '#333' } }, boxplot: { fillColor: '#505053' }, candlestick: { lineColor: 'white' }, errorbar: { color: 'white' } },
                legend: { itemStyle: { color: '#E0E0E3' }, itemHoverStyle: { color: '#FFF' }, itemHiddenStyle: { color: '#606063' } },
                credits: { style: { color: '#666' } },
                labels: { style: { color: '#707073' } },
                drilldown: { activeAxisLabelStyle: { color: '#F0F0F3' }, activeDataLabelStyle: { color: '#F0F0F3' } }, 
                navigation: { buttonOptions: { symbolStroke: '#DDDDDD', theme: { fill: '#505053' } } },
                // scroll charts
                rangeSelector: {
                    buttonTheme: { fill: '#505053', stroke: '#000000', style: { color: '#CCC' }, states: { hover: { fill: '#707073', stroke: '#000000', style: { color: 'white' } }, select: { fill: '#000003', stroke: '#000000', style: { color: 'white' } } } },
                    inputBoxBorderColor: '#505053', inputStyle: { backgroundColor: '#333', color: 'silver' }, labelStyle: { color: 'silver' }
                },
                navigator: {
                    handles: { backgroundColor: '#666', borderColor: '#AAA' },
                    outlineColor: '#CCC', maskFill: 'rgba(255,255,255,0.1)',
                    series: { color: '#7798BF', lineColor: '#A6C7ED' }, 
                    xAxis: { gridLineColor: '#505053' }
                },
                scrollbar: {
                    barBackgroundColor: '#808083',
                    barBorderColor: '#808083',
                    buttonArrowColor: '#CCC',
                    buttonBackgroundColor: '#606063',
                    buttonBorderColor: '#606063',
                    rifleColor: '#FFF',
                    trackBackgroundColor: '#404043',
                    trackBorderColor: '#404043'
                },

                // special colors for some of the
                legendBackgroundColor: 'rgba(0, 0, 0, 0.5)',
                background2: '#505053',
                dataLabelsColor: '#B0B0B3',
                textColor: '#C0C0C0',
                contrastTextColor: '#F0F0F3',
                maskColor: 'rgba(255,255,255,0.3)'
            };

            // Apply the theme
            Highcharts.setOptions(Highcharts.theme);


            vm.buildChart = function (data) {
                var odata = [];
                for (var j = 0; j < data.length; j++) {
                    odata.push([data[j].Key, data[j].Value]);
                }
                var chart = Highcharts;
                
                vm.rialtoChart = chart.stockChart('rialtochart', {
                    rangeSelector: { selected: 1 },
                    title: { text: 'AVE trade' },
                    series: [{
                        name: 'AVE trade',
                        data: odata,
                        type: 'areaspline',
                        threshold: null,
                        tooltip: { valueDecimals: 2 },
                        fillColor: { linearGradient: { x1: 0, y1: 0, x2: 0, y2: 1 },
                            stops: [
                                [0, chart.getOptions().colors[0]],
                                [1, chart.Color(chart.getOptions().colors[0]).setOpacity(0).get('rgba')]
                            ]
                        }
                    }]
                });
            }


            vm.sendMessage = function () {
                $http({ url: '/Rialto/ChatMessage', method: 'POST', data: { message: vm.chatmessage } })
                    .then(function () { vm.chatmessage = ''; },
                          function () { vm.chatMessages.push({ Message: "Connection timeout", SenderName: "System" }); });
            }


            // Signal-R
            $rootScope.$on('aveRateChanges', function (e, message) {
                var msg = JSON.parse(message.msg);
                renewactualdata(false);
                vm.rialtoChart.series[0].addPoint([new Date().getTime(), msg.AveRate], true, true);
            });


            $rootScope.$on('rialtoChatMessage', function (e, message) {
                var msg = JSON.parse(message.msg);
                vm.chatMessages.push(msg);
                $scope.$apply();
            });
        }
    }
})();
