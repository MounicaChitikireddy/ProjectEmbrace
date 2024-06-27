import {
    CampaignAPI,
} from "../../apis/CampaignApi";
import { Campaign } from "../../models/Campaign";
import { useEffect, useState } from "react";
import { FormControl, InputLabel, MenuItem, Select, SelectChangeEvent } from "@mui/material";
import CampaignGradeGraph from "./CampaignGradeGraph";
export default function CampaignGradeView() {
    const [campaignData, setCampaignData] = useState<Campaign[]>([]);
    const [campaignId, setCampaignId] = useState('');
    const [isRefetching, setIsRefetching] = useState(true);
    const [displayGraph, setDisplayGraph] = useState(false);

    useEffect(() => {
        const fetchData = async () => {
            if (!isRefetching) return;
            try {
                const campaignData = await campaignCrudAction("Read");
                setCampaignData(campaignData as Campaign[]);
            } catch (error) {
                console.log(error);
                return;
            }
            setIsRefetching(false);
        };
        fetchData();
    }, [isRefetching]);

    const campaignCrudAction = async (action: string) => {
        switch (action) {
            case "Read":
                return await CampaignAPI.getAll();
            default:
                break;
        }
    };

    const handleChange = (event: SelectChangeEvent) => {
        event.preventDefault();
        setCampaignId(event.target.value);
        setDisplayGraph(true);
    };

    return (
        <>
            <FormControl fullWidth>
                <InputLabel id="campaign-select-label">Select Campaign</InputLabel>
                <Select
                    labelId="campaign-select-component"
                    id="demo-simple-select"
                    value={campaignId}
                    label="Campaign Name"
                    onChange={handleChange}
                >
                    {campaignData.map((camp, index) => {
                        return (
                            <MenuItem key={index} value={camp.campaignId}>{camp.campaignName}</MenuItem>
                        );
                    })}
                </Select>
            </FormControl>
            {displayGraph &&
                <CampaignGradeGraph key={campaignId} id={campaignId} />
            }
        </>
    );
}
