import { CircularProgress } from "@mui/material";
import Backdrop from "@mui/material/Backdrop";
import { Suspense } from "react";
import { Navigate, Outlet, useLocation } from "react-router-dom";
import { User } from "../../models/User";

const SuspenseOutlet = () => {
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

export function hasRequiredRoleForPage(
    role: string,
    pageName: string
): boolean {
    let requiredRoles: string[] = [];
    switch (pageName) {
        case "/":
            requiredRoles = ["Admin", "Reviewer", "Employee"];
            break;
        case "/inventory/add":
            requiredRoles = ["Admin", "Reviewer", "Employee"];
            break;
        case "/inventory/remove":
            requiredRoles = ["Admin", "Reviewer", "Employee"];
            break;
        case "/inventory/changelog":
            requiredRoles = ["Admin", "Reviewer"];
            break;
        case "/users/list":
            requiredRoles = ["Admin"];
            break;
        case "/contact/list":
            requiredRoles = ["Admin", "Reviewer", "Employee"];
            break;
        case "/campaign/list":
            requiredRoles = ["Admin", "Reviewer"];
            break;
        case "/device/list":
            requiredRoles = ["Admin", "Reviewer"];
            break;
        case "/profile":
            requiredRoles = ["Admin", "Reviewer", "Employee"];
            break;
        case "/analytics":
            requiredRoles = ["Admin", "Reviewer", "Employee"];
            break;
        default:
            break;
    }
    if (requiredRoles && !requiredRoles.includes(role)) {
        return false;
    }
    return true;
}

function ProtectedRoutes() {
    const accountData: User = JSON.parse(
        localStorage.getItem("accountData") || "{}"
    );
    const targetPage = useLocation().pathname;

    if (!accountData?.id) {
        return <Navigate to="/login" />;
    } else if (hasRequiredRoleForPage(accountData?.role, targetPage)) {
        return <SuspenseOutlet />;
    } else {
        return <Navigate to="/not-found" />;
    }
}

export default ProtectedRoutes;
