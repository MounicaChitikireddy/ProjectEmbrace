import { useMemo, useState, useEffect } from "react";
import {
  MaterialReactTable,
  useMaterialReactTable,
  type MRT_ColumnDef,
  MRT_RowSelectionState,
  MRT_Row,
} from "material-react-table";
import { DeviceAPI, DeviceAPIResponse } from "../../../apis/InventoryApi";
import { Device } from "../../../models/Device";
import "./InventoryTable.scss";
import {
  Alert,
  AlertTitle,
  IconButton,
  Snackbar,
  Tooltip,
} from "@mui/material";
import { download, generateCsv, mkConfig } from "export-to-csv";
import { DeviceType } from "../../../models/DeviceType";
import { DeviceTypeAPI } from "../../../apis/DeviceTypeApi";
import { User } from "../../../models/User";
import UpgradeGradeDialog from "../../Dialogs/InventoryDialogs/UpdateGradeDialog";
import { Download } from "@mui/icons-material";

export default function InventoryTable() {
  const [data, setData] = useState<Device[]>([]);
  const [deviceTypes, setDeviceTypes] = useState<DeviceType[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const [isRefetching, setIsRefetching] = useState(true);
  const [rowSelection, setRowSelection] = useState<MRT_RowSelectionState>({});
  const [isSuccess, setIsSuccess] = useState(false);
  const [displayMessage, setDisplayMessage] = useState("");
  const [openSnackBar, setOpenSnackBar] = useState(false);

  const accountData: User = JSON.parse(
    localStorage.getItem("accountData") || "{}"
  );

  const findUniqueLocations = (devices: Device[]): any => {
    const locations = devices.map((device) => device.location);
    if (!locations) {
      return [];
    }
    return [...new Set(locations)];
  };

  useEffect(() => {
    const fetchData = async () => {
      if (!isRefetching) return;
      setIsLoading(true);
      try {
        const data = await readData();
        setData(data as Device[]);
        const deviceTypes = await DeviceTypeAPI.getAll();
        setDeviceTypes(deviceTypes as DeviceType[]);
        localStorage.setItem(
          "uniqueLocations",
          JSON.stringify(findUniqueLocations(data as Device[]))
        );
      } catch (error) {
        console.log(error);
        return;
      }
      setIsLoading(false);
      setIsRefetching(false);
      setRowSelection({});
    };
    fetchData();
  }, [isRefetching]);

  const readData = async (data?: any) => {
    return await DeviceAPI.getAll();
  };

  const csvConfig = mkConfig({
    filename: "selected_inventory",
    fieldSeparator: ",",
    decimalSeparator: ".",
    useKeysAsHeaders: true,
  });

  const handleExportSelectedRows = (rows: MRT_Row<Device>[]) => {
    const rowData = rows.map((row) => row.original);
    const csv = generateCsv(csvConfig)(rowData as any);
    download(csvConfig)(csv);
  };

  const handleCrudResponse = async (response: DeviceAPIResponse) => {
    if (response) {
      setIsSuccess(response.isSuccess);
      setDisplayMessage(response.displayMessage);
      setIsRefetching(response.isRefetching);
      setOpenSnackBar(true);
    }
  };

  const columns = useMemo<MRT_ColumnDef<Device>[]>(
    () => [
      {
        accessorKey: "category",
        header: "Category",
        size: 50,
        enableGrouping: true,
      },
      {
        accessorKey: "type",
        header: "Type",
        size: 50,
        enableGrouping: true,
      },
      {
        accessorKey: "size",
        header: "Size",
        size: 50,
        enableGrouping: false,
      },
      {
        accessorKey: "count",
        header: "Count",
        size: 50,
        enableGrouping: false,
      },
      {
        accessorKey: "grade",
        header: "Grade",
        size: 50,
        enableGrouping: false,
      },
      {
        accessorKey: "location",
        header: "Location",
        size: 50,
        enableGrouping: false,
      },
    ],
    []
  );

  const table = useMaterialReactTable({
    columns,
    data,
    initialState: {
      showColumnFilters: false,
      showGlobalFilter: true,
      density: accountData?.settings?.tableDensity,
      isFullScreen: accountData?.settings?.tableFullScreenByDefault,
    },
    state: {
      isLoading,
      rowSelection,
    },
    muiCircularProgressProps: {
      color: "secondary",
      thickness: 5,
      size: 55,
    },
    muiSkeletonProps: {
      animation: "pulse",
      height: 28,
    },
    muiTableHeadCellProps: {
      sx: {
        backgroundColor: "rgb(244, 246, 248)",
      },
    },
    muiTablePaperProps: {
      elevation: 0,
      sx: {
        backgroundColor: "rgb(255, 255, 255)",
        color: "rgb(33, 43, 54)",
        boxShadow:
          "rgba(145, 158, 171, 0.2) 0px 0px 2px 0px, rgba(145, 158, 171, 0.12) 0px 12px 24px -4px",
        borderRadius: "16px",
        zIndex: "0",
      },
    },
    muiSearchTextFieldProps: {
      size: "medium",
      variant: "outlined",
      label: "Search",
      fullWidth: true,
      //sx: {
      //    flex: 1,
      //},
    },
    muiPaginationProps: {
      color: "secondary",
      rowsPerPageOptions: [10, 20, 30],
      shape: "rounded",
      variant: "outlined",
    },
    enableColumnFilterModes: true,
    enableGrouping: true,
    enableColumnDragging: false,
    enableColumnPinning: true,
    enableFacetedValues: true,
    enableRowSelection: true,
    onRowSelectionChange: setRowSelection,
    positionGlobalFilter: "left",
    paginationDisplayMode: "pages",
    positionToolbarAlertBanner: "bottom",
    renderToolbarAlertBannerContent: renderToolbarAlertBannerContent(),
  });

  return (
    <>
      <div className="import-export-buttons">
        {accountData?.role === "Admin" && (
          <UpgradeGradeDialog
            selectedRows={table.getSelectedRowModel().rows}
            deviceTypes={deviceTypes}
            responseFn={handleCrudResponse}
            disabled={
              !table.getIsSomeRowsSelected() && !table.getIsAllRowsSelected()
            }
          ></UpgradeGradeDialog>
        )}
      </div>
      <MaterialReactTable table={table} />
      <Snackbar
        open={openSnackBar}
        autoHideDuration={5000}
        onClose={() => setOpenSnackBar(false)}
      >
        <Alert
          severity={isSuccess ? "success" : "error"}
          onClose={() => setOpenSnackBar(false)}
        >
          <AlertTitle>{isSuccess ? "Success" : "Error"}</AlertTitle>
          {displayMessage}
        </Alert>
      </Snackbar>
    </>
  );

  function renderToolbarAlertBannerContent() {
    return () => {
      const selectedRows = table.getSelectedRowModel().rows;
      const numSelectedRows = selectedRows.length;
      return (
        <div className="delete-selected-rows-container">
          <div className="selected-rows">{numSelectedRows} Rows Selected</div>
          {["Admin", "Reviewer"].includes(accountData?.role) && (
            <Tooltip title="Export Data">
              <IconButton
                onClick={() => handleExportSelectedRows(selectedRows)}
              >
                <Download />
              </IconButton>
            </Tooltip>
          )}
        </div>
      );
    };
  }
}
