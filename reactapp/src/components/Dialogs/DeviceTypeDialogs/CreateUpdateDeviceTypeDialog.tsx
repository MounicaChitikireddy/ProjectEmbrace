import Button from "@mui/material/Button";
import DialogActions from "@mui/material/DialogActions";
import DialogTitle from "@mui/material/DialogTitle";
import DialogContent from "@mui/material/DialogContent";
import { Controller, SubmitHandler, useForm } from "react-hook-form";
import { FormInputText } from "../../Shared/FormInputText";
import "./CreateUpdateDeviceTypeDialog.scss";
import { DeviceType } from "../../../models/DeviceType";
import { TextField } from "@mui/material";

export default function CreateUpdateDeviceTypeDialog({
  row,
  table,
  action,
  actionFn,
}: any) {
  const defaultDeviceType = {
    deviceTypeId: 0,
    category: "",
    type: "",
    size: "",
    description: "",
  };

  const handleClose = () => {
    switch (action) {
      case "Edit":
        table.setEditingRow(null);
        break;
      case "Create":
        table.setCreatingRow(null);
        break;
      default:
        break;
    }
  };

  const dialogTitle: string = action + " Device Type";

  const onSubmit: SubmitHandler<DeviceType> = (data) => {
    actionFn(action, data);
    handleClose();
  };

  let deviceType = defaultDeviceType;
  if (row) {
    const deviceTypeFromRow = row.original as DeviceType;
    deviceType = {
      deviceTypeId: deviceTypeFromRow.deviceTypeId ?? 0,
      category: deviceTypeFromRow.category ?? "",
      type: deviceTypeFromRow.type ?? "",
      size: deviceTypeFromRow.size ?? "",
      description: deviceTypeFromRow.description ?? "",
    };
  }

  const { control, handleSubmit } = useForm({
    defaultValues: {
      ...deviceType,
    } as DeviceType,
  });

  return (
    <>
      <form onSubmit={handleSubmit(onSubmit)}>
        <DialogTitle>{dialogTitle}</DialogTitle>
        <DialogContent>
          <div className="dialog-input">
            <FormInputText name="category" label="Category" control={control} />
          </div>
          <div className="dialog-input">
            <FormInputText name="type" label="Type" control={control} />
          </div>
          <div className="dialog-input">
            <FormInputText name="size" label="Size" control={control} />
          </div>
          <div className="dialog-input">
            <Controller
              control={control}
              name={"description"}
              defaultValue=""
              rules={{ required: { value: false, message: "Invalid input" } }}
              render={({ field: { name, value, onChange } }) => (
                <TextField
                  fullWidth
                  name={name}
                  value={value}
                  onChange={onChange}
                  label={"Description"}
                />
              )}
            />
          </div>
        </DialogContent>
        <DialogActions>
          <Button variant="outlined" onClick={handleClose}>
            Cancel
          </Button>
          <Button variant="contained" type="submit">
            Save
          </Button>
        </DialogActions>
      </form>
    </>
  );
}
