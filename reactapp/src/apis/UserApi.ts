import { User } from "../models/User";
import { UserSettings } from "../models/UserSettings";
export interface UserAPIResponse {
  displayMessage: string;
  isSuccess: boolean;
  isRefetching: boolean;
}

const baseApiUrl = `/api/users`;

export const UserAPI = {
  updateByEmail: async function (user: User): Promise<UserAPIResponse> {
    const response = await fetch(`${baseApiUrl}/UpdateByEmail/${user?.email}`, {
      method: "PUT",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(user),
    });
    return await handleResponse(response);
  },
  getAll: async function (): Promise<User[]> {
    const response = await fetch(`${baseApiUrl}/GetAll`);
    return await response.json();
  },
  getCurrentUser: async function (): Promise<User> {
    const response = await fetch(`${baseApiUrl}/GetCurrentUser`);
    return await response.json();
  },
  update: async function (user: User): Promise<UserAPIResponse> {
    const response = await fetch(`${baseApiUrl}/Update/${user.id}`, {
      method: "PUT",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(user),
    });
    return await handleResponse(response);
  },
  updateSettings: async function (
    settings: UserSettings
  ): Promise<UserAPIResponse> {
    const response = await fetch(`${baseApiUrl}/UpdateUserSettings`, {
      method: "PUT",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(settings),
    });
    return await handleResponse(response);
  },
  delete: async function (userId: number): Promise<UserAPIResponse> {
    const response = await fetch(`${baseApiUrl}/Delete/${userId}`, {
      method: "DELETE",
    });
    return await handleResponse(response);
  },
  deleteMultiple: async function (userIds: number[]): Promise<UserAPIResponse> {
    const response = await fetch(`${baseApiUrl}/DeleteMultiple`, {
      method: "DELETE",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(userIds),
    });
    return await handleResponse(response);
  },
};

const handleResponse = async (response: Response): Promise<UserAPIResponse> => {
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
