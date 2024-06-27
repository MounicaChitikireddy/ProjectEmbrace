import { Container } from "@mui/material";
import { Component } from "react";
import UserListTable from "../Table/UserListTable/UserListTable";
import Header from "../global/Header.js";

export default class UserList extends Component {
  render() {
    return (
      <>
        <Header title="User list" />
        <div className="page-container">
          <Container maxWidth="xl">
            <UserListTable />
          </Container>
        </div>
      </>
    );
  }
}
