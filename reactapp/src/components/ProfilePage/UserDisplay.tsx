import {
  Alert,
  AlertTitle,
  Avatar,
  Button,
  Card,
  CardActions,
  CardContent,
  CardHeader,
  FormControlLabel,
  MenuItem,
  Snackbar,
  Switch,
  TextField,
} from "@mui/material";
import { User } from "../../models/User.js";
import Header from "../global/Header.js";
import { Controller, SubmitHandler, useForm } from "react-hook-form";
import { DeviceGrade } from "../../enums/DeviceGrade.enum.js";
import { TableDensity } from "../../enums/TableDensity.enum.js";
import { UserAPI } from "../../apis/UserApi.js";
import { useEffect, useState } from "react";
import { UserSettings } from "../../models/UserSettings.js";

export default function UserDisplay() {
  const [isSuccess, setIsSuccess] = useState(false);
  const [displayMessage, setDisplayMessage] = useState("");
  const [openSnackBar, setOpenSnackBar] = useState(false);
  const [submittedData, setSubmittedData] = useState();

  const accountData: User = JSON.parse(
    localStorage.getItem("accountData") || "{}"
  );

  const {
    register,
    control,
    handleSubmit,
    watch,
    reset,
    formState,
    formState: { errors, isDirty, isSubmitSuccessful },
  } = useForm({
    defaultValues: {
      ...accountData.settings!,
    },
  });

  const submitForm: SubmitHandler<any> = async (data: any) => {
    const response = await UserAPI.updateSettings(data);
    setIsSuccess(response.isSuccess);
    setDisplayMessage(response.displayMessage);
    setOpenSnackBar(true);

    if (response.isSuccess) {
      setSubmittedData(data);
      reset({ ...data });
    }
  };

  useEffect(() => {
    if (isSubmitSuccessful) {
      UserAPI.getCurrentUser().then((response) => {
        localStorage.setItem("accountData", JSON.stringify(response));
        reset({ ...(submittedData! as UserSettings) });
      });
    }
  }, [formState, submittedData, reset]);

  const watchAvatarColor = watch("avatarColor");
  const watchAvatarText = watch("avatarText");

  return (
    <>
      <Header title="User Profile" />
      <div className="card-container">
        <Card className="card">
          <CardHeader className="card-header" title="Account Information" />
          <CardContent className="card-content">
            <TextField
              sx={{ mb: 1.5 }}
              label="Full Name"
              value={accountData.fullName}
              disabled
            />
            <TextField
              sx={{ mb: 1.5 }}
              label="Title"
              value={accountData.title}
              disabled
            />
            <TextField
              sx={{ mb: 1.5 }}
              label="Department"
              value={accountData.department}
              disabled
            />
            <TextField
              sx={{ mb: 1.5 }}
              label="Role"
              value={accountData.role}
              disabled
            />
            <TextField
              sx={{ mb: 1.5 }}
              label="Phone Number"
              value={accountData.phoneNumber}
              disabled
            />
            <TextField label="Email" value={accountData.email} disabled />
          </CardContent>
        </Card>
        <Card className="card">
          <CardHeader className="card-header" title="User Settings" />
          <form onSubmit={handleSubmit(submitForm)}>
            <CardContent className="card-content">
              <Controller
                control={control}
                name={`tableDensity`}
                render={({ field: { name, value, onChange } }) => (
                  <TextField
                    sx={{ width: "100%" }}
                    onChange={onChange}
                    select
                    required={true}
                    label="Table Density"
                    value={value}
                    fullWidth
                  >
                    <MenuItem
                      key={`tableDensity.${TableDensity.COMPACT}`}
                      value={TableDensity.COMPACT}
                    >
                      {TableDensity.COMPACT}
                    </MenuItem>
                    <MenuItem
                      key={`tableDensity.${TableDensity.COMFORTABLE}`}
                      value={TableDensity.COMFORTABLE}
                    >
                      {TableDensity.COMFORTABLE}
                    </MenuItem>
                    <MenuItem
                      key={`tableDensity.${TableDensity.SPACIOUS}`}
                      value={TableDensity.SPACIOUS}
                    >
                      {TableDensity.SPACIOUS}
                    </MenuItem>
                  </TextField>
                )}
              />
              <Controller
                control={control}
                name={`addInventoryDefaultGrade`}
                render={({ field: { name, value, onChange } }) => (
                  <TextField
                    sx={{ width: "100%" }}
                    onChange={onChange}
                    select
                    required={true}
                    label="Add Inventory Default Grade"
                    value={value === "" ? DeviceGrade.BFRN : value}
                    fullWidth
                  >
                    <MenuItem
                      key={`addInventoryDefaultGrade.${DeviceGrade.A}`}
                      value={DeviceGrade.A}
                    >
                      {DeviceGrade.A}
                    </MenuItem>
                    <MenuItem
                      key={`addInventoryDefaultGrade.${DeviceGrade.BFRN}`}
                      value={DeviceGrade.BFRN}
                    >
                      {DeviceGrade.BFRN}
                    </MenuItem>
                    <MenuItem
                      key={`addInventoryDefaultGrade.${DeviceGrade.C}`}
                      value={DeviceGrade.C}
                    >
                      {DeviceGrade.C}
                    </MenuItem>
                    <MenuItem
                      key={`addInventoryDefaultGrade.${DeviceGrade.UNPROCESSED}`}
                      value={DeviceGrade.UNPROCESSED}
                    >
                      {DeviceGrade.UNPROCESSED}
                    </MenuItem>
                    <MenuItem
                      key={`addInventoryDefaultGrade.${DeviceGrade.NA}`}
                      value={DeviceGrade.NA}
                    >
                      {DeviceGrade.NA}
                    </MenuItem>
                  </TextField>
                )}
              />
              <Controller
                control={control}
                name={`removeInventoryDefaultGrade`}
                render={({ field: { name, value, onChange } }) => (
                  <TextField
                    sx={{ width: "100%" }}
                    onChange={onChange}
                    select
                    required={true}
                    label="Remove Inventory Default Grade"
                    value={value === "" ? DeviceGrade.BFRN : value}
                    fullWidth
                  >
                    <MenuItem
                      key={`removeInventoryDefaultGrade.${DeviceGrade.A}`}
                      value={DeviceGrade.A}
                    >
                      {DeviceGrade.A}
                    </MenuItem>
                    <MenuItem
                      key={`removeInventoryDefaultGrade.${DeviceGrade.BFRN}`}
                      value={DeviceGrade.BFRN}
                    >
                      {DeviceGrade.BFRN}
                    </MenuItem>
                    <MenuItem
                      key={`removeInventoryDefaultGrade.${DeviceGrade.C}`}
                      value={DeviceGrade.C}
                    >
                      {DeviceGrade.C}
                    </MenuItem>
                    <MenuItem
                      key={`removeInventoryDefaultGrade.${DeviceGrade.UNPROCESSED}`}
                      value={DeviceGrade.UNPROCESSED}
                    >
                      {DeviceGrade.UNPROCESSED}
                    </MenuItem>
                    <MenuItem
                      key={`removeInventoryDefaultGrade.${DeviceGrade.NA}`}
                      value={DeviceGrade.NA}
                    >
                      {DeviceGrade.NA}
                    </MenuItem>
                  </TextField>
                )}
              />
              <div className="avatar-container">
                <div className="avatar-inputs">
                  <TextField
                    label="Avatar Text"
                    error={errors.avatarText?.message ? true : false}
                    helperText={errors.avatarText?.message}
                    {...register("avatarText", {
                      required: false,
                      maxLength: {
                        value: 3,
                        message: "Error: Maximum of 3 characters allowed.",
                      },
                    })}
                  />
                  <TextField
                    label="Avatar Color"
                    error={errors.avatarColor?.message ? true : false}
                    helperText={errors.avatarColor?.message}
                    {...register("avatarColor", {
                      pattern: {
                        value: /^#?([a-f0-9]{6}|[a-f0-9]{3})$/,
                        message: "Error: Invalid Hex Color.",
                      },
                    })}
                  />
                </div>
                <div className="avatar-preview">
                  <Avatar sx={{ bgcolor: watchAvatarColor }}>
                    {watchAvatarText}
                  </Avatar>
                </div>
              </div>
              <div className="table-switch">
                <Controller
                  control={control}
                  name={`tableFullScreenByDefault`}
                  render={({ field: { name, value, onChange } }) => (
                    <FormControlLabel
                      control={
                        <Switch onChange={onChange} value={value} name={name} />
                      }
                      label="Table Full Screen By Default"
                      labelPlacement="start"
                    />
                  )}
                />
              </div>
            </CardContent>
            <CardActions
              sx={{
                display: "flex",
                justifyContent: "right",
                paddingTop: "0px",
                paddingBottom: "24px",
                paddingInline: "24px",
              }}
            >
              <Button disabled={!isDirty} type="submit" variant="contained">
                Save Changes
              </Button>
            </CardActions>
          </form>
        </Card>
      </div>
      <Snackbar
        open={openSnackBar}
        autoHideDuration={5000}
        onClose={() => setOpenSnackBar(false)}
      >
        <Alert
          severity={isSuccess ? "success" : "error"}
          sx={{ whiteSpace: "pre-line" }}
          onClose={() => setOpenSnackBar(false)}
        >
          <AlertTitle>{isSuccess ? "Success" : "Error"}</AlertTitle>
          {displayMessage}
        </Alert>
      </Snackbar>
    </>
  );
}
