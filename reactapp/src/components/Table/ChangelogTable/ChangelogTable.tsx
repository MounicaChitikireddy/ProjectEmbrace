import { useMemo, useState, useEffect } from "react";
import {
  MaterialReactTable,
  useMaterialReactTable,
  type MRT_ColumnDef,
} from "material-react-table";
import {
  MenuItem,
  Alert,
  AlertTitle,
  Snackbar,
  Chip,
  AlertColor,
} from "@mui/material";
import "./ChangelogTable.scss";
import { InventoryChange } from "../../../models/InventoryChange";
import { ChangelogAPI, ChangelogAPIResponse } from "../../../apis/ChangelogApi";
import dayjs from "dayjs";
import { useNavigate } from "react-router-dom";
import CreateRecieptDialog from "../../Dialogs/CreateRecieptDialog/CreateRecieptDialog";
import { Contact } from "../../../models/Contact";
import { ContactAPI } from "../../../apis/ContactApi";
import { User } from "../../../models/User";

export default function ChangelogTable() {
  const [data, setData] = useState<InventoryChange[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const [isRefetching, setIsRefetching] = useState(true);
  const [snackBarSeverity, setSnackBarSeverity] = useState("info");
  const [displayMessage, setDisplayMessage] = useState("");
  const [openSnackBar, setOpenSnackBar] = useState(false);
  const [contacts, setContacts] = useState<Contact[]>([]);

  const accountData: User = JSON.parse(
    localStorage.getItem("accountData") || "{}"
  );

  useEffect(() => {
    const fetchData = async () => {
      if (!isRefetching) return;
      setIsLoading(true);
      try {
        const data = await crudAction("Read");
        setData(data as InventoryChange[]);

        const contacts = await ContactAPI.getAll();
        setContacts(contacts as Contact[]);
      } catch (error) {
        console.log(error);
        return;
      }
      setIsLoading(false);
      setIsRefetching(false);
    };
    fetchData();
  }, [isRefetching]);

  const navigate = useNavigate();

  const crudAction = async (action: string, data?: any) => {
    setIsLoading(true);
    switch (action) {
      case "Approve":
        handleCrudResponse(
          await ChangelogAPI.changeApprovalStatus(data, "Approve")
        );
        break;
      case "Deny":
        handleCrudResponse(
          await ChangelogAPI.changeApprovalStatus(data, "Deny")
        );
        break;
      case "Read":
        return await ChangelogAPI.getAll();
      default:
        break;
    }
  };

  const handleCrudResponse = async (response: ChangelogAPIResponse) => {
    if (response) {
      const displayMessage = response.isSuccess
        ? response.displayMessage
        : "An error occurred. Please try again.";
      const severity = response?.isSuccess ? "success" : "error";
      handleSnackBarDisplay(displayMessage, severity);
      setIsRefetching(response.isRefetching);
    }
  };

  const handleSnackBarDisplay = (displayMessage: string, severity: string) => {
    setSnackBarSeverity(severity);
    setDisplayMessage(displayMessage);
    setOpenSnackBar(true);
  };

  const columns = useMemo<MRT_ColumnDef<InventoryChange>[]>(
    () => [
      {
        accessorKey: "id",
        header: "ID",
        maxSize: 50,
        enableGrouping: false,
      },
      {
        accessorKey: "action",
        header: "Type",
        maxSize: 150,
        enableGrouping: true,
        Cell: ({ cell }) => {
          const status = cell.getValue<string>();
          switch (status) {
            case "Addition":
              return <Chip label={status} variant="outlined" color="success" />;
            case "Removal":
              return <Chip label={status} variant="outlined" color="error" />;
          }
        },
      },
      {
        accessorKey: "createdBy",
        header: "Created By",
        maxSize: 150,
        enableGrouping: true,
      },
      {
        accessorKey: "createdOn",
        header: "Date Created",
        maxSize: 150,
        enableGrouping: false,
        accessorFn: (row) => dayjs(row.createdOn),
        Cell: ({ cell }) => dayjs(cell.getValue<Date>()).format("MM-DD-YYYY"),
      },
      {
        accessorKey: "approvedBy",
        header: "Approved By",
        maxSize: 150,
        enableGrouping: true,
        accessorFn: (row) => (row.approvedBy ? row.approvedBy : "N/A"),
      },
      {
        accessorKey: "approvedOn",
        header: "Date Approved",
        maxSize: 150,
        enableGrouping: true,
        accessorFn: (row) => {
          return row.approvedOn ? dayjs(row.approvedOn) : "";
        },
        Cell: ({ cell }) => {
          const date = cell.getValue<Date>();
          if (date) {
            return dayjs(cell.getValue<Date>()).format("MM-DD-YYYY");
          } else {
            return "N/A";
          }
        },
      },
      {
        accessorKey: "reviewStatus",
        header: "Status",
        maxSize: 150,
        enableGrouping: true,
        Cell: ({ cell }) => {
          const status = cell.getValue<string>();
          switch (status) {
            case "Pending":
              return <Chip label={status} variant="outlined" color="warning" />;
            case "Approved":
              return <Chip label={status} variant="outlined" color="success" />;
            case "Denied":
              return <Chip label={status} variant="outlined" color="error" />;
          }
        },
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
    },
    muiPaginationProps: {
      color: "secondary",
      rowsPerPageOptions: [10, 20, 30],
      shape: "rounded",
      variant: "outlined",
    },
    layoutMode: "grid",
    displayColumnDefOptions: {
      "mrt-row-actions": { size: 50 },
    },
    enableColumnFilterModes: true,
    enableGrouping: true,
    enableColumnDragging: false,
    enableColumnPinning: true,
    enableFacetedValues: true,
    enableRowActions: true,
    enableRowSelection: false,
    positionGlobalFilter: "left",
    paginationDisplayMode: "pages",
    positionToolbarAlertBanner: "bottom",
    positionActionsColumn: "last",
    renderRowActionMenuItems: renderRowActionMenuItems(),
  });

  function renderRowActionMenuItems() {
    return ({ row, table }: any) => [
      <MenuItem
        key="View"
        onClick={() => {
          localStorage.setItem("batchToView", JSON.stringify(row.original));
          switch (row.original.action) {
            case "Addition":
              navigate(`/inventory/add?Batch=${row?.original?.id}`, {
                replace: false,
              });
              break;
            case "Removal":
              navigate(`/inventory/remove?Batch=${row?.original?.id}`, {
                replace: false,
              });
              break;
          }
        }}
      >
        View
      </MenuItem>,
      <CreateRecieptDialog
        key="CreateRecieptDialog"
        row={row.original}
        contacts={contacts}
        responseFn={handleCrudResponse}
        snackBarFn={handleSnackBarDisplay}
      />,
      <MenuItem
        key="approve"
        disabled={
          row.original.reviewStatus === "Approved" ||
          row.original.reviewStatus === "Denied"
        }
        onClick={() => {
          crudAction("Approve", row.original.id);
        }}
      >
        Approve
      </MenuItem>,
      <MenuItem
        key="deny"
        disabled={
          row.original.reviewStatus === "Approved" ||
          row.original.reviewStatus === "Denied"
        }
        onClick={() => {
          crudAction("Deny", row.original.id);
        }}
      >
        Deny
      </MenuItem>,
    ];
  }

  return (
    <>
      <MaterialReactTable table={table} />
      <Snackbar
        open={openSnackBar}
        autoHideDuration={5000}
        onClose={() => setOpenSnackBar(false)}
      >
        <Alert
          severity={snackBarSeverity as AlertColor}
          onClose={() => setOpenSnackBar(false)}
        >
          <AlertTitle sx={{ textTransform: "Capitalize" }}>
            {snackBarSeverity}
          </AlertTitle>
          {displayMessage}
        </Alert>
      </Snackbar>
    </>
  );
}
