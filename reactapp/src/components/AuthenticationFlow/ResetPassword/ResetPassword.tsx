import {
  Alert,
  AlertTitle,
  Button,
  Container,
  Link,
  Snackbar,
  Stack,
  TextField,
  Typography,
} from "@mui/material";
import { Component, useState } from "react";
import { Controller, SubmitHandler, useForm } from "react-hook-form";
import { FormInputText } from "../../Shared/FormInputText";
import "./ResetPassword.scss";
import { yupResolver } from "@hookform/resolvers/yup";
import * as yup from "yup";
import { AuthenticationAPI } from "../../../apis/AuthenticationApi";

const ForgotPasswordForm = () => {
  const [openSnackBar, setOpenSnackBar] = useState(false);
  const [isSuccess, setIsSuccess] = useState(false);
  const [displayMessage, setDisplayMessage] = useState("");

  const schema = yup
    .object({
      email: yup.string().required(),
      resetCode: yup.string().required(),
      password: yup.string().required(),
      confirmPassword: yup
        .string()
        .oneOf([yup.ref("password")], "Passwords must match")
        .required(),
    })
    .required();

  const extractErrors = (response: any) => {
    const title = `Error: ${response.title}\n`;
    let errorMessages = "";
    for (const key in response.errors) {
      const errors = response.errors[key];
      errorMessages += `${key}: ${errors.join("\n")}\n`;
    }
    return title + errorMessages;
  };

  const onSubmit: SubmitHandler<any> = async (data: any) => {
    const resetPasswordPayload = {
      email: data.email,
      resetCode: data.resetCode,
      newPassword: data.password,
    };
    const response = await AuthenticationAPI.resetPassword(
      resetPasswordPayload
    );
    if (response.status === 200) {
      setIsSuccess(true);
      setDisplayMessage("Password reset successfully. Please return to login");
      setOpenSnackBar(true);
    } else {
      response.json().then((data) => {
        const errorMessages = extractErrors(data);
        setIsSuccess(false);
        setDisplayMessage(errorMessages);
        setOpenSnackBar(true);
        console.error(response);
      });
    }
  };

  const {
    control,
    handleSubmit,
    formState: { errors },
  } = useForm({
    defaultValues: {
      email: "",
      resetCode: "",
      password: "",
      confirmPassword: "",
    },
    resolver: yupResolver(schema),
  });

  return (
    <>
      <form onSubmit={handleSubmit(onSubmit)}>
        <Stack className="stack-container">
          <Typography
            className="reset-password-title"
            variant="h5"
            align="center"
          >
            Request sent successfully!
          </Typography>
          <Typography
            className="reset-password-subtitle"
            variant="body2"
            align="center"
          >
            We've sent a password reset email containing a reset code to your
            email. Please enter the code in below box to verify your email.
          </Typography>
          <div className="reset-form-input">
            <FormInputText
              name="email"
              label="Email Address"
              control={control}
            />
          </div>
          <div className="reset-form-input">
            <FormInputText
              name="resetCode"
              label="Reset Code"
              control={control}
            />
          </div>
          <div className="reset-form-input">
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
            />{" "}
          </div>
          <div className="reset-form-input">
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
                  helperText={errors.confirmPassword?.message}
                />
              )}
            />
          </div>
          <Button
            type="submit"
            fullWidth
            variant="contained"
            sx={{ mt: 3, mb: 2 }}
          >
            Update Password
          </Button>
          <Button>
            <Link href="/login">Back to login</Link>
          </Button>
        </Stack>
      </form>
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
};

export default class ForgotPassword extends Component {
  render() {
    return (
      <>
        <div className="page-container">
          <Container maxWidth="sm">
            <ForgotPasswordForm />
          </Container>
        </div>
      </>
    );
  }
}
