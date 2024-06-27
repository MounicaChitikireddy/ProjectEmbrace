import { Box, Checkbox, FormControlLabel } from "@mui/material";
import DeviceCountGraph from "./DeviceCountGraph.js";
import GroupedDeviceCountGraph from "./GroupedDeviceCountGraph.js";
import { useState } from "react";
export default function DeviceCountView() {
  const [group, setGroup] = useState(false);

  return (
    <Box sx={{ height: "400px", width: "100%" }}>
      <FormControlLabel
        control={<Checkbox checked={group} onClick={() => setGroup(!group)} />}
        label="Group By Device Type"
      />
      {!group && <DeviceCountGraph />}
      {group && <GroupedDeviceCountGraph />}
    </Box>
  );
}
