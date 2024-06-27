import { Component, Suspense, lazy } from "react";
import { Container, Backdrop, CircularProgress } from "@mui/material";
import Header from "./components/global/Header.js";
import "./assets/scss/Global.scss";
import "./assets/scss/Home.scss";
import {
    createBrowserRouter,
    createRoutesFromElements,
    Outlet,
    Route,
} from "react-router-dom";
import NotFound from "./components/StatusPage/NotFound.js";
import ProtectedRoutes from "./components/Shared/ProtectedRoute.js";

const ForgotPassword = lazy(
    () =>
        import(
            "./components/AuthenticationFlow/ForgotPassword/ForgotPassword.js"
        ) as any
);
const ResetPassword = lazy(
    () =>
        import(
            "./components/AuthenticationFlow/ResetPassword/ResetPassword.js"
        ) as any
);
const DeviceTypesList = lazy(
    () => import("./components/DevicesTypes/DeviceTypesList.js") as any
);
const SignIn = lazy(
    () => import("./components/AuthenticationFlow/LoginPage/LoginForm.js") as any
);
const UserList = lazy(() => import("./components/Users/UserList.js") as any);
const ContactList = lazy(
    () => import("./components/Contacts/ContactList.js") as any
);
const CampaignList = lazy(
    () => import("./components/Campaigns/CampaignList.js") as any
);
const InventoryTable = lazy(
    () => import("./components/Table/InventoryTable/InventoryTable.js") as any
);
const InventoryAdd = lazy(
    () => import("./components/Inventory/InventoryAdd/InventoryAdd.js") as any
);
const InventoryRemove = lazy(
    () =>
        import("./components/Inventory/InventoryRemove/InventoryRemove.js") as any
);
const InventoryChangelog = lazy(
    () =>
        import(
            "./components/Inventory/InventoryChangelog/InventoryChangelog.js"
        ) as any
);
const ProfilePage = lazy(
    () =>
        import(
            "./components/ProfilePage/Profile.js"
        ) as any
);

const AnalyticPage = lazy(
    () =>
        import(
            "./components/Analytics/Analytics.js"
        ) as any
);

export default class App extends Component {
    render() {
        return (
            <>
                <Header title={"Inventory"} />
                <div className="page-container">
                    <Container maxWidth="xl">
                        <InventoryTable />
                    </Container>
                </div>
            </>
        );
    }
}

const LazyLoadRoutes = () => {
    return (
        <Suspense
            fallback={
                <Backdrop
                    sx={{
                        color: "#fff",
                        zIndex: (theme) => theme.zIndex.drawer + 1,
                    }}
                    open={true}
                >
                    <CircularProgress color="inherit" />
                </Backdrop>
            }
        >
            <Outlet />
        </Suspense>
    );
};
export const router = createBrowserRouter(
    createRoutesFromElements(
        <>
            <Route element={<ProtectedRoutes />}>
                <Route path="/" element={<App />} />
                <Route path="/inventory/add" element={<InventoryAdd />} />
                <Route path="/inventory/remove" element={<InventoryRemove />} />
                <Route path="/inventory/changelog" element={<InventoryChangelog />} />
                <Route path="/users/list" element={<UserList />} />
                <Route path="/contact/list" element={<ContactList />} />
                <Route path="/campaign/list" element={<CampaignList />} />
                <Route path="/device/list" element={<DeviceTypesList />} />
                <Route path="/profile" element={<ProfilePage />} />
                <Route path="/analytics" element={<AnalyticPage /> } />
            </Route>
            <Route element={<LazyLoadRoutes />}>
                <Route path="/login" element={<SignIn />} />
                <Route path="/forgot-password" element={<ForgotPassword />} />
                <Route path="/reset-password" element={<ResetPassword />} />
            </Route>
            <Route path="*" element={<NotFound />}></Route>
        </>
    )
);
