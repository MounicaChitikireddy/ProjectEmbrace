import Button from "@mui/material/Button";
import "./UpdateGradeDialog.scss";
import { useState } from "react";
import {
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
  TextField,
  MenuItem,
  useTheme,
} from "@mui/material";
import { Controller, useFieldArray, useForm } from "react-hook-form";
import { DeviceGrade } from "../../../enums/DeviceGrade.enum";
import ArrowForwardIcon from "@mui/icons-material/ArrowForward";
import useMediaQuery from "@mui/material/useMediaQuery";
import { DeviceGradeUpdate } from "../../../models/DeviceGradeUpdate";
import { DeviceAPI } from "../../../apis/InventoryApi";

export default function UpgradeGradeDialog({
  selectedRows,
  deviceTypes,
  disabled,
  responseFn,
}: any) {
  const theme = useTheme();
  const fullScreen = useMediaQuery(theme.breakpoints.down("md"));

  const [open, setOpen] = useState(false);

  const populateForm = () => {
    if (selectedRows.length) {
      const updates = selectedRows.map((row: any) => {
        return {
          deviceTypeId: row.original.typeID,
          currentCount: row.original.count,
          oldGrade: row.original.grade,
          newGrade: "",
          location: row.original.location,
          count: 0,
        } as DeviceGradeUpdate;
      });
      setValue("updates", updates);
    }
  };

  const handleClickOpen = () => {
    populateForm();
    setOpen(true);
  };

  const handleClose = () => {
    setOpen(false);
  };

  const getDeviceTypeDisplayValue = (deviceTypeId: number) => {
    if (deviceTypeId === 0) {
      return "";
    }
    const dt = deviceTypes?.find(
      (type: any) => type.deviceTypeId === deviceTypeId
    );
    return `${dt.category}, ${dt.type}, ${dt.size}`;
  };

  const {
    control,
    register,
    handleSubmit,
    setValue,
    formState: { errors },
  } = useForm({
    defaultValues: {
      updates: [
        {
          deviceTypeId: 0,
          currentCount: 0,
          oldGrade: "",
          newGrade: "",
          location: "",
          count: 0,
        } as DeviceGradeUpdate,
      ],
    },
  });

  const updatesFieldArray = useFieldArray({
    control,
    name: "updates",
  });

  const onSubmit = (data: any) => {
    data.updates.forEach((update: { currentCount: any }) => {
      delete update.currentCount;
    });
    DeviceAPI.updateGrade(data.updates).then((response) => {
      responseFn(response);
      handleClose();
    });
  };

  return (
    <>
      <Button
        component="label"
        variant="contained"
        onClick={handleClickOpen}
        disabled={disabled}
      >
        Update Selected Rows
      </Button>
      <Dialog
        fullScreen={fullScreen}
        fullWidth
        maxWidth={"lg"}
        open={open}
        onClose={handleClose}
        PaperProps={{
          component: "form",
          onSubmit: handleSubmit(onSubmit),
        }}
      >
        <DialogTitle>Update Grades</DialogTitle>
        <DialogContent>
          {updatesFieldArray.fields.map((field, index) => (
            <div className="item-row" key={field.id}>
              <div className="item-info-container">
                <div className="old-data">
                  {deviceTypes?.length && (
                    <div className="item-input">
                      <TextField
                        defaultValue={getDeviceTypeDisplayValue(
                          field.deviceTypeId
                        )}
                        disabled
                        required
                        label="Device Type"
                      />
                    </div>
                  )}
                  <div className="small-input-container">
                    <div className="small-item-input">
                      <TextField
                        defaultValue={field.oldGrade}
                        disabled
                        required
                        label="Old Grade"
                      />
                    </div>
                  </div>
                  <div className="small-input-container">
                    <div className="small-item-input">
                      <TextField
                        defaultValue={field.location}
                        disabled
                        required
                        label="Location"
                      />
                    </div>
                  </div>
                  <div className="small-input-container">
                    <div className="small-item-input">
                      <TextField
                        defaultValue={field.currentCount}
                        disabled
                        required
                        label="Quantity"
                      />
                    </div>
                  </div>
                </div>
                <div className="forward-icon">
                  <ArrowForwardIcon />
                </div>
                <div className="upgrade-inputs">
                  <div className="small-input-container">
                    <div className="small-item-input">
                      <Controller
                        control={control}
                        name={`updates.${index}.newGrade`}
                        rules={{
                          required: { value: true, message: "Invalid input" },
                        }}
                        render={({ field: { name, value, onChange } }) => (
                          <TextField
                            sx={{ width: "100%" }}
                            onChange={onChange}
                            select
                            required={true}
                            label="Grade"
                            value={value}
                            fullWidth
                          >
                            <MenuItem
                              key={`updates.${index}.Grade.${DeviceGrade.A}`}
                              value={DeviceGrade.A}
                            >
                              {DeviceGrade.A}
                            </MenuItem>
                            <MenuItem
                              key={`updates.${index}.Grade.${DeviceGrade.BFRN}`}
                              value={DeviceGrade.BFRN}
                            >
                              {DeviceGrade.BFRN}
                            </MenuItem>
                            <MenuItem
                              key={`updates.${index}.Grade.${DeviceGrade.C}`}
                              value={DeviceGrade.C}
                            >
                              {DeviceGrade.C}
                            </MenuItem>
                            <MenuItem
                              key={`updates.${index}.Grade.${DeviceGrade.UNPROCESSED}`}
                              value={DeviceGrade.UNPROCESSED}
                            >
                              {DeviceGrade.UNPROCESSED}
                            </MenuItem>
                            <MenuItem
                              key={`updates.${index}.Grade.${DeviceGrade.NA}`}
                              value={DeviceGrade.NA}
                            >
                              {DeviceGrade.NA}
                            </MenuItem>
                          </TextField>
                        )}
                      />
                    </div>
                  </div>
                  <div className="small-input-container">
                    <div className="small-item-input">
                      <TextField
                        required
                        fullWidth
                        label="Quantity"
                        error={
                          errors?.updates?.[index]?.count?.message
                            ? true
                            : false
                        }
                        key={field.id}
                        {...register(`updates.${index}.count` as const, {
                          valueAsNumber: true,
                          min: { value: 1, message: " " },
                        })}
                      />
                    </div>
                  </div>
                </div>
              </div>
            </div>
          ))}
        </DialogContent>
        <DialogActions>
          <Button onClick={handleClose}>Cancel</Button>
          <Button variant="contained" type="submit">
            Submit
          </Button>
        </DialogActions>
      </Dialog>
    </>
  );
}
