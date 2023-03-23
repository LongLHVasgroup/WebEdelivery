import React, { useContext, useState } from "react";
import {
  Collapse,
  Container,
  Navbar,
  NavbarToggler,
  NavItem,
  NavLink,
  Dropdown,
  DropdownToggle,
  DropdownItem,
  DropdownMenu,
} from "reactstrap";
import { Link } from "react-router-dom";
import BrandHeader from "../BrandHeader";
import "./NavMenu.css";
import AuthContext from "../../store/auth-context";
const NavMenu = () => {
  const authCtx = useContext(AuthContext);
  const [isShowDropDownGiaoNhan, setIsShowDropDownGiaoNhan] = useState(false);
  const [isShowDropDownDanhMuc, setIsShowDropDownDanhMuc] = useState(false);
  const [isShowDropDownKhaiTau, setIsShowDropDownKhaiTau] = useState(false);
  const [isCollapsed, setIsCollapsed] = useState(false);

  const handleChangeDropDownGiaoNhan = () => {
    setIsShowDropDownGiaoNhan(!isShowDropDownGiaoNhan);
  };
  const handleChangeDropDownDanhMuc = () => {
    setIsShowDropDownDanhMuc(!isShowDropDownDanhMuc);
  };


  const handleChangeDropDownKhaiTau = () => {
    setIsShowDropDownKhaiTau(!isShowDropDownKhaiTau);
  };
  const toggleNavbar = () => {
    setIsCollapsed(!isCollapsed);
  };

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
          <NavbarToggler onClick={toggleNavbar} className="mr-2" />
          <Collapse
            className="d-sm-inline-flex flex-sm-row-reverse"
            isOpen={isCollapsed}
            navbar
          >
            <ul className="navbar-nav flex-grow">
              <NavItem>
                <NavLink tag={Link} to="/khai-bao-thong-tin">
                  Khai báo giao nhận
                </NavLink>
              </NavItem>

              {/* {authCtx.isLoggedIn
                && authCtx.isService
                && <Dropdown
                  nav
                  isOpen={isShowDropDownKhaiTau}
                  toggle={handleChangeDropDownKhaiTau}
                >
                  <DropdownToggle nav caret>
                    Khai báo theo tàu
                  </DropdownToggle>
                  <DropdownMenu>
                    <DropdownItem>
                      <>
                        <NavLink
                          color="danger"
                          tag={Link}
                          to="/khai-bao-theo-tau"
                        >
                          Khai báo danh sách
                        </NavLink>
                      </>
                    </DropdownItem>
                    <DropdownItem>
                      <>
                        <NavLink tag={Link} to={"/lich-su-khai-bao-theo-tau"}>
                          Danh sách đã khai báo
                        </NavLink>
                      </>
                    </DropdownItem>
                    <DropdownItem>
                      <>
                        <NavLink tag={Link} to={"/cap-nhat-khai-bao-theo-tau"}>
                          Cập nhật khai báo
                        </NavLink>
                      </>
                    </DropdownItem>
                  </DropdownMenu>
                </Dropdown>} */}




              <Dropdown
                nav
                isOpen={isShowDropDownGiaoNhan}
                toggle={handleChangeDropDownGiaoNhan}
              >
                <DropdownToggle nav caret>
                  Thông tin giao nhận
                </DropdownToggle>
                <DropdownMenu>
                  <DropdownItem>
                    <>
                      <NavLink
                        color="danger"
                        tag={Link}
                        to="/ke-hoach-giao-nhan"
                      >
                        Kế hoạch giao nhận
                      </NavLink>
                    </>
                  </DropdownItem>
                  <DropdownItem>
                    <>
                      <NavLink tag={Link} to={"/lich-su-giao-nhan"}>
                        Lịch sử giao nhận
                      </NavLink>
                    </>
                  </DropdownItem>
                </DropdownMenu>
              </Dropdown>

              <Dropdown
                nav
                isOpen={isShowDropDownDanhMuc}
                toggle={handleChangeDropDownDanhMuc}
              >
                <DropdownToggle nav caret>
                  Danh mục
                </DropdownToggle>
                <DropdownMenu>
                  <DropdownItem>
                    <NavLink tag={Link} to="/vehicle-manager">
                      Danh sách xe
                    </NavLink>
                  </DropdownItem>
                  <DropdownItem>
                    <NavLink tag={Link} to="/driver">
                      Danh sách tài xế
                    </NavLink>
                  </DropdownItem>
                </DropdownMenu>
              </Dropdown>
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

export default NavMenu;
