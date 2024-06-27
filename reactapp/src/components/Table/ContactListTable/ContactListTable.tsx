import { useMemo, useState, useEffect, ChangeEvent, lazy } from "react";
import {
  MaterialReactTable,
  useMaterialReactTable,
  type MRT_ColumnDef,
  MRT_Row,
  MRT_RowSelectionState,
} from "material-react-table";
import {
  MenuItem,
  Alert,
  AlertTitle,
  Snackbar,
  Button,
  IconButton,
} from "@mui/material";
import { Contact } from "../../../models/Contact";
import { ContactAPI, ContactAPIResponse } from "../../../apis/ContactApi";
import { Delete } from "@mui/icons-material";
import DownloadIcon from "@mui/icons-material/Download";
import "./ContactListTable.scss";
import { download, generateCsv, mkConfig } from "export-to-csv";
import CloudUploadIcon from "@mui/icons-material/CloudUpload";
import Papa from "papaparse";
import CreateUpdateContactDialog from "../../Dialogs/ContactDialogs/CreateUpdateContactDialog";
import { User } from "../../../models/User";

export default function ContactListTable() {
  const [data, setData] = useState<Contact[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const [isRefetching, setIsRefetching] = useState(true);
  const [isSuccess, setIsSuccess] = useState(false);
  const [displayMessage, setDisplayMessage] = useState("");
  const [openSnackBar, setOpenSnackBar] = useState(false);
  const [rowSelection, setRowSelection] = useState<MRT_RowSelectionState>({});

  const accountData: User = JSON.parse(
    localStorage.getItem("accountData") || "{}"
  );

  useEffect(() => {
    const fetchData = async () => {
      if (!isRefetching) return;
      setIsLoading(true);
      try {
        const data = await crudAction("Read");
        setData(data as Contact[]);
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

  const crudAction = async (action: string, data?: any) => {
    switch (action) {
      case "Create":
        handleCrudResponse(await ContactAPI.create(data));
        break;
      case "Create Multiple":
        handleCrudResponse(await ContactAPI.createMultiple(data));
        break;
      case "Edit":
        handleCrudResponse(await ContactAPI.update(data));
        break;
      case "Delete":
        handleCrudResponse(await ContactAPI.delete(data));
        break;
      case "Delete Multiple":
        handleCrudResponse(await ContactAPI.deleteMultiple(data));
        break;
      case "Read":
        return await ContactAPI.getAll();
      default:
        break;
    }
  };

  const handleCrudResponse = async (response: ContactAPIResponse) => {
    if (response) {
      setIsSuccess(response.isSuccess);
      setDisplayMessage(response.displayMessage);
      setIsRefetching(response.isRefetching);
      setOpenSnackBar(true);
    }
  };

  const openDeleteConfirmModal = (row: MRT_Row<Contact>) => {
    if (window.confirm("Are you sure you want to delete this contact?")) {
      crudAction("Delete", row.original.contactId);
    }
  };

  const deleteSelectedRows = () => {
    const idArray = table
      .getSelectedRowModel()
      .rows.map((row) => row.original.contactId);
    if (window.confirm("Are you sure you want to delete these contacts?")) {
      crudAction("Delete Multiple", idArray);
    }
  };

  const csvConfig = mkConfig({
    filename: "selected_contacts",
    fieldSeparator: ",",
    decimalSeparator: ".",
    useKeysAsHeaders: true,
  });

  const handleExportSelectedRows = (rows: MRT_Row<Contact>[]) => {
    const rowData = rows.map((row) => row.original);
    const csv = generateCsv(csvConfig)(rowData as any);
    download(csvConfig)(csv);
  };

  const setSelectedFile = (e: ChangeEvent<HTMLInputElement>) => {
    const file = e?.target?.files?.[0];
    if (file) {
      Papa.parse(file as any, {
        header: true,
        skipEmptyLines: true,
        complete: (results) => {
          crudAction("Create Multiple", results.data);
          e.target.value = "";
        },
      });
    }
  };

  const columns = useMemo<MRT_ColumnDef<Contact>[]>(
    () => [
      {
        accessorKey: "contactId",
        header: "ID",
        maxSize: 75,
        enableGrouping: false,
      },
      {
        accessorKey: "fullName",
        header: "Full Name",
        maxSize: 200,
        enableGrouping: false,
      },
      {
        accessorKey: "phoneNum",
        header: "Phone Number",
        maxSize: 200,
        enableGrouping: false,
      },
      {
        accessorKey: "email",
        header: "Email",
        maxSize: 200,
        enableGrouping: false,
      },
      {
        accessorKey: "role",
        header: "Role",
        maxSize: 200,
        enableGrouping: true,
      },
      {
        accessorKey: "organization",
        header: "Organization",
        maxSize: 200,
        enableGrouping: true,
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
      sorting: [{ id: "contactId", desc: false }],
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
    enableRowActions: true,
    enableRowSelection: true,
    positionGlobalFilter: "left",
    paginationDisplayMode: "pages",
    positionToolbarAlertBanner: "bottom",
    positionActionsColumn: "last",
    layoutMode: "grid",
    displayColumnDefOptions: {
      "mrt-row-actions": { size: 50 },
      "mrt-row-select": { size: 50 },
    },
    renderToolbarAlertBannerContent: renderToolbarAlertBannerContent(),
    renderRowActionMenuItems: renderRowActionMenuItems(),
    renderEditRowDialogContent: renderEditRowDialogContent(),
    renderCreateRowDialogContent: renderCreateRowDialogContent(),
    onRowSelectionChange: setRowSelection,
  });

  function renderToolbarAlertBannerContent() {
    return () => {
      const numSelectedRows = table.getSelectedRowModel().rows.length;
      return (
        <div className="delete-selected-rows-container">
          <div className="selected-rows">{numSelectedRows} Selected</div>
          <IconButton onClick={() => deleteSelectedRows()}>
            <Delete />
          </IconButton>
        </div>
      );
    };
  }

  function renderCreateRowDialogContent() {
    return ({ table }: any) => {
      return (
        <CreateUpdateContactDialog
          row={null}
          table={table}
          action={"Create"}
          actionFn={crudAction}
        />
      );
    };
  }

  function renderEditRowDialogContent() {
    return ({ row, table }: any) => {
      return (
        <CreateUpdateContactDialog
          row={row}
          table={table}
          action={"Edit"}
          actionFn={crudAction}
        />
      );
    };
  }

  function renderRowActionMenuItems() {
    return ({ row, table }: any) => [
      <MenuItem key="edit" onClick={() => table.setEditingRow(row)}>
        Edit
      </MenuItem>,
      <MenuItem key="delete" onClick={() => openDeleteConfirmModal(row)}>
        Delete
      </MenuItem>,
    ];
  }

  return (
    <>
      <div className="create-button">
        <Button
          className="crud-btn"
          onClick={() => table.setCreatingRow(true)}
          variant="contained"
        >
          Create Contact
        </Button>
      </div>
      <div className="import-export-buttons">
        <Button
          component="label"
          variant="contained"
          startIcon={<CloudUploadIcon />}
        >
          Import Contacts
          <input
            accept="csv/*"
            type="file"
            id="import-file-upload"
            style={{ display: "none" }}
            onChange={(e) => setSelectedFile(e)}
          />
        </Button>
        <Button
          variant="contained"
          disabled={
            !table.getIsSomeRowsSelected() && !table.getIsAllRowsSelected()
          }
          //only export selected rows
          onClick={() =>
            handleExportSelectedRows(table.getSelectedRowModel().rows)
          }
          startIcon={<DownloadIcon />}
        >
          Export Selected Rows
        </Button>
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
}
