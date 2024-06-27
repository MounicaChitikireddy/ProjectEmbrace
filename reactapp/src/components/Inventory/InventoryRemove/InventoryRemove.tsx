import {
  Accordion,
  AccordionDetails,
  AccordionSummary,
  Alert,
  AlertColor,
  AlertTitle,
  Autocomplete,
  Button,
  Container,
  FormControlLabel,
  IconButton,
  Paper,
  Snackbar,
  Switch,
  TextField,
  Tooltip,
} from "@mui/material";
import { Component, useState, useEffect } from "react";
import "./InventoryRemove.scss";
import { Delete } from "@mui/icons-material";
import {
  Controller,
  SubmitHandler,
  useFieldArray,
  useForm,
} from "react-hook-form";
import ExpandMoreIcon from "@mui/icons-material/ExpandMore";
import Header from "../../global/Header.js";
import { Device } from "../../../models/Device";
import { useNavigate } from "react-router-dom";
import { DeviceType } from "../../../models/DeviceType";
import MenuItem from "@mui/material/MenuItem";
import { DeviceTypeAPI } from "../../../apis/DeviceTypeApi";
import { DeviceAPI, DeviceAPIResponse } from "../../../apis/InventoryApi";
import { DeviceGrade } from "../../../enums/DeviceGrade.enum";
import { CampaignAPI } from "../../../apis/CampaignApi";
import { Campaign } from "../../../models/Campaign";
import { Contact } from "../../../models/Contact";
import { ContactAPI } from "../../../apis/ContactApi";
import { ChangelogAPI } from "../../../apis/ChangelogApi";
import ImportInventoryDialog from "../../Dialogs/InventoryDialogs/ImportInventoryDialog";
import { User } from "../../../models/User";

type FormData = {
  location: string;
  campaignId: number;
  contactIds: Array<Number>;
  additionalInfo: string;
  additions: Array<Device>;
  deletions: Array<Device>;
  batchApproved: boolean;
};

