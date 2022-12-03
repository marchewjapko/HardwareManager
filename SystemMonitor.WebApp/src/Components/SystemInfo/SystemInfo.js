import {
    Accordion,
    AccordionDetails,
    AccordionSummary,
    Divider,
    Paper,
    Stack, Table, TableBody, TableCell,
    TableContainer, TableHead, TableRow,
    Typography
} from "@mui/material";
import ExpandMoreIcon from '@mui/icons-material/ExpandMore';
import "./SystemInfo.js.css"
import {SystemInfoMock} from "../../Mocks/SystemInfoMock";
import {useState} from "react";

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
                            <Typography>CPU</Typography>
                            {Math.round(systemInfo[0].systemReadingDTOs[0].usageDTO.cpuTotalUsage * 100) / 100} %
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
                                                    {Math.round(row.item2 * 100) / 100} %
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
                                                    {Math.round(row.item2 * 100) / 100} %
                                                </TableCell>
                                            </TableRow>
                                        ))}
                                    </TableBody>
                                </Table>
                            </TableContainer>
                        </AccordionDetails>
                    </Accordion>
                </div>
            </Stack>
        </Paper >
    );
}