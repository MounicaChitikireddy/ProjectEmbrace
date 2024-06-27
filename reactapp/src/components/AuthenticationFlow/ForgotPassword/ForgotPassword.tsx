import {
  Alert,
  AlertTitle,
  Button,
  Container,
  Link,
  Snackbar,
  Stack,
  Typography,
} from "@mui/material";
import { Component, useState } from "react";
import { SubmitHandler, useForm } from "react-hook-form";
import { FormInputText } from "../../Shared/FormInputText";
import "./ForgotPassword.scss";
import { AuthenticationAPI } from "../../../apis/AuthenticationApi";

const ForgotPasswordForm = () => {
  const [openSnackBar, setOpenSnackBar] = useState(false);
  const [isSuccess, setIsSuccess] = useState(false);
  const [displayMessage, setDisplayMessage] = useState("");

  const onSubmit: SubmitHandler<any> = async (data: any) => {
    const response = await AuthenticationAPI.forgotPassword(data.email);
    if (response.status === 200) {
      setIsSuccess(true);
      setDisplayMessage("Email has been sent");
      setOpenSnackBar(true);
      window.location.href = "/reset-password";
    } else {
      setIsSuccess(false);
      setDisplayMessage("An error has occured while sending the email");
      setOpenSnackBar(true);
      console.error(response);
    }
  };

  const { control, handleSubmit } = useForm({
    defaultValues: { email: "" },
  });

  return (
    <>
      <form onSubmit={handleSubmit(onSubmit)}>
        <Stack className="stack-container">
          <Typography
            className="forgot-your-password"
            variant="h5"
            align="center"
          >
            Forgot your password?
          </Typography>
          <Typography
            className="forgot-your-password-subtitle"
            variant="body2"
            align="center"
          >
            Enter your email address and we will send you a code to reset your
            password.
          </Typography>
          <FormInputText name="email" label="Email Address" control={control} />
          <Button
            type="submit"
            fullWidth
            variant="contained"
            sx={{ mt: 3, mb: 2 }}
          >
            Send Request
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
