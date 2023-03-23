import React from "react";
import { Descriptions, Menu, Dropdown } from "antd";
import { MoreOutlined } from "@ant-design/icons";

const UserInfo = (props) => {
  const menu = (
    <Menu>
      <Menu.Item key="1" onClick={props.handleChangePassword}>
        Thay đổi mật khẩu
      </Menu.Item>
      <Menu.Item key="2" onClick={props.handleUpdateInfo}>
        Cập nhật thông tin
      </Menu.Item>
    </Menu>
  );
  return (
    <React.Fragment>
      <Descriptions
        column={1}
        bordered
        title="Thông tin người dùng"
        extra={
          <>
            <Dropdown.Button
              trigger={["click"]}
              overlay={menu}
              icon={<MoreOutlined />}
            />
          </>
        }
      >
        <Descriptions.Item label="Tên tài khoản">
          {props.username}
        </Descriptions.Item>
        <Descriptions.Item label="Họ và tên">
          {props.fullName}
        </Descriptions.Item>
        <Descriptions.Item label="Số điện thoại">
          {props.phone}
        </Descriptions.Item>
        <Descriptions.Item label="Email">{props.email}</Descriptions.Item>
        <Descriptions.Item label="Loại tài khoản">
          {props.userType}
        </Descriptions.Item>
      </Descriptions>
    </React.Fragment>
  );
};

export default UserInfo;
