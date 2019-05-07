$(function () {
    /* CONST */
    const default_colors =
        ['#3366CC', '#DC3912', '#FF9900', '#109618',
            '#990099', '#3B3EAC', '#0099C6', '#DD4477',
            '#66AA00', '#B82E2E', '#316395', '#994499',
            '#22AA99', '#AAAA11', '#6633CC', '#E67300',
            '#8B0707', '#329262', '#5574A6', '#3B3EAC'];

    /* KNOCKOUT */
    let IndexViewModel = function () {
        this.currentCourse = ko.observable();

        this.logData = ko.observableArray();
        this.exchanges = ko.observableArray();
        this.providers = ko.observableArray();
        this.logCount = ko.observable();
        this.lineCount = ko.observable();
    };

    const vm = new IndexViewModel();

    vm.setLabel = function (text) {
        vm.currentCourse(text);
        chartHub.server.fetchExchangeData(text);
    };

    ko.applyBindings(vm);

    /* SIGNALR */
    const chartHub = $.connection.chartHub;

    $.connection.hub.start().done(function () {
        chartHub.server.initChartData();
    });

    let lineChartData = {
        type: "line",
        data: {
            labels: [],
            datasets: []
        },
        options: {
            responsive: true,
            animation: false
        }
    };

    let canvas = document.getElementById('lineCanvas'),
        ctx = canvas.getContext('2d'),
        chart = new Chart(ctx, lineChartData),
        chartSets = chart.data.datasets;

    chartHub.client.getInitData = function (data) {
        vm.exchanges(data.exchanges);
        vm.currentCourse(data.defaultCourse);
        vm.logCount(data.logCount);
        vm.lineCount(data.lineCount);
        vm.providers(data.providers);
        data.providers.forEach((value, index) => {
            chartSets.push({
                label: value,
                data: data.dataValuesInit[value],
                fill: false,
                borderColor: default_colors[index]
            });
        });

        data.shortTimesInit.forEach(value => {
            chart.data.labels.push(value);
        });

        vm.providers.unshift('#');

        data.timesInit.forEach((value, index) => {
            let tmp = [];
            tmp.push(value);

            Object.keys(data.dataValuesInit).forEach(function (key) {
                tmp.push(data.dataValuesInit[key][index]);

            });
            vm.logData.unshift({values: tmp});

        });

        chart.update();
    };

    chartHub.client.getExchangeData = function (data) {
        vm.logData.removeAll();
        chartSets.splice(0, chartSets.length);
        chart.data.labels = [];
        chart.update();
        vm.providers(data.providers);
        
        data.providers.forEach((value, index) => {
            chartSets.push({
                label: value,
                data: data.dataValuesInit[value],
                fill: false,
                borderColor: default_colors[index]
            });
        });

        data.shortTimesInit.forEach(value => {
            chart.data.labels.push(value);
        });

        vm.providers.unshift('#');

        data.timesInit.forEach((value, index) => {
            let tmp = [];
            tmp.push(value);

            Object.keys(data.dataValuesInit).forEach(function (key) {
                tmp.push(data.dataValuesInit[key][index]);

            });
            vm.logData.unshift({values: tmp});

        });
        
        chart.update();
    };

    chartHub.client.addMessage = function (time, message) {
        message.unshift(time);
        vm.logData.unshift({values: message});
        if (vm.logData().length > vm.logCount()) {
            vm.logData.pop();
        }

        chart.data.labels.push(message[0]);
        for (let i = 1; i < message.length; i++) {
            chartSets[i - 1].data.push(message[i]);
        }

        if (chartSets[0].data.length > vm.lineCount()) {
            chartSets.forEach(value => value.data.splice(0, 1));
            chart.data.labels.splice(0, 1);
        }

        chart.update();
    };
});
    