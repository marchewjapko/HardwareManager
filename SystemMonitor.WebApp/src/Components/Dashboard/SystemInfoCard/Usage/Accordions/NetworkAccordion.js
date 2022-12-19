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
    TableRow
} from "@mui/material";
import ExpandMoreIcon from "@mui/icons-material/ExpandMore";
import CircularProgressWithLabel from "../../../../Shared/CircularProgressWithLabel";
import {useState} from "react";
import "../Usage.css"

export default function NetworkAccordion({networkUsage, bandwidths, id}) {
    const [isOpen, setIsOpen] = useState(JSON.parse(localStorage.getItem('is-open-network-usage' + id)) || false)
    const handleAccordionChange = () => {
        setIsOpen(!isOpen)
        if (JSON.parse(localStorage.getItem('is-open-network-usage' + id))) {
            localStorage.setItem('is-open-network-usage' + id, 'false')
        } else {
            localStorage.setItem('is-open-network-usage' + id, 'true')
        }
    }
    return (
        <Accordion expanded={isOpen} onChange={handleAccordionChange}>
            <AccordionSummary expandIcon={<ExpandMoreIcon/>}>
                <div className={"system-info-row-title"}>
                    <div>
                        Network
                    </div>
                </div>
            </AccordionSummary>
            <AccordionDetails>
                <div className={"system-info-accordion-details"}>
                    {networkUsage.map((x) => (
                        <Stack direction={"column"} key={x.adapterName}>
                            <div className={"network-usage-title-container"}>
                                <div>
                                    {x.adapterName}
                                </div>
                                <div>
                                    <CircularProgressWithLabel
                                        value={(x.bytesReceived + x.bytesSent) / bandwidths.filter(a => a.adapterName === x.adapterName)[0].bandwidth/8 * 100}/>
                                </div>
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
                                        >
                                            <TableCell component="th" scope="row">
                                                Bytes received
                                            </TableCell>
                                            <TableCell align="right">
                                                {Math.round(x.bytesReceived / 1000 * 10) / 10} KB/s
                                            </TableCell>
                                        </TableRow>
                                        <TableRow
                                            key={1}
                                        >
                                            <TableCell component="th" scope="row">
                                                Bytes sent
                                            </TableCell>
                                            <TableCell align="right">
                                                {Math.round(x.bytesSent / 1000 * 10) / 10} KB/s
                                            </TableCell>
                                        </TableRow>
                                        <TableRow
                                            key={2}
                                        >
                                            <TableCell component="th" scope="row">
                                                Bandwidth
                                            </TableCell>
                                            <TableCell align="right">
                                                {Math.round(bandwidths.filter(a => a.adapterName === x.adapterName)[0].bandwidth / 1000000)} Mb/s
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