import React from "react";
const HeaderPage = (props) => {
  return (
    <React.Fragment>
      <h2 id="tabelLabel">{props.title}</h2>
      <p>{props.description}</p>
      <br />
      <br />
    </React.Fragment>
  );
};
export default HeaderPage;
