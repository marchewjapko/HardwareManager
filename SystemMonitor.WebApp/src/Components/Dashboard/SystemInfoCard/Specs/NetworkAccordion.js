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

export default function NetworkAccordion({networkSpecs, id}) {
    const [isOpen, setIsOpen] = useState(JSON.parse(localStorage.getItem('is-open-network-specs' + id)) || false)
    const handleAccordionChange = () => {
        setIsOpen(!isOpen)
        if(JSON.parse(localStorage.getItem('is-open-network-specs' + id))) {
            localStorage.setItem('is-open-network-specs' + id, 'false')
        } else {
            localStorage.setItem('is-open-network-specs' + id, 'true')
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
                                    {networkSpecs.map((x) => (
                                        <TableRow
                                            key={x.adapterName}
                                            sx={{'&:last-child td, &:last-child th': {border: 0}}}
                                        >
                                            <TableCell component="th" scope="row">
                                                {x.adapterName}
                                            </TableCell>
                                            <TableCell align="right">
                                                {Math.round(x.bandwidth / 1000000)} Mb/s
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