import { Chart } from "react-google-charts";
import {
    DeviceAPI,
} from "../../apis/InventoryApi";
import { DeviceCount } from "../../models/DeviceCount";
import { useEffect, useState } from "react";
import { download, generateCsv, mkConfig } from "export-to-csv";
import { Button } from "@mui/material";

export const options = {
    title: "Total Device Count",
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
export default function GroupedDeviceCountGraph() {
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
                return await DeviceAPI.getGroupedDeviceCount();
            default:
                break;
        }
    };

    const csvConfig = mkConfig({
        filename: "grouped_device_count_analytics",
        fieldSeparator: ",",
        decimalSeparator: ".",
        useKeysAsHeaders: true,
    });

    const handleExportData = (exportData: DeviceCount[]) => {
        const csv = generateCsv(csvConfig)(exportData as any);
        download(csvConfig)(csv);
    };

    data.map((device) =>
        dataArray.push([device.category, device.count])
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
