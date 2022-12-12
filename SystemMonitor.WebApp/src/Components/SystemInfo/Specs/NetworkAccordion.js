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
import {useState} from "react";

export default function NetworkAccordion({networkAdapters}) {
    const [isOpen, setIsOpen] = useState(JSON.parse(localStorage.getItem('is-open-network-specs')) || false)
    const handleAccordionChange = () => {
        setIsOpen(!isOpen)
        if(JSON.parse(localStorage.getItem('is-open-network-specs'))) {
            localStorage.setItem('is-open-network-specs', 'false')
        } else {
            localStorage.setItem('is-open-network-specs', 'true')
        }
    }
    return (
        <Accordion expanded={isOpen} onChange={handleAccordionChange} >
            <AccordionSummary expandIcon={<ExpandMoreIcon/>}>
                <div className={"system-info-row-title"}>
                    Network
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
                                        <TableCell align="right">Bandwidth</TableCell>
                                    </TableRow>
                                </TableHead>
                                <TableBody>
                                    {networkAdapters.map((x) => (
                                        <TableRow
                                            key={x.item1}
                                            sx={{'&:last-child td, &:last-child th': {border: 0}}}
                                        >
                                            <TableCell component="th" scope="row">
                                                {x.item1}
                                            </TableCell>
                                            <TableCell align="right">
                                                {x.item2 / 1000000} Mb
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