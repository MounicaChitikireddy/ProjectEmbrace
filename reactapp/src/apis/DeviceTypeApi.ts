import { DeviceType } from "../models/DeviceType";

export interface DeviceTypeAPIResponse {
  displayMessage: string;
  isSuccess: boolean;
  isRefetching: boolean;
}

const baseApiUrl = "/api/DeviceTypes/";

export const DeviceTypeAPI = {
  create: async function (device: DeviceType): Promise<DeviceTypeAPIResponse> {
    const response = await fetch(`${baseApiUrl}/CreateDeviceType`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(device),
    });
    return await handleResponse(response);
  },
  createMultiple: async function (
    deviceTypes: DeviceType[]
  ): Promise<DeviceTypeAPIResponse> {
    const response = await fetch(`${baseApiUrl}/AddManyDeviceType`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(deviceTypes),
    });
    return await handleResponse(response);
  },
  getAll: async function (): Promise<DeviceType[]> {
    const response = await fetch(`${baseApiUrl}`);
    return await response.json();
  },
  update: async function (device: DeviceType): Promise<DeviceTypeAPIResponse> {
    const response = await fetch(`${baseApiUrl}EditDeviceType`, {
      method: "PUT",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(device),
    });
    return await handleResponse(response);
  },
  delete: async function (deviceId: number): Promise<DeviceTypeAPIResponse> {
    const response = await fetch(`${baseApiUrl}${deviceId}`, {
      method: "DELETE",
    });
    return await handleResponse(response);
  },
  deleteMultiple: async function (
    deviceIds: number[]
  ): Promise<DeviceTypeAPIResponse> {
    const response = await fetch(`${baseApiUrl}/DeleteManyDeviceType`, {
      method: "DELETE",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(deviceIds),
    });
    return await handleResponse(response);
  },
};

const handleResponse = async (
  response: Response
): Promise<DeviceTypeAPIResponse> => {
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
