import {Accordion, AccordionDetails, AccordionSummary, Typography} from "@mui/material";
import ExpandMoreIcon from "@mui/icons-material/ExpandMore";
import CircularProgressWithLabel from "../../../../Shared/CircularProgressWithLabel/CircularProgressWithLabel";
import {useState} from "react";

export default function MemoryAccordion({availabeMemory, totalMemory, id}) {
    const [isOpen, setIsOpen] = useState(JSON.parse(localStorage.getItem('is-open-memory-usage' + id)) || false)
    const handleAccordionChange = () => {
        setIsOpen(!isOpen)
        if(JSON.parse(localStorage.getItem('is-open-memory-usage' + id))) {
            localStorage.setItem('is-open-memory-usage' + id, 'false')
        } else {
            localStorage.setItem('is-open-memory-usage' + id, 'true')
        }
    }


    return (
        <Accordion expanded={isOpen} onChange={handleAccordionChange}>
            <AccordionSummary expandIcon={<ExpandMoreIcon/>}>
                <div className={"system-info-row-title"}>
                    <div>
                        Memory
                    </div>
                    <CircularProgressWithLabel value={(totalMemory / 1024 - availabeMemory) / (totalMemory / 1024)* 100}/>
                </div>
            </AccordionSummary>
            <AccordionDetails>
                <div className={"system-info-accordion-details"}>
                    <div>
                        Total memory: {Math.round(totalMemory / 1024 / 1024 * 10) / 10} GB
                    </div>
                    <div>
                        Used memory: {Math.round((totalMemory / 1024 / 1024 - availabeMemory / 1024) * 10) / 10} GB
                    </div>
                    <div>
                        Available memory: {Math.round((availabeMemory / 1024) * 10) / 10} GB
                    </div>
                </div>
            </AccordionDetails>
        </Accordion>
    );
}