import { Chart } from "react-google-charts";
import {
    DeviceAPI,
} from "../../apis/InventoryApi";
import { LocationCount } from "../../models/LocationCount";
import { useEffect, useState } from "react";
import { Button } from "@mui/material";
import { download, generateCsv, mkConfig } from "export-to-csv";

export const options = {
    title: "Device Count By Location",
};
export default function GradeCountGraph() {
    const [data, setData] = useState<LocationCount[]>([]);
    const [isRefetching, setIsRefetching] = useState(true);
    const dataArray = [];
    dataArray.push(["Device Location", "Total Count"]);

    useEffect(() => {
        const fetchData = async () => {
            if (!isRefetching) return;
            try {
                const data = await crudAction("Read");
                setData(data as LocationCount[]);
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
                return await DeviceAPI.getLocationCount();
            default:
                break;
        }
    };

    const csvConfig = mkConfig({
        filename: "location_analytics",
        fieldSeparator: ",",
        decimalSeparator: ".",
        useKeysAsHeaders: true,
    });

    const handleExportData = (exportData: LocationCount[]) => {
        const csv = generateCsv(csvConfig)(exportData as any);
        download(csvConfig)(csv);
    };

    data.map((device) =>
        dataArray.push([device.location, device.count])
    );

    return (
        <>
            <Chart
                chartType="ColumnChart"
                width="100%"
                height="400px"
                data={dataArray}
                options={options}
            />
            <Button variant="contained" onClick={() => handleExportData(data)}>Export Data</Button>
        </>
    );
}
