export const AuthenticationAPI = {
  login: async function (email: string, password: string) {
    const response = await fetch(
      `/login?useCookies=true&useSessionCookies=true`,
      {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify({
          email: email,
          password: password,
        }),
      }
    );
    return await response;
  },
  logout: () => {
    return fetch(`/logout`, {
      method: "POST",
    });
  },
  register: (email: string, password: string) => {
    return fetch(`/register`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify({
        email: email,
        password: password,
      }),
    });
  },
  forgotPassword: async (email: string) => {
    return await fetch(`/forgotPassword`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify({
        email,
      }),
    });
  },
  resetPassword: async (data: any) => {
    return await fetch(`/resetPassword`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(data),
    });
  },
  getRoles: async function () {
    const response = await fetch(`/api/Role`, {
      method: "GET",
      headers: {
        "Content-Type": "application/json",
      },
    });
    return await response.json();
  },
};
