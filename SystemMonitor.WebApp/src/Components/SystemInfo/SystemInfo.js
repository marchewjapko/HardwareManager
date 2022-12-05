import {
    Accordion,
    AccordionDetails,
    AccordionSummary,
    Divider,
    Paper,
    Stack, Table, TableBody, TableCell,
    TableContainer, TableHead, TableRow,
    Typography,
} from "@mui/material";
import ExpandMoreIcon from '@mui/icons-material/ExpandMore';
import "./SystemInfo.js.css"
import {SystemInfoMock} from "../../Mocks/SystemInfoMock";
import {useState} from "react";
import CircularProgressWithLabel from "./CircularProgressWithLabel";

export default function SystemInfo() {
    const [systemInfo, setSystemInfo] = useState(SystemInfoMock)
    return (
        <Paper square={false} elevation={20} className={"system-info-card"}>
            <Stack spacing={2}>
                <div className={"system-info-card-title"}>
                    {systemInfo[0].systemName}
                </div>
                <div>
                    <Accordion>
                        <AccordionSummary expandIcon={<ExpandMoreIcon />}>
                            <div className={"system-info-row-title"}>
                                <Typography>CPU</Typography>
                                <CircularProgressWithLabel value={systemInfo[0].systemReadingDTOs[0].usageDTO.cpuTotalUsage} />
                            </div>
                        </AccordionSummary>
                        <AccordionDetails>
                            <TableContainer>
                                <Table size="small">
                                    <TableHead>
                                        <TableRow>
                                            <TableCell>Core #</TableCell>
                                            <TableCell align="right">Usage</TableCell>
                                        </TableRow>
                                    </TableHead>
                                    <TableBody>
                                        {systemInfo[0].systemReadingDTOs[0].usageDTO.cpuPerCoreUsage.map((row) => (
                                            <TableRow
                                                key={row.item1}
                                                sx={{ '&:last-child td, &:last-child th': { border: 0 } }}
                                            >
                                                <TableCell component="th" scope="row">
                                                    {row.item1}
                                                </TableCell>
                                                <TableCell align="right">
                                                    <CircularProgressWithLabel value={row.item2} />
                                                </TableCell>
                                            </TableRow>
                                        ))}
                                    </TableBody>
                                </Table>
                            </TableContainer>
                        </AccordionDetails>
                    </Accordion>
                    <Accordion>
                        <AccordionSummary expandIcon={<ExpandMoreIcon />}>
                            <Typography>Disks</Typography>
                        </AccordionSummary>
                        <AccordionDetails>
                            <TableContainer>
                                <Table size="small">
                                    <TableHead>
                                        <TableRow>
                                            <TableCell>Disk</TableCell>
                                            <TableCell align="right">Usage</TableCell>
                                        </TableRow>
                                    </TableHead>
                                    <TableBody>
                                        {systemInfo[0].systemReadingDTOs[0].usageDTO.diskUsage.map((row) => (
                                            <TableRow
                                                key={row.item1}
                                                sx={{ '&:last-child td, &:last-child th': { border: 0 } }}
                                            >
                                                <TableCell component="th" scope="row">
                                                    {row.item1}
                                                </TableCell>
                                                <TableCell align="right">
                                                    <CircularProgressWithLabel value={row.item2} />
                                                </TableCell>
                                            </TableRow>
                                        ))}
                                    </TableBody>
                                </Table>
                            </TableContainer>
                        </AccordionDetails>
                    </Accordion>
                    <Accordion>
                        <AccordionSummary expandIcon={<ExpandMoreIcon />}>
                            <div className={"system-info-row-title"}>
                                <Typography>Memory</Typography>
                                <CircularProgressWithLabel value={systemInfo[0].systemReadingDTOs[0].usageDTO.memoryUsage / systemInfo[0].systemReadingDTOs[0].systemSpecsDTO.totalMemory * 100} />
                            </div>
                        </AccordionSummary>
                        <AccordionDetails>
                            <div className={"system-info-accordion-details"}>
                                <div>
                                    Total memory: {Math.round(systemInfo[0].systemReadingDTOs[0].systemSpecsDTO.totalMemory / 1024 / 1024 * 10) / 10} GB
                                </div>
                                <div>
                                    Used memory: {Math.round(systemInfo[0].systemReadingDTOs[0].usageDTO.memoryUsage / 1024 / 1024 * 10) / 10} GB
                                </div>
                                <div>
                                    Available memory: {Math.round(((systemInfo[0].systemReadingDTOs[0].systemSpecsDTO.totalMemory - systemInfo[0].systemReadingDTOs[0].usageDTO.memoryUsage) / 1024 / 1024) * 10) / 10} GB
                                </div>
                            </div>
                        </AccordionDetails>
                    </Accordion>
                    <Accordion>
                        <AccordionSummary expandIcon={<ExpandMoreIcon />}>
                            <div className={"system-info-row-title"}>
                                <Typography>Network</Typography>
                            </div>
                        </AccordionSummary>
                        <AccordionDetails>
                            <div className={"system-info-accordion-details"}>
                                {systemInfo[0].systemReadingDTOs[0].usageDTO.bytesReceived.map((x) => (
                                    <Stack direction={"column"}>
                                        <div>
                                            {x.item1}
                                        </div>
                                        <div>
                                            <CircularProgressWithLabel value={(x.item2 + systemInfo[0].systemReadingDTOs[0].usageDTO.bytesSent.filter(a => a.item1 === x.item1)[0].item2) / systemInfo[0].systemReadingDTOs[0].systemSpecsDTO.networkAdapters.filter(a => a.item1 === x.item1)[0].item2 * 100} />
                                        </div>
                                        <TableContainer>
                                            <Table size="small">
                                                <TableHead>
                                                    <TableRow>
                                                        <TableCell></TableCell>
                                                        <TableCell align="right"></TableCell>
                                                    </TableRow>
                                                </TableHead>
                                                <TableBody>
                                                    <TableRow
                                                        key={x.item1}
                                                        sx={{ '&:last-child td, &:last-child th': { border: 0 } }}
                                                    >
                                                        <TableCell component="th" scope="row">
                                                            Bytes received
                                                        </TableCell>
                                                        <TableCell align="right">
                                                            {x.item2 / 1000000} Mb/s
                                                        </TableCell>
                                                    </TableRow>
                                                    <TableRow
                                                        key={x.item1}
                                                        sx={{ '&:last-child td, &:last-child th': { border: 0 } }}
                                                    >
                                                        <TableCell component="th" scope="row">
                                                            Bytes sent
                                                        </TableCell>
                                                        <TableCell align="right">
                                                            {systemInfo[0].systemReadingDTOs[0].usageDTO.bytesSent.filter(a => a.item1 === x.item1)[0].item2 / 1000000} Mb/s
                                                        </TableCell>
                                                    </TableRow>
                                                    <TableRow
                                                        key={x.item1}
                                                        sx={{ '&:last-child td, &:last-child th': { border: 0 } }}
                                                    >
                                                        <TableCell component="th" scope="row">
                                                            Bandwidth
                                                        </TableCell>
                                                        <TableCell align="right">
                                                            {systemInfo[0].systemReadingDTOs[0].systemSpecsDTO.networkAdapters.filter(a => a.item1 === x.item1)[0].item2 / 1000000} Mb/s
                                                        </TableCell>
                                                    </TableRow>
                                                </TableBody>
                                            </Table>
                                        </TableContainer>
                                    </Stack>
                                ))}
                            </div>
                        </AccordionDetails>
                    </Accordion>
                </div>
            </Stack>
        </Paper >
    );
}