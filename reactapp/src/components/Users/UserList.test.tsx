import { render, screen } from "@testing-library/react";
import { describe, it } from "vitest";
import UserListTable from "../Table/UserListTable/UserListTable";

describe("User API", () => {
  it("Should Render Table", () => {
    render(<UserListTable />);
    expect(screen.getByText("Create User")).toBeInTheDocument();
  });
});
