import {
  Button,
  DialogActions,
  DialogContent,
  DialogTitle,
} from "@mui/material";
import { Controller, SubmitHandler, useForm } from "react-hook-form";
import { FormInputText } from "../../Shared/FormInputText";
import "./CreateUpdateCampaignDialog.scss";
import { Campaign } from "../../../models/Campaign";
import { DatePicker } from "@mui/x-date-pickers";
import dayjs from "dayjs";

export default function CreateUpdateCampaignDialog({
  row,
  table,
  action,
  actionFn,
}: any) {
  const defaultCampaign = {
    campaignId: 0,
    campaignName: "",
    startDate: new Date(""),
    endDate: new Date(""),
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

  const dialogTitle: string = action + " Campaign";

  const onSubmit: SubmitHandler<Campaign> = (data) => {
    actionFn(action, data);
    handleClose();
  };

  let campaign = defaultCampaign;

  if (row) {
    campaign = {
      campaignId: row?.original?.campaignId ?? 0,
      campaignName: row?.original?.campaignName ?? "",
      startDate: row?.original?.startDate ?? "",
      endDate: row?.original?.endDate ?? "",
    };
  }

  const { control, handleSubmit } = useForm({
    defaultValues: {
      ...campaign,
    } as Campaign,
  });

  return (
    <>
      <form onSubmit={handleSubmit(onSubmit)}>
        <DialogTitle>{dialogTitle}</DialogTitle>
        <DialogContent>
          <div className="dialog-input">
            <FormInputText name="campaignName" label="Name" control={control} />
          </div>
          <div className="dialog-input">
            <Controller
              control={control}
              name="startDate"
              rules={{ required: { value: true, message: "Invalid input" } }}
              render={({ field: { name, value, onChange } }) => (
                <DatePicker
                  sx={{ width: "100%" }}
                  onChange={onChange}
                  value={dayjs(value)}
                  label="Start Date"
                ></DatePicker>
              )}
            />
          </div>
          <div className="dialog-input">
            <Controller
              control={control}
              name="endDate"
              rules={{ required: { value: true, message: "Invalid input" } }}
              render={({ field: { name, value, onChange } }) => (
                <DatePicker
                  sx={{ width: "100%" }}
                  onChange={onChange}
                  value={dayjs(value)}
                  label="End Date"
                ></DatePicker>
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
