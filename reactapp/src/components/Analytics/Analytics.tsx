import { Component } from "react";
import Header from "../global/Header.js";
import DeviceCountView from "./DeviceCountView.js";
import GroupedDeviceCountGraph from "./GroupedDeviceCountGraph.js";
import GradeCountGraph from "./GradeCountGraph.js";
import LocationCountGraph from "./LocationCountGraph.js";
import CampaignCountView from "./CampaignCountView.js";
import GroupedCampaignCountView from "./GroupedCampaignCountView.js";
import CampaignGradeView from "./CampaignGradeView.js";
import Accordion from '@mui/material/Accordion';
import AccordionSummary from '@mui/material/AccordionSummary';
import { Typography } from "@mui/material";
import { ArrowDropDownIcon } from "@mui/x-date-pickers/icons";
import Paper from '@mui/material/Paper';
import "../../assets/scss/Analytics.scss";

export default class AnalyticPage extends Component {

    render() {
        return (
            <>
                <div className="page-container">
                    <Header title={"Analytic Dashboard"} />
                    <div className="content-container">
                        <Paper elevation={3}>
                            <Typography variant="h5">All Devices</Typography>
                            <Accordion>
                                <AccordionSummary
                                    expandIcon={<ArrowDropDownIcon />}
                                    aria-controls="device-count-content"
                                    id="device-count-header"
                                >
                                    <Typography>Device Count</Typography>
                                </AccordionSummary>
                                <DeviceCountView />
                            </Accordion>
                            <Accordion>
                                <AccordionSummary
                                    expandIcon={<ArrowDropDownIcon />}
                                    aria-controls="grade-count-content"
                                    id="grade-count-header"
                                >
                                    <Typography>Device Grades</Typography>
                                </AccordionSummary>
                                <GradeCountGraph />
                            </Accordion>
                            <Accordion>
                                <AccordionSummary
                                    expandIcon={<ArrowDropDownIcon />}
                                    aria-controls="device-by-location-content"
                                    id="device-by-location-header"
                                >
                                    <Typography>Devices By Location</Typography>
                                </AccordionSummary>
                                <LocationCountGraph />
                            </Accordion>
                        </Paper>
                        <br />
                        <Paper elevation={3}>
                            <Typography variant="h5">Devices Per Campaign</Typography>
                            <Accordion>
                                <AccordionSummary
                                    expandIcon={<ArrowDropDownIcon />}
                                    aria-controls="campaing-device-count-content"
                                    id="campaign-device-count-header"
                                >
                                    <Typography>Device Count By Campaign</Typography>
                                </AccordionSummary>
                                <CampaignCountView />
                            </Accordion>
                            <Accordion>
                                <AccordionSummary
                                    expandIcon={<ArrowDropDownIcon />}
                                    aria-controls="campaign-device-count-content"
                                    id="campaign-device-count-header"
                                >
                                    <Typography>Grade Count By Campaign</Typography>
                                </AccordionSummary>
                                <CampaignGradeView />
                            </Accordion>
                        </Paper>
                    </div>
                </div>
            </>
        );
    }
}
