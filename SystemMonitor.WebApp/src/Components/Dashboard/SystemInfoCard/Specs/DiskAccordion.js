import {
    Accordion,
    AccordionDetails,
    AccordionSummary,
    Stack,
    Table, TableBody, TableCell,
    TableContainer,
    TableHead, TableRow,
} from "@mui/material";
import ExpandMoreIcon from "@mui/icons-material/ExpandMore";
import {useState} from "react";

export default function DiskAccordion({diskSpecs, id}) {
    const [isOpen, setIsOpen] = useState(JSON.parse(localStorage.getItem('is-open-disk-specs' + id)) || false)
    const handleAccordionChange = () => {
        setIsOpen(!isOpen)
        if(JSON.parse(localStorage.getItem('is-open-disk-specs' + id))) {
            localStorage.setItem('is-open-disk-specs' + id, 'false')
        } else {
            localStorage.setItem('is-open-disk-specs' + id, 'true')
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
                <div className={"system-info-accordion-details"}>
                    <Stack direction={"column"}>
                        <TableContainer>
                            <Table size="small">
                                <TableHead>
                                    <TableRow>
                                        <TableCell>Name</TableCell>
                                        <TableCell align="right">Size</TableCell>
                                    </TableRow>
                                </TableHead>
                                <TableBody>
                                    {diskSpecs.map((x) => (
                                        <TableRow
                                            key={x.diskName}
                                            sx={{'&:last-child td, &:last-child th': {border: 0}}}
                                        >
                                            <TableCell component="th" scope="row">
                                                {x.diskName}
                                            </TableCell>
                                            <TableCell align="right">
                                                {Math.round(x.diskSize / 1024 / 1024 / 1024)} GB
                                            </TableCell>
                                        </TableRow>
                                    ))}
                                </TableBody>
                            </Table>
                        </TableContainer>
                    </Stack>
                </div>
            </AccordionDetails>
        </Accordion>
    );
}