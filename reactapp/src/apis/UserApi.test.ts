import { assertType } from "vitest";
import { describe, it } from "vitest";
import { User } from "../models/User";
import { UserAPI } from "./UserApi";

describe("User API", () => {
  it("Read User Data", () => {
    UserAPI.getAll().then((users) => {
      assertType<User[]>(users);
      expect(users.length).toBeGreaterThan(0);
    });
  });
});
