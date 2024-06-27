export interface InventoryChange {
  id: number;
  action: number;
  reviewStatus: number;
  createdOn: Date;
  createdBy: string;
  approvedBy: string;
  approvedOn: Date;
}
