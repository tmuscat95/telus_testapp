import React, { Component } from "react";
import { Route } from "react-router-dom";
import { Layout } from "./components/Layout";
import Common from "./components/Common";
import Login from "./components/Login";
import "./custom.css";

export default class App extends Component {
  static displayName = App.name;
  state = {loggedIn: false,loggedInUser:""};

  async checkLoggedIn() {
    try {
      const response = await fetch("/api/auth/user");
      if (response.ok) {
        let json = await response.json();
        this.setState({ loggedIn: true,loggedInUser:json["username"] });
      }
    } catch (error) {
      console.log(error);
    }
  }

  async componentDidMount() {
    await this.checkLoggedIn();
  }

  onSubmitHandler = async (username, password) => {
    try {
      let response = await fetch("api/auth", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        credentials: "include", //When set to include, allows cookies to be set.
        body: JSON.stringify({
          username: username,
          password: password,
        }),
      });

      if (!response.ok) {
        console.log("Error: " + response.status);
        alert("Login Failed");
        
        return;
      }

      this.setState({error:false ,loggedIn: true, loggedInUser:username });
    } catch (error) {
      console.log(error);
      
    }
  };

  render() {
    return (
      <Layout>
        <Route
          exact
          path="/"
          component={() => (
            <Login
              onSubmitHandler={this.onSubmitHandler}
              redirect={this.state.loggedIn}
              loggedInUser={this.state.loggedInUser}
            />
          )}
        />
        <Route exact path="/common" component={()=><Common loggedInUser={this.state.loggedInUser}/>} />
      </Layout>
    );
  }
}
