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
import CircularProgressWithLabel from "../../../../Shared/CircularProgressWithLabel";
import {useState} from "react";

export default function CpuAccordion ({cpuTotalUsage, cpuPerCoreUsage, id}) {
    const [isOpen, setIsOpen] = useState(JSON.parse(localStorage.getItem('is-open-cpu' + id)) || false)
    const handleAccordionChange = () => {
        setIsOpen(!isOpen)
        if(JSON.parse(localStorage.getItem('is-open-cpu' + id))) {
            localStorage.setItem('is-open-cpu' + id, 'false')
        } else {
            localStorage.setItem('is-open-cpu' + id, 'true')
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
                                    key={row.instance}
                                    sx={{ '&:last-child td, &:last-child th': { border: 0 } }}
                                >
                                    <TableCell component="th" scope="row">
                                        {row.instance}
                                    </TableCell>
                                    <TableCell align="right">
                                        <CircularProgressWithLabel value={row.usage} />
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