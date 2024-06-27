import { Device } from "../models/Device";
import { DeviceCount } from "../models/DeviceCount";
import { GradeCount } from "../models/GradeCount";
import { LocationCount } from "../models/LocationCount";
import { Batch } from "../models/Batch";
import { DeviceGradeUpdate } from "../models/DeviceGradeUpdate";

export interface DeviceAPIResponse {
    displayMessage: string;
    isSuccess: boolean;
    isRefetching: boolean;
}

const baseApiUrl = "/api/currentdevices";

export const DeviceAPI = {
    getAll: async function (): Promise<Device[]> {
        const response = await fetch(`${baseApiUrl}/GetCurrentDevices`);
        return await response.json();
    },

    getDeviceCount: async function(): Promise<DeviceCount[]> {
        const response = await fetch(`${baseApiUrl}/GetDeviceCount`);
        return await response.json();
    },

    getGroupedDeviceCount: async function (): Promise<DeviceCount[]> {
        const response = await fetch(`${baseApiUrl}/GetGroupedDeviceCount`);
        return await response.json();
    },

    getGradeCount: async function (): Promise<GradeCount[]> {
        const response = await fetch(`${baseApiUrl}/GetGradeCount`);
        return await response.json();
    },

    getLocationCount: async function (): Promise<LocationCount[]> {
        const response = await fetch(`${baseApiUrl}/GetLocationCount`);
        return await response.json();
    },

    getDeviceByCampaign: async function (campaignId: string): Promise<DeviceCount[]> {
        const response = await fetch(`${baseApiUrl}/GetDeviceByCampaign/${campaignId}`);
        return await response.json();
    },

    getGroupedDeviceByCampaign: async function (campaignId: string): Promise<DeviceCount[]> {
        const response = await fetch(`${baseApiUrl}/GetGroupedDeviceByCampaign/${campaignId}`);
        return await response.json();
    },

    getGradeCountByCampaign: async function (campaignId: string): Promise<GradeCount[]> {
        const response = await fetch(`${baseApiUrl}/GetGradeCountByCampaign/${campaignId}`)
        return await response.json();
    },

    update: async function (batch: Batch): Promise<DeviceAPIResponse> {
        const response = await fetch(`${baseApiUrl}/UpdateCurrentDevices`, {
            method: "PUT",
            headers: {
                "Content-Type": "application/json",
            },
            body: JSON.stringify(batch),
        });
        return await handleResponse(response, "Devices Updated Successfully!");
    },
    updateGrade: async function (
        updates: Array<DeviceGradeUpdate>
    ): Promise<DeviceAPIResponse> {
        const response = await fetch(`${baseApiUrl}/UpdateDeviceGrades`, {
            method: "PUT",
            headers: {
                "Content-Type": "application/json",
            },
            body: JSON.stringify(updates),
        });
        return await handleResponse(
            response,
            "Device Grades Updated Successfully!"
        );
    },
};

const handleResponse = async (
    response: Response,
    successMessage: string
): Promise<DeviceAPIResponse> => {
    if (!response.ok) {
        const errorMessage = await response.text();
        return {
            displayMessage: errorMessage,
            isSuccess: false,
            isRefetching: false,
        };
    } else {
        return {
            displayMessage: successMessage,
            isSuccess: true,
            isRefetching: true,
        };
    }
};
