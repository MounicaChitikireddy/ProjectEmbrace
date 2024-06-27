import Button from "@mui/material/Button";
import "./CreateRecieptDialog.scss";
import { useState } from "react";
import {
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
  TextField,
  MenuItem,
  useTheme,
  Autocomplete,
  Tooltip,
  IconButton,
  Container,
  CircularProgress,
} from "@mui/material";
import { Controller, useFieldArray, useForm } from "react-hook-form";
import useMediaQuery from "@mui/material/useMediaQuery";
import { Delete } from "@mui/icons-material";
import { ChangelogAPI } from "../../../apis/ChangelogApi";

export default function CreateRecieptDialog({
  row,
  contacts,
  responseFn,
  snackBarFn,
}: any) {
  const theme = useTheme();
  const fullScreen = useMediaQuery(theme.breakpoints.down("md"));

  const [open, setOpen] = useState(false);
  const [batchId, setBatchId] = useState(0);
  const [submitSent, setSubmitSent] = useState(false);

  const handleClickOpen = () => {
    const contactIds = row?.contacts.map((c: any) => c.contactId);
    setBatchId(row?.id);
    setValue("contactIds", contactIds);
    setOpen(true);
  };

  const handleClose = () => {
    setOpen(false);
  };

  const { control, register, handleSubmit, setValue, getValues } = useForm({
    defaultValues: {
      contactIds: [0],
    } as any,
  });

  const contactIdsFieldArray = useFieldArray({
    control,
    name: "contactIds",
  });

  const appendItemForContactIds = () => {
    contactIdsFieldArray.append(0);
  };

  const removeItemForContactIds = (index: number) => {
    contactIdsFieldArray.remove(index);
  };

  const onSubmit = (data: any) => {
    const receiptInfo = {
      batchId: batchId,
      contactIds: data.contactIds,
    };
    setSubmitSent(true);
    ChangelogAPI.sendReceipt(receiptInfo).then((response) => {
      setSubmitSent(false);
      responseFn(response);
      handleClose();
    });
  };

  return (
    <>
      <MenuItem
        key="reciept"
        disabled={row.reviewStatus === "Pending"}
        onClick={() => {
          handleClickOpen();
        }}
      >
        Create Reciept
      </MenuItem>
      {contacts && (
        <Dialog
          fullScreen={fullScreen}
          fullWidth
          maxWidth={"md"}
          open={open}
          onClose={handleClose}
          PaperProps={{
            component: "form",
            onSubmit: handleSubmit(onSubmit),
          }}
        >
          <DialogTitle>Generate Reciept</DialogTitle>
          <DialogContent
            sx={{ mt: "5px" }}
            className={submitSent ? "dialog-loading" : ""}
          >
            {!submitSent && (
              <>
                <div className="item-inputs">
                  {contactIdsFieldArray.fields.map((field, index) => (
                    <div className="item-row" key={field.id}>
                      <div className="item-info-container">
                        {contacts?.length && (
                          <div className="contact-input">
                            <Controller
                              control={control}
                              name={`contactIds.${index}`}
                              rules={{
                                required: {
                                  value: true,
                                  message: "Invalid input",
                                },
                              }}
                              render={({
                                field: { name, value, onChange },
                              }) => (
                                <Autocomplete
                                  options={contacts.map(
                                    (c: any) => c.contactId
                                  )}
                                  isOptionEqualToValue={(option, value) => {
                                    return option === value;
                                  }}
                                  getOptionLabel={(option) => {
                                    const c = contacts?.find(
                                      (c: any) => c.contactId === option
                                    );
                                    return c
                                      ? `${c.organization}: ${c.fullName}`
                                      : "";
                                  }}
                                  value={value ? value : null}
                                  onChange={(e, data) => {
                                    const currentContacts =
                                      getValues().contactIds;
                                    const newData = data ? data : 0;
                                    if (currentContacts.includes(newData)) {
                                      snackBarFn(
                                        "Contact already added to list.",
                                        "warning"
                                      );
                                    } else {
                                      onChange(newData);
                                    }
                                  }}
                                  renderInput={(params) => (
                                    <TextField
                                      required
                                      {...params}
                                      label="Contact"
                                    />
                                  )}
                                />
                              )}
                            />
                          </div>
                        )}
                      </div>
                      <Tooltip title="Delete">
                        <div>
                          <IconButton
                            disabled={contactIdsFieldArray.fields.length === 1}
                            onClick={() => removeItemForContactIds(index)}
                          >
                            <Delete />
                          </IconButton>
                        </div>
                      </Tooltip>
                    </div>
                  ))}
                </div>
                <Container maxWidth="lg" className="item-button-container">
                  <Button
                    onClick={appendItemForContactIds}
                    color="primary"
                    variant="contained"
                  >
                    Add Contact
                  </Button>
                </Container>
              </>
            )}
            {submitSent && <CircularProgress color="inherit" />}
          </DialogContent>
          <DialogActions>
            <Button onClick={handleClose}>Cancel</Button>
            <Button disabled={submitSent} variant="contained" type="submit">
              Submit
            </Button>
          </DialogActions>
        </Dialog>
      )}
    </>
  );
}
