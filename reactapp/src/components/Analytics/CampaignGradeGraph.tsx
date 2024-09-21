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
type CampaignGradeGraphProps = {
    id: string;
}
export default function CampaignGradeGraph({ id }: CampaignGradeGraphProps) {
    const [data, setData] = useState<GradeCount[]>([]);
    const [dataLoaded, setDataLoaded] = useState(false);
    const [isRefetching, setIsRefetching] = useState(true);
    const dataArray = [];
    dataArray.push(["Device Grade", "Total Count"]);

    useEffect(() => {
        const fetchData = async () => {
            if (!isRefetching) return;
            try {
                setDataLoaded(false);
                const data = await crudAction("Read");
                setData(data as GradeCount[]);
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
                return await DeviceAPI.getGradeCountByCampaign(id);
            default:
                break;
        }
    };

    const csvConfig = mkConfig({
        filename: "campaign_grade_analytics",
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
            {dataLoaded &&
                <Chart
                    chartType="PieChart"
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
