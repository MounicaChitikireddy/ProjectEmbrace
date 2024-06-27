import { Component } from "react";
import "./UserDisplay.scss";
import UserDisplay from "./UserDisplay.js";

export default class ProfilePage extends Component {

    render() {
        return (
            <>
                <div className="page-container">
                    <UserDisplay />
                </div>
            </>
        );
    }
}
