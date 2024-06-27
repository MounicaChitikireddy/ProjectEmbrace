import { Device } from "./Device";

export interface Batch {
  additions: Device[];
  deletions: Device[];
}
