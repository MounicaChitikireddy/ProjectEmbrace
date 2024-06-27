import Button from "@mui/material/Button";
import DialogActions from "@mui/material/DialogActions";
import DialogTitle from "@mui/material/DialogTitle";
import MenuItem from "@mui/material/MenuItem";
import DialogContent from "@mui/material/DialogContent";
import TextField from "@mui/material/TextField";
import { User } from "../../../models/User";
import { Controller, SubmitHandler, useForm } from "react-hook-form";
import { FormInputText } from "../../Shared/FormInputText";
import { useState, useEffect } from "react";
import "./UserDialog.scss";
import { AuthenticationAPI } from "../../../apis/AuthenticationApi";

export default function UpdateUserDialog({ row, table, actionFn }: any) {
  const [roles, setRoles] = useState([]);
  useEffect(() => {
    const fetchData = async () => {
      const data = await AuthenticationAPI.getRoles();
      const roleNames = data.map((role: any) => role?.name);
      setRoles(roleNames);
    };
    fetchData();
  }, []);

  const defaultUser = {
    id: "",
    displayId: 0,
    fullName: "",
    title: "",
    department: "",
    role: "",
    phoneNumber: "",
    email: "",
  };

  const handleClose = () => {
    table.setEditingRow(null);
  };

  const onSubmit: SubmitHandler<User> = (data) => {
    actionFn("Edit", data);
    handleClose();
  };

  let user = defaultUser;
  if (row) {
    const userFromRow = row.original as User;
    user = {
      id: userFromRow.id ?? "",
      displayId: userFromRow.displayId ?? 0,
      fullName: userFromRow.fullName ?? "",
      title: userFromRow.title ?? "",
      department: userFromRow.department ?? "",
      role: userFromRow.role ?? "",
      phoneNumber: userFromRow.phoneNumber ?? "",
      email: userFromRow.email ?? "",
    };
  }

  const { control, handleSubmit } = useForm({
    defaultValues: {
      ...user,
    } as User,
  });

  return (
    <>
      <form onSubmit={handleSubmit(onSubmit)}>
        <DialogTitle>Edit User</DialogTitle>
        <DialogContent>
          <div className="dialog-input">
            <FormInputText
              name="fullName"
              label="Full Name"
              control={control}
            />
          </div>
          <div className="dialog-input">
            <FormInputText name="title" label="Title" control={control} />
          </div>
          <div className="dialog-input">
            <FormInputText
              name="department"
              label="Department"
              control={control}
            />
          </div>
          <div className="dialog-input">
            <Controller
              control={control}
              name="role"
              rules={{ required: { value: true, message: "Invalid input" } }}
              render={({ field: { name, value, onChange } }) => (
                <TextField
                  sx={{ width: "100%" }}
                  value={value}
                  onChange={onChange}
                  select
                  required={true}
                  label="Role"
                >
                  {roles.map((role: any) => (
                    <MenuItem key={role} value={role}>
                      {role}
                    </MenuItem>
                  ))}
                </TextField>
              )}
            />
          </div>
          <div className="dialog-input">
            <FormInputText
              name="phoneNumber"
              label="Phone Number"
              control={control}
            />
          </div>
          <div className="dialog-input">
            <FormInputText name="email" label="Email" control={control} />
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
