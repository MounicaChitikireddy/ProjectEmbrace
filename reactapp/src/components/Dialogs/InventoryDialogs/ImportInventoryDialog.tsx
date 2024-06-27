import Button from "@mui/material/Button";
import CloudUploadIcon from "@mui/icons-material/CloudUpload";
import { styled } from "@mui/material/styles";;
import "./ImportInventory.scss";
import { useState } from "react";
import { Dialog, DialogTitle, DialogContent, DialogContentText, DialogActions } from "@mui/material";
import { Device } from "../../../models/Device";
import Papa from "papaparse";

const VisuallyHiddenInput = styled("input")({
  clip: "rect(0 0 0 0)",
  clipPath: "inset(50%)",
  height: 1,
  overflow: "hidden",
  position: "absolute",
  bottom: 0,
  left: 0,
  whiteSpace: "nowrap",
  width: 1,
});


export default function ImportInventoryDialog({deviceTypes, saveFn}: any) {
  const [open, setOpen] = useState(false);
  const [file, setFile] = useState<File>();
  const [readingFile, setReadingFile] = useState(false);

  const handleClickOpen = () => {
    setOpen(true);
  };

  const handleClose = () => {
    setOpen(false);
  };

  const importFile = () => {
    if (file) {
      setReadingFile(true);
      Papa.parse(file as any, {
        header: true,
        skipEmptyLines: true,
        complete: async (results) => {
          const additions: Device[] = [];

          results.data.forEach((row: any) => {
            const deviceType = matchDeviceTypeByProps(
              row.category,
              row.type,
              row.size
            );
            const device: Device = {
              deviceTypeId: deviceType?.deviceTypeId,
              count: row.count,
              grade: row.grade
            } as Device;

            additions.push(device);
          });
          setReadingFile(false);
          saveFn(additions);
          handleClose();
        },
      });
    }
  };

  const matchDeviceTypeByProps = (
    category: string,
    type: string,
    size: string
  ) => {
    return deviceTypes?.find(
      (dt: any) => dt.category === category && dt.type === type && dt.size === size
    );
  };
 
  return (
    <>
      <Button
        component="label"
        variant="outlined"
        startIcon={<CloudUploadIcon />}
        onClick={handleClickOpen}
      >
        Import Items
      </Button>
      <Dialog
        open={open}
        onClose={handleClose}
      >
        <DialogTitle>Import Items</DialogTitle>
        <DialogContent>
          <DialogContentText>
            Import a .csv file to add items to the inventory with the following columns:
          </DialogContentText>
          <DialogContentText sx={{display: 'flex', justifyContent: 'center'}}>
            "category","type","size","grade","count"
          </DialogContentText>
          <div className="upload-container">
            {file && <p className="upload-file">{file?.name}</p>}
            <Button
              component="label"
              variant="contained"
              className="upload-button"
              startIcon={<CloudUploadIcon />}
            >
              {file ? "Change" : "Upload"} file
              <VisuallyHiddenInput
                accept="csv/*"
                id="import-inventory-file-upload"
                onChange={(e) => {
                  setFile(e?.target?.files?.[0] as File);
                  e.target.value = "";
                }}
                type="file"
              />
            </Button>
          </div>
        </DialogContent>
        <DialogActions>
          <Button onClick={handleClose}>Cancel</Button>
          <Button disabled={Boolean(!file) || readingFile} variant="contained" onClick={importFile} autoFocus>
            Import
          </Button>
        </DialogActions>
      </Dialog>
    </>
  );
}
