import { Container } from "@mui/material";
import { Component } from "react";
import Header from "../global/Header.js";
import CampaignListTable from "../Table/CampaignListTable/CampaignListTable.js";

export default class CampaignList extends Component {
  render() {
    return (
      <>
        <Header title="Campaign list" />
        <div className="page-container">
          <Container maxWidth="xl">
            <CampaignListTable />
          </Container>
        </div>
      </>
    );
  }
}
