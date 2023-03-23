import React from "react";
import { Link } from "react-router-dom";
import { CardImg, NavbarBrand } from "reactstrap";
const BrandHeader = () => {
  return (
    <NavbarBrand tag={Link} to="/">
      <CardImg
        right="true"
        style={{
          width: "90px",
          padding: "5px",
          marginTop: "-10px",
          marginBottom: "-10px",
        }}
        src="img/ic_logo.svg"
        alt="Vassteel"
      />
    </NavbarBrand>
  );
};

export default BrandHeader;
