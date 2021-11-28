import React from "react";
import { useState, useEffect } from "react";
import { Link } from "react-router-dom";
import { Table, Button } from "reactstrap";

export default function Common({ loggedInUser }) {
  //const [loading, setLoading] = useState(true);
  const [monitorData, setMonitorData] = useState([]);
  const [loading, setLoading] = useState(true);
  const [refreshCount, setRefreshCount] = useState(0);

  const doLogout = async () => {
    try {
      let response = await fetch("api/auth/logout", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        credentials: "include", //When set to include, allows cookies to be set.
      });

      if (!response.ok) {
        console.log("Error: " + response.status);
        return;
      }

      window.location.href = "/";
    } catch (error) {
      console.log(error);
    }
  };

  useEffect(() => {
    const populateMonitorData = async () => {
      const response = await fetch("api/data");
      try {
        const data = await response.json();
        if (data["title"] == "Unauthorized") {
          window.location.href = "/";
        }
        setMonitorData(data);
        setLoading(false);
      } catch (e) {
        console.log(e);
      }
    };

    populateMonitorData();
    setInterval(() => {
      populateMonitorData();
      setRefreshCount((prevCount) => prevCount + 1);
      console.log("Refreshing");
    }, 10000);
  }, []);

  return loading ? (
    <h4>Loading...</h4>
  ) : (
    <React.Fragment>
      <h4>User: {loggedInUser}</h4>
      <Table bordered>
        <thead>
          <th>Queue Group</th>
          <th>Offered</th>
          <th>Handled</th>
          <th>Talk Time</th>
          <th>Average Talk Time</th>
          <th>Handling Time</th>
          <th>Average Handling Time</th>
          <th>Service Level</th>
        </thead>
        <tbody>
          {monitorData.map((d) => {
            const hhmmss = (ms) => new Date(ms * 1000).toISOString().substr(11, 8);

            let sla = d["handledWithinSL"] / d["offered"];
            let min_sla = d.queueGroup["slA_Percent"];
            let sl_colour = sla * 100 < min_sla ? "red" : "green";
            
            let tt = hhmmss(d["talkTime"]);
            let att = hhmmss(d["talkTime"] / d["handled"]);
            let ht = d["talkTime"] + d["afterCallWorkTime"];
            let aht = hhmmss(ht / d["handled"]);
            ht = hhmmss(d["talkTime"] + d["afterCallWorkTime"]);

            return (
              <tr key={d["queueGroupID"]}>
                <td>{d["queueGroup"]["name"]}</td>
                <td>{d["offered"]}</td>
                <td>{d["handled"]}</td>
                <td>{tt}</td>
                <td>{att}</td>
                <td>{ht}</td>
                <td>{aht}</td>
                <td style={{ color: sl_colour }}>{(sla * 100).toFixed(1)} %</td>
              </tr>
            );
          })}
        </tbody>
      </Table>
      <h3>Refreshed: {refreshCount} times.</h3>
      <div>
        <Button onClick={doLogout}>Log Out</Button>
      </div>
    </React.Fragment>
  );
  /*
    ///Test
    useEffect(()=>{
        setLoading(false);
        setQgroups([
        {
          "ID": 1,
          "Name": "Audi",
          "SLA_Percent": 80,
          "SLA_Time": 20
        },
        {
          "ID": 2,
          "Name": "VW",
          "SLA_Percent": 90,
          "SLA_Time": 60
        },
        {
          "ID": 3,
          "Name": "Ford",
          "SLA_Percent": 80,
          "SLA_Time": 20
        },
        {
          "ID": 4,
          "Name": "Mercedes",
          "SLA_Percent": 80,
          "SLA_Time": 20
        }
      ])},[])
      */
  /////
  /*
      
    return (
        <div>
            <ul>
                {Qgroups.length>0 && Qgroups.map((q)=>{
                    return  <li id={q["id"]}><Link to={`/common/${q["id"]}`}>{q["name"]}</Link></li>
                })}
            </ul>
        </div>
    )*/
}
