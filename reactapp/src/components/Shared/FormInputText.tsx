import { Controller } from "react-hook-form";
import { TextField } from '@mui/material';
export const FormInputText = ({ name, label, control }: any) => {
  return (
    <Controller
      control={control}
      name={name}
      defaultValue=""
      rules={{ required: { value: true, message: "Invalid input" } }}
      render={({ field: { name, value, onChange } }) => (
        <TextField
          required
          fullWidth
          name={name}
          value={value}
          onChange={onChange}
          label={label}
        />
      )}
    />
  );
};
