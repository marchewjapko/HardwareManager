import {Accordion, AccordionSummary, Typography} from "@mui/material";
import ExpandMoreIcon from "@mui/icons-material/ExpandMore";
import CircularProgressWithLabel from "../CircularProgressWithLabel";

export default function () {
    return (
        <div>
            <Accordion>
                <AccordionSummary expandIcon={<ExpandMoreIcon/>} disabled>
                    <div className={"system-info-row-title"}>
                        <Typography>CPU</Typography>
                    </div>
                </AccordionSummary>
            </Accordion>
            <Accordion>
                <AccordionSummary expandIcon={<ExpandMoreIcon/>} disabled>
                    <Typography>Disks</Typography>
                </AccordionSummary>
            </Accordion>
            <Accordion>
                <AccordionSummary expandIcon={<ExpandMoreIcon/>} disabled>
                    <div className={"system-info-row-title"}>
                        <Typography>Memory</Typography>
                    </div>
                </AccordionSummary>
            </Accordion>
            <Accordion>
                <AccordionSummary expandIcon={<ExpandMoreIcon/>} disabled>
                    <div className={"system-info-row-title"}>
                        <Typography>Network</Typography>
                    </div>
                </AccordionSummary>
            </Accordion>
        </div>
    );
}