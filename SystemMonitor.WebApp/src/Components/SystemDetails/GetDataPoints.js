export default function GetDataPoints(readings, metric) {
    switch (metric) {
        case 'cpu-total':
            return readings.map(x => ({x: new Date(x.timestamp), y: x.usageDTO.cpuTotalUsage}))
            break;
        case 'memory':
            return readings.map(x => ({x: new Date(x.timestamp), y: (x.systemSpecsDTO.totalMemory / 1024 - x.usageDTO.memoryUsage) / (x.systemSpecsDTO.totalMemory / 1024) * 100}))
            break;
    }
}