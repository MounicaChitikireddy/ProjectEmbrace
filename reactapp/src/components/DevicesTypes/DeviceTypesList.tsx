import { Container } from "@mui/material";
import { Component } from "react";
import Header from "../global/Header.js";
import DeviceTypeListTable from "../Table/DeviceTypeTable/DeviceTypeListTable.js";

export default class DeviceTypesList extends Component {
  render() {
    return (
      <>
        <Header title="Device list" />
        <div className="page-container">
          <Container maxWidth="xl">
            <DeviceTypeListTable />
          </Container>
        </div>
      </>
    );
  }
}
