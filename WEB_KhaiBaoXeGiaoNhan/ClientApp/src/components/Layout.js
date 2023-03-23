import React, { useContext } from "react";
import { Container } from "reactstrap";
import NavMenu from "./NavBar/NavMenu";
import NavMenuDieuPhoi from "./NavBar/NavMenuDieuPhoi";
import NavMenuAnalysis from "./NavBar/NavMenuAnalysis";
import AuthContext from "../store/auth-context";
import NavMenuCus from "./NavBar/NavMenuCus";

const Layout = (props) => {
  const authCtx = useContext(AuthContext);
  console.log(authCtx)

  return (
    <React.Fragment>
      {authCtx.isLoggedIn && authCtx.userType === 'Corrdinator' && <NavMenuDieuPhoi />}
      {authCtx.isLoggedIn && authCtx.userType === 'Master' && <NavMenuAnalysis />}
      {authCtx.isLoggedIn && authCtx.userType === 'Provider' && <NavMenu />}
      {authCtx.isLoggedIn && authCtx.userType === 'Customer' && <NavMenuCus />}
      
      {authCtx.isLoggedIn && <>
        <Container style={{ minHeight: "80vh" }}>{props.children}</Container>
        <Container>
          <div
            style={{
              textAlign: "center",
              bottom: "0",
              marginTop: "50px",
              paddingBottom: "30px",
            }}
          >
            <p>Bản quyền thuộc VAS Group © {new Date().getFullYear()}</p>
          </div>
        </Container></>
      }
      {!authCtx.isLoggedIn && props.children}
    </React.Fragment >
  );
};

export default Layout;
