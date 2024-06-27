import { TableDensity } from "../enums/TableDensity.enum";

export interface UserSettings {
  tableDensity: TableDensity; // Default: "comfortable"
  addInventoryDefaultGrade: string; // Default: "B-FRN"
  removeInventoryDefaultGrade: string; // Default: "B-FRN"
  tableFullScreenByDefault: boolean; // Default: false
  avatarColor: string; // Default: ""
  avatarText: string; // Default: ""
}
