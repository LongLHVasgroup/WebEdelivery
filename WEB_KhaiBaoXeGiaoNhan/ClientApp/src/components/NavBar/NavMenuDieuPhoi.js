import React, { useContext } from "react";
import {
  Collapse,
  Container,
  Navbar,
  NavItem,
  NavLink,
} from "reactstrap";
import AuthContext from "../../store/auth-context";
import { Link } from "react-router-dom";
import BrandHeader from "../BrandHeader";
import "./NavMenu.css";

const NavMenuDieuPhoi = () => {
  const authCtx = useContext(AuthContext);
  const handleLogout = () => {
    authCtx.logout();
  };
  return (
    <header>
      <Navbar
        className="navbar-expand-sm navbar-toggleable-sm ng-white border-bottom box-shadow mb-3"
        light
      >
        <Container>
          <BrandHeader />
          <Collapse
            className="d-sm-inline-flex flex-sm-row-reverse"
            // isOpen={!this.state.collapsed}
            navbar
          >
            <ul className="navbar-nav flex-grow">
              <NavItem>
                <NavLink tag={Link} to="/dieu-phoi-po">
                  Điều phối đơn hàng
                </NavLink>
              </NavItem>
              <NavItem>
                <NavLink tag={Link} to="/lich-su-dieu-phoi">
                  Lịch sử điều phối
                </NavLink>
              </NavItem>

              <NavItem>
                <NavLink tag={Link} to="/bao-cao/nhap-hang">
                  Nhập hàng
                </NavLink>
              </NavItem>
              <NavItem>
                <NavLink tag={Link} to="/bao-cao/tinh-hinh-giao-hang">
                  Tình hình giao hàng
                </NavLink>
              </NavItem>
              <NavItem>
                <NavLink tag={Link} to="/bao-cao/tinh-hinh-dieu-phoi">
                  Tình hình điều phối
                </NavLink>
              </NavItem>
              <NavItem>
                <NavLink tag={Link} to="/account">
                  Tài khoản
                </NavLink>
              </NavItem>
              <NavItem>
                <NavLink onClick={handleLogout}>Đăng xuất</NavLink>
              </NavItem>
            </ul>
          </Collapse>
        </Container>
      </Navbar>
    </header>
  );
};
export default NavMenuDieuPhoi;
