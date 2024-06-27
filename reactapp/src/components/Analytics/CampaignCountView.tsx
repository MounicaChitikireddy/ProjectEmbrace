import { CampaignAPI } from "../../apis/CampaignApi";
import { Campaign } from "../../models/Campaign";
import { useEffect, useState } from "react";
import {
  Box,
  Checkbox,
  FormControl,
  FormControlLabel,
  InputLabel,
  MenuItem,
  Select,
  SelectChangeEvent,
} from "@mui/material";
import CampaignCountGraph from "./CampaignCountGraph";
import GroupedCampaignCountGraph from "./GroupedCampaignCountGraph";
export default function CampaignCountView() {
  const [campaignData, setCampaignData] = useState<Campaign[]>([]);
  const [group, setGroup] = useState(false);
  const [campaignId, setCampaignId] = useState("");
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
              <MenuItem key={index} value={camp.campaignId}>
                {camp.campaignName}
              </MenuItem>
            );
          })}
        </Select>
      </FormControl>
      {displayGraph && (
        <Box sx={{ height: "400px", width: "100%" }}>
          {displayGraph && (
            <FormControlLabel
              control={
                <Checkbox checked={group} onClick={() => setGroup(!group)} />
              }
              label="Group By Device Type"
            />
          )}
          {displayGraph && !group && (
            <CampaignCountGraph key={campaignId} id={campaignId} />
          )}
          {displayGraph && group && (
            <GroupedCampaignCountGraph key={campaignId} id={campaignId} />
          )}
        </Box>
      )}
    </>
  );
}
