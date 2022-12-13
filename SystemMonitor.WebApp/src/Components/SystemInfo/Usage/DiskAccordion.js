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

export default function DiskAccordion({diskUsage}) {
    const [isOpen, setIsOpen] = useState(JSON.parse(localStorage.getItem('is-open-disk-usage')) || false)
    const handleAccordionChange = () => {
        setIsOpen(!isOpen)
        if(JSON.parse(localStorage.getItem('is-open-disk-usage'))) {
            localStorage.setItem('is-open-disk-usage', 'false')
        } else {
            localStorage.setItem('is-open-disk-usage', 'true')
        }
    }
    return (
        <Accordion expanded={isOpen} onChange={handleAccordionChange}>
            <AccordionSummary expandIcon={<ExpandMoreIcon/>}>
                <div className={"system-info-row-title"}>
                    Disks
                </div>
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
                            {diskUsage.map((row) => (
                                <TableRow
                                    key={row.diskName}
                                    sx={{'&:last-child td, &:last-child th': {border: 0}}}
                                >
                                    <TableCell component="th" scope="row">
                                        {row.diskName}
                                    </TableCell>
                                    <TableCell align="right">
                                        <CircularProgressWithLabel value={row.usage > 100 ? 100 : row.usage}/>
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