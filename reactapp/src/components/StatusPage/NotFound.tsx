import { Container } from "@mui/material";
import { Component } from "react";

export default class NotFound extends Component {
  render() {
    return (
      <>
        <div className="page-container">
          <Container maxWidth="xl">404 Not Found</Container>
        </div>
      </>
    );
  }
}
