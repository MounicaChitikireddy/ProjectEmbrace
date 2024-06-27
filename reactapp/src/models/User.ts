import { UserSettings } from "./UserSettings";

export interface User {
  id: string;
  displayId: number;
  fullName: string;
  title: string;
  department: string;
  role: string;
  phoneNumber: string;
  email: string;
  settings?: UserSettings;
}
