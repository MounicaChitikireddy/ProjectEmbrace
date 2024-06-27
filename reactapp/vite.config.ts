/// <reference types="vitest" />
/// <reference types="vite/client" />
import { defineConfig } from "vite";
import { fileURLToPath, URL } from "node:url";
import plugin from "@vitejs/plugin-react";
import tsconfigPaths from "vite-tsconfig-paths";
import fs from "fs";
import path from "path";

const baseFolder =
  process.env.APPDATA !== undefined && process.env.APPDATA !== ""
    ? `${process.env.APPDATA}/ASP.NET/https`
    : `${process.env.HOME}/.aspnet/https`;

const certificateArg = process.argv
  .map((arg) => arg.match(/--name=(?<value>.+)/i))
  .filter(Boolean)[0];
const certificateName = certificateArg
  ? certificateArg?.groups?.value
  : "reactapp";

if (!certificateName) {
  console.error(
    "Invalid certificate name. Run this script in the context of an npm/yarn script or pass --name=<<app>> explicitly."
  );
  process.exit(-1);
}

const certFilePath = path.join(baseFolder, `${certificateName}.pem`);
const keyFilePath = path.join(baseFolder, `${certificateName}.key`);

// https://vitejs.dev/config/
export default defineConfig({
  test: {
    globals: true,
    environment: "jsdom",
    setupFiles: ["./src/setupTests.ts"],
    browser: {
      enabled: true,
      name: "chrome", // browser name is required
    },
  },
  plugins: [plugin(), tsconfigPaths()],
  resolve: {
    alias: {
      "@": fileURLToPath(new URL("./src", import.meta.url)),
    },
  },
  server: {
    proxy: {
      "^/api/users": {
        target: "https://localhost:7166/",
        secure: false,
      },
      "^/api/contacts": {
        target: "https://localhost:7166/",
        secure: false,
      },
      "^/api/currentdevices": {
        target: "https://localhost:7166/",
        secure: false,
      },
      "^/api/campaigns": {
        target: "https://localhost:7166/",
        secure: false,
      },
      "^/api/Role": {
        target: "https://localhost:7166/",
        secure: false,
      },
      "^/api/DeviceTypes": {
        target: "https://localhost:7166/",
        secure: false,
      },
      "^/api/ChangelogBatches": {
        target: "https://localhost:7166/",
        secure: false,
      },
      "^/api/Receipt": {
        target: "https://localhost:7166/",
        secure: false,
      },
      "/login?useCookies=true&useSessionCookies=true": {
        target: "https://localhost:7166/",
        changeOrigin: true,
        secure: false,
        rewrite: (path) =>
          path.replace(/^\/login?useCookies=true&useSessionCookies=true/, ""),
      },
      "/register": {
        target: "https://localhost:7166/",
        changeOrigin: true,
        secure: false,
        rewrite: (path) =>
          path.replace(/^\/login?useCookies=true&useSessionCookies=true/, ""),
      },
      "/logout": {
        target: "https://localhost:7166/",
        changeOrigin: true,
        secure: false,
        rewrite: (path) =>
          path.replace(/^\/login?useCookies=true&useSessionCookies=true/, ""),
      },
      "/forgotPassword": {
        target: "https://localhost:7166/",
        changeOrigin: true,
        secure: false,
        rewrite: (path) =>
          path.replace(/^\/login?useCookies=true&useSessionCookies=true/, ""),
      },
      "/resetPassword": {
        target: "https://localhost:7166/",
        changeOrigin: true,
        secure: false,
        rewrite: (path) =>
          path.replace(/^\/login?useCookies=true&useSessionCookies=true/, ""),
      },
    },
    port: 5173,
    https: {
      key: fs.readFileSync(keyFilePath),
      cert: fs.readFileSync(certFilePath),
    },
  },
} as any);
