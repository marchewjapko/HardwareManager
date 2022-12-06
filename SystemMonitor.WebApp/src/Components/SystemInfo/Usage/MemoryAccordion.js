import {Accordion, AccordionDetails, AccordionSummary, Typography} from "@mui/material";
import ExpandMoreIcon from "@mui/icons-material/ExpandMore";
import CircularProgressWithLabel from "../CircularProgressWithLabel";

export default function MemoryAccordion({memoryUsage, totalMemory}) {
    return (
        <Accordion>
            <AccordionSummary expandIcon={<ExpandMoreIcon/>}>
                <div className={"system-info-row-title"}>
                    <Typography>Memory</Typography>
                    <CircularProgressWithLabel value={memoryUsage / totalMemory * 100}/>
                </div>
            </AccordionSummary>
            <AccordionDetails>
                <div className={"system-info-accordion-details"}>
                    <div>
                        Total memory: {Math.round(totalMemory / 1024 / 1024 * 10) / 10} GB
                    </div>
                    <div>
                        Used memory: {Math.round(memoryUsage / 1024 / 1024 * 10) / 10} GB
                    </div>
                    <div>
                        Available memory: {Math.round(((totalMemory - memoryUsage) / 1024 / 1024) * 10) / 10} GB
                    </div>
                </div>
            </AccordionDetails>
        </Accordion>
    );
}