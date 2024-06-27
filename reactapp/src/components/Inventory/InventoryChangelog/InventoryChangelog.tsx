import { Container } from "@mui/material";
import { Component } from "react";
import Header from "../../global/Header";
import ChangelogTable from "../../Table/ChangelogTable/ChangelogTable";

export default class InventoryChangelog extends Component {
  render() {
    return (
      <>
        <Header title="Inventory Changelog" />
        <div className="page-container">
          <Container maxWidth="xl">
            <ChangelogTable />
          </Container>
        </div>
      </>
    );
  }
}
