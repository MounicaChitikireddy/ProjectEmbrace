import {
  Button,
  DialogActions,
  DialogContent,
  DialogTitle,
} from "@mui/material";
import { SubmitHandler, useForm } from "react-hook-form";
import { FormInputText } from "../../Shared/FormInputText";
import "./CreateUpdateContactDialog.scss";
import { Contact } from "../../../models/Contact";

export default function CreateUpdateContactDialog({
  row,
  table,
  action,
  actionFn,
}: any) {
  const defaultContact = {
    contactId: 0,
    fullName: "",
    phoneNum: "",
    email: "",
    role: "",
    organization: "",
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

  const dialogTitle: string = action + " Contact";

  const onSubmit: SubmitHandler<Contact> = (data) => {
    actionFn(action, data);
    handleClose();
  };

  let contact = defaultContact;
  if (row) {
    contact = row.original as Contact;
  }

  const { control, handleSubmit } = useForm({
    defaultValues: {
      ...contact,
    } as Contact,
  });

  return (
    <>
      <form onSubmit={handleSubmit(onSubmit)}>
        <DialogTitle>{dialogTitle}</DialogTitle>
        <DialogContent>
          <div className="dialog-input">
            <FormInputText
              name="fullName"
              label="Full Name"
              control={control}
            />
          </div>
          <div className="dialog-input">
            <FormInputText
              name="phoneNum"
              label="Phone Number"
              control={control}
            />
          </div>
          <div className="dialog-input">
            <FormInputText name="email" label="Email" control={control} />
          </div>
          <div className="dialog-input">
            <FormInputText name="role" label="Role" control={control} />
          </div>
          <div className="dialog-input">
            <FormInputText
              name="organization"
              label="Organization"
              control={control}
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
