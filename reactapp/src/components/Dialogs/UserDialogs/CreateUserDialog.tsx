import Button from "@mui/material/Button";
import DialogActions from "@mui/material/DialogActions";
import DialogTitle from "@mui/material/DialogTitle";
import MenuItem from "@mui/material/MenuItem";
import DialogContent from "@mui/material/DialogContent";
import TextField from "@mui/material/TextField";
import { Controller, SubmitHandler, useForm } from "react-hook-form";
import { FormInputText } from "../../Shared/FormInputText";
import { useState, useEffect } from "react";
import "./UserDialog.scss";
import { AuthenticationAPI } from "../../../apis/AuthenticationApi";
import { yupResolver } from "@hookform/resolvers/yup";
import * as yup from "yup";

export default function CreateUserDialog({ row, table, actionFn }: any) {
  const [roles, setRoles] = useState([]);
  useEffect(() => {
    const fetchData = async () => {
      const data = await AuthenticationAPI.getRoles();
      const roleNames = data.map((role: any) => role?.name);
      setRoles(roleNames);
    };
    fetchData();
  }, []);

  const schema = yup
  .object({
    id: yup.string(),
    displayId: yup.number(),
    fullName: yup.string().required(),
    title: yup.string().required(),
    department: yup.string().required(),
    role: yup.string().required(),
    phoneNumber: yup.string().required(),
    email: yup.string().required(),
    password: yup.string().required(),
    confirmPassword: yup
      .string()
      .oneOf([yup.ref("password")], "Passwords must match")
      .required(),
  })
  .required();

  const defaultUser = {
    id: "",
    displayId: 0,
    fullName: "",
    title: "",
    department: "",
    role: "",
    phoneNumber: "",
    email: "",
    password: "",
    confirmPassword: "",
  };

  const handleClose = () => {
    table.setCreatingRow(null);
  };

  const onSubmit: SubmitHandler<any> = (data) => {
    const createUserPayload = {
      id: data.id,
      displayId: data.displayId,
      fullName: data.fullName,
      title: data.title,
      department: data.department,
      role: data.role,
      phoneNumber: data.phoneNumber,
      email: data.email,
      password: data.password,
    };
    actionFn("Create", createUserPayload);
    handleClose();
  };

  let user = defaultUser;
  if (row) {
    const userFromRow = row.original;
    user = {
      id: userFromRow.id ?? "",
      displayId: userFromRow.displayId ?? 0,
      fullName: userFromRow.fullName ?? "",
      title: userFromRow.title ?? "",
      department: userFromRow.department ?? "",
      role: userFromRow.role ?? "",
      phoneNumber: userFromRow.phoneNumber ?? "",
      email: userFromRow.email ?? "",
      password: "",
      confirmPassword: "",
    };
  }

  const { 
    control,
    handleSubmit,
    formState: { errors },
    } = useForm({
    defaultValues: {
      ...user,
    } as any,
    resolver: yupResolver(schema),
  });

  return (
    <form onSubmit={handleSubmit(onSubmit)}>
      <DialogTitle>Create User</DialogTitle>
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
        <div className="dialog-input">
          <Controller
            control={control}
            name="password"
            defaultValue=""
            rules={{ required: { value: true, message: "Invalid input" } }}
            render={({ field: { name, value, onChange } }) => (
              <TextField
                required
                fullWidth
                type="password"
                name={name}
                value={value}
                onChange={onChange}
                label="Password"
              />
            )}
          />
        </div>
        <div className="dialog-input">
          <Controller
            control={control}
            name="confirmPassword"
            defaultValue=""
            rules={{ required: { value: true, message: "Invalid input" } }}
            render={({ field: { name, value, onChange } }) => (
              <TextField
                required
                fullWidth
                name={name}
                value={value}
                onChange={onChange}
                type="password"
                label="Confirm Password"
                error={errors.confirmPassword?.message ? true : false}
                helperText={errors?.confirmPassword?.message ? String(errors.confirmPassword?.message) : ""}
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
  );
}
