$(function () {
    var IndexViewModel = function (rubusd, rubeur, rubcny) {
        this.rubUsdText = ko.observable(rubusd);
        this.rubEurText = ko.observable(rubeur);
        this.rubCnyText = ko.observable(rubcny);
        this.currentCourse = ko.observable(rubusd);
        this.chartHub = $.connection.chartHub;
        this.lineChartData = {
            type: "line",
            data: {
                labels: [],
                datasets: [
                    {label: "First", data: [0], fill: false, borderColor: '#4dc9f6'},
                    {label: "Second", data: [0], fill: false, borderColor: '#f67019'},
                    {label: "Third", data: [0], fill: false, borderColor: '#f53794'},
                ]
            },
            options: {
                responsive: true,
                animation: false
            }
        };
        var canvas = document.getElementById('lineCanvas'),
            ctx = canvas.getContext('2d'),
            chart = new Chart(ctx, this.lineChartData),
            chartDatas = chart.data.datasets;

        this.chartHub.client.updateChart = function (data) {

            chartDatas[0].data.push(data.lineChartData[0]);
            chartDatas[1].data.push(data.lineChartData[1]);
            chartDatas[2].data.push(data.lineChartData[2]);
            if (chartDatas[0].data.length > 20) {
                chartDatas[0].data.splice(0, 1);
                chartDatas[1].data.splice(0, 1);
                chartDatas[2].data.splice(0, 1);
            }
            else
            {
                chart.data.labels.push(chartDatas[0].data.length);
            }
            chart.update();

        };


        this.chartHub.client.addMessage = function (time, message) {
            $('#log').append('<tr><td>' + time + '</td><td>' + message + '</td></tr>');
        };

        this.init = function () {

        };

        this.setRubUsd = function () {
            this.currentCourse(this.rubUsdText());
        };

        this.setRubEur = function () {
            this.currentCourse(this.rubEurText());
        };

        this.setRubCny = function () {
            this.currentCourse(this.rubCnyText());
        };
    };

    var vm = new IndexViewModel("RUB/USD", "RUB/EUR", "RUB/CNY");
    ko.applyBindings(vm);

    $.connection.hub.start(function () {
        vm.init();
    });
});
    