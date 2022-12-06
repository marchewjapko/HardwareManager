import {
    Accordion,
    AccordionDetails,
    AccordionSummary,
    Stack,
    Table,
    TableBody,
    TableCell,
    TableContainer,
    TableHead,
    TableRow,
    Typography
} from "@mui/material";
import ExpandMoreIcon from "@mui/icons-material/ExpandMore";
import CircularProgressWithLabel from "../CircularProgressWithLabel";

export default function NetworkAccordion({bytesReceived, bytesSent, networkAdapters}) {
    return (
        <Accordion defaultExpanded>
            <AccordionSummary expandIcon={<ExpandMoreIcon/>}>
                <div className={"system-info-row-title"}>
                    <Typography>Network</Typography>
                </div>
            </AccordionSummary>
            <AccordionDetails>
                <div className={"system-info-accordion-details"}>
                    {bytesReceived.map((x) => (
                        <Stack direction={"column"} key={x.item1}>
                            <div>
                                {x.item1}
                            </div>
                            <div>
                                <CircularProgressWithLabel
                                    value={(x.item2 + bytesSent.filter(a => a.item1 === x.item1)[0].item2) / networkAdapters.filter(a => a.item1 === x.item1)[0].item2 * 100}/>
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
                                            key={0}
                                            sx={{'&:last-child td, &:last-child th': {border: 0}}}
                                        >
                                            <TableCell component="th" scope="row">
                                                Bytes received
                                            </TableCell>
                                            <TableCell align="right">
                                                {x.item2 / 1000000} Mb/s
                                            </TableCell>
                                        </TableRow>
                                        <TableRow
                                            key={1}
                                            sx={{'&:last-child td, &:last-child th': {border: 0}}}
                                        >
                                            <TableCell component="th" scope="row">
                                                Bytes sent
                                            </TableCell>
                                            <TableCell align="right">
                                                {bytesSent.filter(a => a.item1 === x.item1)[0].item2 / 1000000} Mb/s
                                            </TableCell>
                                        </TableRow>
                                        <TableRow
                                            key={2}
                                            sx={{'&:last-child td, &:last-child th': {border: 0}}}
                                        >
                                            <TableCell component="th" scope="row">
                                                Bandwidth
                                            </TableCell>
                                            <TableCell align="right">
                                                {networkAdapters.filter(a => a.item1 === x.item1)[0].item2 / 1000000} Mb/s
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
    );
}