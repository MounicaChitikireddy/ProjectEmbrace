import { Chart } from "react-google-charts";
import {
    DeviceAPI,
} from "../../apis/InventoryApi";
import { GradeCount } from "../../models/GradeCount";
import { useEffect, useState } from "react";
import { Button } from "@mui/material";
import { download, generateCsv, mkConfig } from "export-to-csv";

export const options = {
    title: "Device Count By Grade",
    is3D: true,
};
export default function GradeCountGraph() {
    const [data, setData] = useState<GradeCount[]>([]);
    const [isRefetching, setIsRefetching] = useState(true);
    const dataArray = [];
    dataArray.push(["Device Grade", "Total Count"]);

    useEffect(() => {
        const fetchData = async () => {
            if (!isRefetching) return;
            try {
                const data = await crudAction("Read");
                setData(data as GradeCount[]);
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
                return await DeviceAPI.getGradeCount();
            default:
                break;
        }
    };

    const csvConfig = mkConfig({
        filename: "grade_analytics",
        fieldSeparator: ",",
        decimalSeparator: ".",
        useKeysAsHeaders: true,
    });

    const handleExportData = (exportData: GradeCount[]) => {
        const csv = generateCsv(csvConfig)(exportData as any);
        download(csvConfig)(csv);
    };

    data.map((device) =>
        dataArray.push([device.grade, device.count])
    );

    return (
        <>
            <Chart
                chartType="PieChart"
                width="100%"
                height="400px"
                data={dataArray}
                options={options}
            />
            <Button variant="contained" onClick={() => handleExportData(data)}>Export Data</Button>
        </>
    );
}
