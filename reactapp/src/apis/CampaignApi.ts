import { Campaign } from "../models/Campaign";

export interface CampaignAPIResponse {
  displayMessage: string;
  isSuccess: boolean;
  isRefetching: boolean;
}

const baseApiUrl = "/api/campaigns";

export const CampaignAPI = {
  create: async function (Campaign: Campaign): Promise<CampaignAPIResponse> {
    const response = await fetch(`${baseApiUrl}/Create`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(Campaign),
    });
    return await handleResponse(response);
  },
  createMultiple: async function (
    campaigns: Campaign[]
  ): Promise<CampaignAPIResponse> {
    const response = await fetch(`${baseApiUrl}/ListImport`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(campaigns),
    });
    return await handleResponse(response);
  },
  getAll: async function (): Promise<Campaign[]> {
    const response = await fetch(`${baseApiUrl}/GetAll`);
    return await response.json();
  },
  update: async function (Campaign: Campaign): Promise<CampaignAPIResponse> {
    const response = await fetch(`${baseApiUrl}/Edit/${Campaign.campaignId}`, {
      method: "PUT",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(Campaign),
    });
    return await handleResponse(response);
  },
  delete: async function (CampaignId: number): Promise<CampaignAPIResponse> {
    const response = await fetch(`${baseApiUrl}/Delete/${CampaignId}`, {
      method: "DELETE",
    });
    return await handleResponse(response);
  },
  deleteMultiple: async function (
    CampaignIds: number[]
  ): Promise<CampaignAPIResponse> {
    const response = await fetch(`${baseApiUrl}/DeleteMultiple`, {
      method: "DELETE",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(CampaignIds),
    });
    return await handleResponse(response);
  },
};

const handleResponse = async (
  response: Response
): Promise<CampaignAPIResponse> => {
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
