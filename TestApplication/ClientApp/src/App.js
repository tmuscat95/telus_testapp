import React, { Component } from "react";
import { Route } from "react-router-dom";
import { Layout } from "./components/Layout";
import Common from "./components/Common";
import Login from "./components/Login";
import "./custom.css";

export default class App extends Component {
  static displayName = App.name;

  async checkLoggedIn() {
    try {
      const response = await fetch("/api/auth/user");
      if (response.ok) {
        let json = await response.json();
        this.setState({ error: false, loggedIn: true,loggedInUser:json["username"] });
      }
    } catch (error) {
      console.log(error);
    }
  }

  async componentWillMount() {
    this.setState({ error: false, loggedIn: false,loggedInUser:"" });
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
        this.setState({ error: true });
        return;
      }

      this.setState({error:false ,loggedIn: true, loggedInUser:username });
    } catch (error) {
      console.log(error);
      this.setState({ error: true });
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
              error={this.state.error}
              redirect={this.state.loggedIn}
              loggedInUser={this.state.loggedInUser}
            />
          )}
        />
        <Route exact path="/common" component={()=><Common loggedInUser={this.state.loggedInUser}/>} />
        {/*<Route path='/common/:id' component={CommonTable}/>*/}
      </Layout>
    );
  }
}
