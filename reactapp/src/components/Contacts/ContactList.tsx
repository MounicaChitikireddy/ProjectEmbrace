import { Container } from "@mui/material";
import { Component } from "react";
import Header from "../global/Header.js";
import ContactListTable from "../Table/ContactListTable/ContactListTable.js";

export default class ContactList extends Component {
  render() {
    return (
      <>
        <Header title="Contact list" />
        <div className="page-container">
          <Container maxWidth="xl">
            <ContactListTable />
          </Container>
        </div>
      </>
    );
  }
}
