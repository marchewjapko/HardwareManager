import {CircularProgress, Paper} from "@mui/material";

export default function SystemDetailsCardSkeleton() {
    return (
        <Paper square={false} elevation={20} className={"system-details-main-card system-details-skeleton-card-container"}>
            <div className={"skeleton-overlay"}/>
            <div className={"system-details-skeleton-spinner"}>
                <CircularProgress color="inherit" size={"2em"}/>
            </div>
        </Paper>
    );
}