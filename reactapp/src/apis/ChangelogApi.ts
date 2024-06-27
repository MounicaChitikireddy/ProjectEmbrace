import { InventoryChange } from "../models/InventoryChange";

export interface ChangelogAPIResponse {
  displayMessage: string;
  isSuccess: boolean;
  isRefetching: boolean;
}

const baseApiUrl = "/api/ChangelogBatches";

export const ChangelogAPI = {
  getAll: async function (): Promise<InventoryChange[]> {
    const response = await fetch(`${baseApiUrl}/GetBatches`);
    return await response.json();
  },
  getById: async function (id: number): Promise<any> {
    const response = await fetch(`${baseApiUrl}/GetBatchDetails?batchId=${id}`);
    return await response.json();
  },
  update: async function (updates: any): Promise<ChangelogAPIResponse> {
    const response = await fetch(`${baseApiUrl}/EditBatch`, {
      method: "PUT",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(updates),
    });
    return await handleResponse(response);
  },
  sendReceipt: async function (
    receiptInfo: any
  ): Promise<ChangelogAPIResponse> {
    const response = await fetch(`/api/Receipt/SendReceipt`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(receiptInfo),
    });
    return await handleResponse(response);
  },
  changeApprovalStatus: async function (
    id: number,
    status: string
  ): Promise<ChangelogAPIResponse> {
    let statusNum = 0;
    switch (status) {
      case "Approve":
        statusNum = 1;
        break;
      case "Deny":
        statusNum = 2;
        break;
      default:
        break;
    }
    const response = await fetch(
      `${baseApiUrl}/SetBatchStatus?batchId=${id}&status=${statusNum}`,
      {
        method: "PUT",
      }
    );
    return await handleResponse(response);
  },
};

const handleResponse = async (
  response: Response
): Promise<ChangelogAPIResponse> => {
  const displayMessage = await response.text();
  if (!response.ok) {
    return {
      displayMessage: displayMessage,
      isSuccess: false,
      isRefetching: false,
    };
  } else {
    return {
      displayMessage: displayMessage,
      isSuccess: true,
      isRefetching: true,
    };
  }
};
