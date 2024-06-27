import { Component, lazy } from "react";
import "../../assets/scss/Header.scss";
import { Link } from "react-router-dom";
import { red } from "@mui/material/colors";
import {
  Breadcrumbs,
  Typography,
  AppBar,
  Box,
  Toolbar,
  Avatar,
} from "@mui/material";
import { AuthenticationAPI } from "../../apis/AuthenticationApi.js";
import { User } from "../../models/User";

const Sidebar = lazy(() => import("./Sidebar.js") as any);

const logout = () => {
  localStorage.removeItem("accountData");
  AuthenticationAPI.logout();
};

export default class Header extends Component<{ title: string }> {
  render() {
    const accountData: User = JSON.parse(
      localStorage.getItem("accountData") || "{}"
    );

    const avatarColor = accountData.settings?.avatarColor;
    const avatarText = accountData.settings?.avatarText;

    const renderProfileDropdown = () => {
      return (
        <div className="profile-dropdown">
          <Avatar
            className="drop-button"
            sx={{ bgcolor: avatarColor ? avatarColor : red[800] }}
          >
            {avatarText ? avatarText : null}
          </Avatar>
          <div className="dropdown-content">
            <a href="/profile">Profile</a>
            <a onClick={logout} href="/login">
              Logout
            </a>
          </div>
        </div>
      );
    };

    return (
      <>
        <Box sx={{ flexGrow: 1 }}>
          <AppBar position="static">
            <Toolbar>
              <Sidebar />
              <Typography variant="h6" component="div" sx={{ flexGrow: 1 }}>
                Project Embrace
              </Typography>
              <span className="role">Role: {accountData.role}</span>
              {renderProfileDropdown()}
              <a href="/login"></a>
            </Toolbar>
          </AppBar>
        </Box>
        <div className="page-heading">
          <Typography variant="h1">{this.props.title}</Typography>
        </div>
      </>
    );
  }
}
