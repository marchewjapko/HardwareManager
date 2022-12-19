import moment from "moment";

export default function GetGraphData(newReading, lastReading, ref) {
    let data = []
    if(lastReading && lastReading.x) {
        let timestamp2 = moment(lastReading.x).utc()
        let timestamp1 = moment(newReading[0].timestamp).utc()
        AddMissingReading(timestamp1, timestamp2, data)
    }
    data.push({
        x: new Date(newReading[0].timestamp),
        y: {
            totalCPU: newReading[0].usageDTO.cpuTotalUsage,
            memory: (newReading[0].systemSpecsDTO.totalMemory / 1024 - newReading[0].usageDTO.memoryUsage) / (newReading[0].systemSpecsDTO.totalMemory / 1024) * 100
        }
    })

    for (let i = 1; i < newReading.length; i++) {
        let timestamp1 = moment(newReading[i].timestamp).utc()
        let timestamp2 = moment(newReading[i - 1].timestamp).utc()
        AddMissingReading(timestamp1, timestamp2, data)
        data.push({
            x: new Date(newReading[i].timestamp),
            y: {
                totalCPU: newReading[i].usageDTO.cpuTotalUsage,
                memory: (newReading[i].systemSpecsDTO.totalMemory / 1024 - newReading[i].usageDTO.memoryUsage) / (newReading[i].systemSpecsDTO.totalMemory / 1024) * 100
            }
        })
    }
    return data
}

function AddMissingReading(timestamp1, timestamp2, data) {
    const difference = timestamp1.diff(timestamp2)
    if (moment.duration(difference).asSeconds() > 30) {
        data.push({
            x: new Date(timestamp2.add(parseInt(moment.duration(difference).asSeconds()), 's').toISOString()),
            y: {
                totalCPU: null,
                memory: null
            }
        })
    }
}