const InventoryRemoveForm = () => {
  const [deviceTypes, setDeviceTypes] = useState<DeviceType[]>();
  const [campaigns, setCampaigns] = useState<Campaign[]>();
  const [contacts, setContacts] = useState<Contact[]>();
  const [displayMessage, setDisplayMessage] = useState("");
  const [openSnackBar, setOpenSnackBar] = useState(false);
  const [snackBarSeverity, setSnackBarSeverity] = useState("info");
  const [isApprovalView, setIsApprovalView] = useState(false);
  const [alreadyApprovedOrDenied, setAlreadyApprovedOrDenied] = useState(false);
  const [batchToView, setBatchToView] = useState({});

  const accountData: User = JSON.parse(
    localStorage.getItem("accountData") || "{}"
  );

  const uniqueLocations: any = JSON.parse(
    localStorage.getItem("uniqueLocations") || "[]"
  );

  useEffect(() => {
    const fetchData = async () => {
      const devices = await DeviceTypeAPI.getAll();
      setDeviceTypes(devices);

      const campaigns = await CampaignAPI.getAll();
      setCampaigns(campaigns);

      const contacts = await ContactAPI.getAll();
      setContacts(contacts);
    };
    fetchData();

    const search = window.location.search;
    const params = new URLSearchParams(search);
    const batchId = params.get("Batch");

    if (batchId) {
      const fetchBatch = async () => {
        const data = await ChangelogAPI.getById(Number(batchId));
        const mappedData = data.map((removal: any) => {
          return {
            deviceTypeId: removal.typeID,
            count: removal.count,
            grade: removal.grade,
          };
        });

        const batchToView = JSON.parse(
          localStorage.getItem("batchToView") || "{}"
        );
        setBatchToView(batchToView);
        setIsApprovalView(true);

        if (
          batchToView.reviewStatus === "Approved" ||
          batchToView.reviewStatus === "Denied"
        ) {
          setAlreadyApprovedOrDenied(true);
          setSnackBarSeverity("info");
          setDisplayMessage(
            "Batch has already been approved. Loading read-only view."
          );
          setOpenSnackBar(true);
        }
        const batchToViewContacts = batchToView?.contacts.map(
          (c: any) => c.contactId
        );
        reset({
          campaignId: batchToView?.campaignId,
          contactIds: batchToViewContacts,
          location: data[0]?.location,
          additionalInfo: batchToView?.additionalInfo,
          additions: [],
          deletions: mappedData,
          batchApproved: accountData.role === "Admin",
        } as FormData);
      };
      fetchBatch();
    }
  }, []);

  const navigate = useNavigate();

  const {
    control,
    register,
    handleSubmit,
    getValues,
    setValue,
    reset,
    watch,
    formState: { isDirty, errors },
  } = useForm<FormData>({
    defaultValues: {
      campaignId: 0,
      location: "",
      additionalInfo: "",
      contactIds: [],
      additions: [],
      deletions: [
        {
          deviceTypeId: 0,
          count: 0,
          grade:
            accountData?.settings?.removeInventoryDefaultGrade ||
            DeviceGrade.BFRN,
        },
      ],
      batchApproved: accountData.role === "Admin",
    } as FormData,
  });

  const deletionsFieldArray = useFieldArray({
    control,
    name: "deletions",
  });

  const appendItemForRemoveForm = () => {
    deletionsFieldArray.append({
      deviceTypeId: 0,
      count: 0,
      grade:
        accountData?.settings?.removeInventoryDefaultGrade || DeviceGrade.BFRN,
    });
  };

  const removeItemForRemoveForm = (index: number) => {
    deletionsFieldArray.remove(index);
  };

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

  const saveEdits = (edits: any) => {
    const batch = batchToView as any;
    let action = 0;
    switch (batch.action) {
      case "Addition":
        action = 0;
        break;
      case "Removal":
        action = 1;
        break;
    }
    const data = {
      id: batch?.id,
      additionalInfo: edits.additionalInfo,
      contactIds: edits.contactIds,
      campaignId: edits.campaignId,
      reviewStatus: 0,
      action,
      location: edits.location,
      updates: edits.deletions,
      newDeviceTypes: [],
    };
    ChangelogAPI.update(data).then((response) => {
      handleCrudResponse(response);
      if (response.isSuccess) {
        navigate("/inventory/changelog", { replace: false });
      }
    });
  };

  const addImportedItemsToForm = (deletions: Device[]) => {
    // const currentDeletions = getValues().deletions;
    // const newDeletions = currentDeletions.concat(deletions);
    setValue("deletions", deletions);
  };

  const onSubmit: SubmitHandler<FormData> = (data) => {
    if (isApprovalView) {
      saveEdits(data);
    } else {
      try {
        DeviceAPI.update(data).then((response) => {
          handleCrudResponse(response);
          if (response.isSuccess) {
            navigate("/", { replace: true });
          }
        });
      } catch (error) {
        console.error(error);
      }
    }
  };

  const handleCrudResponse = async (response: DeviceAPIResponse) => {
    if (response) {
      setSnackBarSeverity(response.isSuccess ? "success" : "error");
      setDisplayMessage(response.displayMessage);
      setOpenSnackBar(true);
    }
  };

  return (
    <>
      <form onSubmit={handleSubmit(onSubmit)}>
        <div className="inventory-container">
          {!alreadyApprovedOrDenied && (
            <ImportInventoryDialog
              deviceTypes={deviceTypes as DeviceType[]}
              saveFn={addImportedItemsToForm}
            />
          )}
          <Paper className="form-section section-container">
            <h2 className="section-title campaign-title">
              Provision Information
            </h2>
            <div className="centered-full-width-row">
              <div className="smaller-input-container">
                {campaigns?.length && (
                  <Controller
                    control={control}
                    name={`campaignId`}
                    rules={{
                      required: { value: true, message: "Invalid input" },
                    }}
                    render={({ field: { name, value, onChange } }) => (
                      <Autocomplete
                        disabled={alreadyApprovedOrDenied}
                        options={campaigns.map((c) => c.campaignId)}
                        isOptionEqualToValue={(option, value) => {
                          return option === value;
                        }}
                        getOptionLabel={(option) => {
                          const c = campaigns?.find(
                            (c) => c.campaignId === option
                          );
                          return c ? c.campaignName : "";
                        }}
                        value={value ? value : null}
                        onChange={(e, data) => {
                          onChange(data ? data : 0);
                        }}
                        renderInput={(params) => (
                          <TextField required {...params} label="Campaign" />
                        )}
                      />
                    )}
                  />
                )}
              </div>
              <div className="smaller-input-container">
                {uniqueLocations && (
                  <Controller
                    control={control}
                    name={`location`}
                    render={({ field: { name, value, onChange } }) => (
                      <Autocomplete
                        freeSolo={true}
                        disabled={alreadyApprovedOrDenied}
                        options={uniqueLocations}
                        isOptionEqualToValue={(option, value) => {
                          return option === value;
                        }}
                        getOptionLabel={(option) => {
                          return option;
                        }}
                        value={value ? value : null}
                        onInputChange={(e, data) => {
                          onChange(data);
                        }}
                        renderInput={(params) => (
                          <TextField
                            required
                            {...params}
                            label="Storage Location"
                          />
                        )}
                      />
                    )}
                  />
                )}
              </div>
              <div className="campaign-input">
                <Controller
                  control={control}
                  name="additionalInfo"
                  defaultValue=""
                  render={({ field: { name, value, onChange } }) => (
                    <TextField
                      fullWidth
                      name={name}
                      value={value}
                      onChange={onChange}
                      label="Additional Information"
                      multiline
                      disabled={alreadyApprovedOrDenied}
                      rows={4}
                    />
                  )}
                />
              </div>
            </div>
          </Paper>
          <Accordion defaultExpanded={true}>
            <AccordionSummary expandIcon={<ExpandMoreIcon />}>
              <h2 className="section-title">Provision Contact Information</h2>
            </AccordionSummary>
            <AccordionDetails className="item-inputs">
              {contactIdsFieldArray.fields.map((field, index) => (
                <div className="item-row" key={field.id}>
                  <div className="item-info-container">
                    {contacts?.length && (
                      <div className="contact-input">
                        <Controller
                          control={control}
                          name={`contactIds.${index}`}
                          rules={{
                            required: { value: true, message: "Invalid input" },
                          }}
                          render={({ field: { name, value, onChange } }) => (
                            <Autocomplete
                              disabled={alreadyApprovedOrDenied}
                              options={contacts.map((c) => c.contactId)}
                              isOptionEqualToValue={(option, value) => {
                                return option === value;
                              }}
                              getOptionLabel={(option) => {
                                const c = contacts?.find(
                                  (c) => c.contactId === option
                                );
                                return c
                                  ? `${c.organization}: ${c.fullName}`
                                  : "";
                              }}
                              value={value ? value : null}
                              onChange={(e, data) => {
                                const currentContacts = getValues().contactIds;
                                const newData = data ? data : 0;
                                if (currentContacts.includes(newData)) {
                                  setSnackBarSeverity("warning");
                                  setDisplayMessage(
                                    "Contact already added to list."
                                  );
                                  setOpenSnackBar(true);
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
                        disabled={
                          alreadyApprovedOrDenied ||
                          contactIdsFieldArray.fields.length === 1
                        }
                        onClick={() => removeItemForContactIds(index)}
                      >
                        <Delete />
                      </IconButton>
                    </div>
                  </Tooltip>
                </div>
              ))}
              <Container maxWidth="lg" className="item-button-container">
                <Button
                  onClick={appendItemForContactIds}
                  color="primary"
                  variant="contained"
                  disabled={alreadyApprovedOrDenied}
                >
                  Add Contact
                </Button>
              </Container>
            </AccordionDetails>
          </Accordion>
          <Accordion defaultExpanded={true}>
            <AccordionSummary expandIcon={<ExpandMoreIcon />}>
              <h2 className="section-title">Add Items</h2>
            </AccordionSummary>
            <AccordionDetails className="item-inputs">
              {deletionsFieldArray.fields.map((field, index) => (
                <div className="item-row" key={field.id}>
                  <div className="item-info-container">
                    {deviceTypes?.length && (
                      <div className="item-input">
                        <Controller
                          control={control}
                          name={`deletions.${index}.deviceTypeId`}
                          rules={{
                            required: { value: true, message: "Invalid input" },
                          }}
                          render={({ field: { name, value, onChange } }) => (
                            <Autocomplete
                              disabled={alreadyApprovedOrDenied}
                              options={deviceTypes.map((dt) => dt.deviceTypeId)}
                              getOptionLabel={(option) => {
                                const dt = deviceTypes?.find(
                                  (dt) => dt.deviceTypeId === option
                                );
                                return dt
                                  ? `${dt.category}, ${dt.type}, ${dt.size}`
                                  : "";
                              }}
                              value={value ? value : null}
                              onChange={(e, data) => {
                                onChange(data);
                              }}
                              renderInput={(params) => (
                                <TextField
                                  required
                                  {...params}
                                  label="Device Type"
                                />
                              )}
                            />
                          )}
                        />
                      </div>
                    )}
                    <div className="small-input-container">
                      <div className="small-item-input">
                        <Controller
                          control={control}
                          name={`deletions.${index}.grade`}
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
                              value={value === "" ? DeviceGrade.BFRN : value}
                              fullWidth
                              disabled={alreadyApprovedOrDenied}
                            >
                              <MenuItem
                                key={`deletions.${index}.Grade.${DeviceGrade.A}`}
                                value={DeviceGrade.A}
                              >
                                {DeviceGrade.A}
                              </MenuItem>
                              <MenuItem
                                key={`deletions.${index}.Grade.${DeviceGrade.BFRN}`}
                                value={DeviceGrade.BFRN}
                              >
                                {DeviceGrade.BFRN}
                              </MenuItem>
                              <MenuItem
                                key={`deletions.${index}.Grade.${DeviceGrade.C}`}
                                value={DeviceGrade.C}
                              >
                                {DeviceGrade.C}
                              </MenuItem>
                              <MenuItem
                                key={`deletions.${index}.Grade.${DeviceGrade.UNPROCESSED}`}
                                value={DeviceGrade.UNPROCESSED}
                              >
                                {DeviceGrade.UNPROCESSED}
                              </MenuItem>
                              <MenuItem
                                key={`deletions.${index}.Grade.${DeviceGrade.NA}`}
                                value={DeviceGrade.NA}
                              >
                                {DeviceGrade.NA}
                              </MenuItem>
                            </TextField>
                          )}
                        />
                      </div>
                      <div className="small-item-input">
                        <TextField
                          required
                          fullWidth
                          disabled={alreadyApprovedOrDenied}
                          label="Quantity"
                          error={
                            errors?.deletions?.[index]?.count?.message
                              ? true
                              : false
                          }
                          key={field.id}
                          {...register(`deletions.${index}.count` as const, {
                            valueAsNumber: true,
                            min: { value: 1, message: " " },
                          })}
                        />
                      </div>
                    </div>
                  </div>
                  <Tooltip title="Delete">
                    <div>
                      <IconButton
                        disabled={deletionsFieldArray.fields.length === 1}
                        onClick={() => {
                          removeItemForRemoveForm(index);
                        }}
                      >
                        <Delete />
                      </IconButton>
                    </div>
                  </Tooltip>
                </div>
              ))}
              <Container maxWidth="lg" className="item-button-container">
                <Button
                  onClick={appendItemForRemoveForm}
                  color="primary"
                  variant="contained"
                  disabled={alreadyApprovedOrDenied}
                >
                  Add Item
                </Button>
              </Container>
            </AccordionDetails>
          </Accordion>
          <Paper
            className={
              accountData.role === "Admin" &&
              !alreadyApprovedOrDenied &&
              !isApprovalView
                ? "approve-batch-switch"
                : "hidden-input"
            }
          >
            <Controller
              control={control}
              name={`batchApproved`}
              render={({ field: { name, value, onChange } }) => (
                <FormControlLabel
                  control={
                    <Switch
                      disabled={alreadyApprovedOrDenied}
                      defaultChecked={accountData.role === "Admin"}
                      onChange={onChange}
                      value={value}
                      name={name}
                    />
                  }
                  label="Approve Batch Automatically"
                  labelPlacement="start"
                />
              )}
            />
          </Paper>
          {!isApprovalView && (
            <Container maxWidth="lg" className="submit-button-container">
              <Button size="large" variant="contained" type="submit">
                Submit
              </Button>
            </Container>
          )}
          {isApprovalView && (
            <Container maxWidth="lg" className="submit-button-container">
              <Button
                size="large"
                variant="contained"
                color="primary"
                type="submit"
                disabled={!isDirty}
              >
                Save Changes
              </Button>
              <Button
                size="large"
                variant="outlined"
                color="primary"
                onClick={() => {
                  localStorage.removeItem("batchToView");
                  navigate("/inventory/changelog", { replace: true });
                }}
              >
                Cancel
              </Button>
            </Container>
          )}
        </div>
      </form>
      <Snackbar
        open={openSnackBar}
        autoHideDuration={5000}
        onClose={() => setOpenSnackBar(false)}
      >
        <Alert
          severity={snackBarSeverity as AlertColor}
          onClose={() => setOpenSnackBar(false)}
        >
          <AlertTitle sx={{ textTransform: "Capitalize" }}>
            {snackBarSeverity}
          </AlertTitle>
          {displayMessage}
        </Alert>
      </Snackbar>
    </>
  );
};
export default class InventoryAdd extends Component {
  render() {
    return (
      <>
        <Header title="Remove Inventory" />
        <div className="page-container">
          <div className="table-title">
            <InventoryRemoveForm></InventoryRemoveForm>
          </div>
        </div>
      </>
    );
  }
}
