import React from "react";
import { Layout, Menu, Breadcrumb } from "antd";
import {
  UserOutlined,
  LaptopOutlined,
  BarsOutlined,
  DashboardOutlined,
  ShareAltOutlined,
  ReconciliationOutlined
} from "@ant-design/icons";
import classes from "./AdminLayout.module.css";
import { Link } from "react-router-dom";
import { CardImg } from "reactstrap";

const { SubMenu } = Menu;
const { Header, Content, Sider } = Layout;

const AdminLayout = (props) => {
  return (
    <Layout style={{ minHeight: "100vh" }}>
      <Header style={{ padding: "0px" }}>
        <Link className={classes.logo} to="/">
          <CardImg src="img/ic_logo.svg" alt="Vassteel" height="35px"></CardImg>
        </Link>
        {/* <BrandHeader /> */}
        <Menu
          theme="light"
          breakpoint="lg"
          mode="horizontal"
          defaultSelectedKeys={["2"]}
        >
          <Menu.Item key="1">An Hưng Tường</Menu.Item>
          <Menu.Item key="2">Tuệ Minh</Menu.Item>
          <Menu.Item key="3">Nghi Sơn</Menu.Item>
          <Menu.Item key="4">Việt Mỹ</Menu.Item>
          <Menu.Item key="5">Đà Nẵng</Menu.Item>
        </Menu>
      </Header>
      <Layout>
        <Sider
          breakpoint="lg"
          collapsedWidth="0"
          width={200}
          className={classes["site-layout-background"]}
        >
          <Menu
            mode="inline"
            defaultSelectedKeys={["1"]}
            defaultOpenKeys={["sub1"]}
            style={{ height: "100%", borderRight: 0 }}
          >
            <Menu.Item key="10" icon={<DashboardOutlined />}>
              Dashboard
            </Menu.Item>
            <SubMenu key="sub1" icon={<UserOutlined />} title="Xe đến giao">
              <Menu.Item key="1">option1</Menu.Item>
              <Menu.Item key="2">option2</Menu.Item>
              <Menu.Item key="3">option3</Menu.Item>
              <Menu.Item key="4">option4</Menu.Item>
            </SubMenu>
            <SubMenu key="sub2" icon={<LaptopOutlined />} title="Xe đến nhận">
              <Menu.Item key="5">option5</Menu.Item>
              <Menu.Item key="6">option6</Menu.Item>
              <Menu.Item key="7">option7</Menu.Item>
              <Menu.Item key="8">option8</Menu.Item>
            </SubMenu>
            <Menu.Item key="9" icon={<ShareAltOutlined />}>
              Điều phối
            </Menu.Item>

            <SubMenu
              key="sub4"
              icon={<BarsOutlined />}
              title="Danh mục"
            >
              <Menu.Item key="9">Người dùng</Menu.Item>
              <Menu.Item key="10">Nhà cung cấp</Menu.Item>
              <Menu.Item key="11">Dịch vụ vận chuyển</Menu.Item>
              <Menu.Item key="12">Khách hàng</Menu.Item>
              <Menu.Item key="13">Cung đường</Menu.Item>
              <Menu.Item key="14">Danh sách xe</Menu.Item>
            </SubMenu>
            <SubMenu
              key="report"
              icon={<ReconciliationOutlined />}
              title="Báo Cáo"
            >
              <Menu.Item key="9">Nhập Hàng</Menu.Item>
              <Menu.Item key="10">Xuất Hàng</Menu.Item>
              <Menu.Item key="11">Tình hình giao hàng</Menu.Item>
              <Menu.Item key="12">Tình hình điều phối</Menu.Item>
            </SubMenu>
            
          </Menu>
        </Sider>
        <Layout style={{ padding: "0 24px 24px" }}>
          <Breadcrumb style={{ margin: "16px 0" }}>
            <Breadcrumb.Item>Home</Breadcrumb.Item>
            <Breadcrumb.Item>List</Breadcrumb.Item>
            <Breadcrumb.Item>App</Breadcrumb.Item>
          </Breadcrumb>
          <Content className={classes["contetn-site-layout"]}>Content</Content>
        </Layout>
      </Layout>
    </Layout>
  );
};
export default AdminLayout;
