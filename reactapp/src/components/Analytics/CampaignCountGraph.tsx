import { Chart } from "react-google-charts";
import {
    DeviceAPI,
} from "../../apis/InventoryApi";
import { DeviceCount } from "../../models/DeviceCount";
import { useEffect, useState } from "react";
import { Button } from "@mui/material";
import { download, generateCsv, mkConfig } from "export-to-csv";

export const options = {
    title: "Total Device Count (By Campaign)",
    chartArea: { width: "50%" },
    hAxis: {
        title: "Count",
        minValue: 0,
    },
    vAxis: {
        title: "Device",
    },
    animation: {
        startup: true,   /* Need to add this for animations */
        duration: 1000,
        easing: 'out'
    },
};
interface CampaignCountGraphProps {
    id: string;
}
export default function CampaignCountGraph({ id }: CampaignCountGraphProps) {
    const [data, setData] = useState<DeviceCount[]>([]);
    const [dataLoaded, setDataLoaded] = useState(false);
    const [isRefetching, setIsRefetching] = useState(true);
    const dataArray = [];
    dataArray.push(["Device", "Total Count"]);

    useEffect(() => {
        const fetchData = async () => {
            if (!isRefetching) return;
            try {
                setDataLoaded(false);
                const data = await crudAction("Read");
                setData(data as DeviceCount[]);
                setDataLoaded(true);
            } catch (error) {
                console.log(error);
                return;
            }
            setIsRefetching(false);
        };
        fetchData();
    }, [isRefetching]);

    const crudAction = async (action: string) => {
        switch (action) {
            case "Read":
                return await DeviceAPI.getDeviceByCampaign(id);
            default:
                break;
        }
    };

    const csvConfig = mkConfig({
        filename: "campaign_device_count_analytics",
        fieldSeparator: ",",
        decimalSeparator: ".",
        useKeysAsHeaders: true,
    });

    const handleExportData = (exportData: DeviceCount[]) => {
        const csv = generateCsv(csvConfig)(exportData as any);
        download(csvConfig)(csv);
    };

    data.map((device) =>
        dataArray.push([device.type + " " + device.size + " " + device.category, device.count])
    );

    return (
        <>
            {dataLoaded &&               
                <Chart
                    chartType="BarChart"
                    width="100%"
                    height="400px"
                    data={dataArray}
                    options={options}
                />
            }
            {dataLoaded && 
                <Button variant="contained" onClick={() => handleExportData(data)}>Export Data</Button>
            }
        </>
    );
}
