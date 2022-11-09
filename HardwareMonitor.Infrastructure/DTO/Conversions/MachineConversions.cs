using HardwareMonitor.Core.Domain;
using HardwareMonitor.Infrastructure.Commands;

namespace HardwareMonitor.Infrastructure.DTO.Conversions
{
    public static class MachineConversions
    {
        public static Machine ToDomain(this CreateMachine createMachine)
        {
            return new Machine()
            {
                MachineId = createMachine.MachineId,
                MachineName = createMachine.MachineName,
                IsAuthorised = createMachine.IsAuthorised,
            };
        }
        public static MachineDTO ToDTO(this Machine machine)
        {
            return new MachineDTO()
            {
                MachineId = machine.MachineId,
                MachineName = machine.MachineName,
                IsAuthorised = machine.IsAuthorised,
            };
        }
        public static Machine ToDomain(this UpdateMachine updateMachine)
        {
            return new Machine()
            {
                MachineName = updateMachine.MachineName,
                IsAuthorised = updateMachine.IsAuthorised,
            };
        }
    }
}
