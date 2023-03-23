import React from "react";
import { Modal, Form, Input, Button } from "antd";

const CreateDriver = (props) => {
  return (
    <Modal
      visible={props.visible}
      title="Thêm tài xế"
      onCancel={props.onCancel}
      footer={[
        <Button type="primary" form="formAdd" key="submit" htmlType="submit">
          Lưu
        </Button>,
      ]}
    >
      <Form id="formAdd" onFinish={props.onFinish}>
        <Form.Item
          name="driverName"
          label="Tên tài xế"
          rules={[
            {
              required: true,
              message: "Chưa nhập tên tài xế!",
            },
          ]}
        >
          <Input />
        </Form.Item>
        <Form.Item
          name="driverCardNo"
          label="CMND/GPLX"
          rules={[
            {
              required: true,
              message: "Chưa nhập thông tin!",
            },
          ]}
        >
          <Input />
        </Form.Item>
      </Form>
    </Modal>
  );
};

export default CreateDriver;
