export default function GetChartData(readings, metric, lineColor) {
    if (metric === 'cpu') {
        const result = [{
            type: "spline",
            name: "total",
            lineThickness: 3,
            lineColor: lineColor,
            connectNullData: true,
            dataPoints: []
        }];
        result[0].dataPoints = readings.map(x => ({
            x: new Date(x.timestamp),
            y: x.usageDTO.cpuTotalUsage
        }))

        const readingsGroupedByInstance = GroupByArray(readings.map((x) => x.usageDTO.cpuPerCoreUsage.map((a) => ({
            ...a,
            timestamp: x.timestamp
        }))).flat(), 'instance')

        for (const property in readingsGroupedByInstance) {
            result.push({
                type: "spline",
                name: readingsGroupedByInstance[property][0].instance,
                showInLegend: true,
                lineThickness: 3,
                connectNullData: true,
                dataPoints: readingsGroupedByInstance[property].map(a => ({
                    x: new Date(a.timestamp),
                    y: a.usage
                }))
            })
        }
        return result
    }
    if (metric === 'memory') {
        const result = [{
            type: "spline",
            lineThickness: 3,
            lineColor: lineColor,
            connectNullData: true,
            dataPoints: readings.map(x => ({
                x: new Date(x.timestamp),
                y: (x.systemSpecsDTO.totalMemory / 1024 - x.usageDTO.memoryUsage) / (x.systemSpecsDTO.totalMemory / 1024) * 100
            }))
        }];
        return result
    }
    if (metric === 'disks') {
        const result = []
        const readingsGroupedByInstance = GroupByArray(readings.map((x) => x.usageDTO.diskUsage.map((a) => ({
            ...a,
            timestamp: x.timestamp
        }))).flat(), 'diskName')

        for (const property in readingsGroupedByInstance) {
            result.push({
                type: "spline",
                name: readingsGroupedByInstance[property][0].diskName,
                showInLegend: true,
                lineThickness: 3,
                connectNullData: true,
                dataPoints: readingsGroupedByInstance[property].map(a => ({
                    x: new Date(a.timestamp),
                    y: a.usage > 100 ? 100 : a.usage
                }))
            })
        }
        return result
    }
    if (metric === 'network') {
        const result = []
        const readingsGroupedByInstance = GroupByArray(readings.map((x) => x.usageDTO.networkUsage.map((a) => ({
            ...a,
            timestamp: x.timestamp
        }))).flat(), 'adapterName')

        for (const property in readingsGroupedByInstance) {
            result.push({
                type: "spline",
                name: readingsGroupedByInstance[property][0].adapterName + " - sent",
                showInLegend: true,
                lineThickness: 3,
                connectNullData: true,
                dataPoints: readingsGroupedByInstance[property].map(a => ({
                    x: new Date(a.timestamp),
                    y: a.bytesSent / 1000
                }))
            })
            result.push({
                type: "spline",
                name: readingsGroupedByInstance[property][0].adapterName + " - received",
                showInLegend: true,
                lineThickness: 3,
                connectNullData: true,
                dataPoints: readingsGroupedByInstance[property].map(a => ({
                    x: new Date(a.timestamp),
                    y: a.bytesReceived / 1000
                }))
            })
            result.push({
                type: "spline",
                name: readingsGroupedByInstance[property][0].adapterName,
                showInLegend: true,
                lineThickness: 3,
                connectNullData: true,
                dataPoints: readingsGroupedByInstance[property].map(a => ({
                    x: new Date(a.timestamp),
                    y: Math.round((a.bytesReceived + a.bytesSent) / readings.filter((x) => 1 === 1)[0].systemSpecsDTO.networkSpecs.filter((x) => x.adapterName === a.adapterName)[0].bandwidth / 8 * 100 * 100) / 10
                }))
            })
        }
        return result
    }
}

function GroupByArray(array, key) {
    return array.reduce(function (rv, x) {
        (rv[x[key]] = rv[x[key]] || []).push(x);
        return rv;
    }, {});
}