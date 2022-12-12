import {
    Accordion,
    AccordionDetails,
    AccordionSummary,
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
import {useState} from "react";

export default function CpuAccordion ({cpuTotalUsage, cpuPerCoreUsage}) {
    const [isOpen, setIsOpen] = useState(JSON.parse(localStorage.getItem('is-open-cpu')) || false)
    const handleAccordionChange = () => {
        setIsOpen(!isOpen)
        if(JSON.parse(localStorage.getItem('is-open-cpu'))) {
            localStorage.setItem('is-open-cpu', 'false')
        } else {
            localStorage.setItem('is-open-cpu', 'true')
        }
    }
    return (
        <Accordion expanded={isOpen} onChange={handleAccordionChange}>
            <AccordionSummary expandIcon={<ExpandMoreIcon />}>
                <div className={"system-info-row-title"}>
                    CPU
                    <CircularProgressWithLabel value={cpuTotalUsage} />
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
                            {cpuPerCoreUsage.map((row) => (
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
    );
}