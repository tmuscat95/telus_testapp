import React, { useState } from "react";
import { Form, Button, FormGroup, Label, Input } from "reactstrap";
import { Redirect } from "react-router";

const Login = ({onSubmitHandler,error,redirect}) => {
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");

  const onChangeUsername = (e) => {
    setUsername(e.target.value);
  };

  const onChangePassword = (e) => {
    setPassword(e.target.value);
  };


  if (redirect) return <Redirect to="/common" />;
  return (
    <div>
      <Form onSubmit={(e)=>{e.preventDefault(); onSubmitHandler(username,password);}}>
        <FormGroup>
          <Label for="username">Username</Label>
          <Input
            onChange={onChangeUsername}
            value={username}
            id="username"
            name="username"
            placeholder="username"
            type="text"
          />
        </FormGroup>

        <FormGroup>
          <Label for="password">Password</Label>
          <Input
            onChange={onChangePassword}
            value={password}
            id="password"
            name="password"
            placeholder="password"
            type="password"
          />
        </FormGroup>
        <Button variant="primary" type="submit">
          Log In
        </Button>
      </Form>
      
      
    </div>
  );
};

export default Login;
